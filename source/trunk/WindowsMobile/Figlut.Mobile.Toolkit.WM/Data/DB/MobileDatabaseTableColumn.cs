namespace Figlut.Mobile.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
using System.Xml.Serialization;

    #endregion //Using Directives

    public abstract class MobileDatabaseTableColumn
    {
        #region Constructors

        public MobileDatabaseTableColumn()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaRow">The DataRow retrieved from a database schema containing information about this column.</param>
        public MobileDatabaseTableColumn(DataRow schemaRow)
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
        protected bool _isForeignKey;
        protected string _parentTableName;
        protected string _parentTablePrimaryKeyName;
        protected string _constraintName;

        #endregion //Fields

        #region Properties

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
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
                    "{0} is an invalid value for {1} on {2}.",
                    _isNullable,
                    EntityReader<MobileDatabaseTableColumn>.GetPropertyName(p => p.IsNullable, false),
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

        public bool IsForeignKey
        {
            get { return _isForeignKey; }
            set { _isForeignKey = value; }
        }

        public string ParentTableName
        {
            get { return _parentTableName; }
            set { _parentTableName = value; }
        }

        public string ParentTablePrimaryKeyName
        {
            get { return _parentTablePrimaryKeyName; }
            set { _parentTablePrimaryKeyName = value; }
        }

        public string ConstraintName
        {
            get { return _constraintName; }
            set { _constraintName = value; }
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