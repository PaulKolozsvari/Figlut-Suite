namespace Figlut.Mobile.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Synchronization.Data;
    using System.Data;
using System.Xml.Serialization;

    #endregion //Using Directives

    public abstract class MobileDatabaseTable
    {
        #region Constructors

        public MobileDatabaseTable()
        {
            _columns = new EntityCache<string, MobileDatabaseTableColumn>();
            _tableName = this.GetType().Name;
            _childrenTables = new EntityCache<string, EntityCache<string, ForeignKeyInfo>>();
        }

        public MobileDatabaseTable(string tableName, string connectionString)
        {
            _columns = new EntityCache<string, MobileDatabaseTableColumn>();
            _tableName = tableName;
            _connectionString = connectionString;
            _childrenTables = new EntityCache<string, EntityCache<string, ForeignKeyInfo>>();
        }

        public MobileDatabaseTable(DataRow schemaRow)
        {
            _columns = new EntityCache<string, MobileDatabaseTableColumn>();
            PopulateFromSchema(schemaRow);
            _childrenTables = new EntityCache<string, EntityCache<string, ForeignKeyInfo>>();
        }

        public MobileDatabaseTable(DataRow schemaRow, string connectionString)
        {
            _columns = new EntityCache<string, MobileDatabaseTableColumn>();
            PopulateFromSchema(schemaRow);
            _connectionString = connectionString;
            _childrenTables = new EntityCache<string, EntityCache<string, ForeignKeyInfo>>();
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected bool _isSystemTable;
        protected EntityCache<string, MobileDatabaseTableColumn> _columns;
        protected Type _mappedType;
        protected string _mappedTypeName;
        protected string _connectionString;
        protected EntityCache<string, EntityCache<string, ForeignKeyInfo>> _childrenTables;

        #endregion //Fields

        #region Properties

        public virtual string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public virtual bool IsSystemTable
        {
            get { return _isSystemTable; }
            set { _isSystemTable = value; }
        }

        public virtual EntityCache<string, MobileDatabaseTableColumn> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        [XmlIgnore]
        public virtual Type MappedType
        {
            get { return _mappedType; }
            set 
            { 
                _mappedType = value;
                _mappedTypeName = _mappedType.AssemblyQualifiedName;
            }
        }

        //public string MappedTypeName
        //{
        //    get { return _mappedType.AssemblyQualifiedName; }
        //    set { _mappedType = Type.GetType(value); }
        //}

        public virtual SyncDirection SyncDirection { get; set; }

        /// <summary>
        /// Contains the children table names (keys) and a collection of foreign keys (values) that are mapped to this table's primary key.
        /// </summary>
        public virtual EntityCache<string, EntityCache<string, ForeignKeyInfo>> ChildrenTables
        {
            get { return _childrenTables; }
            set { _childrenTables = value; }
        }

        #endregion //Properties

        #region Methods

        public override string ToString()
        {
            return _tableName;
        }

        public virtual EntityCache<string, MobileDatabaseTableColumn> GetForeignKeyColumns()
        {
            EntityCache<string, MobileDatabaseTableColumn> result = new EntityCache<string, MobileDatabaseTableColumn>();
            _columns.Where(c => c.IsForeignKey).ToList().ForEach(c => result.Add(c.ColumnName, c));
            return result;
        }

        public abstract void Initialize();

        public abstract List<object> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive,
            Type entityType);

        public abstract void Insert(object e);

        public abstract void Insert(List<object> entities);

        public abstract void Delete(object e, string columnName);

        public abstract void Delete(List<object> entities, string columnName);

        public abstract void Update(object e, string columnName);

        public abstract void Update(List<object> entities, string columnName);

        public abstract void PopulateFromSchema(DataRow schemaRow);

        public abstract void PopulateColumnsFromSchema(DataTable columnsSchema);

        #endregion //Methods
    }
}