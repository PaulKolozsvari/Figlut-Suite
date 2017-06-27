namespace Figlut.Server.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Data;
    using System.Reflection;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using System.Data.SqlClient;
    using System.Reflection.Emit;
    using Figlut.Server.Toolkit.Data.ORM;
    using System.Data.Common;

    #endregion //Using Directives

    [Serializable]
    public class SqlDatabase : Database
    {
        #region Constructors

        public SqlDatabase()
        {
        }

        public SqlDatabase(
            string connectionString, 
            bool populateTablesFromSchema,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            bool copyOrmAssembly, 
            string ormAssemblyCopyDirectory,
            bool overrideNameWithDatabaseNameFromSchema)
            : base(
            connectionString,
            populateTablesFromSchema,
            createOrmAssembly, 
            saveOrmAssembly,
            copyOrmAssembly,
            ormAssemblyCopyDirectory,
            overrideNameWithDatabaseNameFromSchema)
        {
        }

        public SqlDatabase(
            string name, 
            string connectionString, 
            bool populateTablesFromSchema,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            string ormAssemblyOutputDirectory,
            bool overrideNameWithDatabaseNameFromSchema)
            : base(
            name, 
            connectionString,
            populateTablesFromSchema,
            createOrmAssembly, 
            saveOrmAssembly,
            ormAssemblyOutputDirectory,
            overrideNameWithDatabaseNameFromSchema)
        {
        }

        #endregion //Constructors

        #region Constants

        public const string DATABASE_NAME_SCHEMA_ATTRIBUTE = "database_name";

        #endregion //Constants

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

        public void AddTable(DatabaseTable table)
        {
            _tables.Add(table);
        }

        public void AddTable<E>() where E : class
        {
            AddTable<E>(typeof(E).Name);
        }

        public void AddTable<E>(string tableName) where E : class
        {
            if (_tables.Exists(tableName))
            {
                throw new Exception(string.Format(
                    "{0} with name {1} already added to {2}.",
                    typeof(SqlDatabaseTable<E>).FullName,
                    tableName,
                    this.GetType().FullName));
            }
            SqlDatabaseTable<E> table = new SqlDatabaseTable<E>(tableName, _connectionString);
            _tables.Add(table);
        }

        public override void Dispose()
        {
            if (!string.IsNullOrEmpty(_ormAssembly.AssemblyFilePath) && 
                File.Exists(_ormAssembly.AssemblyFilePath))
            {
                File.Delete(_ormAssembly.AssemblyFilePath); //Delete the ORM assembly from the output directory.
            }
        }

        public List<object> Query(string sqlQueryString, 
            OrmAssemblySql ormCollectibleAssembly, 
            string typeName,
            out OrmType ormCollecibleType)
        {
            if (ormCollectibleAssembly == null)
            {
                throw new NullReferenceException(string.Format("ormCollectibleAssembly may not be null."));
            }
            if (ormCollectibleAssembly.AssemblyBuilderAccess != AssemblyBuilderAccess.RunAndCollect)
            {
                throw new ArgumentException(string.Format(
                    "Querying the database with a raw SQL query string requires an {0} with the {1} property set to {2}.",
                    typeof(OrmAssembly).FullName,
                    EntityReader<OrmAssembly>.GetPropertyName(p => p.AssemblyBuilderAccess, false),
                    AssemblyBuilderAccess.RunAndCollect));
            }
            List<object> result = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQueryString, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ormCollecibleType = ormCollectibleAssembly.CreateOrmTypeFromSqlDataReader(typeName, reader, true);
                        result = DataHelper.ParseReaderToEntities(reader, ormCollecibleType.DotNetType);
                    }
                }
            }
            return result;
        }

        public List<object> Query(SqlQuery query, Type entityType)
        {
            List<DatabaseTable> tablesMentioned = GetTablesMentionedInQuery(query);
            List<object> result = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query.SqlQuerySring, connection))
                {
                    query.SqlCeParameters.ForEach(p => command.Parameters.Add(p));
                    command.CommandType = System.Data.CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = DataHelper.ParseReaderToEntities(reader, entityType);
                    }
                }
            }
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query.SqlQuerySring, connection))
                {
                    query.SqlCeParameters.ForEach(p => command.Parameters.Add(p));
                    if (command.ExecuteNonQuery() < 1)
                    {
                        throw new Exception(string.Format("Sql script failed: {0}.", query.SqlQuerySring));
                    }
                }
            }
        }

        public DataTable GetSchema()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Close();
                return connection.GetSchema();
            }
        }

        public override string GetDatabaseNameFromSchema(
            DbConnection connection, 
            bool disposeConnectionAfterExecute)
        {
            try
            {
                if (connection == null)
                {
                    connection = new SqlConnection(_connectionString);
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                DataTable schema = connection.GetSchema("Databases");
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
            finally
            {
                if (disposeConnectionAfterExecute &&
                    connection != null &&
                    connection.State != ConnectionState.Closed)
                {
                    connection.Dispose();
                }
            }
        }

        public override DataTable GetRawTablesSchema(
            bool includeColumns,
            DbConnection connection,
            bool disposeConnectionAfterExecute)
        {
            try
            {
                if (connection == null)
                {
                    connection = new SqlConnection(_connectionString);
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return connection.GetSchema("Tables");
            }
            finally
            {
                if (disposeConnectionAfterExecute &&
                    connection != null &&
                    connection.State != ConnectionState.Closed)
                {
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/library/ms254969.aspx
        /// http://www.devart.com/dotconnect/salesforce/docs/Metadata-GetSchema.html
        /// </summary>
        /// <param name="includeColumns"></param>
        public override void PopulateTablesFromSchema(
            bool includeColumns, 
            DbConnection connection, 
            bool disposeConnectionAfterExecute)
        {
            ///TODO Look at using SQL MSO (Server Management Objects) http://msdn.microsoft.com/en-us/magazine/cc163409.aspx
            try
            {
                if (connection == null)
                {
                    connection = new SqlConnection(_connectionString);
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                DataTable schema = connection.GetSchema("Tables");
                _tables.Clear();
                foreach (DataRow row in schema.Rows)
                {
                    SqlDatabaseTable<object> table = new SqlDatabaseTable<object>(row, _connectionString);
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
                    _tables.ToList().ForEach(t => t.PopulateColumnsFromSchema(connection, false));
                }
                PopulateChildrenTables();
            }
            finally
            {
                if (disposeConnectionAfterExecute &&
                    connection != null &&
                    connection.State != ConnectionState.Closed)
                {
                    connection.Dispose();
                }
            }
        }

        private void PopulateChildrenTables()
        {
            foreach (DatabaseTable pkTable in _tables)
            {
                foreach (DatabaseTable fkTable in _tables) //Find children tables i.e. tables that have foreign keys mapped this table's primary keys'.
                {
                    EntityCache<string, ForeignKeyInfo> mappedForeignKeys = new EntityCache<string, ForeignKeyInfo>();
                    fkTable.GetForeignKeyColumns().Where(c => c.ParentTableName == pkTable.TableName).ToList().ForEach(fk => mappedForeignKeys.Add(fk.ColumnName, new ForeignKeyInfo()
                    {
                        ChildTableName = fkTable.TableName,
                        ChildTableForeignKeyName = fk.ColumnName,
                        ParentTableName = fk.ParentTableName,
                        ParentTablePrimaryKeyName = fk.ParentTablePrimaryKeyName,
                        ConstraintName = fk.ConstraintName

                    }));
                    if (mappedForeignKeys.Count > 0) //If there are any foreign keys mapped to parent table's name.
                    {
                        pkTable.ChildrenTables.Add(fkTable.TableName, mappedForeignKeys);
                    }
                }
            }
        }

        public override List<DatabaseTableKeyColumns> GetTableKeyColumns()
        {
            List<DatabaseTableKeyColumns> result = new List<DatabaseTableKeyColumns>();
            foreach (SqlDatabaseTable t in _tables)
            {
                DatabaseTableKeyColumns tableKeyColumns = new DatabaseTableKeyColumns(t.TableName);
                foreach (SqlDatabaseTableColumn c in t.Columns)
                {
                    if (c.IsKey)
                    {
                        tableKeyColumns.KeyNames.Add(c.ColumnName);
                    }
                }
                result.Add(tableKeyColumns);
            }
            return result;
        }

        public override List<DatabaseTableForeignKeyColumns> GetTableForeignKeyColumns()
        {
            List<DatabaseTableForeignKeyColumns> result = new List<DatabaseTableForeignKeyColumns>();
            foreach (SqlDatabaseTable t in _tables)
            {
                DatabaseTableForeignKeyColumns foreignKeyColumns = new DatabaseTableForeignKeyColumns(t.TableName);
                t.GetForeignKeyColumns().ToList().ForEach(c => foreignKeyColumns.ForeignKeys.Add(new ForeignKeyInfo()
                {
                    ChildTableName = t.TableName,
                    ChildTableForeignKeyName = c.ColumnName,
                    ParentTableName = c.ParentTableName,
                    ParentTablePrimaryKeyName = c.ParentTablePrimaryKeyName,
                    ConstraintName = c.ConstraintName
                }));
                result.Add(foreignKeyColumns);
            }
            return result;
        }

        #endregion //Methods
    }
}