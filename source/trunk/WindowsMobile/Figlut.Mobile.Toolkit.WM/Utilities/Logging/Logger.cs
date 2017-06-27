namespace Figlut.Mobile.Toolkit.Utilities.Logging
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using Figlut.Mobile.Toolkit.Data;
    using OpenNETCF.Diagnostics;

    #endregion //Using Directives

    /// <summary>
    /// A helper class that can be used to messages to a
    /// log file and Windows EventLog that will be written in the assembly's executing directory.
    /// </summary>
    public class Logger
    {
        #region Constructors

        public Logger()
        {
            Initialize(
                true,
                false,
                DEFAULT_LOG_FILE_NAME, 
                LoggingLevel.Normal, null);
        }

        public Logger(
            bool logToFile, 
            bool logToWindowsEventLog, 
            string logFileName, 
            LoggingLevel loggingLevel, 
            EventLog eventLog)
        {
            Initialize(
                logToFile,
                logToWindowsEventLog,
                logFileName, 
                loggingLevel, 
                eventLog);
        }

        public Logger(
            bool logToFile,
            bool logToWindowsEventLog,
            string logFileName, 
            LoggingLevel loggingLevel, 
            string eventSourceName, 
            string eventLogName)
        {
            Initialize(
                logToFile,
                logToWindowsEventLog,
                logFileName,
                loggingLevel,
                new EventLog(eventLogName, eventSourceName, EventLogWriterType.XML));
        }

        #endregion //Constructors

        protected void Initialize(
            bool logToFile,
            bool logToWindowsEventLog, 
            string logFileName, 
            LoggingLevel loggingLevel, 
            EventLog eventLog)
        {
            if (string.IsNullOrEmpty(logFileName))
            {
                throw new ArgumentNullException(string.Format(
                    "{0} may not be null or empty when constructing {1}.",
                    EntityReader<Logger>.GetPropertyName(p => p.LogFileName, false),
                    this.GetType().FullName));
            }
            _logToFile = logToFile;
            _loggingLevel = loggingLevel;
            _logFileName = logFileName;
            _loggingLevel = loggingLevel;
            _eventLog = eventLog;
        }

        #region Constants

        /// <summary>
        /// The log file name where all errors and info etc. will be logged to in the same
        /// directory as the executing assembly.
        /// </summary>
        public static string DEFAULT_LOG_FILE_NAME = "FiglutMobileToolkitLog.txt";

        #endregion //Constants

        #region Fields

        protected string _logFileName;
        protected LoggingLevel _loggingLevel;
        protected EventLog _eventLog;
        protected bool _logToFile;
        protected bool _logToEventLog;

        #endregion //Fields

        #region Properties

        public string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        public LoggingLevel LoggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        public EventLog EventLog
        {
            get { return _eventLog; }
            set { _eventLog = value; }
        }

        #endregion //Properties

        #region Methods

        #region Event Log

        #endregion //Event Log

        #region Log File

        private void LogMessageToFile(LogMessage logMessage)
        {
            string logFilePath = Path.Combine(Information.GetExecutingDirectory(), _logFileName);
            using (StreamWriter writer = new StreamWriter(logFilePath, true) { AutoFlush = true })
            {
                writer.WriteLine(logMessage.ToString());
            }
        }

        #endregion //Log File

        public void LogMessage(LogMessage logMessage)
        {
            if ((_loggingLevel == LoggingLevel.None) ||
                (logMessage.LoggingLevel == LoggingLevel.None)) //LogMessage is set to not be logged.
            {
                return;
            }
            if ((_loggingLevel == LoggingLevel.None) || //Logger is configured to not log anything or ... 
                (_loggingLevel < logMessage.LoggingLevel))
            {
                /*The LogMessage's logging level is higher than what the Logger 
                 * is configured to log e.g. Logger is configured to Minimum logging
                 but the message is of LoggingLevel maximum, in which case the message
                 should not be logged.*/
                return;
            }
            if (_logToFile)
            {
                LogMessageToFile(logMessage);
            }
            if (_logToEventLog && (_eventLog != null))
            {
                switch (logMessage.LogMessageType)
                {
                    case LogMessageType.Exception:
                        _eventLog.WriteEntry(logMessage.ToString(), EventLogEntryType.Error);
                        break;
                    case LogMessageType.Error:
                        _eventLog.WriteEntry(logMessage.ToString(), EventLogEntryType.Error);
                        break;
                    case LogMessageType.Information:
                        _eventLog.WriteEntry(logMessage.ToString(), EventLogEntryType.Information);
                        break;
                    case LogMessageType.Warning:
                        _eventLog.WriteEntry(logMessage.ToString(), EventLogEntryType.Warning);
                        break;
                    case LogMessageType.SuccessAudit:
                        _eventLog.WriteEntry(logMessage.ToString(), EventLogEntryType.SuccessAudit);
                        break;
                    case LogMessageType.FailureAudit:
                        _eventLog.WriteEntry(logMessage.ToString(), EventLogEntryType.FailureAudit);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion //Methods
    }
}