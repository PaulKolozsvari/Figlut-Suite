namespace Figlut.Web.Service.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Data.QueryRunners;
    using Figlut.Server.Toolkit.Extensions.WebService;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Web.Service.Configuration;

    #endregion //Using Directives

    public class FiglutWebServiceApplication
    {
        #region Singleton Setup

        private static FiglutWebServiceApplication _instance;

        public static FiglutWebServiceApplication Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FiglutWebServiceApplication();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private FiglutWebServiceApplication()
        {
        }

        #endregion //Constructors

        #region Fields

        private Assembly _extensionAssembly;
        private WebServiceCrudInterceptor _crudInterceptor;
        private WebServiceExtension _extension;

        #endregion //Fields

        #region Properties

        public Assembly ExtensionAssembly
        {
            get { return _extensionAssembly; }
        }

        public WebServiceCrudInterceptor CrudInterceptor
        {
            get { return _crudInterceptor; }
        }

        public WebServiceExtension Extension
        {
            get { return _extension; }
        }

        #endregion //Properties

        #region Methods

        internal void Initialize(FiglutWebServiceSettings settings)
        {
            GOC.Instance.Logger = new Logger(
                settings.LogToFile,
                settings.LogToWindowsEventLog,
                settings.LogToConsole,
                settings.LoggingLevel,
                settings.LogFileName,
                settings.EventSourceName,
                settings.EventLogName);
            GOC.Instance.JsonSerializer.IncludeOrmTypeNamesInJsonResponse =
                settings.IncludeOrmTypeNamesInJsonResponse;
            GOC.Instance.SetEncoding(settings.TextResponseEncoding);
            InitializeDatabase(settings);
            InitializeServiceHost(settings);
            InitializeQueryRunnerAssembly(settings);
            LoadExtensions(settings);
        }

        private void InitializeDatabase(FiglutWebServiceSettings settings)
        {
            SqlDatabase db = GOC.Instance.GetDatabase<SqlDatabase>();
            db.Initialize(
                settings.DatabaseConnectionString,
                true,
                true,
                settings.SaveOrmAssembly,
                settings.OrmAssemblyOutputDirectory,
                true);
            if (settings.SaveOrmAssembly && !File.Exists(db.GetOrmAssembly().AssemblyFilePath))
            {
                throw new FileNotFoundException(string.Format(
                    "ORM assembly created successfully, but could not be saved to {0}.",
                    db.GetOrmAssembly().AssemblyFilePath));
            }
            string message = settings.SaveOrmAssembly ?
                string.Format("Database {0} initialized. ORM assembly saved to {1}.", db.Name, db.GetOrmAssembly().AssemblyFilePath) :
                string.Format("Database {0} initialized.", db.Name);
            GOC.Instance.Logger.LogMessage(new LogMessage(
                message,
                LogMessageType.Information,
                LoggingLevel.Normal));
        }

        private void InitializeServiceHost(FiglutWebServiceSettings settings)
        {
            WebHttpBinding binding = new WebHttpBinding()
            {
                MaxBufferPoolSize = settings.MaxBufferPoolSize,
                MaxBufferSize = Convert.ToInt32(settings.MaxBufferSize),
                MaxReceivedMessageSize = settings.MaxReceivedMessageSize
            };
            if (settings.UseAuthentication)
            {
                binding.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            }
            ServiceHost serviceHost = new ServiceHost(typeof(RestService));
            string address = string.Format(
                "http://127.0.0.1:{0}/{1}",
                settings.PortNumber,
                settings.HostAddressSuffix);
            ServiceDebugBehavior debugBehaviour = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (debugBehaviour == null) //This should never be the case, but it might be so. Better safe than sorry.
            {
                debugBehaviour = new ServiceDebugBehavior();
                serviceHost.Description.Behaviors.Add(debugBehaviour);
            }
            debugBehaviour.IncludeExceptionDetailInFaults = settings.IncludeExceptionDetailInResponse;
            serviceHost.AddServiceEndpoint(typeof(RestService), binding, address).Behaviors.Add(new WebHttpBehavior());
            if (GOC.Instance.GetByTypeName<ServiceHost>() != null)
            {
                GOC.Instance.DeleteByTypeName<ServiceHost>();
            }
            GOC.Instance.AddByTypeName(serviceHost);
            serviceHost.Open();

            GOC.Instance.Logger.LogMessage(new LogMessage(
                string.Format("Figlut Web Service started at address {0}.", address),
                LogMessageType.Information,
                LoggingLevel.Minimum));
        }

        private void InitializeQueryRunnerAssembly(FiglutWebServiceSettings settings)
        {
            GOC.Instance.AddByTypeName(new SqlQueryRunnerConfig(
                Path.Combine(Information.GetExecutingDirectory(), settings.QueryRunnnerAssemblyName),
                settings.SqlQueryRunnerFullTypeName));
        }

        private void LoadExtensions(FiglutWebServiceSettings settings)
        {
            if (!settings.UseExtensions)
            {
                _crudInterceptor = new DefaultWebServiceCrudInterceptor();
                return;
            }
            if (string.IsNullOrEmpty(settings.ExtensionAssemblyName))
            {
                throw new UserThrownException(
                    string.Format(
                    "Cannot load extension. {0} not configured.",
                    EntityReader<FiglutWebServiceSettings>.GetPropertyName(p => p.ExtensionAssemblyName, true)),
                    LoggingLevel.Minimum,
                    true);
            }
            if (string.IsNullOrEmpty(settings.CrudInterceptorFullTypeName))
            {
                throw new UserThrownException(
                    string.Format(
                    "Cannot load extension. {0} not configured.",
                    EntityReader<FiglutWebServiceSettings>.GetPropertyName(p => p.CrudInterceptorFullTypeName, true)),
                    LoggingLevel.Minimum,
                    true);
            }
            if (string.IsNullOrEmpty(settings.ExtensionFullTypeName))
            {
                throw new UserThrownException(
                    string.Format(
                    "Cannot load extension. {0} not configured.",
                    EntityReader<FiglutWebServiceSettings>.GetPropertyName(p => p.ExtensionFullTypeName, true)),
                    LoggingLevel.Minimum,
                    true);
            }
            string extensionAssemblyFilePath = Path.Combine(Information.GetExecutingDirectory(), settings.ExtensionAssemblyName);
            FileSystemHelper.ValidateFileExists(extensionAssemblyFilePath);
            _extensionAssembly = Assembly.LoadFile(extensionAssemblyFilePath);
            _crudInterceptor = AssemblyReader.CreateClassInstance<WebServiceCrudInterceptor>(_extensionAssembly, settings.CrudInterceptorFullTypeName);
            _extension = AssemblyReader.CreateClassInstance<WebServiceExtension>(_extensionAssembly, settings.ExtensionFullTypeName);
        }

        #endregion //Methods
    }
}
