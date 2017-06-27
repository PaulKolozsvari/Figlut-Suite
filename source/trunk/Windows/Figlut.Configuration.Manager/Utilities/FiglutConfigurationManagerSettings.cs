namespace Figlut.Configuration.Manager.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public class FiglutConfigurationManagerSettings : Settings
    {
        #region Constructors

        public FiglutConfigurationManagerSettings()
            : base()
        {
        }

        public FiglutConfigurationManagerSettings(string filePath)
            : base(filePath)
        {
        }

        public FiglutConfigurationManagerSettings(string name, string filePath)
            : base(name, filePath)
        {
        }

        #endregion //Constructors

        #region Fields

        private bool _logToFile;
        private bool _logToWindowsEventLog;
        private bool _logToConsole;
        private string _logFileName;
        private string _eventSourceName;
        private string _eventLogName;
        private LoggingLevel _loggingLevel;

        #endregion //Fields

        #region Properties

        [SettingInfo("Logging", DisplayName = "To File", Description = "Whether or not to log to a text log file in the executing directory.", CategorySequenceId = 0)]
        public bool LogToFile
        {
            get { return _logToFile; }
            set { _logToFile = value; }
        }

        [SettingInfo("Logging", DisplayName = "To Windows Event Log", Description = "Whether or not to log to the Windows Event Log.", CategorySequenceId = 1)]
        public bool LogToWindowsEventLog
        {
            get { return _logToWindowsEventLog; }
            set { _logToWindowsEventLog = value; }
        }

        [SettingInfo("Logging", DisplayName = "To Console", Description = "Whether or not to write all log messages to the console. Useful when running the service as a console application i.e. running the executable with the /test_mode switch.", CategorySequenceId = 2)]
        public bool LogToConsole
        {
            get { return _logToConsole; }
            set { _logToConsole = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the text log file to log to. The log file is written in the executing directory.", CategorySequenceId = 3)]
        public string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the event source to use when logging to the Windows Event Log.", CategorySequenceId = 4)]
        public string EventSourceName
        {
            get { return _eventSourceName; }
            set { _eventSourceName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the Windows Event Log to log to.", CategorySequenceId = 5)]
        public string EventLogName
        {
            get { return _eventLogName; }
            set { _eventLogName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The extent of messages being logged: None = logging is disabled, Minimum = logs server start/stop and exceptions, Normal = logs additional information messages, Maximum = logs all requests and responses to the server.", CategorySequenceId = 6)]
        public LoggingLevel LoggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        #endregion //Properties
    }
}
