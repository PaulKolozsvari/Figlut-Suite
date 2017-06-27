namespace Figlut.Mobile.DataBox.Utilities
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Web.Client;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using Figlut.Mobile.Toolkit.Utilities.Serialization;
    using System.Net;
    using Figlut.Mobile.Toolkit.Web.Client.FiglutWebService;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;
    using System.Reflection;
    using Figlut.Mobile.Toolkit.Extensions.DataBox;
    using Figlut.Mobile.Toolkit.Extensions.DataBox.Managers;
    using Figlut.Mobile.Toolkit.Data;
    using System.IO;
    using System.Drawing;
    using Figlut.Mobile.Toolkit.WM.Data;
    using Figlut.Mobile.Toolkit.WM.Data.DB.SQLCE;

    #endregion //Using Directives

    public class FiglutDataBoxApplication
    {
        #region Signleton Setup

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

        #endregion //Signleton Setup

        #region Constructors

        private FiglutDataBoxApplication()
        {
        }

        #endregion //Constructors

        #region Fields

        private string _applicationTitle;
        private string _applicationVersion;
        private Color _themeColor;
        private Color _dataBoxSelectRowColor;
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
            set { _applicationVersion = null; }
        }

        public Color ThemeColor
        {
            get { return _themeColor; }
            set { _themeColor = value; }
        }

        public Color DataBoxSelectRowColor
        {
            get { return _dataBoxSelectRowColor; }
            set { _dataBoxSelectRowColor = value; }
        }

        public Image ApplicationBannerImage
        {
            get { return _applicationBannerImage; }
            set { _applicationBannerImage = value; }
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
            FiglutMobileDataBoxSettings settings,
            bool wrapWebException,
            WaitProcess w)
        {
            GOC.Instance.ShowMessageBoxOnException = settings.ShowMessageBoxOnException;
            w.ChangeStatus("Initializing logger ...");
            GOC.Instance.Logger = new Logger(
                settings.LogToFile,
                settings.LogToWindowsEventLog,
                settings.LogFileName,
                settings.LoggingLevel,
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
            if (!settings.OfflineMode)
            {
                w.ChangeStatus("Acquiring schema ...");
                AcquireSchema(wrapWebException, Path.Combine(Information.GetExecutingDirectory(), settings.DatabaseSchemaFileName));
            }
            else
            {
                w.ChangeStatus(string.Format("Importing {0} ...", settings.DatabaseSchemaFileName));
                ImportSchemaFromFile(Path.Combine(Information.GetExecutingDirectory(), settings.DatabaseSchemaFileName));
            }
            w.ChangeStatus("Loading extensions ...");
            LoadExtensions(settings);
        }

        public void InitializeCustomizationParameters(FiglutMobileDataBoxSettings settings)
        {
            _applicationTitle = settings.ApplicationTitle;
            _applicationVersion = settings.ApplicationVersion;

            Dictionary<string, Color> systemColors = Information.GetSystemColors();
            _themeColor = string.IsNullOrEmpty(settings.ThemeColor) ? Color.SteelBlue : systemColors[settings.ThemeColor];
            _dataBoxSelectRowColor = string.IsNullOrEmpty(settings.DataBoxSelectRowColor) ? Color.SteelBlue : systemColors[settings.DataBoxSelectRowColor];

            string executingDirectory = Information.GetExecutingDirectory();
            string applicationBannerImageFilePath = Path.Combine(executingDirectory, settings.ApplicationBannerImageFileName);
            FileSystemHelper.ValidateFileExists(applicationBannerImageFilePath);
            _applicationBannerImage = new Bitmap(applicationBannerImageFilePath);
        }

        public void AcquireSchema(bool wrapWebException, string databaseSchemaFilePath)
        {
            SqlCeDatabase db = GOC.Instance.FiglutWebServiceClientSchema.GetSqlDatabase(true, true, null, wrapWebException);
            SqlDatabaseSchemaFile.ExportSchema(db, databaseSchemaFilePath);
            GOC.Instance.ClearMobileDatabases();
            GOC.Instance.AddDatabase<SqlCeDatabase>(db);
        }

        public void ImportSchemaFromFile(string databaseSchemaFilePath)
        {
            SqlCeDatabase db = SqlDatabaseSchemaFile.ImportSchema(databaseSchemaFilePath, true, true, null);
            GOC.Instance.ClearMobileDatabases();
            GOC.Instance.AddDatabase<SqlCeDatabase>(db);
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

        private void LoadExtensions(FiglutMobileDataBoxSettings settings)
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
                    EntityReader<FiglutMobileDataBoxSettings>.GetPropertyName(p => p.ExtensionAssemblyName, true)),
                    LoggingLevel.Minimum,
                    true);
            }
            string extensionAssemblyFilePath = Path.Combine(Information.GetExecutingDirectory(), settings.ExtensionAssemblyName);
            FileSystemHelper.ValidateFileExists(extensionAssemblyFilePath);
            _extensionAssembly = Assembly.LoadFrom(extensionAssemblyFilePath);
            _crudInterceptor = 
                string.IsNullOrEmpty(settings.CrudInterceptorFullTypeName) ?
                new DefaultDataBoxCrudInterceptor() :
                AssemblyReader.CreateClassInstance<DataBoxCrudInterceptor>(_extensionAssembly, settings.CrudInterceptorFullTypeName);
            _mainMenuExtension = 
                string.IsNullOrEmpty(settings.MainMenuExtentionFullTypeName) ?
                new DefaultDataBoxMainMenuExtension() :
                AssemblyReader.CreateClassInstance<DataBoxMainMenuExtension>(_extensionAssembly, settings.MainMenuExtentionFullTypeName);
        }

        #endregion //Methods
    }
}