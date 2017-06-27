namespace Figlut.Web.Service
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Web.Service.Configuration;

    #endregion //Using Directives

    static class Program
    {
        #region Constants

        private const string HELP_ARGUMENT = "/help";
        private const string HELP_QUESTION_MARK_ARGUMENT = "/?";
        private const string RESET_SETTINGS_ARGUMENT = "/reset_settings";
        private const string TEST_MODE_ARGUMENT = "/test_mode";

        #endregion //Constants

        #region Fields

        private static bool _displayHelp;
        private static bool _resetSettings;
        private static bool _testMode;

        #endregion //Fields

        #region Methods

        private static bool ParseArguments(string[] args)
        {
            foreach (string a in args)
            {
                string aLower = a.ToLower();
                switch (aLower)
                {
                    case HELP_ARGUMENT:
                        DisplayHelp();
                        return false;
                    case HELP_QUESTION_MARK_ARGUMENT:
                        DisplayHelp();
                        return false;
                    case RESET_SETTINGS_ARGUMENT:
                        ResetSettings();
                        return false;
                    case TEST_MODE_ARGUMENT:
                        _testMode = true;
                        return true;
                    default:
                        throw new ArgumentException(string.Format("Invalid argument '{0}'.", a));
                }
            }
            return true;
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("*** Figlut Web Service Usage ***");
            Console.WriteLine();
            Console.WriteLine("{0} or {1} : Display usage (service is not started).", HELP_ARGUMENT, HELP_QUESTION_MARK_ARGUMENT);
            Console.WriteLine("{0} : Resets the service's settings file with the default settings (server is not started).", RESET_SETTINGS_ARGUMENT);
            Console.WriteLine("{0} : Starts the service as a console application instead of a windows service.", TEST_MODE_ARGUMENT);
            Console.WriteLine();
            Console.WriteLine("N.B. Executing without any parameters runs the server as a windows service.");
        }

        private static void ResetSettings()
        {
            FiglutWebServiceSettings s = new FiglutWebServiceSettings()
            {
                ShowMessageBoxOnException = false,
                LogToFile = true,
                LogToWindowsEventLog = true,
                LogToConsole = true,
                LogFileName = "Figlut.Web.Service.Log.txt",
                EventSourceName = "Figlut.Web.Service.Source",
                EventLogName = "Figlut.Web.Service.Log",
                LoggingLevel = LoggingLevel.Normal,
                PortNumber = 8889,
                HostAddressSuffix = "Figlut",
                DatabaseConnectionString = "<Enter connection string to database here.>",
                SaveOrmAssembly = false,
                TextResponseEncoding  = TextEncodingType.UTF8,
                IncludeOrmTypeNamesInJsonResponse = false,
            };
            Console.Write("Reset settings file {0}, are you sure (Y/N)?", s.FilePath);
            string response = Console.ReadLine();
            if (response.Trim().ToLower() != "y")
            {
                return;
            }
            s.SaveToFile();
            Console.WriteLine("Settings file reset successfully.");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                if (!ParseArguments(args))
                {
                    return;
                }
                if (_testMode)
                {
                    FiglutWebService.Start();
                    Console.WriteLine("Press any key to stop service.")
                        ;
                    Console.Read();
                    FiglutWebService.Stop();
                    return;
                }
                ServiceBase.Run(new ServiceBase[] { new FiglutWebService() });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                throw ex;
            }
        }

        #endregion //Methods
    }
}
