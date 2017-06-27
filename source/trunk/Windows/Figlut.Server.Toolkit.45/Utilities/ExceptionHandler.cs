﻿namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    /// <summary>
    /// A helper class for handling exceptions i.e. logging and displaying them.
    /// </summary>
    public class ExceptionHandler
    {
        #region Methods

        /// <summary>
        /// Gets the complete error message including the exception message, inner exception message (if it exists) and stack trace.
        /// </summary>
        /// <param name="ex">Exception whose message to be retrieved.</param>
        /// <param name="includeStackTrace">Whether to include the exeption's stack trace in the message.</param>
        /// <returns></returns>
        public static string GetCompleteExceptionMessage(Exception ex, bool includeStackTrace)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(ex.Message);
            if (ex.InnerException != null)
            {
                result.AppendLine(ex.InnerException.Message);
            }
            if (includeStackTrace)
            {
                result.AppendLine(ex.StackTrace);
            }
            return result.ToString();
        }

        /// <summary>
        /// Handles an exption by logging and/or displaying it.
        /// Also temporarily disables the specified form's key up event handler.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="log">Whether or not to use the Logger to log the exception to a log file.</param>
        /// <param name="display">Whether or not to display the exception mesage and its inner exception message if one exists.</param>
        public static bool HandleException(Exception exception)
        {
            return HandleException(exception, null, null);
        }

        /// <summary>
        /// Handles an exception by logging and/or displaying it.
        /// Also temporarily disables the specified form's key up event handler.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="log">Whether or not to use the Logger to log the exception to a log file.</param>
        /// <param name="display">Whether or not to display the exception mesage and its inner exception message if one exists.</param>
        public static bool HandleException(Exception exception, Form form, KeyEventHandler keyEventHandler)
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
                    UIHelper.DisplayException(exception, form, keyEventHandler);
                }
                if (GOC.Instance.ShowWindowsFormsNotificationOnException && GOC.Instance.NotifyIcon != null)
                {
                    string applicationName = string.IsNullOrEmpty(GOC.Instance.ApplicationName) ? "Notification" : GOC.Instance.ApplicationName;
                    GOC.Instance.NotifyIcon.ShowBalloonTip(10, applicationName, exception.Message, ToolTipIcon.Error);
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
                return closeApplication;
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