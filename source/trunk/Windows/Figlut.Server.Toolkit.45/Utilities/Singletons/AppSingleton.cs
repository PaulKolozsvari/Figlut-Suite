namespace Figlut.Server.Toolkit.Utilities.Singletons
{
    #region region name

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Utilities.Email;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.SettingsFile.Default;

    #endregion //region name

    public class AppSingleton<E> where E : class
    {
        /* Singleton pattern for web applications http://www.alexzaitzev.pro/2013/02/singleton-pattern.html
         * 	Using the volatile keyword for the singleton instance object:
         * 	https://msdn.microsoft.com/en-us/library/x13ttww7.aspx
         * 	http://www.c-sharpcorner.com/UploadFile/1d42da/volatile-keyword-in-C-Sharp-threading/
         */

        #region Singleton Setup

        protected static volatile E _instance;
        protected static object _syncRoot = new object();

        public static E Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        _instance = Activator.CreateInstance<E>();
                    }
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        protected AppSingleton()
        {
        }

        #endregion //Constructors

        #region Methods

        protected virtual void InitializeAllDefaultSettings(AppSettings settings, bool logSettings)
        {
            InitializeGOCSettings(settings);
            InitializeLogger(settings);
            if (logSettings)
            {
                GOC.Instance.Logger.LogSettings(settings);
            }
            InitializeEmailClient(settings);
            InitializeThreadingSettings(settings);
        }

        protected virtual void InitializeGOCSettings(AppSettings settings)
        {
            GOC.Instance.ApplicationName = settings.ApplicationName;
            GOC.Instance.ShowMessageBoxOnException = settings.ShowMessageBoxOnException;
            GOC.Instance.SendEmailOnException = settings.SendEmailOnException;
            GOC.Instance.AppendHostNameToExceptionEmails = settings.AppendHostNameToExceptionEmails;
        }

        protected virtual void InitializeLogger(AppSettings s)
        {
            GOC.Instance.Logger = new Logger(//Creates a global Logger to be used throughout the application to be stored in the Global Object Cache which is a singleton.
                s.LogToFile,
                s.LogToWindowsEventLog,
                s.LogToConsole,
                s.LoggingLevel,
                s.LogFileName,
                s.EventSourceName,
                s.EventLogName);
        }

        protected virtual void InitializeEmailClient(AppSettings s)
        {
            GOC.Instance.EmailClient = new EmailClient(
                s.EmailNotificationsEnabled,
                s.ThrowEmailFailExceptions,
                s.EmailProvider,
                s.ExchangeSmtpServer,
                s.ExchangeSmtpUserName,
                s.ExchangeSmtpPassword,
                s.ExchangeSmtpPort,
                s.ExchangeSmtpEnableSsl,
                s.GMailSmtpServer,
                s.GMailSmtpUserName,
                s.GMailSmtpPassword,
                s.GMailSmtpPort,
                s.SenderEmailAddress,
                s.SenderDisplayName,
                s.ExceptionEmailSubject,
                s.EmailLoggingEnabled,
                s.IncludeDefaultEmailRecipients,
                s.DefaultEmailRecipients.ToList());
        }

        protected virtual void InitializeThreadingSettings(AppSettings setting)
        {
            ThreadHelper.SetMinimumThreadsCount(setting.MinimumWorkerThreadCount, setting.MinimumCompletionPortThreadCount);
        }

        #endregion //Methods
    }
}
