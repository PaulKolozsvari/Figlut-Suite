using System.Diagnostics;

namespace Figlut.MonoDroid.Toolkit.Utilities.Logging
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
	using Figlut.MonoDroid.Toolkit.Utilities;

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
            Initialize(true, LoggingLevel.Normal, DEFAULT_LOG_FILE_NAME);
        }

        public Logger(
            bool logToFile,
            LoggingLevel loggingLevel, 
            string logFileName)
        {
            Initialize(
                logToFile,
                loggingLevel, 
                logFileName);
        }

        #endregion //Constructors

        protected void Initialize(
            bool logToFile,
            LoggingLevel loggingLevel,
            string logFileName)
        {
            _logToFile = logToFile;
            _logFileName = logFileName;
            _loggingLevel = loggingLevel;
        }

        #region Constants

        /// <summary>
        /// The log file name where all errors and info etc. will be logged to in the same
        /// directory as the executing assembly.
        /// </summary>
        public static string DEFAULT_LOG_FILE_NAME = "FiglutMonoDroidToolkitLog.txt";

        #endregion //Constants

        #region Fields

        protected bool _logToFile;

        protected string _logFileName;
        protected LoggingLevel _loggingLevel;

        #endregion //Fields

        #region Properties

        public bool LogToFile
        {
            get { return _logToFile; }
            set { _logToFile = value; }
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

        #endregion //Properties

        #region Methods

        public void LogMessageToFile(LogMessage logMessage)
        {
			string logFilePath = Path.Combine (Information.GetExecutingDirectory(), _logFileName);
			using (StreamWriter writer = new StreamWriter(logFilePath, true) { AutoFlush = true }) {
                writer.WriteLine(logMessage.ToString());
            }
        }

		public string ReadLogContents()
		{
			string logFilePath = Path.Combine (Information.GetExecutingDirectory (), _logFileName);
			if (!File.Exists (logFilePath)) {
				return string.Empty;
			}
			using (StreamReader reader = new StreamReader (logFilePath)) {
				return reader.ReadToEnd ();
			}
		}

		public void DeleteLog()
		{
			string logFilePath = Path.Combine (Information.GetExecutingDirectory (), _logFileName);
			if (File.Exists (logFilePath)) {
				File.Delete (logFilePath);
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
            if (_logToFile && !string.IsNullOrEmpty(_logFileName))
            {
                LogMessageToFile(logMessage);
            }
        }

        #endregion //Methods
    }
}