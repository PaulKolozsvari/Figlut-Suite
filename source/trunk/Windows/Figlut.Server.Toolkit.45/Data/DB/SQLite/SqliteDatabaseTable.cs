namespace Figlut.Server.Toolkit.Data.DB.SQLite
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using System.Data;
    using System.Data.Common;
	using SQLite;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using System.Data.SQLite;

    #endregion //Using Directives

    [Serializable]
	public class SqliteDatabaseTable<E> : SqliteDatabaseTable where E : class
    {
        #region Constructors

        public SqliteDatabaseTable() : base()
        {
        }

        public SqliteDatabaseTable(string tableName, string connectionString)
            : base(tableName, connectionString)
        {
        }

        public SqliteDatabaseTable(DataRow schemaRow, string connectionString)
            : base(schemaRow, connectionString)
        {
        }

        #endregion //Constructors

        #region Methods

        public int Insert(E e, bool disposeConnectionAfterExecute, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            return base.Insert(e, disposeConnectionAfterExecute, connection, transaction);
        }

        public void Insert(List<E> entities, bool useTransaction)
        {
            List<object> objects = new List<object>();
            entities.ForEach(e => objects.Add(e));
            base.Insert(objects, useTransaction);
        }

        public int Delete(E e, string columnName, bool disposeConnectionAfterExecute, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            return base.Delete(e, columnName, disposeConnectionAfterExecute, connection, transaction);
        }

        public void Delete(List<E> entities, string columnName, bool useTransaction)
        {
            List<object> objects = new List<object>();
            entities.ForEach(e => objects.Add(e));
            base.Delete(objects, columnName, useTransaction);
        }

        public int Update(E e, string columnName, bool disposeConnectionAfterExecute, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            return base.Update(e, columnName, disposeConnectionAfterExecute, connection, transaction);
        }

        public void Update(List<E> entities, string columnName, bool useTransaction)
        {
            List<object> objects = new List<object>();
            entities.ForEach(e => objects.Add(e));
            base.Update(objects, columnName, useTransaction);
        }

        #endregion //Methods
    }
}