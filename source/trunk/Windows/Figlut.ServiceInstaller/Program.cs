namespace Figlut.ServiceInstaller
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.ServiceInstaller.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    class Program
    {
        #region Constants

        private const string INSTALL_UTIL_LOG_TO_CONSOLE_SWITCH = "/LogToConsole";
        private const string INSTALL_UTIL_UNINSTALL_SWITCH = "-u";

        private const string HELP_ARGUMENT = "/help";
        private const string HELP_QUESTION_MARK_ARGUMENT = "/?";
        private const string RESET_SETTINGS_ARGUMENT = "/reset_settings";
        private const string INSTALL_SERVICE = "/install_service";
        private const string UNINSTALL_SERVICE = "/uninstall_service";
        private const string CREATE_EVENT_LOG = "/create_event_log";
        private const string DELETE_EVENT_LOG = "/delete_event_log";

        #endregion //Constants

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
                    case INSTALL_SERVICE:
                        if (args.Length != 2)
                        {
                            throw new Exception(string.Format("Expected path to the service executable after the {0} switch e.g. {0} service.exe", INSTALL_SERVICE));
                        }
                        string installPath = args[1];
                        RunInstallUtil(installPath, true);
                        return true;
                    case UNINSTALL_SERVICE:
                        if (args.Length != 2)
                        {
                            throw new Exception(string.Format("Expected path to the service executable after the {0} switch e.g. {0} service.exe", UNINSTALL_SERVICE));
                        }
                        string uninstallPath = args[1];
                        RunInstallUtil(uninstallPath, false);
                        return true;
                    case CREATE_EVENT_LOG:
                        if (args.Length != 2)
                        {
                            throw new Exception(string.Format("Expected name of the event log after the {0} switch e.g. {0} service.log", CREATE_EVENT_LOG));
                        }
                        string createLogName = args[1];
                        CreateEventLog(createLogName, createLogName);
                        return true;
                    case DELETE_EVENT_LOG:
                        if (args.Length != 2)
                        {
                            throw new Exception(string.Format("Expected name of the event log after the {0} switch e.g. {0} service.log", CREATE_EVENT_LOG));
                        }
                        string deleteLogName = args[1];
                        DeleteEventLog(deleteLogName, deleteLogName);
                        return true;
                    default:
                        throw new ArgumentException(string.Format("Invalid argument '{0}'.", a));
                }
            }
            return true;
        }

        private static void CreateEventLog(string source, string logName)
        {
            if (!System.Diagnostics.EventLog.SourceExists(source))
            {
                System.Diagnostics.EventLog.CreateEventSource(source, logName);
            }
        }

        private static void DeleteEventLog(string source, string logName)
        {
            if (System.Diagnostics.EventLog.SourceExists(source))
            {
                System.Diagnostics.EventLog.DeleteEventSource(source);
            }
            if (System.Diagnostics.EventLog.Exists(logName))
            {
                System.Diagnostics.EventLog.Delete(logName);
            }
        }

        private static void ValidateInstallUtilParametersValid(string serviceExecutableFilePath)
        {
            string installUtilDirectory = Path.GetDirectoryName(FiglutServiceInstallerApplication.Instance.Settings.InstallUtilFilePath);
            if (!Directory.Exists(installUtilDirectory))
            {
                throw new DirectoryNotFoundException(string.Format("Could not find directory '{0}'. Ensure .NET Framework the appropriate .NET Frameowork is installed.", installUtilDirectory));
            }
            if (!File.Exists(FiglutServiceInstallerApplication.Instance.Settings.InstallUtilFilePath))
            {
                throw new FileNotFoundException(string.Format(".NET Framework is installed but could not find service installer utility '{0}'", FiglutServiceInstallerApplication.Instance.Settings.InstallUtilFilePath));
            }
            if (!File.Exists(serviceExecutableFilePath))
            {
                throw new FileNotFoundException(string.Format("Could not find executable of service to be installed: {0}", serviceExecutableFilePath));
            }
            if (!Path.GetExtension(serviceExecutableFilePath).ToLower().Equals(".exe"))
            {
                throw new Exception(string.Format("Executable of service to be installed must have an '.exe' file extension."));
            }
        }

        private static void RunInstallUtil(string serviceExecutableFilePath, bool install)
        {
            FiglutServiceInstallerApplication.Instance.Initialize();
            ValidateInstallUtilParametersValid(serviceExecutableFilePath);
            string installUtilArguments = null;
            if (install)
            {
                installUtilArguments = string.Format("{0} \"{1}\"",
                    INSTALL_UTIL_LOG_TO_CONSOLE_SWITCH,
                    serviceExecutableFilePath);
            }
            else
            {
                installUtilArguments = string.Format("{0} {1} \"{2}\"",
                    INSTALL_UTIL_UNINSTALL_SWITCH,
                    INSTALL_UTIL_LOG_TO_CONSOLE_SWITCH,
                    serviceExecutableFilePath);
            }
            GOC.Instance.Logger.LogMessage(new LogMessage(string.Format("Executing service installer with parameters: {0}", installUtilArguments), LogMessageType.Information, LoggingLevel.Maximum));
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = FiglutServiceInstallerApplication.Instance.Settings.InstallUtilFilePath,
                    Arguments = installUtilArguments,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                }
            };
            p.OutputDataReceived += p_OutputDataReceived;
            p.ErrorDataReceived += p_ErrorDataReceived;
            p.Start();

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
            GOC.Instance.Logger.LogMessage(new LogMessage(string.Format("Completed execution of service installer: {0}", installUtilArguments), LogMessageType.Information, LoggingLevel.Normal));
        }

        private static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.Error.WriteLine(e.Data);
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("*** Figlut Service Installer Usage ***");
            Console.WriteLine();
            Console.WriteLine("{0} or {1} : Display usage (service is not started).", HELP_ARGUMENT, HELP_QUESTION_MARK_ARGUMENT);
            Console.WriteLine("{0} : Resets the service's settings file with the default settings (server is not started).", RESET_SETTINGS_ARGUMENT);
            Console.WriteLine("{0} : Installs a .NET windows service executable. Supply path to the executable e.g. {0} service.exe", INSTALL_SERVICE);
            Console.WriteLine("{0} : Uninstalls a .NET windows service executable. Supply path to the executable e.g. {0} service.exe", UNINSTALL_SERVICE);
            Console.WriteLine("{0} : Creates a windows event log with the given name and the source will be the same name e.g. {0} service.log", CREATE_EVENT_LOG);
            Console.WriteLine("{0} : Deletes a windows event log with the given name and the source will be the same name e.g. {0} service.log", DELETE_EVENT_LOG);
            Console.WriteLine();
            Console.WriteLine("N.B. Executing without any parameters runs the server as a windows service.");
        }

        private static void ResetSettings()
        {
            FiglutServiceInstallerSettings s = new FiglutServiceInstallerSettings()
            {
                InstallUtilFilePath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe",
                LogToFile = true,
                LogToWindowsEventLog = false,
                LogToConsole = true,
                LogFileName = "Figlut.ReModem.Log.txt",
                EventSourceName = "Figlut.ReModem.Source",
                EventLogName = "Figlut.ReModem.Log",
                LoggingLevel = LoggingLevel.Normal
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

        static void Main(string[] args)
        {
            try
            {
                CreateEventLog(FiglutServiceInstallerApplication.Instance.Settings.EventSourceName, FiglutServiceInstallerApplication.Instance.Settings.EventLogName);
                if (!ParseArguments(args))
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                throw;
            }
        }

        #endregion //Methods
    }
}
