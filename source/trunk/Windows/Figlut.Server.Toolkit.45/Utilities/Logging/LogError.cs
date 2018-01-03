namespace Figlut.Server.Toolkit.Utilities.Logging
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    /// <summary>
    /// A class to hold an exception and the date/time it occured. To be used when logging.
    /// </summary>
    public class LogError : LogMessage
    {
        public LogError(string errorMessage, LoggingLevel loggingLevel) :
            base(errorMessage, LogMessageType.Error, loggingLevel)
        {
        }

        public LogError(Exception exception, LoggingLevel loggingLevel) :
            base(GetErrorMessageFromException(exception, null), LogMessageType.Exception, loggingLevel)
        {
        }

        public LogError(Exception exception, string eventDetailsMessage, LoggingLevel loggingLevel) :
            base(GetErrorMessageFromException(exception, eventDetailsMessage), LogMessageType.Exception, loggingLevel)
        {
        }

        public LogError(string errorMessage, LoggingLevel loggingLevel, DateTime date) : 
            base(errorMessage, LogMessageType.Error, loggingLevel, date)
        {
        }

        public LogError(Exception exception, LoggingLevel loggingLevel, DateTime date) :
            base(GetErrorMessageFromException(exception, null), LogMessageType.Exception, loggingLevel, date)
        {
        }

        public LogError(Exception exception, string eventDetailsMessage, LoggingLevel loggingLevel, DateTime date) :
            base(GetErrorMessageFromException(exception, eventDetailsMessage), LogMessageType.Exception, loggingLevel, date)
        {
        }

        #region Methods

        public static string GetErrorMessageFromException(Exception exception, string eventDetailsMessage)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(exception.Message);
            if (exception.InnerException != null)
            {
                result.AppendLine(string.Format("Inner Exception : {0}", exception.InnerException.ToString()));
            }
            result.AppendLine(exception.StackTrace);
            if (!string.IsNullOrEmpty(eventDetailsMessage))
            {
                result.AppendLine(string.Format("Event Details:"));
                result.AppendLine(eventDetailsMessage);
            }
            return result.ToString();
        }

        #endregion //Methods
    }
}