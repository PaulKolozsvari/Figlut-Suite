namespace Figlut.Desktop.DataBox.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Desktop.DataBox.Configuration;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Extensions.DataBox;
    using Figlut.Server.Toolkit.Extensions.DataBox.Managers;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Web;
    using Figlut.Server.Toolkit.Web.Client;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;

    #endregion //Using Directives

    public class FiglutDataBoxApplication
    {
        #region Singleton Setup

        private static FiglutDataBoxApplication _instance;

        public static FiglutDataBoxApplication Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FiglutDataBoxApplication();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private FiglutDataBoxApplication()
        {
        }

        #endregion //Constructors

        #region Fields

        private string _applicationTitle;
        private string _applicationVersion;
        private Color _themeStartColor;
        private Color _themeEndColor;
        private Color _dataBoxSelectRowColor;
        private Image _splashScreenImage;
        private Image _applicationBannerImage;

        private Assembly _extensionAssembly;
        private DataBoxCrudInterceptor _crudInterceptor;
        private DataBoxMainMenuExtension _mainMenuExtension;

        #endregion //Fields

        #region Properties

        public string ApplicationTitle
        {
            get { return _applicationTitle; }
            set { _applicationTitle = value; }
        }

        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set { _applicationVersion = value; }
        }

        public Color ThemeStartColor
        {
            get { return _themeStartColor; }
            set { _themeStartColor = value; }
        }

        public Color ThemeEndColor
        {
            get { return _themeEndColor; }
            set { _themeEndColor = value; }
        }

        public Color DataBoxSelectRowColor
        {
            get { return _dataBoxSelectRowColor; }
            set { _dataBoxSelectRowColor = value; }
        }

        public Image SplashScreenImage
        {
            get { return _splashScreenImage; }
            set { _splashScreenImage = value; }
        }

        public Image ApplicationBannerImage
        {
            get { return _applicationBannerImage; }
        }

        public Assembly ExtensionAssembly
        {
            get { return _extensionAssembly; }
        }

        public DataBoxCrudInterceptor CrudInterceptor
        {
            get { return _crudInterceptor; }
        }

        public DataBoxMainMenuExtension MainMenuExtension
        {
            get { return _mainMenuExtension; }
        }

        #endregion //Properties

        #region Methods

        public void Initialize(
            string authenticationUserName,
            string authenticationPassword,
            FiglutDesktopDataBoxSettings settings,
            bool wrapWebException,
            DataBoxManager dataBoxManager,
            WaitProcess w)
        {
            
            GOC.Instance.ShowMessageBoxOnException = settings.ShowMessageBoxOnException;
            w.ChangeStatus("Initializing logger ...");
            GOC.Instance.Logger = new Logger(
                settings.LogToFile,
                settings.LogToWindowsEventLog,
                settings.LogToConsole,
                settings.LoggingLevel,
                settings.LogFileName,
                settings.EventSourceName,
                settings.EventLogName);
            w.ChangeStatus("Setting text response encoding ...");
            GOC.Instance.SetEncoding(settings.FiglutWebServiceTextResponseEncoding);

            w.ChangeStatus("Initializing web service clients ...");
            IMimeWebServiceClient webServiceClient = GetWebServiceClient(
                settings.FiglutWebServiceMessagingFormat,
                settings.FiglutWebServiceBaseUrl,
                settings.UseAuthentication,
                authenticationUserName,
                authenticationPassword,
                settings.AuthenticationDomainName);
            IMimeWebServiceClient webServiceClientSchema = GetWebServiceClient(
                settings.FiglutWebServiceSchemaAcquisitionMessagingFormat, 
                settings.FiglutWebServiceBaseUrl,
                settings.UseAuthentication,
                authenticationUserName,
                authenticationPassword,
                settings.AuthenticationDomainName);

            GOC.Instance.FiglutWebServiceClient = new FiglutWebServiceClientWrapper(webServiceClient, settings.FiglutWebServiceWebRequestTimeout);
            GOC.Instance.FiglutWebServiceClientSchema = new FiglutWebServiceClientWrapper(webServiceClientSchema, settings.FiglutWebServiceWebRequestTimeout);
            //if (!settings.OfflineMode)
            //{
                w.ChangeStatus("Acquiring schema ...");
                AcquireSchema(wrapWebException, Path.Combine(Information.GetExecutingDirectory(), settings.DatabaseSchemaFileName));
            //}
            //else
            //{
            //    w.ChangeStatus(string.Format("Importing {0} ...", settings.DatabaseSchemaFileName));
            //    ImportSchemaFromFile(Path.Combine(Information.GetExecutingDirectory(), settings.DatabaseSchemaFileName));
            //}
            w.ChangeStatus("Loading extensions ...");
            LoadExtensions(settings, dataBoxManager);
        }

        public void InitializeCustomizationParameters(FiglutDesktopDataBoxSettings settings)
        {
            _applicationTitle = settings.ApplicationTitle;
            _applicationVersion = settings.ApplicationVersion;
            _themeStartColor = string.IsNullOrEmpty(settings.ThemeStartColor) ? Color.White : Color.FromName(settings.ThemeStartColor);
            _themeEndColor = string.IsNullOrEmpty(settings.ThemeEndColor) ? Color.SteelBlue : Color.FromName(settings.ThemeEndColor);
            _dataBoxSelectRowColor = string.IsNullOrEmpty(settings.DataBoxSelectRowColor) ? Color.SteelBlue : Color.FromName(settings.DataBoxSelectRowColor);

            string executingDirectory = Information.GetExecutingDirectory();
            string splashScreenImageFilePath = Path.Combine(executingDirectory, settings.SplashScreenImageFileName);
            FileSystemHelper.ValidateFileExists(splashScreenImageFilePath);
            string applicationBannerImageFilePath = Path.Combine(executingDirectory, settings.ApplicationBannerImageFileName);
            FileSystemHelper.ValidateFileExists(applicationBannerImageFilePath);
            _splashScreenImage = Image.FromFile(splashScreenImageFilePath);
            _applicationBannerImage = Image.FromFile(applicationBannerImageFilePath);
        }

        public void AcquireSchema(bool wrapWebException, string databaseSchemaFilePath)
        {
            SqlDatabase db = GOC.Instance.FiglutWebServiceClientSchema.GetSqlDatabase(true, true, null, wrapWebException);
            SqlDatabaseSchemaFile.ExportSchema(db, databaseSchemaFilePath);
            GOC.Instance.ClearDatabases();
            GOC.Instance.AddDatabase<SqlDatabase>(db);
        }

        public void ImportSchemaFromFile(string databaseSchemaFilePath)
        {
            SqlDatabase db = SqlDatabaseSchemaFile.ImportSchema(databaseSchemaFilePath, true, true, null);
            GOC.Instance.ClearDatabases();
            GOC.Instance.AddDatabase<SqlDatabase>(db);
        }

        private IMimeWebServiceClient GetWebServiceClient(
            SerializerType serializerType, 
            string figlutWebServiceBaseUrl,
            bool useAuthentication,
            string authenticationUserName,
            string authenticationPassword,
            string authenticationDomainName)
        {
            IMimeWebServiceClient result = null;
            WebServiceClient webServiceClient = null;
            switch (serializerType)
            {
                case SerializerType.XML:
                    webServiceClient = new XmlWebServiceClient(figlutWebServiceBaseUrl);
                    break;
                case SerializerType.JSON:
                    webServiceClient = new JsonWebServiceClient(figlutWebServiceBaseUrl);
                    break;
                case SerializerType.CSV:
                    webServiceClient = new CsvWebServiceClient(figlutWebServiceBaseUrl);
                    break;
                default:
                    throw new ArgumentException(string.Format(
                        "{0} not supported as a messaging format.",
                        serializerType.ToString()));
            }
            if (useAuthentication)
            {
                webServiceClient.NetworkCredential = new NetworkCredential(
                    authenticationUserName,
                    authenticationPassword,
                    authenticationDomainName);
            }
            result = (IMimeWebServiceClient)webServiceClient;
            return result;
        }

        private void LoadExtensions(FiglutDesktopDataBoxSettings settings, DataBoxManager dataBoxManager)
        {
            if (!settings.UseExtensions)
            {
                _extensionAssembly = Assembly.GetExecutingAssembly();
                _crudInterceptor = new DefaultDataBoxCrudInterceptor();
                _mainMenuExtension = new DefaultDataBoxMainMenuExtension();
                return;
            }
            if (string.IsNullOrEmpty(settings.ExtensionAssemblyName))
            {
                throw new UserThrownException(
                    string.Format(
                    "Cannot load extension. {0} not configured.", 
                    EntityReader<FiglutDesktopDataBoxSettings>.GetPropertyName(p => p.ExtensionAssemblyName, true)),
                    LoggingLevel.Minimum,
                    true);
            }
            if (string.IsNullOrEmpty(settings.CrudInterceptorFullTypeName))
            {
                throw new UserThrownException(
                    string.Format(
                    "Cannot load extension. {0} not configured.", 
                    EntityReader<FiglutDesktopDataBoxSettings>.GetPropertyName(p => p.CrudInterceptorFullTypeName, true)),
                    LoggingLevel.Minimum,
                    true);
            }
            if (string.IsNullOrEmpty(settings.MainMenuExtentionFullTypeName))
            {
                throw new UserThrownException(
                    string.Format(
                    "Cannot load extension. {0} not configured.",
                    EntityReader<FiglutDesktopDataBoxSettings>.GetPropertyName(p => p.CrudInterceptorFullTypeName, true)),
                    LoggingLevel.Minimum,
                    true);
            }
            string extensionAssemblyFilePath = Path.Combine(Information.GetExecutingDirectory(), settings.ExtensionAssemblyName);
            FileSystemHelper.ValidateFileExists(extensionAssemblyFilePath);
            _extensionAssembly = Assembly.LoadFile(extensionAssemblyFilePath);
            _crudInterceptor = AssemblyReader.CreateClassInstance<DataBoxCrudInterceptor>(_extensionAssembly, settings.CrudInterceptorFullTypeName);
            _mainMenuExtension = AssemblyReader.CreateClassInstance<DataBoxMainMenuExtension>(_extensionAssembly, settings.MainMenuExtentionFullTypeName);
            _crudInterceptor.SetDataBoxManager(dataBoxManager);
            _mainMenuExtension.SetDataBoxManager(dataBoxManager);
        }

        #endregion //Methods
    }
}