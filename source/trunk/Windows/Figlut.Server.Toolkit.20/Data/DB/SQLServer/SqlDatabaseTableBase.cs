namespace Figlut.Server.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using System.Data.SqlClient;
    using System.Data;
    using System.Data.Common;

    #endregion //Using Directives

    [Serializable]
    public class SqlDatabaseTable : DatabaseTable
    {
        #region Constructors

        public SqlDatabaseTable() : base()
        {
        }

        public SqlDatabaseTable(string tableName, string connectionString) 
            : base(tableName, connectionString)
        {
        }

        public SqlDatabaseTable(DataRow schemaRow)
            : base(schemaRow)
        {
        }

        public SqlDatabaseTable(DataRow schemaRow, string connectionString) 
            : base(schemaRow, connectionString)
        {
        }

        #endregion //Constructors

        #region Constants

        public const string TABLE_NAME_SCHEMA_ATTRIBUTE = "table_name";
        public const string SYS_DIAGRAMS_TABLE_NAME = "sysdiagrams";

        #endregion //Constants

        #region Methods

        public override List<object> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive,
            Type entityType)
        {
            //TODO Implement code to query data by filter.
            throw new NotImplementedException();
        }

        public override void Insert(object e, bool disposeConnectionAfterExecute)
        {
            Insert(e, disposeConnectionAfterExecute, null, null);
        }

        public void Insert(
            object e, 
            bool disposeConnectionAfterExecute,
            SqlConnection connection, 
            SqlTransaction transaction)
        {
            Type entityType = e.GetType();
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sqlInsertCommand = new StringBuilder();
            sqlInsertCommand.AppendLine(string.Format("INSERT INTO [{0}]", entityType.Name));
            sqlInsertCommand.AppendLine("(");
            PropertyInfo[] entityProperties = entityType.GetProperties();
            if (entityProperties.Length < 1)
            {
                throw new ArgumentException(string.Format(
                    "{0} has no properties to update in the local database.",
                    entityType.FullName));
            }
            bool firstInsertColumn = true;
            foreach (PropertyInfo p in entityProperties) //The columns to insert.
            {
                if ((p.PropertyType != typeof(string) &&
                    p.PropertyType != typeof(byte) &&
                    p.PropertyType != typeof(byte[])) &&
                    (p.PropertyType.IsClass ||
                    p.PropertyType.IsEnum ||
                    p.PropertyType.IsInterface ||
                    p.PropertyType.IsNotPublic ||
                    p.PropertyType.IsPointer))
                {
                    continue;
                }
                if (!firstInsertColumn)
                {
                    sqlInsertCommand.AppendLine(",");
                }
                object value = p.GetValue(e, null);
                if (value == null)
                {
                    value = DBNull.Value;
                }
                parameters.Add(new SqlParameter(string.Format("@{0}", p.Name), value));
                sqlInsertCommand.Append(string.Format("[{0}]", p.Name));
                firstInsertColumn = false;
            }
            sqlInsertCommand.AppendLine();
            sqlInsertCommand.AppendLine(")");
            sqlInsertCommand.AppendLine("VALUES");
            sqlInsertCommand.AppendLine("(");
            bool firstInsertValue = true;
            foreach (SqlParameter p in parameters)
            {
                if (!firstInsertValue)
                {
                    sqlInsertCommand.AppendLine(",");
                }
                sqlInsertCommand.Append(string.Format("{0}", p.ParameterName));
                firstInsertValue = false;
            }
            sqlInsertCommand.AppendLine();
            sqlInsertCommand.AppendLine(")");
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
                using (SqlCommand command = new SqlCommand(sqlInsertCommand.ToString(), connection))
                {
                    if (transaction != null)
                    {
                        command.Transaction = transaction;
                    }
                    parameters.ForEach(p => command.Parameters.Add(p));
                    if (command.ExecuteNonQuery() < 1)
                    {
                        throw new Exception(string.Format(
                            "Could not insert {0} into database with value {1} on column {2}.",
                            entityType.Name));
                    }
                }
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

        public override void Insert(List<object> entities, bool useTransaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction t = null;
                try
                {
                    connection.Open();
                    if (!useTransaction)
                    {
                        entities.ForEach(e => Insert(e, false, connection, null));
                        return;
                    }
                    using (t = connection.BeginTransaction())
                    {
                        entities.ForEach(e => Insert(e, false, connection, t));
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
        }

        public void Delete(
            object e, 
            string columnName, 
            bool disposeConnectionAfterExecute,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            Type entityType = e.GetType();
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            StringBuilder sqlDeleteCommand = new StringBuilder();
            sqlDeleteCommand.AppendLine(string.Format("DELETE FROM [{0}]", entityType.Name));

            StringBuilder whereClause = new StringBuilder();
            whereClause.AppendLine("WHERE ");
            bool firstUpdateColumn = true;
            if (string.IsNullOrEmpty(columnName)) //Look up the record to delete based on the key columns because no column name was specified.
            {
                foreach (PropertyInfo p in entityProperties)
                {
                    if ((p.PropertyType != typeof(string) &&
                        p.PropertyType != typeof(byte) &&
                        p.PropertyType != typeof(byte[])) &&
                        (p.PropertyType.IsClass ||
                        p.PropertyType.IsEnum ||
                        p.PropertyType.IsInterface ||
                        p.PropertyType.IsNotPublic ||
                        p.PropertyType.IsPointer))
                    {
                        continue;
                    }
                    SqlDatabaseTableColumn column = (SqlDatabaseTableColumn)_columns[p.Name];
                    if (!column.IsKey)
                    {
                        continue;
                    }
                    object value = p.GetValue(e, null);
                    if (value == null)
                    {
                        value = DBNull.Value;
                    }
                    SqlParameter parameter = new SqlParameter(string.Format("@{0}", p.Name), value) { SqlDbType = column.SqlDbType };
                    parameters.Add(parameter);
                    if (!firstUpdateColumn)
                    {
                        whereClause.AppendLine(",");
                    }
                    whereClause.Append(string.Format("[{0}] = @{0}", p.Name));
                    firstUpdateColumn = false;
                }
            }
            else //Look up the record to update based on the specified column name.
            {
                whereClause.Append(string.Format("[{0}] = @{0}", columnName));
                object value = EntityReader<object>.GetPropertyValue(columnName, e, true);
                parameters.Add(new SqlParameter(string.Format("@{0}", columnName), value));
            }
            sqlDeleteCommand.AppendLine(whereClause.ToString());
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
                using (SqlCommand command = new SqlCommand(sqlDeleteCommand.ToString(), connection))
                {
                    if (transaction != null)
                    {
                        command.Transaction = transaction;
                    }
                    parameters.ForEach(p => command.Parameters.Add(p));
                    if (command.ExecuteNonQuery() < 1)
                    {

                        if (string.IsNullOrEmpty(columnName))
                        {
                            throw new Exception(string.Format("Could not delete {0} on database against key columns.", entityType.Name));
                        }
                        else
                        {
                            object columnValue = EntityReader<object>.GetPropertyValue(columnName, e, true);
                            throw new Exception(string.Format("Could not delete {0} on database with value {1} on column {2}.", entityType.Name, columnValue, columnName));
                        }
                    }
                }
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

        public override void Delete(object e, string columnName, bool disposeConnectionAfterExecute)
        {
            Delete(e, columnName, disposeConnectionAfterExecute, null, null);
        }

        public override void Delete(List<object> entities, string columnName, bool useTransaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction t = null;
                try
                {
                    connection.Open();
                    if (!useTransaction)
                    {
                        entities.ForEach(e => Delete(e, columnName, false, connection, null));
                        return;
                    }
                    using (t = connection.BeginTransaction())
                    {
                        entities.ForEach(e => Delete(e, columnName, false, connection, t));
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
        }

        public override void Update(object e, string columnName, bool disposeConnectionAfterExecute)
        {
            Update(e, columnName, disposeConnectionAfterExecute, null, null);
        }

        public void Update(
            object e, 
            string columnName, 
            bool disposeConnectionAfterExecute, 
            SqlConnection connection, 
            SqlTransaction transaction)
        {
            Type entityType = e.GetType();
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sqlUpdateCommand = new StringBuilder();
            sqlUpdateCommand.AppendLine(string.Format("UPDATE [{0}]", entityType.Name));
            sqlUpdateCommand.AppendLine("SET ");
            PropertyInfo[] entityProperties = entityType.GetProperties();
            if (entityProperties.Length < 1)
            {
                throw new ArgumentException(string.Format(
                    "{0} has no properties to update in the local database.",
                    entityType.FullName));
            }
            bool firstUpdateColumn = true;
            StringBuilder whereClause = new StringBuilder();
            whereClause.AppendLine("WHERE ");
            foreach (PropertyInfo p in entityProperties)
            {
                if ((p.PropertyType != typeof(string) &&
                    p.PropertyType != typeof(byte) &&
                    p.PropertyType != typeof(byte[])) &&
                    (p.PropertyType.IsClass ||
                    p.PropertyType.IsEnum ||
                    p.PropertyType.IsInterface ||
                    p.PropertyType.IsNotPublic ||
                    p.PropertyType.IsPointer))
                {
                    continue;
                }
                SqlDatabaseTableColumn column = (SqlDatabaseTableColumn)_columns[p.Name];
                if (!firstUpdateColumn)
                {
                    sqlUpdateCommand.AppendLine(",");
                }
                object value = p.GetValue(e, null);
                if (value == null)
                {
                    value = DBNull.Value;
                }
                SqlParameter parameter = new SqlParameter(string.Format("@{0}", p.Name), value) { SqlDbType = column.SqlDbType };
                parameters.Add(parameter);
                sqlUpdateCommand.Append(string.Format("[{0}] = @{0}", p.Name));
                if (string.IsNullOrEmpty(columnName)) //Look up the record to update based on the key columns because no column name was specified.
                {
                    if (column.IsKey)
                    {
                        if (!firstUpdateColumn)
                        {
                            whereClause.AppendLine(",");
                        }
                        whereClause.Append(string.Format("[{0}] = @{0}", p.Name));
                    }
                }
                firstUpdateColumn = false;
            }
            sqlUpdateCommand.AppendLine();
            if (!string.IsNullOrEmpty(columnName)) //Look up the record to update based on the specified column name.
            {
                whereClause.Append(string.Format("[{0}] = @{0}", columnName));
            }
            sqlUpdateCommand.Append(whereClause);
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
                string commandText = sqlUpdateCommand.ToString();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    if (transaction != null)
                    {
                        command.Transaction = transaction;
                    }
                    parameters.ForEach(p => command.Parameters.Add(p));
                    if (command.ExecuteNonQuery() < 1)
                    {
                        if (string.IsNullOrEmpty(columnName))
                        {
                            throw new Exception(string.Format("Could not update {0} on database against key columns.", entityType.Name));
                        }
                        else
                        {
                            object columnValue = EntityReader<object>.GetPropertyValue(columnName, e, true);
                            throw new Exception(string.Format("Could not update {0} on database with value {1} on column {2}.", entityType.Name, columnValue, columnName));
                        }
                    }
                }
            }
            finally
            {
                if (disposeConnectionAfterExecute &&
                    connection != null &
                    connection.State != ConnectionState.Closed)
                {
                    connection.Dispose();
                }
            }
        }

        //public void Update(object e, string columnName, bool disposeConnectionAfterExecute, SqlConnection connection, SqlTransaction transaction)
        //{
        //    Type entityType = e.GetType();
        //    object columnValue = EntityReader<object>.GetPropertyValue(columnName, e, true);
        //    Type valueType = columnValue.GetType();
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    StringBuilder sqlUpdateCommand = new StringBuilder();
        //    sqlUpdateCommand.AppendLine(string.Format("UPDATE [{0}]", entityType.Name));
        //    sqlUpdateCommand.AppendLine("SET ");
        //    PropertyInfo[] entityProperties = entityType.GetProperties();
        //    if (entityProperties.Length < 1)
        //    {
        //        throw new ArgumentException(string.Format(
        //            "{0} has no properties to update in the local database.",
        //            entityType.FullName));
        //    }
        //    bool firstUpdateColumn = true;
        //    foreach (PropertyInfo p in entityProperties)
        //    {
        //        if ((p.PropertyType != typeof(string) &&
        //            p.PropertyType != typeof(byte) &&
        //            p.PropertyType != typeof(byte[])) &&
        //            (p.PropertyType.IsClass ||
        //            p.PropertyType.IsEnum ||
        //            p.PropertyType.IsInterface ||
        //            p.PropertyType.IsNotPublic ||
        //            p.PropertyType.IsPointer))
        //        {
        //            continue;
        //        }
        //        if (!firstUpdateColumn)
        //        {
        //            sqlUpdateCommand.AppendLine(",");
        //        }
        //        object value = p.GetValue(e, null);
        //        if (value == null)
        //        {
        //            value = DBNull.Value;
        //        }
        //        parameters.Add(new SqlParameter(string.Format("@{0}", p.Name), value));
        //        sqlUpdateCommand.Append(string.Format("[{0}] = @{0}", p.Name));
        //        firstUpdateColumn = false;
        //    }
        //    sqlUpdateCommand.AppendLine();
        //    sqlUpdateCommand.AppendLine(string.Format("WHERE [{0}] = @{0}", columnName));
        //    try
        //    {
        //        if (connection == null)
        //        {
        //            connection = new SqlConnection(_connectionString);
        //        }
        //        if (connection.State != ConnectionState.Open)
        //        {
        //            connection.Open();
        //        }
        //        using (SqlCommand command = new SqlCommand(sqlUpdateCommand.ToString(), connection))
        //        {
        //            if (transaction != null)
        //            {
        //                command.Transaction = transaction;
        //            }
        //            parameters.ForEach(p => command.Parameters.Add(p));
        //            if (command.ExecuteNonQuery() < 1)
        //            {
        //                throw new Exception(string.Format(
        //                    "Could not update {0} on database with value {1} on column {2}.",
        //                    entityType.Name,
        //                    columnValue,
        //                    columnName));
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        if (disposeConnectionAfterExecute &&
        //            connection != null &
        //            connection.State != ConnectionState.Closed)
        //        {
        //            connection.Dispose();
        //        }
        //    }
        //}

        public override void Update(List<object> entities, bool useTransaction)
        {
            Update(entities, null, false);
        }

        public override void Update(List<object> entities, string columnName, bool useTransaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction t = null;
                try
                {
                    connection.Open();
                    if (!useTransaction)
                    {
                        entities.ForEach(e => Update(e, columnName, false, connection, null));
                        return;
                    }
                    using (t = connection.BeginTransaction())
                    {
                        entities.ForEach(e => Update(e, columnName, false, connection, t));
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
        }

        public override void PopulateFromSchema(DataRow schemaRow)
        {
            _tableName = schemaRow[TABLE_NAME_SCHEMA_ATTRIBUTE].ToString();
            _isSystemTable = _tableName.Contains(SYS_DIAGRAMS_TABLE_NAME);
        }

        public override void PopulateColumnsFromSchema(DataTable columnsSchema)
        {
            _columns.Clear();
            List<DatabaseTableColumn> tempColumns = new List<DatabaseTableColumn>();
            foreach (DataRow row in columnsSchema.Rows)
            {
                tempColumns.Add(new SqlDatabaseTableColumn(row));
            }
            tempColumns.ForEach(c => _columns.Add(c.ColumnName, c));
        }

        public override void PopulateColumnsFromSchema(DbConnection connection, bool disposeConnectionAfterExecute)
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
                _columns.Clear();
                DataTable schema = connection.GetSchema("Columns", new string[] { null, null, _tableName, null });
                List<DatabaseTableColumn> tempColumns = new List<DatabaseTableColumn>();
                foreach (DataRow row in schema.Rows)
                {
                    tempColumns.Add(new SqlDatabaseTableColumn(row));
                }
                tempColumns.ForEach(c => _columns.Add(c.ColumnName, c));
                if (!(connection is SqlConnection))
                {
                    throw new InvalidCastException(string.Format(
                        "Expected a {0}, and received a {1}.",
                        typeof(SqlConnection).FullName,
                        connection.GetType().FullName));
                }
                List<string> keyColumns = GetKeyColummnNames(connection, false);
                if (keyColumns.Count < 1)
                {
                    throw new Exception(string.Format("Table {0} has no key columns.", _tableName));
                }
                foreach (string k in keyColumns)
                {
                    DatabaseTableColumn c = _columns[k];
                    if (c == null)
                    {
                        throw new NullReferenceException(string.Format(
                            "Could not find key column {0} on table {1}.",
                            k,
                            _tableName));
                    }
                    c.IsKey = true;
                }
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

        public override List<string> GetKeyColummnNames(DbConnection connection, bool disposeConnectionAfterExecute)
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
                List<string> result = new List<string>();
                using (SqlCommand command = new SqlCommand("sp_pkeys", (SqlConnection)connection)) //Get the primary key columns.
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@table_name", _tableName) { Direction = ParameterDirection.Input });
                    //command.Parameters.Add(new SqlParameter("@table_owner", "dbo") { Direction = ParameterDirection.Input });
                    //command.Parameters.Add(new SqlParameter("@table_qualifier", "DigisticsStoreDelivery") { Direction = ParameterDirection.Input });
                    //using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string keyColumnName = reader["COLUMN_NAME"].ToString();
                            result.Add(keyColumnName);
                        }
                    }
                }
                return result;
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

        public override DataTable GetRawColumnsSchema(DbConnection connection, bool disposeConnectionAfterExecute)
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
                return connection.GetSchema("Columns", new string[] { null, null, _tableName, null });
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

        #endregion //Methods
    }
}