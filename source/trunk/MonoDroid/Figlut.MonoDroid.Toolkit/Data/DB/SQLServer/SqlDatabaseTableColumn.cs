namespace Figlut.MonoDroid.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    #endregion //Using Directives

    /// <summary>
    /// http://msdn.microsoft.com/library/ms254969.aspx
    /// </summary>
    [Serializable]
    public class SqlDatabaseTableColumn : DatabaseTableColumn
    {
        #region Constructors

        public SqlDatabaseTableColumn()
        {
        }

        public SqlDatabaseTableColumn(DataRow schemaRow) : base(schemaRow)
        {
        }

        #endregion //Constructors

        #region Constants

        public const string COLUMN_NAME_SCHEMA_ATTRIBUTE = "column_name";
        public const string ORDINAL_POSITION_SCHEMA_ATTRIBUTE = "ordinal_position";
        public const string COLUMN_DEFAULT_SCHEMA_ATTRIBUTE = "column_default";
        public const string IS_NULLABLE_SCHEMA_ATTRIBUTE = "is_nullable";
        public const string DATA_TYPE_SCHEMA_ATTRIBUTE = "data_type";

        #endregion //Constants

        #region Fields

        protected SqlDbType _sqlDbType;

        #endregion //Fields

        #region Properties

        public SqlDbType SqlDbType
        {
            get { return _sqlDbType; }
            set { _sqlDbType = value; }
        }

        #endregion //Properties

        #region Methods

        public override void PopulateFromSchema(DataRow schemaRow)
        {
            _columnName = schemaRow[COLUMN_NAME_SCHEMA_ATTRIBUTE].ToString();
            _ordinalPosition = Convert.ToInt16(schemaRow[ORDINAL_POSITION_SCHEMA_ATTRIBUTE]);
            _columnDefault = schemaRow[COLUMN_DEFAULT_SCHEMA_ATTRIBUTE].ToString();
            _isNullable = schemaRow[IS_NULLABLE_SCHEMA_ATTRIBUTE].ToString();
            _dataType = schemaRow[DATA_TYPE_SCHEMA_ATTRIBUTE].ToString();
            _sqlDbType = SqlTypeConverter.Instance.GetSqlDbType(_dataType);
        }

        #endregion //Methods
    }
}