using SQLite;
using Android.Content;

namespace Figlut.MonoDroid.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Data;
    using System.Reflection;
    using Figlut.MonoDroid.Toolkit.Data.DB.SQLQuery;
    using Figlut.MonoDroid.Toolkit.Utilities.Logging;
    using System.Reflection.Emit;
    using Figlut.MonoDroid.Toolkit.Data.ORM;
    using System.Data.Common;

    #endregion //Using Directives

    [Serializable]
    public class SqliteDatabase : Database
    {
        #region Constructors

        public SqliteDatabase()
        {
        }

		public SqliteDatabase(string name, int version)
            : base(name)
        {
			_version = version;
        }

        #endregion //Constructors

        #region Constants

        public const string DATABASE_NAME_SCHEMA_ATTRIBUTE = "database_name";

        #endregion //Constants

		#region Fields

		private SqliteDataManagerHelper _dbHelper;
		private int _version;
		private Dictionary<string, string> _createTableScripts;

		#endregion //Fields

		#region Properties

		public SqliteDataManagerHelper Helper
		{
			get{ return _dbHelper; }
		}

		public int Version
		{
			get{ return _version; }
		}

		#endregion //Properties

        #region Methods

		public SqliteDatabaseTable<E> GetSqlDatabaseTable<E>() where E : class, new()
        {
            return GetSqlDatabaseTable<E>(typeof(E).Name);
        }

		public SqliteDatabaseTable<E> GetSqlDatabaseTable<E>(string tableName) where E : class, new()
        {
            if (!_tables.Exists(tableName))
            {
                return null;
            }
            SqliteDatabaseTable<E> result = _tables[tableName] as SqliteDatabaseTable<E>;
            if (result == null)
            {
                throw new InvalidCastException(string.Format(
                    "Unexpected table type in {0}. Could not type cast {1} to a {2}.",
                    this.GetType().FullName,
                    typeof(Database).FullName,
                    typeof(SqliteDatabaseTable<E>).FullName));
            }
            return result;
        }

        public void AddTable(DatabaseTable table)
        {
			if (_tables.Exists(table.TableName)) {
				throw new Exception(string.Format(
					"{0} with name {1} already added to {2}.",
					table.GetType().FullName,
					table.TableName,
					this.GetType().FullName));
			}
            _tables.Add(table.TableName, table);
        }

		public SqliteDatabaseTable<E> AddTable<E>() where E : class, new()
        {
            return AddTable<E>(typeof(E).Name);
        }

		public SqliteDatabaseTable<E> AddTable<E>(string tableName) where E : class, new()
        {
            if (_tables.Exists(tableName))
            {
                throw new Exception(string.Format(
                    "{0} with name {1} already added to {2}.",
                    typeof(SqliteDatabaseTable<E>).FullName,
                    tableName,
                    this.GetType().FullName));
            }
			SqliteDatabaseTable<E> table = new SqliteDatabaseTable<E> (tableName);
//            _tables.Add(table.TableName, table);
			table.DbHelper = _dbHelper;
			_tables.Add(table);
			return table;
        }

        public override void Dispose()
        {
            if (!string.IsNullOrEmpty(_ormAssembly.AssemblyFilePath) && 
                File.Exists(_ormAssembly.AssemblyFilePath))
            {
                File.Delete(_ormAssembly.AssemblyFilePath); //Delete the ORM assembly from the output directory.
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
            foreach (SqliteDatabaseTable t in _tables)
            {
                DatabaseTableKeyColumns tableKeyColumns = new DatabaseTableKeyColumns(t.TableName);
                foreach (SqliteDatabaseTableColumn c in t.Columns)
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
            foreach (SqliteDatabaseTable t in _tables)
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

		public void CreateDatabase()
		{
			using (SQLiteConnection connection = new SQLiteConnection(_dbHelper.WritableDatabase.Path))
			{
				/*No need to do anything here: when creating a WritableDatabase connection the 
				 * SqliteDataManagerHelper.onCreate method
				 *is called which runs the createTableScripts*/
			}
		}

		public SQLiteConnection GetConnection()
		{
			return new SQLiteConnection (_dbHelper.WritableDatabase.Path);
		}

		public List<E> Query<E>(SqlQuery query) where E : class
		{
			List<E> result = null;
			using (SQLiteConnection connection = new SQLiteConnection(_dbHelper.ReadableDatabase.Path))
			{
				SQLiteCommand cmd = connection.CreateCommand (query.SqlQuerySring);
				result = cmd.ExecuteQuery<E>();
			}
			return result;
		}

		public List<E> Query<E>() where E : new()
		{
			List<E> result = null;
			using (SQLiteConnection connection = new SQLiteConnection (_dbHelper.ReadableDatabase.Path))
			{
				result = connection.Table<E> ().ToList();
			}
			return result;
		}

		public void SetContext(Context context)
		{
			if (_createTableScripts == null) 
			{
//				_sqlCreateDatabaseScript = @"
//                        CREATE TABLE IF NOT EXISTS Customer (
//                            Id              INTEGER PRIMARY KEY AUTOINCREMENT,
//                            FirstName       TEXT NOT NULL,
//                            LastName        TEXT NOT NULL )";
				_createTableScripts = new Dictionary<string, string> ();
				foreach (SqliteDatabaseTable table in _tables) 
				{
					_createTableScripts.Add (table.TableName, table.GetSqlCreateTableScript ());
				}
			}
			_dbHelper = new SqliteDataManagerHelper (context, _name, _version, _createTableScripts);
			foreach (SqliteDatabaseTable table in Tables) 
			{
				table.DbHelper = _dbHelper;
			}
		}

        #endregion //Methods
    }
}