namespace Figlut.Mobile.Toolkit.Data.DB.SQLCE
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    #endregion //Using Directives

    public class SqlTypeConversionInfo
    {
        #region Constructors

        public SqlTypeConversionInfo()
        {
        }

        public SqlTypeConversionInfo(
            string sqlTypeName,
            Type sqlType,
            SqlDbType sqlDbType,
            Type dotNetType)
        {
            _sqlTypeName = sqlTypeName;
            _sqlType = sqlType;
            _sqlDbType = sqlDbType;
            _dotNetType = dotNetType;
        }

        #endregion //Constructors

        #region Fields

        protected string _sqlTypeName;
        protected Type _sqlType;
        protected SqlDbType _sqlDbType;
        protected Type _dotNetType;
        protected bool _isNUllable;

        #endregion //Fields

        #region Properties

        public string SqlTypeName
        {
            get { return _sqlTypeName; }
        }

        public Type SqlType
        {
            get { return _sqlType; }
        }

        public Type DotNetType
        {
            get { return _dotNetType; }
        }

        public SqlDbType SqlDbType
        {
            get { return _sqlDbType; }
        }

        public bool IsNullable
        {
            get { return _isNUllable; }
        }

        #endregion //Properties
    }
}