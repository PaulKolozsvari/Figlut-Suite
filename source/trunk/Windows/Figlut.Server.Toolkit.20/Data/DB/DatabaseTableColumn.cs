namespace Figlut.Server.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;

    #endregion //Using Directives

    public abstract class DatabaseTableColumn
    {
        #region Constructors
		
        public DatabaseTableColumn()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaRow">The DataRow retrieved from a database schema containing information about this column.</param>
        public DatabaseTableColumn(DataRow schemaRow)
        {
            PopulateFromSchema(schemaRow);
        }
		
        #endregion //Constructors

        #region Fields

        protected string _columnName;
        protected Int16 _ordinalPosition;
        protected string _columnDefault;
        protected string _isNullable;
        protected string _dataType;
        protected bool _isKey;

        #endregion //Fields

        #region Properties

        public string ColumnName
        {
            get { return _columnName; }
        }

        public Int16 OrdinalPosition
        {
            get { return _ordinalPosition; }
            set { _ordinalPosition = value; }
        }

        public string ColumnDefault
        {
            get { return _columnDefault; }
            set { _columnDefault = value; }
        }

        public bool IsNullable
        {
            get
            {
                if (_isNullable.ToUpper() == "YES")
                {
                    return true;
                }
                else if (_isNullable.ToUpper() == "NO")
                {
                    return false;
                }
                throw new Exception(string.Format(
                    "{0} is an invalid value for IsNullable on {1}.",
                    _isNullable,
                    this.GetType().FullName));
            }
            set
            {
                _isNullable = value ? "YES" : "NO";
            }
        }

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public bool IsKey
        {
            get { return _isKey; }
            set { _isKey = value; }
        }

        #endregion //Properties

        #region Methods

        public abstract void PopulateFromSchema(DataRow row);

        public override string ToString()
        {
            return string.Format(
                "{0} {1} {2} {3} {4}",
                ColumnName,
                OrdinalPosition,
                ColumnDefault,
                IsNullable,
                DataType);
        }

        #endregion //Methods
    }
}