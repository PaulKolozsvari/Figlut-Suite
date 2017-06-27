namespace Figlut.QueryRunner.Driver
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;

    #endregion //Using Directives

    public class QueryRunnerDriverArguments
    {
        #region Constructors

        public QueryRunnerDriverArguments(string[] args)
        {
            ParseArguments(args);
            ValidateMandatoryArgumentsSet();
        }

        #endregion //Constructors

        #region Constants

        private const string ORM_ASSEMBLY_NAME_ARGUMENT_TYPE__NAME = "-oan";
        private const string ORM_TYPE_NAME_ARGUMENT_TYPE_NAME = "-otn";
        private const string ACCEPT_CONTENT_TYPE_ARGUMENT_TYPE_NAME = "-a";
        private const string SQL_QUERY_STRING_ARGUMENT_TYPE_NAME = "-sql";

        #endregion //Constants

        #region Fields

        private string _ormAssemblyName;
        private string _ormTypeName;
        private string _acceptContentType;
        private string _sqlQueryString;

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

        public string AcceptContentType
        {
            get { return _acceptContentType; }
        }

        public string SqlQueryString
        {
            get { return _sqlQueryString; }
        }

        #endregion //Properties

        #region Methods
        
        private void ParseArguments(string[] args)
        {
            foreach(string argument in args)
            {
                string[] argumentParameters = argument.Split(':');
                if (argumentParameters.Length != 2)
                {
                    throw new ArgumentException(string.Format(
                        "Invalid argument format in {0}. Argument type and value need to be separated by a colon i.e. :",
                        argument));
                }
                string argumentType = argumentParameters[0].Trim().ToLower();
                string argumentValue = argumentParameters[1].Trim();
                switch (argumentType)
                {
                    case ORM_ASSEMBLY_NAME_ARGUMENT_TYPE__NAME:
                        _ormAssemblyName = argumentValue;
                        break;
                    case ORM_TYPE_NAME_ARGUMENT_TYPE_NAME:
                        _ormTypeName = argumentValue;
                        break;
                    case ACCEPT_CONTENT_TYPE_ARGUMENT_TYPE_NAME:
                        _acceptContentType = argumentValue;
                        break;
                    case SQL_QUERY_STRING_ARGUMENT_TYPE_NAME:
                        _sqlQueryString = argumentValue;
                        break;
                    default:
                        throw new ArgumentException(string.Format(
                            "Invalid argument type {0}.",
                            argumentType));
                }
            }
        }

        public void ValidateMandatoryArgumentsSet()
        {
            if (string.IsNullOrEmpty(_ormAssemblyName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set.", 
                    EntityReader<QueryRunnerDriverArguments>.GetPropertyName(p => p.OrmAssemblyName, false)));
            }
            if (string.IsNullOrEmpty(_ormTypeName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set.",
                    EntityReader<QueryRunnerDriverArguments>.GetPropertyName(p => p.OrmTypeName, false)));
            }
            if (string.IsNullOrEmpty(_acceptContentType))
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set.",
                    EntityReader<QueryRunnerDriverArguments>.GetPropertyName(p => p.AcceptContentType, false)));
            }
            if (string.IsNullOrEmpty(_sqlQueryString))
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set.",
                    EntityReader<QueryRunnerDriverArguments>.GetPropertyName(p => p.SqlQueryString, false)));
            }
        }

        #endregion //Methods
    }
}