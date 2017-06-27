namespace Psion.Server.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Data;
    using System.Reflection;
    using Psion.Server.Toolkit.Data.DB.SQLQuery;
    using Psion.Server.Toolkit.Utilities.Logging;
    using System.Data.SqlClient;
    using System.Reflection.Emit;
    using Psion.Server.Toolkit.Data.ORM;

    #endregion //Using Directives

    public class SqlDatabase : Database
    {
        #region Constructors

        public SqlDatabase()
        {
        }

        public SqlDatabase(
            string connectionString, 
            bool initializeDbConnection,
            bool openConnection,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            bool overrideNameWithDatabaseNameFromSchema)
            : base(
            connectionString, 
            initializeDbConnection, 
            openConnection, 
            createOrmAssembly, 
            saveOrmAssembly, 
            overrideNameWithDatabaseNameFromSchema)
        {
        }

        public SqlDatabase(
            string name, 
            string connectionString, 
            bool initializeDbConnection,
            bool openConnection,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            bool overrideNameWithDatabaseNameFromSchema)
            : base(
            name, 
            connectionString, 
            initializeDbConnection, 
            openConnection, 
            createOrmAssembly, 
            saveOrmAssembly, 
            overrideNameWithDatabaseNameFromSchema)
        {
        }

        #endregion //Constructors

        #region Constants

        public const string DATABASE_NAME_SCHEMA_ATTRIBUTE = "database_name";

        #endregion //Constants

        #region Properties

        public SqlConnection Connection
        {
            get { return (SqlConnection)_connection; }
            set { _connection = value; }
        }

        public bool IsConectionInitialized
        {
            get { return _connection != null; }
        }

        #endregion //Properties

        #region Methods

        public SqlDatabaseTable<E> GetSqlDatabaseTable<E>() where E : class
        {
            return GetSqlDatabaseTable<E>(typeof(E).Name);
        }

        public SqlDatabaseTable<E> GetSqlDatabaseTable<E>(string tableName) where E : class
        {
            if (!_tables.Exists(tableName))
            {
                return null;
            }
            SqlDatabaseTable<E> result = _tables[tableName] as SqlDatabaseTable<E>;
            if (result == null)
            {
                throw new InvalidCastException(string.Format(
                    "Unexpected table type in {0}. Could not type cast {1} to a {2}.",
                    this.GetType().FullName,
                    typeof(Database).FullName,
                    typeof(SqlDatabaseTable<E>).FullName));
            }
            return result;
        }

        public void AddTable<E>() where E : class
        {
            AddTable<E>(typeof(E).Name);
        }

        public void AddTable<E>(string tableName) where E : class
        {
            VerifyDbConnectionInitialized();
            if (_tables.Exists(tableName))
            {
                throw new Exception(string.Format(
                    "{0} with name {1} already added to {2}.",
                    typeof(SqlDatabaseTable<E>).FullName,
                    tableName,
                    this.GetType().FullName));
            }
            SqlDatabaseTable<E> table = new SqlDatabaseTable<E>(tableName, (SqlConnection)_connection);
            _tables.Add(table);
        }

        public override void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public override void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public override void InitliazeDbConnection()
        {
            InitliazeDbConnection(null);
        }

        public override void InitliazeDbConnection(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                _connectionString = connectionString;
            }
            _connection = new SqlConnection(_connectionString);
        }

        public override void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
            if (!string.IsNullOrEmpty(_ormAssembly.AssemblyFilePath) && 
                File.Exists(_ormAssembly.AssemblyFilePath))
            {
                File.Delete(_ormAssembly.AssemblyFilePath);
            }
        }

        public void VerifyDbConnectionInitialized()
        {
            if (_connection == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} not initialized in {1}.",
                    typeof(SqlConnection).FullName,
                    this.GetType().FullName));
            }
        }

        public void InitializeAllTables()
        {
            _tables.ToList().ForEach(p => p.Initialize());
        }

        public List<object> Query(SqlQuery query, Type entityType)
        {
            List<DatabaseTable> tablesMentioned = GetTablesMentionedInQuery(query);
            List<object> result = null;
            using (SqlCommand command = new SqlCommand(query.SqlQuerySring, (SqlConnection)_connection))
            {
                query.SqlCeParameters.ForEach(p => command.Parameters.Add(p));
                command.CommandType = System.Data.CommandType.Text;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    result = DataHelper.ParseReaderToEntities(reader, entityType);
                }
            }
            tablesMentioned.ForEach(t => t.Initialize());
            return result;
        }

        public List<E> Query<E>(SqlQuery query) where E : class
        {
            List<object> objects= Query(query, typeof(E));
            List<E> result = new List<E>();
            objects.ForEach(o => result.Add((E)o));
            return result;
        }

        public virtual List<object> Query(
            string comlumnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive,
            Type entityType)
        {
            return Query(comlumnName, columnValue, useLikeFilter, caseSensitive, entityType.Name);
        }

        public virtual List<object> Query(
            string comlumnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive,
            string tableName)
        {
            DatabaseTable table = GetDatabaseTable(tableName);
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(DatabaseTable).FullName,
                    tableName));
            }
            return table.Query(comlumnName, columnValue, useLikeFilter, caseSensitive);
        }

        public List<E> Query<E>(
            string comlumnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive) where E : class
        {
            SqlDatabaseTable<E> table = GetSqlDatabaseTable<E>();
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(DatabaseTable).FullName,
                    typeof(E).Name));
            }
            return table.Query(comlumnName, columnValue, useLikeFilter, caseSensitive);
        }

        public void ExecuteNonQuery(SqlQuery query)
        {
            List<DatabaseTable> tablesMentioned = GetTablesMentionedInQuery(query);
            using (SqlCommand command = new SqlCommand(query.SqlQuerySring, (SqlConnection)_connection))
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

        public void Insert<E>(E e, SqlDatabaseTable<E> table) where E : class
        {
            if (table == null)
            {
                table = GetSqlDatabaseTable<E>();
            }
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(DatabaseTable).FullName,
                    typeof(E).Name));
            }
            table.Insert(e);
        }

        public void Insert<E>(List<E> entities, bool useTransaction) where E : class
        {
            SqlTransaction t = null;
            try
            {
                if (!useTransaction)
                {
                    entities.ForEach(e => Insert<E>(e, GetSqlDatabaseTable<E>()));
                    return;
                }
                using (t = ((SqlConnection)_connection).BeginTransaction())
                {
                    entities.ForEach(e => Insert<E>(e, GetSqlDatabaseTable<E>()));
                    t.Commit();
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

        public void Delete<E>(string columnName, object columnValue) where E : class
        {
            SqlDatabaseTable<E> table = GetSqlDatabaseTable<E>();
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(DatabaseTable).FullName,
                    typeof(E).Name));
            }
            table.Delete(columnName, columnValue);
        }

        public void Update<E>(List<E> entities, string columnName, bool useTransaction) where E : class
        {
            SqlTransaction t = null;
            try
            {
                if (!useTransaction)
                {
                    entities.ForEach(e => Update<E>(e, columnName, GetSqlDatabaseTable<E>()));
                    return;
                }
                using (t = ((SqlConnection)_connection).BeginTransaction())
                {
                    entities.ForEach(e => Update<E>(e, columnName, GetSqlDatabaseTable<E>()));
                    t.Commit();
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

        protected void Update<E>(E e, string columnName, SqlDatabaseTable<E> table) where E : class
        {
            if (table == null)
            {
                table = GetSqlDatabaseTable<E>();
            }
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(DatabaseTable).FullName,
                    typeof(E).Name));
            }
            table.Update(e, columnName);
        }

        public DataTable GetSchema()
        {
            VerifyDbConnectionInitialized();
            return _connection.GetSchema();
        }

        public override string GetDatabaseNameFromSchema()
        {
            VerifyDbConnectionInitialized();
            DataTable schema = _connection.GetSchema("Databases");
            foreach (DataRow row in schema.Rows)
            {
                string databaseName = row[DATABASE_NAME_SCHEMA_ATTRIBUTE].ToString();
                if (_connectionString.Contains(databaseName))
                {
                    return databaseName;
                }
            }
            throw new Exception(string.Format(
                "No database with exists on the server with a name as mentioned in the connection string {0}.",
                _connectionString));
        }

        /// <summary>
        /// http://msdn.microsoft.com/library/ms254969.aspx
        /// http://www.devart.com/dotconnect/salesforce/docs/Metadata-GetSchema.html
        /// </summary>
        /// <param name="includeColumns"></param>
        public override void PopulateTablesFromSchema(bool includeColumns)
        {
            ///TODO Look at using SQL MSO (Server Management Objects) http://msdn.microsoft.com/en-us/magazine/cc163409.aspx
            VerifyDbConnectionInitialized();
            _tables.Clear();
            DataTable schema = _connection.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                SqlDatabaseTable<object> table = new SqlDatabaseTable<object>((SqlConnection)_connection, row);
                if (table.IsSystemTable)
                {
                    continue;
                }
                if (_tables.Exists(table.TableName))
                {
                    throw new Exception(string.Format(
                        "{0} with name {1} already added to {2}.",
                        typeof(SqlDatabaseTable<object>).FullName,
                        table.TableName,
                        this.GetType().FullName));
                }
                _tables.Add(table.TableName, table);
            }
            if (includeColumns)
            {
                _tables.ToList().ForEach(t => t.PopulateColumnsFromSchema());
            }
        }

        #endregion //Methods
    }
}