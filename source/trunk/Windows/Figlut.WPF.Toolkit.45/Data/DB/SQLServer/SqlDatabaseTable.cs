namespace Figlut.Server.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using System.Data.SqlClient;
    using System.Data;
    using System.Data.Common;

    #endregion //Using Directives

    [Serializable]
    public class SqlDatabaseTable<E> : SqlDatabaseTable where E : class
    {
        #region Constructors

        public SqlDatabaseTable() : base()
        {
        }

        public SqlDatabaseTable(string tableName, string connectionString) 
            : base(tableName, connectionString)
        {
        }

        public SqlDatabaseTable(DataRow schemaRow, string connectionString) 
            : base(schemaRow, connectionString)
        {
        }

        #endregion //Constructors

        #region Methods

        public List<E> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive)
        {
            List<object> objects = base.Query(columnName, columnValue, useLikeFilter, caseSensitive, typeof(E));
            List<E> result = new List<E>();
            objects.ForEach(o => result.Add((E)o));
            return result;
        }

        public void Insert(E e, bool disposeConnectionAfterExecute, SqlConnection connection, SqlTransaction transaction)
        {
            base.Insert(e, disposeConnectionAfterExecute, connection, transaction);
        }

        public void Insert<E>(List<E> entities, bool useTransaction) where E : class
        {
            List<object> objects = new List<object>();
            entities.ForEach(e => objects.Add(e));
            base.Insert(objects, useTransaction);
        }

        public void Delete<E>(E e, string columnName, bool disposeConnectionAfterExecute, SqlConnection connection, SqlTransaction transaction) where E : class
        {
            Delete(e, columnName, disposeConnectionAfterExecute, connection, transaction);
        }

        public void Delete<E>(List<E> entities, string columnName, bool useTransaction) where E : class
        {
            List<object> objects = new List<object>();
            entities.ForEach(e => objects.Add(e));
            base.Delete(objects, columnName, useTransaction);
        }

        public void Update(E e, string columnName, bool disposeConnectionAfterExecute, SqlConnection connection, SqlTransaction transaction)
        {
            base.Update(e, columnName, disposeConnectionAfterExecute, connection, transaction);
        }

        public void Update<E>(List<E> entities, string columnName, bool useTransaction) where E : class
        {
            List<object> objects = new List<object>();
            entities.ForEach(e => objects.Add(e));
            base.Update(objects, columnName, useTransaction);
        }

        #endregion //Methods
    }
}