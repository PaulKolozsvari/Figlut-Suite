namespace Figlut.Server.Toolkit.Data.Redis
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using StackExchange.Redis;

    #endregion //Using Directives

    public class RedisClient : IDisposable
    {
        #region Constructors

        public RedisClient(string host)
        {
            Host = host;
        }

        #endregion //Constructors

        #region Constants

        private const string ALLOW_ADMIN = "allowAdmin=1,";

        #endregion //Constants

        #region Fields

        private string _host;
        private ConnectionMultiplexer _connection;
        private EndPoint _endPoint;
        private string _endPointAddress;
        private IServer _server;
        private IDatabase _database;

        #endregion //Fields

        #region Properties

        public string Host
        {
            get { return _host; }
            set
            {
                DataValidator.ValidateObjectNotNull(value, nameof(Host), nameof(RedisClient));
                _host = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                if (_connection == null)
                {
                    return false;
                }
                bool result = _connection.IsConnected && _server.IsConnected;
                return result;
            }
        }
        
        public ConnectionMultiplexer Connection
        {
            get { return _connection; }
        }

        public EndPoint Endpoint
        {
            get { return _endPoint; }
        }

        public string EndpointAddress
        {
            get { return _endPointAddress; }
        }

        public IServer Server
        {
            get { return _server; }
        }

        public IDatabase Database
        {
            get { return _database; }
        }

        #endregion //Properties

        #region Methods

        #region Methods

        private void EnsureConnection()
        {
            if (IsConnected)
            {
                return;
            }
            OpenConnection(_host);
        }

        public void OpenConnection(string host)
        {
            host = string.IsNullOrEmpty(host) ? _host : host;
            string connectionString = $"{ALLOW_ADMIN} {_host}";
            _connection = ConnectionMultiplexer.Connect(connectionString);
            EndPoint endPoint = _connection.GetEndPoints().FirstOrDefault();
            _endPoint = endPoint ?? throw new NullReferenceException($"Redis on host {_host} contains no endpoints.");
            _endPointAddress = endPoint.ToString();
            _server = _connection.GetServer(_endPoint);
            _database = _connection.GetDatabase();
            Host = host;
        }

        public void CloseConnection()
        {
            this.Dispose();
        }

        #endregion //Methods

        public void FlushDatabase()
        {
            EnsureConnection();
            _server.FlushDatabase();
        }

        public void FlushAllDatabases()
        {
            EnsureConnection();
            _server.FlushAllDatabases();
        }

        public void ChangeDatabase(int databaseId)
        {
            IDatabase db = _connection.GetDatabase(databaseId);
            _database = db ?? throw new NullReferenceException($"Could not find Redis database with ID of {databaseId} on host {_host}.");
        }

        public void AddEntity(string key, object entity)
        {
            EnsureConnection();
            List<HashEntry> hashSet = GetEntityHashSet(entity, out string entityHashKey);
            _database.HashSet(key, hashSet.ToArray());
        }

        public void AddEntity(object entity)
        {
            EnsureConnection();
            List<HashEntry> hashSet = GetEntityHashSet(entity, out string entityHashKey);
            _database.HashSet(entityHashKey, hashSet.ToArray());
        }

        public List<HashEntry> GetEntityHashSet(object entity, out string entityHashKey)
        {
            StringBuilder key = new StringBuilder();
            Type entityType = entity.GetType();
            key.Append(entityType.Name);
            var result = new List<HashEntry>();
            foreach (var property in entityType.GetProperties())
            {
                object value = property.GetValue(entity);
                string valueString = value != null ? value.ToString() : string.Empty;
                HashEntry hashEntry = new HashEntry(property.Name, valueString);
                key.Append($":{value}");
                result.Add(hashEntry);
            }
            entityHashKey = key.ToString();
            return result;
        }

        public string GetEntityKey(object entity)
        {
            StringBuilder key = new StringBuilder();
            Type entityType = entity.GetType();
            key.Append(entityType.Name);
            foreach (var property in entityType.GetProperties())
            {
                object value = property.GetValue(entity);
                string valueString = value != null ? value.ToString() : string.Empty;
                key.Append($":{value}");
            }
            return key.ToString();
        }

        public string GetSearchKey(List<string> entityFieldValues, Type entityType)
        {
            StringBuilder key = new StringBuilder();
            key.Append(entityType.Name);
            foreach (var value in entityFieldValues)
            {
                key.Append($":{value}");
            }
            return key.ToString();
        }

        public string GetSearchKey(object prototypeEntity)
        {
            StringBuilder key = new StringBuilder();
            Type entityType = prototypeEntity.GetType();
            key.Append(entityType.Name);
            foreach (var property in entityType.GetProperties())
            {
                object value = property.GetValue(prototypeEntity);
                string valueString = value != null ? value.ToString() : "*";
                if (string.IsNullOrEmpty(valueString))
                {
                    valueString = "*";
                }
                key.Append($":{value}");
            }
            return key.ToString();
        }

        public bool RemoveEntity(object entity, bool throwExceptionOnNotFound)
        {
            string key = GetEntityKey(entity);;
            return RemoveEntity(key, throwExceptionOnNotFound);
        }

        public bool RemoveEntity(string key, bool throwExceptionOnNotFound)
        {
            EnsureConnection();
            if (!_database.KeyExists(key))
            {
                if (throwExceptionOnNotFound)
                {
                    throw new NullReferenceException($"Could not find entity with key {key}.");
                }
                return false;
            }
            return _database.KeyDelete(key);
        }

        public object GetEntity(string key, bool throwExceptionOnNotFound, Type entityType)
        {
            EnsureConnection();
            if (!_database.KeyExists(key))
            {
                if (throwExceptionOnNotFound)
                {
                    throw new NullReferenceException($"Could not find entity with key {key}.");
                }
                return null;
            }
            HashEntry[] hashSet = _database.HashGetAll(key);
            object result = Activator.CreateInstance(entityType);
            foreach (var property in entityType.GetProperties())
            {
                HashEntry entry = hashSet.Where(e => e.Name.Equals(property.Name)).FirstOrDefault();
                if (entry == null)
                {
                    throw new NullReferenceException($"Could not find {nameof(HashEntry)} for property {property.Name} for entity {entityType.Name}.");
                }
                object value = EntityReader.ConvertValueTypeTo(entry.Value, property.PropertyType);
                property.SetValue(result, value);
            }
            return result;
        }

        public Dictionary<string, E> GetEntities<E>(string searchKey) where E : class
        {
            Dictionary<string, E> result = new Dictionary<string, E>();
            foreach (KeyValuePair<string, object> e in GetEntities(searchKey, typeof(E)))
            {
                result.Add(e.Key, (E)e.Value);
            }
            return result;
        }

        public Dictionary<string, object> GetEntities(string searchKey, Type entityType)
        {
            EnsureConnection();
            _database.Sort(searchKey, sortType: SortType.Alphabetic);
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (RedisKey key in _server.Keys(_database.Database, searchKey, pageSize: 30000))
            {
                object entity = GetEntity(key, true, entityType);
                result.Add(key, entity);
            }
            return result;
        }

        public long GetEntityCount()
        {
            EnsureConnection();
            return _server.Keys().LongCount();
        }

        public void Dispose()
        {
            if (_connection == null)
            {
                return;
            }
            if (_connection.IsConnected)
            {
                _connection.Close();
            }
            _connection.Dispose();
            _connection = null;
            _database = null;
            _server = null;
            _endPoint = null;
            _endPointAddress = null;
        }

        #endregion //Methods
    }
}