namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    /// <summary>
    /// A helper class for handling exceptions i.e. logging and displaying them.
    /// </summary>
    public class ExceptionHandler
    {
        #region Methods

        /// <summary>
        /// Handles an exption by logging and/or displaying it.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        public static bool HandleException(Exception exception)
        {
            try
            {
                bool closeApplication = false;
                if (exception == null)
                {
                    throw new NullReferenceException("exception to be handled may not be null.");
                }
                if (GOC.Instance.ShowMessageBoxOnException)
                {
                    UIHelper.DisplayException(exception);
                }
                UserThrownException userThrownException = exception as UserThrownException;
                if (userThrownException != null)
                {
                    closeApplication = userThrownException.CloseApplication;
                    GOC.Instance.Logger.LogMessage(new LogError(exception, userThrownException.LoggingLevel));
                }
                else
                {
                    GOC.Instance.Logger.LogMessage(new LogError(exception, LoggingLevel.Normal));
                }
                if (closeApplication)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Exception wrappedException = new Exception(ex.Message, exception);
                Console.WriteLine(LogError.GetErrorMessageFromException(new Exception(ex.Message, exception)));
                GOC.Instance.Logger.LogMessageToFile(new LogError(wrappedException, LoggingLevel.Minimum));
                return true;
            }
        }

        #endregion //Methods
    }
}