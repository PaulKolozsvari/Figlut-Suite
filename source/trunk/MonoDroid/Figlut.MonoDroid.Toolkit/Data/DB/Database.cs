using Android.Content;

namespace Figlut.MonoDroid.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection.Emit;
    using Figlut.MonoDroid.Toolkit.Data.ORM;
    using System.Data.Common;
    using Figlut.MonoDroid.Toolkit.Data.DB.SQLQuery;
    using Figlut.MonoDroid.Toolkit.Data.DB.SQLServer;
    using System.Data;
    using System.IO;
    using Figlut.MonoDroid.Toolkit.Utilities;
    using Figlut.MonoDroid.Toolkit.Utilities.Logging;
	using System.Xml.Serialization;
	using Android.Database.Sqlite;

    #endregion //Using Directives

    [Serializable]
    public abstract class Database : IDisposable
    {
        #region Constructors

		public Database ()
        {
			_tables = new EntityCache<string, DatabaseTable> ();
            _name = this.GetType().Name;
        }

		public Database(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(string.Format(
                    "{0} not be null or empty when constructing {1}.",
                    EntityReader<Database>.GetPropertyName(p => p.Name, false),
                    this.GetType().FullName));
            }
			_tables = new EntityCache<string, DatabaseTable> ();
            _name = name;
        }

        #endregion //Constructors

        #region Fields

        protected string _name;
		protected string _connectionString;
        protected EntityCache<string, DatabaseTable> _tables;
        protected OrmAssembly _ormAssembly;

        #endregion //Fields

        #region Properties

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public EntityCache<string, DatabaseTable> Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }

        #endregion //Properties

        #region Methods

        public OrmAssembly GetOrmAssembly()
        {
            return _ormAssembly;
        }
        
        public override string ToString()
        {
            return _name;
        }

        public abstract List<DatabaseTableKeyColumns> GetTableKeyColumns();

        public abstract List<DatabaseTableForeignKeyColumns> GetTableForeignKeyColumns();

        public abstract void Dispose();

        public virtual void ClearTables()
        {
            _tables.Clear();
        }

		public List<DatabaseTable> GetTablesMentionedInQuery(SqlQuery query)
		{
			List<DatabaseTable> result = new List<DatabaseTable>();
			foreach (string t in query.TableNamesInQuery)
			{
				DatabaseTable table = _tables[t];
				if (table == null)
				{
					throw new NullReferenceException(string.Format(
						"Could not find table {0} mentioned in {1} inside {2}.",
						t,
						query.GetType().FullName,
						this.GetType().FullName));
				}
				result.Add(table);
			}
			return result;
		}

        public virtual DatabaseTable GetDatabaseTable(string tableName)
        {
            if (!_tables.Exists(tableName))
            {
                return null;
            }
            return (DatabaseTable)_tables[tableName];
        }

        public void CreateOrmAssembly(
            bool saveOrmAssembly, 
            string ormAssemblyOutputDirectory)
        {
            string assemblyFileName = string.Format("{0}.dll", this.Name);
            _ormAssembly = new OrmAssembly(
                this.Name,
                assemblyFileName,
                AssemblyBuilderAccess.RunAndSave);
            foreach (DatabaseTable table in _tables)
            {
                OrmType ormType = _ormAssembly.CreateOrmType(table.TableName, true);
                foreach (DatabaseTableColumn column in table.Columns)
                {
                    ormType.CreateOrmProperty(
                        column.ColumnName,
                        SqlTypeConverter.Instance.GetDotNetType(column.DataType, column.IsNullable));
                }
                table.MappedType = ormType.CreateType();
            }
            if (!saveOrmAssembly)
            {
                return;
            }
            _ormAssembly.Save(ormAssemblyOutputDirectory);
        }

        #endregion //Methods
    }
}