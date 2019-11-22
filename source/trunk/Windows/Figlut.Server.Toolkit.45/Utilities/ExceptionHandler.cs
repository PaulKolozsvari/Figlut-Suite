namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Data;

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

        public static bool HandleException(Exception exception)
        {
            return HandleException(exception, out string emailErrorMessage, out string emailLogErrorMessage);
        }

        public static bool HandleException(Exception exception, out string emailErrorMessage, out string emailLogMessageText)
        {
            return HandleException(exception, null, null, null, true, out emailErrorMessage, out emailLogMessageText);
        }

        public static bool HandleException(Exception exception, bool emailException, out string emailErrorMessage, out string emailLogMessageText)
        {
            return HandleException(exception, null, null, null, emailException, out emailErrorMessage, out emailLogMessageText);
        }

        public static bool HandleException(Exception exception, string eventDetailsMessage, out string emailErrorMessage, out string emailLogMessageText)
        {
            return HandleException(exception, null, null, eventDetailsMessage, true, out emailErrorMessage, out emailLogMessageText);
        }

        public static bool HandleException(Exception exception, string eventDetailsMessage, bool emailException, out string emailErrorMessage, out string emailLogMessageText)
        {
            return HandleException(exception, null, null, eventDetailsMessage, emailException, out emailErrorMessage, out emailLogMessageText);
        }

        public static bool HandleException(Exception exception, Form form, KeyEventHandler keyEventHandler, string eventDetailsMessage, out string emailErrorMessage, out string emailLogMessageText)
        {
            return HandleException(exception, form, keyEventHandler, eventDetailsMessage, true, out emailErrorMessage, out emailLogMessageText);
        }

        public static bool HandleException(Exception exception, Form form, KeyEventHandler keyEventHandler, string eventDetailsMessage, bool emailException, out string emailErrorMessage, out string emailLogMessageText)
        {
            try
            {
                emailErrorMessage = string.Empty;
                emailLogMessageText = string.Empty;
                bool closeApplication = false;
                if (exception == null)
                {
                    throw new NullReferenceException("exception to be handled may not be null.");
                }
                if (GOC.Instance.ShowMessageBoxOnException)
                {
                    UIHelper.DisplayException(exception, form, keyEventHandler, eventDetailsMessage);
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
                    GOC.Instance.Logger.LogMessage(new LogError(exception, eventDetailsMessage, userThrownException.LoggingLevel));
                }
                else
                {
                    GOC.Instance.Logger.LogMessage(new LogError(exception, eventDetailsMessage, LoggingLevel.Normal));
                }
                if (emailException && GOC.Instance.SendEmailOnException)
                {
                    if (GOC.Instance.EmailClient == null)
                    {
                        throw new Exception(string.Format("{0} enabled, but {1} not set.",
                            EntityReader<GOC>.GetPropertyName(p => p.SendEmailOnException, false),
                            EntityReader<GOC>.GetPropertyName(p => p.EmailClient, false)));
                    }
                    GOC.Instance.EmailClient.SendExceptionEmailNotification(exception, out emailErrorMessage, out emailLogMessageText, GOC.Instance.AppendHostNameToExceptionEmails, eventDetailsMessage);
                }
                return closeApplication;
            }
            catch (Exception ex)
            {
                Exception wrappedException = new Exception(ex.Message, exception);
                Console.WriteLine(LogError.GetErrorMessageFromException(new Exception(ex.Message, exception), eventDetailsMessage));
                GOC.Instance.Logger.LogMessageToFile(new LogError(wrappedException, eventDetailsMessage, LoggingLevel.Minimum));
                emailErrorMessage = string.Empty;
                emailLogMessageText = string.Empty;
                return true;
            }
        }

        #endregion //Methods
    }
}