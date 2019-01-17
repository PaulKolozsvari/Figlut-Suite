using System.Runtime.Remoting.Contexts;
using Android.Widget;
using Android.App;

namespace Figlut.MonoDroid.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.MonoDroid.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    /// <summary>
    /// A helper class for handling exceptions i.e. logging and displaying them.
    /// </summary>
    public class ExceptionHandler
    {
        #region Methods

        /// <summary>
        /// Handles an exption by logging it.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        public static bool HandleException(Exception exception)
        {
            return HandleException(exception, null);
        }

        /// <summary>
        /// Handles an exption by logging and/or displaying it (if an actvity is specified.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="activity">If an activity is specified, a Toast will be shown with the exception's message.</param>
		public static bool HandleException(Exception exception, Activity activity)
        {
            try
            {
                bool closeApplication = false;
                if (exception == null)
                {
                    throw new NullReferenceException("exception to be handled may not be null.");
                }
                if (GOC.Instance.ShowMessageBoxOnException && (activity != null))
                {
					Toast.MakeText(activity, exception.Message, ToastLength.Long).Show();
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
                if (closeApplication && (activity != null))
                {
					activity.Finish();
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