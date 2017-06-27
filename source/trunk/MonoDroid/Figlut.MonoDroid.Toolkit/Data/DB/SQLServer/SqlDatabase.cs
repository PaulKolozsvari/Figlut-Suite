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
    public class SqlDatabase : Database
    {
        #region Constructors

        public SqlDatabase()
        {
        }

        public SqlDatabase(string name)
            : base(name)
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
			if (_tables.Exists(table.TableName)) {
				throw new Exception(string.Format(
					"{0} with name {1} already added to {2}.",
					table.GetType().FullName,
					table.TableName,
					this.GetType().FullName));
			}
            _tables.Add(table.TableName, table);
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
			SqlDatabaseTable<E> table = new SqlDatabaseTable<E> (tableName);
//            _tables.Add(table.TableName, table);
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