namespace Figlut.Mobile.Toolkit.Data.DB.SQLCE
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Data.SqlServerCe;
    using System.ServiceModel.Channels;
    using System.IO;
    using System.Data;
    using Microsoft.Synchronization.Data;
    using Microsoft.Synchronization;
    using System.Reflection;
    using Microsoft.Synchronization.Data.SqlServerCe;
    using Figlut.Mobile.Toolkit.Data.DB.SyncService;
    using Figlut.Mobile.Toolkit.Data.DB.SQLQuery;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Data.ORM;
    using SystemCF.Reflection.Emit;

    #endregion //Using Directives

    public class SqlCeDatabase : MobileDatabase
    {
        #region Constructors

        public SqlCeDatabase()
            : base()
        {
            _name = this.GetType().Name;
        }

        public SqlCeDatabase(string localDbFilePath)
            : base(localDbFilePath)
        {
        }

        public SqlCeDatabase(string name, string localDbFilePath)
            : base(name, localDbFilePath)
        {
        }

        public SqlCeDatabase(string name, string localDbFilePath, SyncInitializationConfig syncServiceConfiguration)
            : base(name, localDbFilePath, syncServiceConfiguration)
        {
        }

        #endregion //Constructors

        #region Constants

        public const int DEFAULT_CONNECTION_MAX_BUFFER_SIZE = 256;

        #endregion //Constants

        #region Fields

        protected SqlCeConnection _connection;

        #region Sync Progress

        protected SyncConfig _syncConfig;
        protected SyncResult _syncResult;

        #endregion //Sync Progress

        #endregion //Fields

        #region Properties

        public SqlCeConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        #endregion //Properties

        #region Methods

        public void CreateDatabase(bool openConnectionAfterCreate)
        {
            VerifyLocalDbConnectionInitialized();
            if (File.Exists(_localDbFilePath))
            {
                File.Delete(_localDbFilePath);
            }
            SqlCeEngine engine = new SqlCeEngine(GetLocalDbConnectionString());
            engine.CreateDatabase();
            if (!File.Exists(_localDbFilePath))
            {
                throw new FileNotFoundException(string.Format("Could not find {0} after creating it.", _localDbFilePath));
            }
            if (openConnectionAfterCreate)
            {
                OpenConnection();
            }
        }

        public void CreateSqlCeDatabaseTable<E>(List<string> primaryKeyNames) where E : class
        {
            VerifyLocalDbConnectionInitialized();
            using (SqlCeCommand command = _connection.CreateCommand())
            {
                command.CommandText = GetSqlCreateTableScript<E>(primaryKeyNames);
                command.ExecuteNonQuery();
            }
        }

        public string GetSqlCreateTableScript<E>(List<string> primaryKeyNames) where E : class
        {
            StringBuilder result = new StringBuilder();
            Type entityType = typeof(E);
            result.AppendLine(string.Format("CREATE TABLE {0}", entityType.Name));
            result.AppendLine("(");
            PropertyInfo[] properties = entityType.GetProperties();
            for(int i = 0; i < properties.Length; i++)
            {
                PropertyInfo p = properties[i];
                Type propertyType = p.PropertyType;
                bool isNullable = propertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                string nullableString = isNullable ? "NULL" : "NOT NULL";
                string sqlTypeName = ApplySqlTypeNameLength(
                    p,
                    SqlTypeConverter.Instance.GetSqlTypeNameFromDotNetType(p.PropertyType, isNullable));
                string primaryKeyString = primaryKeyNames.Contains(p.Name) ? "PRIMARY KEY" : string.Empty;

                result.Append(string.Format("{0} {1} {2}", p.Name, sqlTypeName, nullableString));
                if (primaryKeyNames.Contains(p.Name))
                {
                    result.Append(" PRIMARY KEY");
                }
                if (i < (properties.Length - 1)) //This is not the last property.
                {
                    result.AppendLine(",");
                }
            }
            result.AppendLine(");");
            return result.ToString();
        }

        private string ApplySqlTypeNameLength(PropertyInfo p, string sqlTypeName)
        {
            if (p.PropertyType.Equals(typeof(string)))
            {
                return sqlTypeName += string.Format("({0})", 50);
            }
            return sqlTypeName;
        }

        public SqlCeDatabaseTable<E> GetSqlCeDatabaseTable<E>() where E : class
        {
            return GetSqlCeDatabaseTable<E>(typeof(E).Name);
        }

        public SqlCeDatabaseTable<E> GetSqlCeDatabaseTable<E>(string tableName) where E : class
        {
            if (!_tables.Exists(tableName))
            {
                return null;
            }
            SqlCeDatabaseTable<E> result = _tables[tableName] as SqlCeDatabaseTable<E>;
            if (result == null)
            {
                throw new InvalidCastException(string.Format(
                    "Unexpected table type in {0}. Could not type cast {1} to a {2}.",
                    this.GetType().FullName,
                    typeof(MobileDatabase).FullName,
                    typeof(SqlCeDatabaseTable<E>).FullName));
            }
            return result;
        }

        public void AddTable(MobileDatabaseTable table)
        {
            _tables.Add(table);
        }

        public void AddTable<E>() where E : class
        {
            AddTable<E>(typeof(E).Name);
        }

        public void AddTable<E>(string tableName) where E : class
        {
            AddTable<E>(tableName, null);
        }

        public void AddTable<E>(Nullable<SyncDirection> syncDirection) where E : class
        {
            AddTable<E>(typeof(E).Name, syncDirection);
        }

        public void AddTable<E>(string tableName, Nullable<SyncDirection> syncDirection) where E : class
        {
            VerifyLocalDbConnectionInitialized();
            if (_tables.Exists(tableName))
            {
                throw new Exception(string.Format(
                    "{0} with name {1} already added to {2}.",
                    typeof(SqlCeDatabaseTable<E>).FullName,
                    tableName,
                    this.GetType().FullName));
            }
            SqlCeDatabaseTable<E> table = null;
            if (!syncDirection.HasValue)
            {
                table = new SqlCeDatabaseTable<E>(tableName, _connection);
                _tables.Add(table);
            }
            else
            {
                table = new SqlCeDatabaseTable<E>(tableName, _connection, syncDirection.Value);
                _tables.Add(table);
                SetTableSyncDirection<E>(table.TableName, table.SyncDirection);
            }
        }

        public void SetTableSyncDirection<E>(string tableName, SyncDirection syncDirection) where E : class
        {
            VerifySynchronizationInitialized();
            if (!_tables.Exists(tableName))
            {
                throw new Exception(string.Format(
                    "{0} with name {1} does not exist in {2}.",
                    typeof(SqlCeDatabaseTable<E>).FullName,
                    tableName,
                    this.GetType().FullName));
            }
            SqlCeDatabaseTable<E> table = GetSqlCeDatabaseTable<E>(tableName);
            SyncTable syncTable = _syncInitializationConfig.SyncAgent.Configuration.SyncTables[table.TableName];
            if (syncTable == null)
            {
                throw new NullReferenceException(string.Format(
                    string.Format("No {0} exists in {1} with name {2}.",
                    typeof(SyncTable).FullName,
                    typeof(SyncAgent).FullName,
                    tableName)));
            }
            syncTable.SyncDirection = syncDirection;
        }

        public override string GetLocalDbConnectionString()
        {
            return GetLocalDbConnectionString(DEFAULT_CONNECTION_MAX_BUFFER_SIZE);
        }

        public override string GetLocalDbConnectionString(int maxBufferSize)
        {
            if (string.IsNullOrEmpty(_localDbFilePath))
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set on {1}.",
                    EntityReader<SqlCeDatabase>.GetPropertyName(p => p.LocalDbFilePath, false),
                    this.GetType().FullName));
            }
            return string.Format(
                "Data Source={0};Max Buffer Size = {1}",
                _localDbFilePath,
                maxBufferSize);
        }

        public bool LocalDbExists()
        {
            if (string.IsNullOrEmpty(_localDbFilePath))
            {
                throw new FileNotFoundException(string.Format(
                    "{0} not set on {1}.",
                    EntityReader<SqlCeDatabase>.GetPropertyName(p => p.LocalDbFilePath, false),
                    this.GetType().FullName));
            }
            return File.Exists(_localDbFilePath);
        }

        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public void InitliazeLocalDbConnection()
        {
            InitliazeLocalDbConnection(null, null);
        }

        public void InitliazeLocalDbConnection(Nullable<int> localDbConnectionMaxBufferSize)
        {
            InitliazeLocalDbConnection(localDbConnectionMaxBufferSize, null);
        }

        public void InitliazeLocalDbConnection(string localDbFilePath)
        {
            InitliazeLocalDbConnection(null, localDbFilePath);
        }

        public void InitliazeLocalDbConnection(Nullable<int> localDbConnectionMaxBufferSize, string localDbFilePath)
        {
            if (!string.IsNullOrEmpty(localDbFilePath))
            {
                _localDbFilePath = localDbFilePath;
            }
            string localDbConnectionString =
                localDbConnectionMaxBufferSize.HasValue ?
                GetLocalDbConnectionString(localDbConnectionMaxBufferSize.Value) :
                GetLocalDbConnectionString();
            _connection = new SqlCeConnection(localDbConnectionString);
        }

        public void InitalizeSynchronization(SyncInitializationConfig syncInitializationConfig)
        {
            try
            {
                VerifyLocalDbConnectionInitialized();
                if (syncInitializationConfig == null)
                {
                    throw new NullReferenceException(string.Format(
                        "syncInitializationConfig when calling InitalizeSynchronization in {0}.",
                        this.GetType().FullName));
                }
                UnsubscribeFromSyncAgentEvents();
                _syncInitializationConfig = syncInitializationConfig;
                _syncInitializationConfig.SyncAgent.StateChanged += new EventHandler<SessionStateChangedEventArgs>(SyncAgent_StateChanged);
                _syncInitializationConfig.SyncAgent.SessionProgress += new EventHandler<SessionProgressEventArgs>(SyncAgent_SessionProgress);

                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                if (syncInitializationConfig.ClearTablesAfterInitialization)
                {
                    ClearTables();
                }
            }
            finally
            {
                if ((syncInitializationConfig.OpenConnectionAfterInitialization) &&
                    (_connection.State != ConnectionState.Open))
                {
                    _connection.Open();
                }
            }
        }

        private void UnsubscribeFromSyncAgentEvents()
        {
            if ((_syncInitializationConfig != null))
            {
                _syncInitializationConfig.SyncAgent.StateChanged -= new EventHandler<SessionStateChangedEventArgs>(SyncAgent_StateChanged);
                _syncInitializationConfig.SyncAgent.SessionProgress -= new EventHandler<SessionProgressEventArgs>(SyncAgent_SessionProgress);   
            }
        }

        public override void Dispose()
        {
            if (_syncConfig != null)
            {
                throw new Exception(string.Format(
                    "May not call Dispose on {0} while a sync is in progress.",
                    this.GetType().FullName));
            }
            UnsubscribeFromSyncAgentEvents();
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
        }

        public void VerifyLocalDbConnectionInitialized()
        {
            if (_connection == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} not initialized in {1}.",
                    typeof(SqlCeConnection).FullName,
                    this.GetType().FullName));
            }
        }

        public void VerifySynchronizationInitialized()
        {
            if ((_connection == null) ||
                (_syncInitializationConfig == null))
            {
                throw new Exception(string.Format(
                    "{0} has not been initialized.",
                    this.GetType().FullName));
            }
        }

        public void InitializeAllTableResultsets()
        {
            _tables.ToList().ForEach(p => p.Initialize());
        }

        public override SyncResult Synchronize(SyncConfig syncConfig)
        {
            return Synchronize(syncConfig, false);
        }

        public SyncResult Synchronize(SyncConfig syncConfig, bool reinitializeAllTableResultSetsAfterSync)
        {
            SyncStatistics syncStatistics = null;
            try
            {
                try
                {
                    if (_syncConfig != null)
                    {
                        throw new Exception(string.Format(
                            "Cannot start a new sync on {0} while another sync is progress.",
                            this.GetType().FullName));
                    }
                    _syncConfig = syncConfig;
                    _syncResult = new SyncResult();
                    syncStatistics = Synchronize(
                        false,
                        syncConfig.HandleLocalDbConnection,
                        reinitializeAllTableResultSetsAfterSync);
                }
                catch (TargetInvocationException ex)
                {
                    if ((_syncInitializationConfig.ReinitializeSubscriptionIfExpired) &&
                        (ex.InnerException != null) &&
                        (ex.InnerException.Message.Contains(_syncInitializationConfig.SubscriptionExpiredExpectedMessage)))
                    {
                        /*Initialize the client database by resetting the anchors on all tables i.e. 
                         * the versions of the anchors must be greater than that of the server or be reset.*/
                        syncStatistics = Synchronize(true, syncConfig.HandleLocalDbConnection, reinitializeAllTableResultSetsAfterSync);
                    }
                    else
                    {
                        throw ex;
                    }
                }
                _syncResult.SyncCompleted(syncStatistics);
                if (_syncConfig.LogSyncStatisticsSummary)
                {
                    GOC.Instance.Logger.LogMessage(new LogMessage(
                        _syncResult.SyncStatisticsSummaryMessage, 
                        LogMessageType.Information, 
                        LoggingLevel.Normal));
                }
            }
            finally
            {
                _syncConfig = null;
            }
            return _syncResult;
        }

        private void SyncAgent_SessionProgress(object sender, SessionProgressEventArgs e)
        {
            if (_syncConfig == null)
            {
                throw new Exception(string.Format(
                    "Cannot run session progress handler in {0} if sync is not in progress.",
                    this.GetType().FullName));
            }
            string syncSessionMessage = _syncResult.SyncSessionProgress(e, _syncConfig.ShowPercentageInProgressSummary);
            _syncConfig.ChangeStatus(syncSessionMessage);
            base.FireSyncSessionProgressEvent(this, new SyncSessionProgressEventArgs(syncSessionMessage, e));
        }

        private void SyncAgent_StateChanged(object sender, SessionStateChangedEventArgs e)
        {
            if (_syncConfig == null)
            {
                throw new Exception(string.Format(
                    "Cannot run state changed progress handler in {0} if sync is not in progress.",
                    this.GetType().FullName));
            }
            string syncStateChangedMessage = _syncResult.SyncStateChanged(e);
            _syncConfig.ChangeStatus(syncStateChangedMessage);
            base.FireSyncSessionStateChangedEvent(this, new SyncSessionStateChangedEventArgs(syncStateChangedMessage, e));
        }

        private SyncStatistics Synchronize(
            bool reinitializeSubscription, 
            bool handleLocalDbConnection, 
            bool reinitializeAllTableResultSetsAfterSync)
        {
            if (reinitializeSubscription)
            {
                SqlCeClientSyncProvider sqlCeProvider = ((SqlCeClientSyncProvider)(_syncInitializationConfig.SyncAgent.LocalProvider));
                foreach (SyncTable st in _syncInitializationConfig.SyncAgent.Configuration.SyncTables)
                {
                    if (st.SyncDirection != SyncDirection.Snapshot)
                    {
                        sqlCeProvider.SetTableReceivedAnchor(st.TableName, new SyncAnchor());
                    }
                }
            }
            if (handleLocalDbConnection)
            {
                CloseConnection();
            }
            SyncStatistics result = _syncInitializationConfig.SyncAgent.Synchronize();
            if (handleLocalDbConnection)
            {
                OpenConnection();
            }
            if (reinitializeAllTableResultSetsAfterSync)
            {
                InitializeAllTableResultsets();
            }
            return result;
        }

        public List<MobileDatabaseTable> GetTablesMentionedInQuery(SqlQuery query)
        {
            List<MobileDatabaseTable> result = new List<MobileDatabaseTable>();
            foreach (string t in query.TableNamesInQuery)
            {
                MobileDatabaseTable table = _tables[t];
                if (table == null)
                {
                    throw new NullReferenceException(string.Format(
                        "Could not find table {0} mentioned in {1} inside {2}.",
                        t,
                        query.GetType().FullName,
                        this.GetType().FullName));
                }
                result.Add(table);
            }
            return result;
        }

        public List<E> Query<E>(SqlQuery query) where E : class
        {
            List<MobileDatabaseTable> tablesMentioned = GetTablesMentionedInQuery(query);
            SqlCeResultSet queryResultSet = null;
            using (SqlCeCommand command = new SqlCeCommand(query.SqlQuerySring, _connection))
            {
                query.SqlCeParameters.ForEach(p => command.Parameters.Add(p));
                command.CommandType = System.Data.CommandType.Text;
                queryResultSet = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            }
            List<E> result = DataHelper.ParseSqlResultSetToEntities<E>(queryResultSet);
            tablesMentioned.ForEach(t => t.Initialize());
            return result;
        }

        public T QueryScalar<T>(SqlQuery query)
        {
            List<MobileDatabaseTable> tablesMentioned = GetTablesMentionedInQuery(query);
            SqlCeResultSet queryResultSet = null;
            using (SqlCeCommand command = new SqlCeCommand(query.SqlQuerySring, _connection))
            {
                query.SqlCeParameters.ForEach(p => command.Parameters.Add(p));
                command.CommandType = System.Data.CommandType.Text;
                queryResultSet = command.ExecuteResultSet(ResultSetOptions.Scrollable);
            }
            return DataHelper.ParseSqlCeResultSetToScalarValue<T>(queryResultSet);
        }

        public List<E> Query<E>(
            string comlumnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive) where E : class
        {
            SqlCeDatabaseTable<E> table = GetSqlCeDatabaseTable<E>();
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(MobileDatabaseTable).FullName,
                    typeof(E).Name));
            }
            return table.Query(comlumnName, columnValue, useLikeFilter, caseSensitive);
        }

        public void ExecuteNonQuery(SqlQuery query)
        {
            List<MobileDatabaseTable> tablesMentioned = GetTablesMentionedInQuery(query);
            using (SqlCeCommand command = new SqlCeCommand(query.SqlQuerySring, _connection))
            {
                query.SqlCeParameters.ForEach(p => command.Parameters.Add(p));
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new Exception(string.Format("Sql script failed: {0}.", query.SqlQuerySring));
                }
            }
            tablesMentioned.ForEach(t => t.Initialize());
        }

        public void Insert<E>(E e) where E : class
        {
            Insert<E>(e, null);
        }

        public void Insert<E>(E e, SqlCeDatabaseTable<E> table) where E : class
        {
            if (table == null)
            {
                table = GetSqlCeDatabaseTable<E>();
            }
            table.Insert(e);
            table.Initialize();
        }

        public void Insert<E>(List<E> entities, bool useTransaction) where E : class
        {
            SqlCeTransaction t = null;
            try
            {
                SqlCeDatabaseTable<E> table = GetSqlCeDatabaseTable<E>();
                if (useTransaction)
                {
                    using (t = _connection.BeginTransaction())
                    {
                        entities.ForEach(e => Insert<E>(e, table));
                        t.Commit(CommitMode.Immediate);
                    }
                }
                else
                {
                    entities.ForEach(e => Insert<E>(e, table));
                }
                table.Initialize();
            }
            catch (Exception ex)
            {
                if (t != null)
                {
                    t.Rollback();
                }
                throw ex;
            }
        }

        public void Delete<E>(E e, string columnName) where E : class
        {
            SqlCeDatabaseTable<E> table = GetSqlCeDatabaseTable<E>();
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(MobileDatabaseTable).FullName,
                    typeof(E).Name));
            }
            table.Delete(e, columnName);
        }

        public void Update<E>(List<E> entities, string columnName, bool useTransaction) where E : class
        {
            SqlCeTransaction t = null;
            try
            {
                if (useTransaction)
                {
                    using (t = _connection.BeginTransaction())
                    {
                        entities.ForEach(e => Update<E>(e, columnName, GetSqlCeDatabaseTable<E>()));
                        t.Commit(CommitMode.Immediate);
                    }
                }
                else
                {
                    entities.ForEach(e => Update<E>(e, columnName, GetSqlCeDatabaseTable<E>()));
                }
            }
            catch (Exception ex)
            {
                if (t != null)
                {
                    t.Rollback();
                }
                throw ex;
            }
        }

        public void Update<E>(E e, string columnName) where E : class
        {
            Update(e, columnName, null);
        }

        protected void Update<E>(E e, string columnName, SqlCeDatabaseTable<E> table) where E : class
        {
            if (table == null)
            {
                table = GetSqlCeDatabaseTable<E>();
            }
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(MobileDatabaseTable).FullName,
                    typeof(E).Name));
            }
            table.Update(e, columnName);
        }

        #endregion //Methods
    }
}