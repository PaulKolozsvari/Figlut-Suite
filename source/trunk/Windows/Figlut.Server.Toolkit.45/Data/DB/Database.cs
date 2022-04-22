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
    using System.IO;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    [Serializable]
    public abstract class Database : IDisposable
    {
        #region Inner Types

        public class OnDatabaseFeedbackEventArgs : EventArgs
        {
            #region Constructors

            public OnDatabaseFeedbackEventArgs(string feedbackInfo)
            {
                _feedbackInfo = feedbackInfo;
            }

            #region Fields

            private string _feedbackInfo;

            #endregion //Fields

            #endregion //Constructors

            #region Properties

            public string FeedbackInfo
            {
                get { return _feedbackInfo; }
            }

            #endregion //Properties
        }

        public delegate void OnDatabaseFeedbackHandler(object sender, OnDatabaseFeedbackEventArgs e);

        #endregion //Inner Types

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
            bool copyOrmAssembly, 
            string ormAssemblyOutputDirectory,
            bool overrideNameWithDatabaseNameFromSchema)
        {
            _name = this.GetType().Name;
            Initialize( 
                connectionString,
                populateTablesFromSchema,
                createOrmAssembly, 
                saveOrmAssembly,
                ormAssemblyOutputDirectory,
                overrideNameWithDatabaseNameFromSchema);
        }

        public Database(
            string name, 
            string connectionString,
            bool populateTablesFromSchema,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            string ormAssemblyOutputDirectory,
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
                ormAssemblyOutputDirectory,
                overrideNameWithDatabaseNameFromSchema);
        }

        #endregion //Constructors

        #region Events

        public event OnDatabaseFeedbackHandler OnDatabaseFeedback;

        #endregion //Events

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

        public abstract void Initialize(
            string connectionString,
            bool populateTablesFromSchema,
            bool createOrmAssembly,
            bool saveOrmAssembly,
            string ormAssemblyOutputDirectory,
            bool overrideNameWithDatabaseNameFromSchema);

        protected void PublishFeedback(string feedback)
        {
            if (OnDatabaseFeedback != null)
            {
                OnDatabaseFeedback(this, new OnDatabaseFeedbackEventArgs(feedback));
            }
        }

        public abstract string GetDatabaseNameFromSchema(DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract DataTable GetRawTablesSchema(bool includeColumns, DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract void PopulateTablesFromSchema(bool includeColumns, DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract List<DatabaseTableKeyColumns> GetTableKeyColumns();

        public abstract List<DatabaseTableForeignKeyColumns> GetTableForeignKeyColumns();

        public abstract void Dispose();

        public virtual void ClearTables()
        {
            _tables.Clear();
        } 

        public List<DatabaseTable> GetTablesMentionedInQuery(Query query)
        {
            List<DatabaseTable> result = new List<DatabaseTable>();
            foreach (string t in query.TableNamesInQuery)
            {
                DatabaseTable table = _tables[t];
                if (table != null)
                {
                    result.Add(table);
                }
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
                PublishFeedback(string.Format("Created type {0}.", ormType.TypeName));
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

        public virtual List<object> Query(
            string columnName,
            object columnValue,
            string propertyNameFilter,
            Type entityType,
            bool disposeConnectionAfterExecute,
            DbConnection connection,
            DbTransaction transaction)
        {
            return Query(columnName, columnValue, entityType.Name, propertyNameFilter, entityType, disposeConnectionAfterExecute, connection, transaction);
        }

        public abstract List<object> Query(string sqlQueryString,
            OrmAssemblySql ormCollectibleAssembly,
            string typeName,
            string propertyNameFilter,
            out OrmType ormCollecibleType);

        public abstract List<object> Query(
            Query query,
            string propertyNameFilter,
            Type entityType);

        public abstract List<object> Query(
            Query query,
            string propertyNameFilter,
            Type entityType,
            bool disposeConnectionAfterExecute,
            DbConnection connection,
            DbTransaction transaction);

        public virtual List<object> Query(
            string columnName,
            object columnValue,
            string tableName,
            string propertyNameFilter,
            Type entityType,
            bool disposeConnectionAfterExecute,
            DbConnection connection,
            DbTransaction transaction)
        {
            DatabaseTable table = GetDatabaseTable(tableName);
            if (table == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with name {1}.",
                    typeof(DatabaseTable).FullName,
                    tableName));
            }
            return table.Query(columnName, columnValue, propertyNameFilter, entityType, disposeConnectionAfterExecute, connection, transaction);
        }

        public List<E> Query<E>(Query query, string propertyNameFilter) where E : class
        {
            List<object> objects = Query(query, propertyNameFilter, typeof(E));
            List<E> result = new List<E>();
            objects.ForEach(o => result.Add((E)o));
            return result;
        }

        #endregion //Methods
    }
}