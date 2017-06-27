namespace Figlut.Server.Toolkit.Utilities.Logging
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    /// <summary>
    /// A class to hold some a message and the date/time. To be used when logging.
    /// </summary>
    public class LogMessage
    {
        #region Constructors

        public LogMessage(string message, LogMessageType logMessageType, LoggingLevel loggingLevel)
        {
            _message = message;
            _logMessageType = logMessageType;
            _loggingLevel = loggingLevel;
            _date = DateTime.Now;
        }

        public LogMessage(string message, LogMessageType logMessageType, LoggingLevel loggingLevel, DateTime datetime)
        {
            _message = message;
            _logMessageType = logMessageType;
            _loggingLevel = loggingLevel;
            _date = datetime;            
        }

        #endregion //Constructors

        #region Fields

        protected string _message;
        protected LogMessageType _logMessageType;
        protected LoggingLevel _loggingLevel;
        protected DateTime _date;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// The message to log.
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        public LogMessageType LogMessageType
        {
            get { return _logMessageType; }
        }

        public LoggingLevel LoggingLevel
        {
            get { return _loggingLevel; }
        }

        /// <summary>
        /// The date/time to record in the log when logging this LogInfo.
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
        }

        #endregion //Properties

        #region Methods

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(string.Format("Date Time : {0}", _date.ToString()));
            result.AppendLine(string.Format("{0} : {1}", _logMessageType.ToString(), _message));
            return result.ToString();
        }

        #endregion //Methods
    }
}