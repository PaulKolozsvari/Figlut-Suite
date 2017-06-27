namespace Figlut.Server.Toolkit.Data.QueryRunners
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    [Serializable]
    public class SqlQueryRunnerDispatcher
    {
        #region Constructors

        public SqlQueryRunnerDispatcher()
        {
        }

        public SqlQueryRunnerDispatcher(SqlQueryRunnerConfig sqlQueryRunnerConfig)
        {
            _sqlQueryRunnerConfig = sqlQueryRunnerConfig;
        }

        #endregion //Constructors

        #region Fields

        //public const string QUERY_RUNNER_APP_DOMAIN_NAME = "QueryRunnerDomain";
        public const string SQL_QUERY_RUNNER_EXECUTE_QUERY_METHOD_NAME = "ExecuteQuery";
        public const string SQL_QUERY_RUNNER_EXECUTE_QUERY_METHOD_RESULT_NAME = "ExecuteQueryResult";

        #endregion //Fields

        #region Fields

        protected SqlQueryRunnerConfig _sqlQueryRunnerConfig;

        #endregion //Fields

        #region Methods

        public string DispatchSqlQueryRunner(
            string ormAssemblyName,
            string ormTypeName,
            string sqlQueryString,
            string acceptContentType,
            string connectionString,
            bool includeOrmTypeNamesInJsonResponse)
        {
            AppDomain queryRunnerDomain = null;
            try
            {
                SqlQueryRunnerInput input = new SqlQueryRunnerInput(
                    ormAssemblyName,
                    ormTypeName,
                    sqlQueryString,
                    acceptContentType,
                    connectionString,
                    includeOrmTypeNamesInJsonResponse);
                AppDomainSetup domainSetup = new AppDomainSetup() 
                { 
                    ApplicationBase = Path.GetDirectoryName(_sqlQueryRunnerConfig.SqlQueryRunnerAssemblyPath) 
                };
                queryRunnerDomain = AppDomain.CreateDomain(DataShaper.GetUniqueIdentifier(), null, domainSetup);
                queryRunnerDomain.SetData(SQL_QUERY_RUNNER_EXECUTE_QUERY_METHOD_NAME, input);
                queryRunnerDomain.DoCallBack(new CrossAppDomainDelegate(ExecuteSqlQueryRunnerInAnotherDomain));
                SqlQueryRunnerOutput result = (SqlQueryRunnerOutput)queryRunnerDomain.GetData(SQL_QUERY_RUNNER_EXECUTE_QUERY_METHOD_RESULT_NAME);
                if (!result.Success)
                {
                    throw new Exception(result.ResultMessage);
                }
                return result.ResultMessage;
            }
            finally
            {
                if (queryRunnerDomain != null)
                {
                    AppDomain.Unload(queryRunnerDomain);
                }
            }
        }

        private void ExecuteSqlQueryRunnerInAnotherDomain()
        {
            try
            {
                SqlQueryRunnerInput input = (SqlQueryRunnerInput)AppDomain.CurrentDomain.GetData(SQL_QUERY_RUNNER_EXECUTE_QUERY_METHOD_NAME);
                Assembly queryRunnerAssembly = AppDomain.CurrentDomain.Load(_sqlQueryRunnerConfig.QueryRunnerAssemblyBytes);
                Type sqlQueryRunnerType = queryRunnerAssembly.GetType(_sqlQueryRunnerConfig.SqlQueryRunnerFullTypeName);
                ISqlQueryRunner sqlQueryRunner = Activator.CreateInstance(sqlQueryRunnerType) as ISqlQueryRunner;
                if (sqlQueryRunner == null)
                {
                    throw new InvalidCastException(string.Format(
                        "Specified SQL Query runner type {0} does not implement the interface {1}.",
                        _sqlQueryRunnerConfig.SqlQueryRunnerFullTypeName,
                        typeof(ISqlQueryRunner).FullName));
                }
                SqlQueryRunnerOutput output = sqlQueryRunner.ExecuteQuery(input);
                AppDomain.CurrentDomain.SetData(
                    SQL_QUERY_RUNNER_EXECUTE_QUERY_METHOD_RESULT_NAME, 
                    output);
            }
            catch (Exception ex)
            {
                AppDomain.CurrentDomain.SetData(
                    SQL_QUERY_RUNNER_EXECUTE_QUERY_METHOD_RESULT_NAME, 
                    new SqlQueryRunnerOutput(false, ex.Message));
            }
        }

        #endregion //Methods
    }
}