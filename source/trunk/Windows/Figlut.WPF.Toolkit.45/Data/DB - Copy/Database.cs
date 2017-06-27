namespace Psion.Server.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection.Emit;
    using Psion.Server.Toolkit.Data.ORM;
    using System.Data.Common;
    using Psion.Server.Toolkit.Data.DB.SQLQuery;
    using Psion.Server.Toolkit.Data.DB.SQLServer;

    #endregion //Using Directives

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
            bool initializeDbConnection,
            bool openConnection,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            bool overrideNameWithDatabaseNameFromSchema)
        {
            _name = this.GetType().Name;
            Initialize( 
                connectionString, 
                initializeDbConnection, 
                openConnection, 
                createOrmAssembly, 
                saveOrmAssembly,
                overrideNameWithDatabaseNameFromSchema);
        }

        public Database(
            string name, 
            string connectionString,
            bool initializeDbConnection,
            bool openConnection,
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
                initializeDbConnection, 
                openConnection, 
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
        protected DbConnection _connection;

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

        public virtual DbConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public EntityCache<string, DatabaseTable> Tables
        {
            get { return _tables; }
        }

        public virtual OrmAssembly OrmAssembly
        {
            get { return _ormAssembly; }
        }

        #endregion //Properties

        #region Methods

        public void Initialize(
            string connectionString,
            bool initializeDbConnection,
            bool openConnection,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            bool overrideNameWithDatabaseNameFromSchema)
        {
            _tables = new EntityCache<string, DatabaseTable>();
            _connectionString = connectionString;
            if (initializeDbConnection) { InitliazeDbConnection(); }
            if (openConnection) 
            {
                if (!initializeDbConnection)
                {
                    throw new ArgumentException(string.Format(
                        "May not open connection on constructor of {0} if connection has not been initialized.",
                        this.GetType().FullName));
                }
                OpenConnection(); 
            }
            if (overrideNameWithDatabaseNameFromSchema)
            {
                if (!openConnection)
                {
                    throw new ArgumentException(string.Format(
                        "May not override {0} with database name from schema on constructor of {1} if connection has not been initialized and opened.",
                        EntityReader<Database>.GetPropertyName(p => p.Name, false),
                        this.GetType().FullName));
                }
                _name = GetDatabaseNameFromSchema();
            }
            if (createOrmAssembly)
            {
                if(!openConnection)
                {
                    throw new ArgumentException(string.Format(
                        "May not create ORM Assembly on constructor of {0} if connection has not been initialized and opened.",
                        this.GetType().FullName));
                }
                PopulateTablesFromSchema(true);
                CreateOrmAssembly(saveOrmAssembly);
            }
        }

        public abstract string GetDatabaseNameFromSchema();

        public abstract void OpenConnection();

        public abstract void CloseConnection();

        public abstract void InitliazeDbConnection();

        public abstract void InitliazeDbConnection(string connectionString);

        public abstract void PopulateTablesFromSchema(bool includeColumns);

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