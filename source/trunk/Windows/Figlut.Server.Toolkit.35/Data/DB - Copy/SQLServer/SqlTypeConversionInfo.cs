namespace Psion.Server.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
            Type dotNetType)
        {
            _sqlTypeName = sqlTypeName;
            _sqlType = sqlType;
            _dotNetType = dotNetType;
        }

        #endregion //Constructors

        #region Fields

        protected string _sqlTypeName;
        protected Type _sqlType;
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

        public bool IsNullable
        {
            get { return _isNUllable; }
        }

        #endregion //Properties
    }
}