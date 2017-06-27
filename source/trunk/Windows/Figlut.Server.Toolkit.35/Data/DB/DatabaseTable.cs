namespace Figlut.Server.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Data.Common;

    #endregion //Using Directives

    public abstract class DatabaseTable
    {
        #region Constructors

        public DatabaseTable()
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            _tableName = this.GetType().Name;
        }

        public DatabaseTable(string tableName, string connectionString)
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            _tableName = tableName;
            _connectionString = connectionString;
        }

        public DatabaseTable(DataRow schemaRow)
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            PopulateFromSchema(schemaRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaRow">The DataRow retrieved from a database schema containing information about this column.</param>
        public DatabaseTable(DataRow schemaRow, string connectionString)
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            PopulateFromSchema(schemaRow);
            _connectionString = connectionString;
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected bool _isSystemTable;
        protected EntityCache<string, DatabaseTableColumn> _columns;
        protected Type _mappedType;
        protected string _connectionString;

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
        }

        public virtual Type MappedType
        {
            get { return _mappedType; }
            set { _mappedType = value; }
        }

        #endregion //Properties

        #region Methods

        public override string ToString()
        {
            return _tableName;
        }

        public abstract List<object> Query(string columnName, object columnValue, bool useLikeFilter, bool caseSensitive, Type entityType);

        public List<object> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive)
        {
            //TODO Implement code to query data by filter.
            throw new NotImplementedException();
        }

        public abstract void Insert(object e, bool disposeConnectionAfterExecute);

        public abstract void Insert(List<object> entities, bool useTransaction);

        public abstract void Delete(object e, string columnName, bool disposeConnectionAfterExecute);

        public abstract void Delete(List<object> entities, string columnName, bool useTransaction);

        public abstract void Update(object e, string columnName, bool disposeConnectionAfterExecute);

        public abstract void Update(List<object> entities, string columnName, bool useTransaction);

        public abstract void Update(List<object> entities, bool useTransaction);

        public abstract void PopulateFromSchema(DataRow row);

        public abstract void PopulateColumnsFromSchema(DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract List<string> GetKeyColummnNames(DbConnection connection, bool disposeConnectionAfterExecute);

        public abstract void PopulateColumnsFromSchema(DataTable columnsSchema);

        public abstract DataTable GetRawColumnsSchema(DbConnection connection, bool disposeConnectionAfterExecute);

        #endregion //Methods
    }
}