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
using System.Xml.Serialization;

    #endregion //Using Directives

    public class SqlCeDatabaseTable<E> : SqlCeDatabaseTable where E : class
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

        #region Properties

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

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

        public List<E> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive)
        {
            Type entityType = typeof(E);
            List<object> query = base.Query(columnName, columnValue, useLikeFilter, caseSensitive, entityType);
            return (List<E>)DataHelper.ConvertObjectsToTypedList(entityType, query);
        }

        public void Insert(E e)
        {
            base.Insert(e);
        }

        public void Insert(List<E> entities, bool useTransaction)
        {
            List<object> objects = new List<object>();
            entities.ForEach(p => objects.Add(p));
            base.Insert(objects);
        }

        public void Delete(E e, string columnName)
        {
            base.Delete(e, columnName);
        }

        public void Delete(List<E> entities, string columnName)
        {
            List<object> objects = new List<object>();
            entities.ForEach(p => objects.Add(p));
            base.Delete(objects, columnName);
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
            base.Update(e, columnName);
        }

        public void Update(List<E> entities, string columnName)
        {
            List<object> objects = new List<object>();
            entities.ForEach(p => objects.Add(p));
            base.Update(objects, columnName);
        }

        #endregion //Methods
    }
}