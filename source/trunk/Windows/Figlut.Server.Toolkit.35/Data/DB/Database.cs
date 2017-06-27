namespace Figlut.Server.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection.Emit;
    using Figlut.Server.Toolkit.Data.ORM;
    using System.Data.Common;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using System.Data.SqlClient;
    using System.Data;

    #endregion //Using Directives

    [Serializable]
    public abstract class Database : IDisposable
    {
        #region Constructors

        public Database()
        {
            _tables = new EntityCache<string, DatabaseTable>();
            _name = this.GetType().Name;
        }

        public Database(
            string connectionString,
            bool populateTablesFromSchema,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            bool overrideNameWithDatabaseNameFromSchema)
        {
            _name = this.GetType().Name;
            Initialize( 
                connectionString,
                populateTablesFromSchema,
                createOrmAssembly, 
                saveOrmAssembly,
                overrideNameWithDatabaseNameFromSchema);
        }

        public Database(
            string name, 
            string connectionString,
            bool populateTablesFromSchema,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            bool overrideNameWithDatabaseNameFromSchema)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(string.Format(
                    "{0} not be null or empty when constructing {1}.",
                    EntityReader<Database>.GetPropertyName(p => p.Name, false),
                    this.GetType().FullName));
            }
            _name = name;
            Initialize(
                connectionString,
                populateTablesFromSchema,
                createOrmAssembly,
                saveOrmAssembly, 
                overrideNameWithDatabaseNameFromSchema);
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

        public virtual string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public EntityCache<string, DatabaseTable> Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }

        public virtual OrmAssembly OrmAssembly
        {
            get { return _ormAssembly; }
            set { _ormAssembly = value; }
        }

        #endregion //Properties

        #region Methods

        public override string ToString()
        {
            return _name;
        }

        public void Initialize(
            string connectionString,
            bool populateTablesFromSchema,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            bool overrideNameWithDatabaseNameFromSchema)
        {
            _tables = new EntityCache<string, DatabaseTable>();
            _connectionString = connectionString;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                if (overrideNameWithDatabaseNameFromSchema)
                {
                    _name = GetDatabaseNameFromSchema(connection, false);
                }
                if (populateTablesFromSchema)
                {
                    PopulateTablesFromSchema(true, connection, false);
                }
                if (createOrmAssembly)
                {
                    CreateOrmAssembly(saveOrmAssembly);
                }
            }
        }

        public abstract string GetDatabaseNameFromSchema(DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract DataTable GetRawTablesSchema(bool includeColumns, DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract void PopulateTablesFromSchema(bool includeColumns, DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract List<DatabaseTableKeyColumns> GetTableKeyColumns();

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

        public virtual DatabaseTable GetDatabaseTable(Type entityType)
        {
            return GetDatabaseTable(entityType.Name);
        }

        public virtual DatabaseTable GetDatabaseTable(string tableName)
        {
            if (!_tables.Exists(tableName))
            {
                return null;
            }
            return (DatabaseTable)_tables[tableName];
        }

        public void CreateOrmAssembly(bool saveOrmAssembly)
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
            if (saveOrmAssembly)
            {
                _ormAssembly.Save();
            }
        }

        #endregion //Methods
    }
}