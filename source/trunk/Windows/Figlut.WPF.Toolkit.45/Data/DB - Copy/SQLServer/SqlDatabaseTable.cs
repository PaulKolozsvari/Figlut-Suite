namespace Psion.Server.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Psion.Server.Toolkit.Data.DB.SQLQuery;
    using System.Data.SqlClient;
    using System.Data;

    #endregion //Using Directives

    public class SqlDatabaseTable<E> : DatabaseTable where E : class
    {
        #region Constructors

        public SqlDatabaseTable()
        {
        }

        public SqlDatabaseTable(string tableName) : base(tableName)
        {
        }

        public SqlDatabaseTable(string tableName, SqlConnection connection) : base(connection, tableName)
        {
        }

        public SqlDatabaseTable(SqlConnection connection, DataRow schemaRow) : base(connection , schemaRow)
        {
        }

        #endregion //Constructors

        #region Constants

        public const string TABLE_NAME_SCHEMA_ATTRIBUTE = "table_name";
        public const string SYS_DIAGRAMS_TABLE_NAME = "sysdiagrams";

        #endregion //Constants

        #region Properties

        public SqlConnection Connection
        {
            get { return (SqlConnection)_connection; }
            set { _connection = value; }
        }

        #endregion //Properties

        #region Methods

        public override void Initialize()
        {
            if (string.IsNullOrEmpty(_tableName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set in {1}.",
                    EntityReader<SqlDatabaseTable<E>>.GetPropertyName(p => p.TableName, false),
                    this.GetType().FullName));
            }
            if (_connection == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set in {1}.",
                    EntityReader<SqlDatabaseTable<E>>.GetPropertyName(p => p.Connection, false),
                    this.GetType().FullName));
            }
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                throw new Exception("Connection to database closed. Cannot connect.");
            }
        }

        public List<E> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive)
        {
            //TODO Implement code to query data by filter.
            throw new NotImplementedException();
        }

        public void Insert(E e)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sqlInsertCommand = new StringBuilder();
            sqlInsertCommand.AppendLine(string.Format("INSERT INTO [{0}]", typeof(E).Name));
            sqlInsertCommand.AppendLine("(");
            PropertyInfo[] entityProperties = typeof(E).GetProperties();
            if (entityProperties.Length < 1)
            {
                throw new ArgumentException(string.Format(
                    "{0} has no properties to update in the local database.",
                    typeof(E).FullName));
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
                sqlInsertCommand.Append(string.Format("@{0}", p.ParameterName));
            }
            sqlInsertCommand.AppendLine();
            sqlInsertCommand.AppendLine(")");
            using (SqlCommand command = new SqlCommand(sqlInsertCommand.ToString(), (SqlConnection)_connection))
            {
                parameters.ForEach(p => command.Parameters.Add(p));
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new Exception(string.Format(
                        "Could not insert {0} into database with value {1} on column {2}.",
                        typeof(E).Name));
                }
            }
            Initialize();
        }

        public void Delete(string columnName, object columnValue)
        {
            Type valueType = columnValue.GetType();
            SqlParameter param = new SqlParameter(string.Format("@{0}"), columnValue);
            string sqlDeleteCommand = string.Format("DELETE [{0}] WHERE [{1}] = @{1}", typeof(E).Name, columnName);
            using (SqlCommand command = new SqlCommand(sqlDeleteCommand, (SqlConnection)_connection))
            {
                command.Parameters.Add(param);
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new Exception(string.Format(
                        "Could not delete {0} with value {1} on column {2} although it exists in database. Contact administrator.",
                        typeof(E).Name,
                        columnValue,
                        columnName));
                }
            }
            Initialize();
        }

        public void Update(E e, string columnName)
        {
            object columnValue = EntityReader<E>.GetPropertyValue(columnName, e, true);
            Type valueType = columnValue.GetType();
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sqlUpdateCommand = new StringBuilder();
            sqlUpdateCommand.AppendLine(string.Format("UPDATE [{0}]", typeof(E).Name));
            sqlUpdateCommand.AppendLine("SET ");
            PropertyInfo[] entityProperties = typeof(E).GetProperties();
            if (entityProperties.Length < 1)
            {
                throw new ArgumentException(string.Format(
                    "{0} has no properties to update in the local database.",
                    typeof(E).FullName));
            }
            bool firstUpdateColumn = true;
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
                if (!firstUpdateColumn)
                {
                    sqlUpdateCommand.AppendLine(",");
                }
                object value = p.GetValue(e, null);
                if (value == null)
                {
                    value = DBNull.Value;
                }
                parameters.Add(new SqlParameter(string.Format("@{0}", p.Name), value));
                sqlUpdateCommand.Append(string.Format("[{0}] = @{0}", p.Name));
                firstUpdateColumn = false;
            }
            sqlUpdateCommand.AppendLine();
            sqlUpdateCommand.AppendLine(string.Format("WHERE [{0}] = @{0}", columnName));
            using (SqlCommand command = new SqlCommand(sqlUpdateCommand.ToString(), (SqlConnection)_connection))
            {
                parameters.ForEach(p => command.Parameters.Add(p));
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new Exception(string.Format(
                        "Could not update {0} on database with value {1} on column {2}.",
                        typeof(E).Name,
                        columnValue,
                        columnName));
                }
            }
            Initialize();
        }

        public override void PopulateFromSchema(DataRow schemaRow)
        {
            _tableName = schemaRow[TABLE_NAME_SCHEMA_ATTRIBUTE].ToString();
            _isSystemTable = _tableName.Contains(SYS_DIAGRAMS_TABLE_NAME);
        }

        public override void PopulateColumnsFromSchema()
        {
            _columns.Clear();
            DataTable schema = _connection.GetSchema("Columns", new string[] { null, null, _tableName, null });
            List<DatabaseTableColumn> tempColumns = new List<DatabaseTableColumn>();
            foreach (DataRow row in schema.Rows)
            {
                tempColumns.Add(new SqlDatabaseTableColumn(row));
            }
            tempColumns.OrderBy(c => c.OrdinalPosition).ToList().ForEach(c => _columns.Add(c.ColumnName, c));
        }

        #endregion //Methods
    }
}