namespace Figlut.MonoDroid.Toolkit.Utilities.Logging
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
            base(GetErrorMessageFromException(exception), LogMessageType.Exception, loggingLevel)
        {
        }

        public LogError(string errorMessage, LoggingLevel loggingLevel, DateTime date) : 
            base(errorMessage, LogMessageType.Error, loggingLevel, date)
        {
        }

        public LogError(Exception exception, LoggingLevel loggingLevel, DateTime date) :
            base(GetErrorMessageFromException(exception), LogMessageType.Exception, loggingLevel, date)
        {
        }

        #region Methods

        public static string GetErrorMessageFromException(Exception exception)
        {
            StringBuilder result = new StringBuilder();
			result.Append(exception.Message);
            if (exception.InnerException != null)
            {
				result.AppendLine ();
				result.Append(string.Format("Inner Exception : {0}", exception.InnerException.ToString()));
            }
            return result.ToString();
        }

        #endregion //Methods
    }
}