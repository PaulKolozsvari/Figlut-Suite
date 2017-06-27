namespace Figlut.Mobile.Toolkit.Data.DB.SQLCE
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Mobile.Toolkit.Data.DB;
    using System.Data;
    using Microsoft.Synchronization.Data;
    using System.Data.SqlServerCe;
    using System.Reflection;
    using Figlut.Mobile.Toolkit.WM.Data.DB.SQLCE;
using System.Xml.Serialization;

    #endregion //Using Directives

    [Serializable]
    public class SqlCeDatabaseTable : MobileDatabaseTable
    {
        #region Constructors

        public SqlCeDatabaseTable() : base()
        {
        }

        public SqlCeDatabaseTable(string tableName, string connectionString) 
            : base(tableName, connectionString)
        {
        }

        public SqlCeDatabaseTable(DataRow schemaRow)
            : base(schemaRow)
        {
        }

        public SqlCeDatabaseTable(DataRow schemaRow, string connectionString) 
            : base(schemaRow, connectionString)
        {
        }

        #endregion //Constructors

        #region Constants

        public const string TABLE_NAME_SCHEMA_ATTRIBUTE = "table_name";
        public const string SYS_DIAGRAMS_TABLE_NAME = "sysdiagrams";

        #endregion //Constants

        #region Fields

        protected SqlCeResultSet _resultSet;
        protected SqlCeConnection _connection;
        protected SyncDirection _syncDirection;

        #endregion //Fields

        #region Properties

        [XmlIgnore]
        public SqlCeResultSet ResultSet
        {
            get { return _resultSet; }
            set { _resultSet = value; }
        }

        public SqlCeConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public SyncDirection SyncDirection
        {
            get { return _syncDirection; }
            set { _syncDirection = value; }
        }

        public bool IsInitialized
        {
            get { return _resultSet != null; }
        }

        #endregion //Properties

        #region Methods

        public override void Initialize()
        {
            if (string.IsNullOrEmpty(_tableName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set in {1}.",
                    EntityReader<SqlCeDatabaseTable>.GetPropertyName(p => p.TableName, false),
                    this.GetType().FullName));
            }
            if (_connection == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set in {1}.",
                    EntityReader<SqlCeDatabaseTable>.GetPropertyName(p => p.Connection, false),
                    this.GetType().FullName));
            }
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                throw new Exception("Connection to database closed. Cannot connect.");
            }
            using (SqlCeCommand command = new SqlCeCommand(_tableName, _connection))
            {
                command.CommandType = System.Data.CommandType.TableDirect;
                _resultSet = command.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
            }
        }

        public void VerifyTableInitialized()
        {
            if (_resultSet == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} with {1} of {2} not initialized.",
                    this.GetType().FullName,
                    EntityReader<MobileDatabaseTable>.GetPropertyName(p => p.TableName, false),
                    _tableName));
            }
        }

        public override List<object> Query(
            string columnName, 
            object columnValue, 
            bool useLikeFilter, 
            bool caseSensitive,
            Type entityType)
        {
            VerifyTableInitialized();
            List<object> query = null;
            if (caseSensitive)
            {
                string columnValueString = columnValue.ToString();
                if (!useLikeFilter)
                {
                    query = (from SqlCeUpdatableRecord record in _resultSet
                             where (record[columnName] == DBNull.Value && columnValue == null) ||
                                   (record[columnName] == columnValue) ||
                                   (record[columnName].ToString() == columnValueString)
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity(record, entityType)).ToList();
                }
                else
                {
                    query = (from SqlCeUpdatableRecord record in _resultSet
                             where (record[columnName] == DBNull.Value && columnValue == null) ||
                                   (record[columnName] == columnValue) ||
                                   (record[columnName].ToString().Contains(columnValueString))
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity(record, entityType)).ToList();
                }
            }
            else
            {
                string columnValueStringLower = columnValue.ToString().ToLower();
                if (!useLikeFilter)
                {
                    query = (from SqlCeUpdatableRecord record in _resultSet
                             where (record[columnName] == DBNull.Value && columnValue == null) ||
                                    (record[columnName] == columnValue) ||
                                    (record[columnName].ToString().ToLower() == columnValueStringLower)
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity(record, entityType)).ToList();
                }
                else
                {
                    query = (from SqlCeUpdatableRecord record in _resultSet
                             where (record[columnName] == DBNull.Value && columnValue == null) ||
                                    (record[columnName] == columnValue) ||
                                    (record[columnName].ToString().ToLower().Contains(columnValueStringLower))
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity(record, entityType)).ToList();
                }
            }
            Initialize();
            return query;
        }

        public override void Insert(object e)
        {
            VerifyTableInitialized();
            Type entityType = e.GetType();
            SqlCeUpdatableRecord record = _resultSet.CreateRecord();
            foreach (PropertyInfo p in entityType.GetProperties())
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
                object value = p.GetValue(e, null);
                if (value != null)
                {
                    record.SetValue(record.GetOrdinal(p.Name), value);
                }
                else
                {
                    record.SetValue(record.GetOrdinal(p.Name), DBNull.Value);
                }
            }
            _resultSet.Insert(record);
            Initialize();
        }

        public override void Insert(List<object> entities)
        {
            entities.ForEach(p => Insert(p));
        }

        public override void Delete(object e, string columnName)
        {
            VerifyTableInitialized();
            Type entityType = e.GetType();
            PropertyInfo columnValueProperty = entityType.GetProperty(columnName);
            object columnValue = columnValueProperty.GetValue(e, null);
            Type valueType = columnValue.GetType();
            List<object> query = (from SqlCeUpdatableRecord record in _resultSet
                             where (DataHelper.ChangeType(record[columnName], valueType).Equals(columnValue))
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity(record, entityType)).ToList();
            if (query.Count < 1)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with value {1} on column {2} to be deleted.",
                    entityType.FullName,
                    columnValue,
                    columnName));
            }
            SqlCeParameter param = new SqlCeParameter(string.Format("@{0}", columnName), columnValue);
            string sqlDeleteCommand = string.Format("DELETE [{0}] WHERE [{1}] = @{1}", entityType.Name, columnName);
            using (SqlCeCommand command = new SqlCeCommand(sqlDeleteCommand, _connection))
            {
                command.Parameters.Add(param);
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new Exception(string.Format(
                        "Could not delete {0} with value {1} on column {2} although it exists in database. Contact administrator.",
                        entityType.Name,
                        columnValue,
                        columnName));
                }
            }
            Initialize();
        }

        public override void Delete(List<object> entities, string columnName)
        {
            entities.ForEach(p => Delete(p, columnName));
        }

        public override void Update(object e, string columnName)
        {
            VerifyTableInitialized();

            Type entityType = e.GetType();
            PropertyInfo columnValueProperty = entityType.GetProperty(columnName);
            object columnValue = columnValueProperty.GetValue(e, null);
            Type valueType = columnValue.GetType();
            List<object> query = (from SqlCeUpdatableRecord record in _resultSet
                             where (DataHelper.ChangeType(record[columnName], valueType).Equals(columnValue))
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity(record, entityType)).ToList();
            if (query.Count < 1)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with value {1} on column {2} to be updated.",
                    entityType.FullName,
                    columnValue,
                    columnName));
            }
            List<SqlCeParameter> parameters = new List<SqlCeParameter>();
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
                parameters.Add(new SqlCeParameter(string.Format("@{0}", p.Name), value));
                sqlUpdateCommand.Append(string.Format("[{0}] = @{0}", p.Name));
                firstUpdateColumn = false;
            }
            sqlUpdateCommand.AppendLine();
            sqlUpdateCommand.AppendLine(string.Format("WHERE [{0}] = @{0}", columnName));
            using (SqlCeCommand command = new SqlCeCommand(sqlUpdateCommand.ToString(), _connection))
            {
                parameters.ForEach(p => command.Parameters.Add(p));
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new Exception(string.Format(
                        "Could not update {0} on device database with value {1} on column {2}.",
                        entityType.Name,
                        columnValue,
                        columnName));
                }
            }
            Initialize();
        }

        public override void Update(List<object> entities, string columnName)
        {
            entities.ForEach(p => Update(p, columnName));
        }

        public override void PopulateFromSchema(DataRow schemaRow)
        {
            _tableName = schemaRow[TABLE_NAME_SCHEMA_ATTRIBUTE].ToString();
            _isSystemTable = _tableName.Contains(SYS_DIAGRAMS_TABLE_NAME);
        }

        public override void PopulateColumnsFromSchema(DataTable columnsSchema)
        {
            _columns.Clear();
            List<MobileDatabaseTableColumn> tempColumns = new List<MobileDatabaseTableColumn>();
            foreach (DataRow row in columnsSchema.Rows)
            {
                tempColumns.Add(new SqlCeDatabaseTableColumn(row));
            }
            tempColumns.OrderBy(c => c.OrdinalPosition).ToList().ForEach(c => _columns.Add(c.ColumnName, c));
        }

        #endregion //Methods
    }
}
