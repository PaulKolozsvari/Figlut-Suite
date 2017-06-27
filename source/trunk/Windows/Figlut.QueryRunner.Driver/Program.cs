namespace Figlut.QueryRunner.Driver
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.QueryRunners;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Web.Service.Configuration;

    #endregion //Using Directives

    class Program
    {
        #region Methods

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Parsing arguments ... ");
                QueryRunnerDriverArguments arguments = new QueryRunnerDriverArguments(args);
                Console.WriteLine("done!");

                Console.Write("Initializing settings ... ");
                FiglutWebServiceSettings settings = GOC.Instance.GetSettings<FiglutWebServiceSettings>(true, true);
                GOC.Instance.Logger = new Logger(
                    settings.LogToFile,
                    settings.LogToWindowsEventLog,
                    settings.LogToConsole,
                    settings.LoggingLevel,
                    settings.LogFileName,
                    settings.EventSourceName,
                    settings.EventLogName);
                GOC.Instance.JsonSerializer.IncludeOrmTypeNamesInJsonResponse =
                    settings.IncludeOrmTypeNamesInJsonResponse;
                GOC.Instance.SetEncoding(settings.TextResponseEncoding);
                Console.WriteLine("done!");

                Console.Write("Executing SQL query '{0}' ... ", arguments.SqlQueryString);
                SqlQueryRunnerConfig sqlQueryRunnnerConfig = new SqlQueryRunnerConfig(
                    Path.Combine(Information.GetExecutingDirectory(), settings.QueryRunnnerAssemblyName), 
                    settings.SqlQueryRunnerFullTypeName);

                string result = (new SqlQueryRunnerDispatcher(sqlQueryRunnnerConfig)).DispatchSqlQueryRunner(
                    arguments.OrmAssemblyName,
                    arguments.OrmTypeName,
                    arguments.SqlQueryString,
                    arguments.AcceptContentType,
                    settings.DatabaseConnectionString,
                    settings.IncludeOrmTypeNamesInJsonResponse);
                Console.WriteLine("done!");
                Console.WriteLine();
                Console.WriteLine("".PadRight(75, '*'));
                Console.WriteLine();
                Console.WriteLine(result);
                Console.Read();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion //Methods
    }
}
