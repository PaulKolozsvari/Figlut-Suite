namespace Figlut.Mobile.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
using System.Windows.Forms;

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
        /// <param name="log">Whether or not to use the Logger to log the exception to a log file.</param>
        /// <param name="display">Whether or not to display the exception mesage and its inner exception message if one exists.</param>
        public static void HandleException(Exception exception, bool log, bool display)
        {
            HandleException(exception, log, display, null, null);
        }

        /// <summary>
        /// Handles an exception by logging and/or displaying it.
        /// Also temporarily disables the specified form's key up event handler.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="log">Whether or not to use the Logger to log the exception to a log file.</param>
        /// <param name="display">Whether or not to display the exception mesage and its inner exception message if one exists.</param>
        public static void HandleException(Exception exception, bool log, bool display, Form form, KeyEventHandler keyEventHandler)
        {
            try
            {
                bool closeApplication = false;
                if (exception == null)
                {
                    throw new NullReferenceException("exception to be handled may not be null.");
                }
                if (GOC.Instance.ShowMessageBoxOnException && display)
                {
                    UIHelper.DisplayException(exception, form, keyEventHandler);
                }
                if (log)
                {
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
                }
                if (closeApplication)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                Exception wrappedException = new Exception(ex.Message, exception);
                UIHelper.DisplayError(LogError.GetErrorMessageFromException(new Exception(ex.Message, exception)));
                GOC.Instance.Logger.LogMessage(new LogError(wrappedException, LoggingLevel.Normal));
            }
        }

        #endregion //Methods
    }
}