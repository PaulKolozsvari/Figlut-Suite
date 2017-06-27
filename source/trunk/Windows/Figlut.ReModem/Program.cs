namespace Figlut.ReModem
{
    #region Using Directives

    using Figlut.ReModem.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities;

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
            Console.WriteLine("*** Figlut ReModem Usage ***");
            Console.WriteLine();
            Console.WriteLine("{0} or {1} : Display usage (service is not started).", HELP_ARGUMENT, HELP_QUESTION_MARK_ARGUMENT);
            Console.WriteLine("{0} : Resets the service's settings file with the default settings (server is not started).", RESET_SETTINGS_ARGUMENT);
            Console.WriteLine("{0} : Starts the service as a console application instead of a windows service.", TEST_MODE_ARGUMENT);
            Console.WriteLine();
            Console.WriteLine("N.B. Executing without any parameters runs the server as a windows service.");
        }

        private static void ResetSettings()
        {
            FiglutReModemSettings s = new FiglutReModemSettings()
            {
                ModemSerialPortName = "COM13",
                ModemSerialPortBaudRate = 2400,
                ModemSerialPortParity = System.IO.Ports.Parity.None,
                ModemSerialPortDatabits = 8,
                ModemSerialPortStopBits = System.IO.Ports.StopBits.One,
                ModemSerialPortHandshake = System.IO.Ports.Handshake.RequestToSend,
                ModemSerialPortDtrEnable = true,
                ModemSerialPortRtsEnable = true,
                IgnoreTimeZoneOnSmsDateReceived = true,
                LogAllSmsReceived = true,
                LogAllSmsSent = true,
                LogModemSerialPortCommunication = true,
                DefaultDialupConnectionName = "Dial-up Connection",
                DefaultDialupConnectionUserName = string.Empty,
                DefaultDialupConnectionPassword = string.Empty,
                DialupConnectionAttemptTimeout = 60000,
                LogAllDialupStateChangedEvents = true,
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
                    FiglutReModemService.Start();
                    Console.WriteLine("Press any key to stop service.");
                    Console.Read();
                    FiglutReModemService.Stop();
                    return;
                }
                ServiceBase.Run(new ServiceBase[] { new FiglutReModemService() });
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
