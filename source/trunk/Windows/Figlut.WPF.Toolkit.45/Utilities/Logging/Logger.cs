﻿namespace Figlut.Server.Toolkit.Utilities.Logging
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using Figlut.Server.Toolkit.Data;

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
            Initialize(true, false, false, LoggingLevel.Normal, DEFAULT_LOG_FILE_NAME, null);
        }

        public Logger(
            bool logToFile,
            bool logToWindowsEventLog,
            bool logToConsole,
            LoggingLevel loggingLevel, 
            string logFileName, 
            EventLog eventLog)
        {
            Initialize(
                logToFile,
                logToWindowsEventLog,
                logToConsole,
                loggingLevel, 
                logFileName, 
                eventLog);
        }

        public Logger(
            bool logToFile,
            bool logToWindowsEventLog,
            bool logToConsole,
            LoggingLevel loggingLevel,
            string logFileName,  
            string eventSourceName, 
            string eventLogName)
        {
            Initialize(
                logToFile,
                logToWindowsEventLog,
                logToConsole,
                loggingLevel,
                logFileName,
                new EventLog() { Source = eventSourceName, Log = eventLogName });
        }

        #endregion //Constructors

        protected void Initialize(
            bool logToFile,
            bool logToWindowsEventLog,
            bool logToConsole,
            LoggingLevel loggingLevel,
            string logFileName, 
            EventLog eventLog)
        {
            _logToFile = logToFile;
            _logToWindowsEventLog = logToWindowsEventLog;
            _logToConsole = logToConsole;

            _logFileName = logFileName;
            _loggingLevel = loggingLevel;
            if (eventLog == null)
            {
                return;
            }
            _eventLog = eventLog;
            if (!System.Diagnostics.EventLog.SourceExists(_eventLog.Source))
            {
                System.Diagnostics.EventLog.CreateEventSource(_eventLog.Source, _eventLog.Log);
            }
        }

        #region Constants

        /// <summary>
        /// The log file name where all errors and info etc. will be logged to in the same
        /// directory as the executing assembly.
        /// </summary>
        public static string DEFAULT_LOG_FILE_NAME = "FiglutServerToolkitLog.txt";

        #endregion //Constants

        #region Fields

        protected bool _logToFile;
        protected bool _logToWindowsEventLog;
        protected bool _logToConsole;

        protected string _logFileName;
        protected LoggingLevel _loggingLevel;
        protected EventLog _eventLog;

        #endregion //Fields

        #region Properties

        public bool LogToFile
        {
            get { return _logToFile; }
            set { _logToFile = value; }
        }

        public bool LogToWindowsEventLog
        {
            get { return _logToWindowsEventLog; }
            set { _logToWindowsEventLog = value; }
        }

        public bool LogToConsole
        {
            get { return _logToConsole; }
            set { _logToConsole = value; }
        }

        public LoggingLevel LoggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        public string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        public EventLog EventLog
        {
            get { return _eventLog; }
            set { _eventLog = value; }
        }

        #endregion //Properties

        #region Methods

        public void LogMessageToFile(LogMessage logMessage)
        {
            string logFilePath = Path.Combine(Information.GetExecutingDirectory(), _logFileName);
            using (StreamWriter writer = new StreamWriter(logFilePath, true) { AutoFlush = true })
            {
                writer.WriteLine(logMessage.ToString());
            }
        }

        public void LogMessage(LogMessage logMessage)
        {
            if ((_loggingLevel == LoggingLevel.None) || //Logger is configured to not log anything or ... 
                (_loggingLevel < logMessage.LoggingLevel))
            {
                /*The LogMessage's logging level is higher than what the Logger 
                 * is configured to log e.g. Logger is configured to Minimum logging
                 but the message is of LoggingLevel maximum, in which case the message
                 should not be logged.*/
                return;
            }
            if (_logToConsole)
            {
                Console.WriteLine(logMessage.Message);
            }
            if (_logToFile && !string.IsNullOrEmpty(_logFileName))
            {
                LogMessageToFile(logMessage);
            }
            if (_logToWindowsEventLog && (_eventLog != null))
            {
                switch (logMessage.LogMessageType)
                {
                    case LogMessageType.Exception:
                        _eventLog.WriteEntry(logMessage.Message, EventLogEntryType.Error);
                        break;
                    case LogMessageType.Error:
                        _eventLog.WriteEntry(logMessage.Message, EventLogEntryType.Error);
                        break;
                    case LogMessageType.Information:
                        _eventLog.WriteEntry(logMessage.Message, EventLogEntryType.Information);
                        break;
                    case LogMessageType.Warning:
                        _eventLog.WriteEntry(logMessage.Message, EventLogEntryType.Warning);
                        break;
                    case LogMessageType.SuccessAudit:
                        _eventLog.WriteEntry(logMessage.Message, EventLogEntryType.SuccessAudit);
                        break;
                    case LogMessageType.FailureAudit:
                        _eventLog.WriteEntry(logMessage.Message, EventLogEntryType.FailureAudit);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion //Methods
    }
}