namespace Figlut.MonoDroid.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Figlut.MonoDroid.Toolkit.Data.DB.SQLQuery;
    using System.Data;
    using System.Data.Common;
	using SQLite;

    #endregion //Using Directives

    [Serializable]
	public class SqliteDatabaseTable<E> : SqliteDatabaseTable where E : class, new()
    {
        #region Constructors

        public SqliteDatabaseTable() : base()
        {
        }

        public SqliteDatabaseTable(string tableName) 
            : base(tableName)
        {
        }

        public SqliteDatabaseTable(DataRow schemaRow) 
            : base(schemaRow)
        {
        }

        #endregion //Constructors

        #region Methods

		public void AddColumnsByEntityType()
		{
			base.AddColumnsByEntityType (typeof(E));
		}

		public void Insert(E e, SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.WritableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				connection.Insert(e);
			}
			finally 
			{
				if (connection != null) 
				{
					if (beginNewTransaction) 
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
		}

		public void Insert(List<E> entities, SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.WritableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				entities.ForEach(e => connection.Insert(e));
			}
			finally 
			{
				if (connection != null) 
				{
					if (beginNewTransaction) 
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
		}

		public void Delete(E e, SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.WritableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				connection.Delete(e);
			}
			finally 
			{
				if (connection != null) 
				{
					if (beginNewTransaction) 
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
		}
		public void Delete(List<E> entities, SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.WritableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				entities.ForEach(e => connection.Delete(e));
			}
			finally 
			{
				if (connection != null) 
				{
					if (beginNewTransaction) 
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
		}

		public void DeleteAll(SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.WritableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				connection.DeleteAll<E>();
			}
			finally 
			{
				if (connection != null) 
				{
					if (beginNewTransaction) 
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
		}

		public void Update(E e, SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.WritableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				connection.Update(e);
			}
			finally 
			{
				if (connection != null) 
				{
					if (beginNewTransaction) 
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
		}

		public void Update(List<E> entities, SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.WritableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				entities.ForEach(e => connection.Update(e));
			}
			finally 
			{
				if (connection != null) 
				{
					if (beginNewTransaction) 
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
		}

		public List<E> Query(SqlQuery query, SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			List<E> result = null;
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.ReadableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				SQLiteCommand cmd = connection.CreateCommand(query.SqlQuerySring);
				result = cmd.ExecuteQuery<E>();
			}
			finally
			{
				if (connection != null) 
				{
					if (beginNewTransaction) 
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
			return result;
		}

		public int QueryRecordCount(SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			int result = 0;
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.ReadableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				string sqlQuery = string.Format("SELECT COUNT(*) FROM {0}", typeof(E).Name);
				SQLiteCommand cmd = connection.CreateCommand(sqlQuery);
				result = cmd.ExecuteScalar<int>();
			}
			catch(Exception ex) 
			{
				if (connection != null) 
				{
					if (beginNewTransaction)
					{
						connection.Commit ();
					}
					if (disposeConnection) 
					{
						connection.Dispose ();
					}
				}
			}
			return result;
		}

		public List<E> Query(SQLiteConnection connection, bool beginNewTransaction, bool disposeConnection)
		{
			List<E> result = null;
			try
			{
				if(connection == null)
				{
					connection = new SQLiteConnection(DbHelper.ReadableDatabase.Path);
				}
				if(beginNewTransaction)
				{
					connection.BeginTransaction();
				}
				result = connection.Table<E> ().ToList();
			}
			finally
			{
				if(connection != null)
				{
					if(beginNewTransaction)
					{
						connection.Commit();
					}
					if(disposeConnection)
					{
						connection.Dispose();
					}
				}
			}
			return result;
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
                tempColumns.Add(new SqliteDatabaseTableColumn(row));
            }
            tempColumns.OrderBy(c => c.OrdinalPosition).ToList().ForEach(c => _columns.Add(c.ColumnName, c));
        }

        #endregion //Methods
    }
}