namespace Figlut.Mobile.Toolkit.Data.DB.SQLCE
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Data.SqlServerCe;
    using Microsoft.Synchronization.Data;
    using System.Reflection;
    using Figlut.Mobile.Toolkit.Data.DB.SQLQuery;

    #endregion //Using Directives

    public class SqlCeDatabaseTable<E> : MobileDatabaseTable where E : class
    {
        #region Constructors

        public SqlCeDatabaseTable()
        {
        }

        public SqlCeDatabaseTable(
            string tableName,
            SqlCeConnection connection)
        {
            _tableName = tableName;
            _connection = connection;
        }

        public SqlCeDatabaseTable(
            string tableName,
            SyncDirection syncDirection)
        {
            _tableName = tableName;
            _syncDirection = syncDirection;
        }

        public SqlCeDatabaseTable(
            string tableName,
            SqlCeConnection connection,
            SyncDirection syncDirection)
        {
            _tableName = tableName;
            _connection = connection;
            _syncDirection = syncDirection;
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected SqlCeResultSet _resultSet;
        protected SqlCeConnection _connection;
        protected SyncDirection _syncDirection;

        #endregion //Fields

        #region Properties

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

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
                    EntityReader<SqlCeDatabaseTable<E>>.GetPropertyName(p => p.TableName, false),
                    this.GetType().FullName));
            }
            if (_connection == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set in {1}.",
                    EntityReader<SqlCeDatabaseTable<E>>.GetPropertyName(p => p.Connection, false),
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

        public List<E> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive)
        {
            VerifyTableInitialized();
            List<E> query = null;
            if (caseSensitive)
            {
                string columnValueString = columnValue.ToString();
                if (!useLikeFilter)
                {
                    query = (from SqlCeUpdatableRecord record in _resultSet
                             where (record[columnName] == DBNull.Value && columnValue == null) ||
                                   (record[columnName] == columnValue) ||
                                   (record[columnName].ToString() == columnValueString)
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity<E>(record)).ToList();
                }
                else
                {
                    query = (from SqlCeUpdatableRecord record in _resultSet
                             where (record[columnName] == DBNull.Value && columnValue == null) ||
                                   (record[columnName] == columnValue) ||
                                   (record[columnName].ToString().Contains(columnValueString))
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity<E>(record)).ToList();
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
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity<E>(record)).ToList();
                }
                else
                {
                    query = (from SqlCeUpdatableRecord record in _resultSet
                             where (record[columnName] == DBNull.Value && columnValue == null) ||
                                    (record[columnName] == columnValue) ||
                                    (record[columnName].ToString().ToLower().Contains(columnValueStringLower))
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity<E>(record)).ToList();
                }
            }
            Initialize();
            return query;
        }

        public void Insert(E e)
        {
            VerifyTableInitialized();
            SqlCeUpdatableRecord record = _resultSet.CreateRecord();
            foreach (PropertyInfo p in typeof(E).GetProperties())
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

        public void Delete(string columnName, object columnValue)
        {
            VerifyTableInitialized();
            Type valueType = columnValue.GetType();
            List<E> query = (from SqlCeUpdatableRecord record in _resultSet
                             where (DataHelper.ChangeType(record[columnName], valueType).Equals(columnValue))
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity<E>(record)).ToList();
            if (query.Count < 1)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with value {1} on column {2} to be deleted.",
                    typeof(E).FullName,
                    columnValue,
                    columnName));
            }
            SqlCeParameter param = new SqlCeParameter(string.Format("@{0}", columnName), columnValue);
            string sqlDeleteCommand = string.Format("DELETE [{0}] WHERE [{1}] = @{1}", typeof(E).Name, columnName);
            using (SqlCeCommand command = new SqlCeCommand(sqlDeleteCommand, _connection))
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

        //public void Update(E e, string columnName, object columnValue)
        //{
        //    VerifyTableInitialized();
        //    Initialize();
        //    Type valueType = columnValue.GetType();
        //    List<E> query = (from SqlCeUpdatableRecord record in _resultSet
        //                     where (DataHelper.ChangeType(record[columnName], valueType).Equals(columnValue))
        //                     select DataHelper.ParseSqlCeUpdatableRecordToEntity<E>(record)).ToList();
        //    if (query.Count < 1)
        //    {
        //        throw new NullReferenceException(string.Format(
        //            "Could not find {0} with value {1} on column {2} to be deleted.",
        //            typeof(E).FullName,
        //            columnValue,
        //            columnName));
        //    }
        //    List<SqlCeParameter> parameters = new List<SqlCeParameter>();
        //    StringBuilder sqlUpdateCommand = new StringBuilder();
        //    sqlUpdateCommand.AppendLine(string.Format("UPDATE [{0}]", typeof(E).Name));
        //    sqlUpdateCommand.AppendLine("SET ");
        //    PropertyInfo[] entityProperties = typeof(E).GetProperties();
        //    if (entityProperties.Length < 1)
        //    {
        //        throw new ArgumentException(string.Format(
        //            "{0} has no properties to update in the local database.",
        //            typeof(E).FullName));
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
        //        parameters.Add(new SqlCeParameter(string.Format("@{0}", p.Name), value));
        //        sqlUpdateCommand.Append(string.Format("[{0}] = @{0}", p.Name));
        //        firstUpdateColumn = false;
        //    }
        //    sqlUpdateCommand.AppendLine();
        //    sqlUpdateCommand.AppendLine(string.Format("WHERE [{0}] = @{0}", columnName));
        //    using (SqlCeCommand command = new SqlCeCommand(sqlUpdateCommand.ToString(), _connection))
        //    {
        //        parameters.ForEach(p => command.Parameters.Add(p));
        //        if (command.ExecuteNonQuery() < 1)
        //        {
        //            throw new Exception(string.Format(
        //                "Could not update {0} on device database with value {1} on column {2}.",
        //                typeof(E).Name,
        //                columnValue,
        //                columnName));
        //        }
        //    }
        //    Initialize();
        //}

        public void Update(E e, string columnName)
        {
            VerifyTableInitialized();
            object columnValue = EntityReader<E>.GetPropertyValue(columnName, e, true);
            Type valueType = columnValue.GetType();
            List<E> query = (from SqlCeUpdatableRecord record in _resultSet
                             where (DataHelper.ChangeType(record[columnName], valueType).Equals(columnValue))
                             select DataHelper.ParseSqlCeUpdatableRecordToEntity<E>(record)).ToList();
            if (query.Count < 1)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with value {1} on column {2} to be deleted.",
                    typeof(E).FullName,
                    columnValue,
                    columnName));
            }
            List<SqlCeParameter> parameters = new List<SqlCeParameter>();
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
                        typeof(E).Name,
                        columnValue,
                        columnName));
                }
            }
            Initialize();
        }

        #endregion //Methods
    }
}