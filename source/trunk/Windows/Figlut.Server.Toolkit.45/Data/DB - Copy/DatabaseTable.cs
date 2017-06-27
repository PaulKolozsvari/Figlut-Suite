namespace Psion.Server.Toolkit.Data.DB
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
        }

        public DatabaseTable(string tableName)
        {
            _tableName = tableName;
        }

        public DatabaseTable(DbConnection connection, string tableName)
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            _connection = connection;
            _tableName = tableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaRow">The DataRow retrieved from a database schema containing information about this column.</param>
        public DatabaseTable(DbConnection connection, DataRow schemaRow)
        {
            _columns = new EntityCache<string, DatabaseTableColumn>();
            _connection = connection;
            PopulateFromSchema(schemaRow);
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected bool _isSystemTable;
        protected EntityCache<string, DatabaseTableColumn> _columns;
        protected DbConnection _connection;
        protected Type _mappedType;

        #endregion //Fields

        #region Properties

        public virtual DbConnection Connection
        {
            get { return (DbConnection)_connection; }
            set { _connection = value; }
        }

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

        public abstract void Initialize();

        public abstract void PopulateFromSchema(DataRow row);

        public abstract void PopulateColumnsFromSchema();

        public List<object> Query(
            string columnName,
            object columnValue,
            bool useLikeFilter,
            bool caseSensitive)
        {
            //TODO Implement code to query data by filter.
            throw new NotImplementedException();
        }

        #endregion //Methods
    }
}