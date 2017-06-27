﻿namespace Figlut.MonoDroid.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Data.Common;
    using System.Xml.Serialization;

    #endregion //Using Directives

    [Serializable]
    public abstract class DatabaseTable
    {
        #region Constructors

        public DatabaseTable()
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            _tableName = this.GetType().Name;
            _childrenTables = new EntityCache<string, EntityCache<string, ForeignKeyInfo>>();
        }

        public DatabaseTable(string tableName)
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            _tableName = tableName;
            _childrenTables = new EntityCache<string, EntityCache<string, ForeignKeyInfo>>();
        }

        public DatabaseTable(DataRow schemaRow)
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            PopulateFromSchema(schemaRow);
            _childrenTables = new EntityCache<string, EntityCache<string, ForeignKeyInfo>>();
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected bool _isSystemTable;
        protected EntityCache<string, DatabaseTableColumn> _columns;
        protected Type _mappedType;
        protected string _mappedTypeName;
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

        public virtual EntityCache<string, DatabaseTableColumn> Columns
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

        //public virtual string MappedTypeName
        //{
        //    get { return _mappedType.AssemblyQualifiedName; }
        //    set { _mappedType = Type.GetType(value); }
        //}

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

        public virtual EntityCache<string, DatabaseTableColumn> GetForeignKeyColumns()
        {
            EntityCache<string, DatabaseTableColumn> result = new EntityCache<string, DatabaseTableColumn>();
            _columns.Where(c => c.IsForeignKey).ToList().ForEach(c => result.Add(c.ColumnName, c));
            return result;
        }

        public List<object> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive)
        {
            //TODO Implement code to query data by filter.
            throw new NotImplementedException();
        }

        public abstract void PopulateFromSchema(DataRow schemaRow);

		//TODO Implement PopulateColumnsFromSchema in Android.
//        public abstract void PopulateColumnsFromSchema(DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract void PopulateColumnsFromSchema(DataTable columnsSchema);

        #endregion //Methods
    }
}