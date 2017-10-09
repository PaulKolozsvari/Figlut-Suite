namespace Figlut.ServiceInstaller
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.ServiceInstaller.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class FiglutServiceInstallerApplication
    {
        #region Singleton Setup

        private static FiglutServiceInstallerApplication _instance;

        public static FiglutServiceInstallerApplication Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FiglutServiceInstallerApplication();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private FiglutServiceInstallerApplication()
        {
        }

        #endregion //Constructors

        #region Fields

        private FiglutServiceInstallerSettings _settings;

        #endregion //Fields

        #region Properties

        public FiglutServiceInstallerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = GOC.Instance.GetSettings<FiglutServiceInstallerSettings>(true, true);
                }
                return _settings;
            }
        }

        #endregion //Properties

        #region Methods

        public void Initialize()
        {
            FiglutServiceInstallerSettings settings = Settings;
            GOC.Instance.Logger = new Logger(
                settings.LogToFile,
                settings.LogToWindowsEventLog,
                settings.LogToConsole,
                settings.LoggingLevel,
                settings.LogFileName,
                settings.EventSourceName,
                settings.EventLogName);
            GOC.Instance.Logger.LogMessage(new LogMessage("Initializing Service Installer ...", LogMessageType.Information, LoggingLevel.Maximum));
        }

        #endregion //Methods
    }
}
