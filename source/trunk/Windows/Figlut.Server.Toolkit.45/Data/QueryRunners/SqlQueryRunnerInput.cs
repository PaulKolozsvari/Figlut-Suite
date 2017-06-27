namespace Figlut.Server.Toolkit.Data.QueryRunners
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    [Serializable]
    public class SqlQueryRunnerInput
    {
        #region Constructors

        public SqlQueryRunnerInput()
        {
        }

        public SqlQueryRunnerInput(
            string ormAssemblyName,
            string ormTypeName,
            string sqlQueryString,
            string acceptContentType,
            string connectionString,
            bool includeOrmTypeNamesInJsonResponse)
        {
            _ormAssemblyName = ormAssemblyName;
            _ormTypeName = ormTypeName;
            _sqlQueryString = sqlQueryString;
            _acceptContentType = acceptContentType;
            _connectionString = connectionString;
            _includeOrmTypeNamesInJsonResponse = includeOrmTypeNamesInJsonResponse;
        }

        #endregion //Constructors

        #region Fields

        protected string _ormAssemblyName;
        protected string _ormTypeName;
        protected string _sqlQueryString;
        protected string _acceptContentType;
        protected string _connectionString;
        protected bool _includeOrmTypeNamesInJsonResponse;

        #endregion //Fields

        #region Properties

        public string OrmAssemblyName
        {
            get { return _ormAssemblyName; }
        }

        public string OrmTypeName
        {
            get { return _ormTypeName; }
        }

        public string SqlQueryString
        {
            get { return _sqlQueryString; }
        }

        public string AcceptContentType
        {
            get { return _acceptContentType; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public bool IncludeOrmTypeNamesInJsonResponse
        {
            get { return _includeOrmTypeNamesInJsonResponse; }
        }

        #endregion //Properties
    }
}