namespace Figlut.Mobile.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Mobile.Toolkit.Data.DB.SyncService;
    using Figlut.Mobile.Toolkit.Data.DB.SQLQuery;
    using Figlut.Mobile.Toolkit.Data.ORM;
    using SystemCF.Reflection.Emit;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;
    using System.IO;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    public abstract class MobileDatabase : IDisposable
    {
        #region Constructors

        public MobileDatabase()
        {
            Initialize(this.GetType().Name, null, null);
        }

        public MobileDatabase(string localDbFilePath)
        {
            Initialize(this.GetType().Name, localDbFilePath, null);
        }

        public MobileDatabase(string name, string localDbFilePath)
        {
            Initialize(name, localDbFilePath, null);
        }

        public MobileDatabase(string name, string localDbFilePath, SyncInitializationConfig syncServiceConfiguration)
        {
            Initialize(name, localDbFilePath, syncServiceConfiguration);
        }

        #endregion //Constructors

        #region Events

        public delegate void SyncSessionProgressHandler(object sender, SyncSessionProgressEventArgs e);
        public delegate void SyncSessionStateChangedHandler(object sender, SyncSessionStateChangedEventArgs e);

        public event SyncSessionProgressHandler SyncSessionProgress;
        public event SyncSessionStateChangedHandler SyncSessionStateChanged;

        #endregion //Events

        #region Fields
		
        protected string _name;
        protected string _localDbFilePath;
        protected EntityCache<string, MobileDatabaseTable> _tables;
        protected OrmAssembly _ormAssembly;
        protected SyncInitializationConfig _syncInitializationConfig;
		
        #endregion //Fields

        #region Properties

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string LocalDbFilePath
        {
            get { return _localDbFilePath; }
            set { _localDbFilePath = value; }
        }

        public EntityCache<string, MobileDatabaseTable> Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }

        public SyncInitializationConfig SyncInitializationConfig
        {
            get { return _syncInitializationConfig; }
            set { _syncInitializationConfig = value; }
        }

        #endregion //Properties

        #region Methods

        public OrmAssembly GetOrmAssembly()
        {
            return _ormAssembly;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Initialize(
            string name,
            string localDbFilePath,
            SyncInitializationConfig syncServiceConfiguration)
        {
            _tables = new EntityCache<string, MobileDatabaseTable>();
            _name = name;
            _localDbFilePath = localDbFilePath;
            _syncInitializationConfig = syncServiceConfiguration;
        }

        public void ClearTables()
        {
            _tables.Clear();
        }

        public List<MobileDatabaseTable> GetTablesMentionedInQuery(SqlQuery query)
        {
            List<MobileDatabaseTable> result = new List<MobileDatabaseTable>();
            foreach (string t in query.TableNamesInQuery)
            {
                MobileDatabaseTable table = _tables[t];
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

        public virtual MobileDatabaseTable GetDatabaseTable(Type entityType)
        {
            return GetDatabaseTable(entityType.Name);
        }

        public virtual MobileDatabaseTable GetDatabaseTable(string tableName)
        {
            if (!_tables.Exists(tableName))
            {
                return null;
            }
            return (MobileDatabaseTable)_tables[tableName];
        }

        public abstract string GetLocalDbConnectionString();

        public abstract string GetLocalDbConnectionString(int maxBufferSize);

        public abstract SyncResult Synchronize(SyncConfig syncConfig);

        public abstract void Dispose();

        protected void FireSyncSessionProgressEvent(object sender, SyncSessionProgressEventArgs e)
        {
            if (SyncSessionProgress != null)
            {
                SyncSessionProgress(sender, e);
            }
        }

        protected void FireSyncSessionStateChangedEvent(object sender, SyncSessionStateChangedEventArgs e)
        {
            if (SyncSessionStateChanged != null)
            {
                SyncSessionStateChanged(sender, e);
            }
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
            foreach (MobileDatabaseTable table in _tables)
            {
                OrmType ormType = _ormAssembly.CreateOrmType(table.TableName, true);
                foreach (MobileDatabaseTableColumn column in table.Columns)
                {
                    ormType.CreateOrmProperty(
                        column.ColumnName,
                        SqlTypeConverter.Instance.GetDotNetType(column.DataType, column.IsNullable));
                }
            }
            foreach (MobileDatabaseTable table in _tables)
            {
                string typeName = string.Format("{0}.{1}", _ormAssembly.AssemblyName, table.TableName);
                OrmType ormType = _ormAssembly.OrmTypes[typeName];
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