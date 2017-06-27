namespace Figlut.Server
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Collections.Specialized;
    using System.Collections;
    using Figlut.Server.Toolkit.Web;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Web.Client;
    using Figlut.Web.Service.Configuration;
    using System.Net;
    using System.Reflection;
    using Figlut.Web.Service.Utilities;
    using Microsoft.Win32;
    using Figlut.Server.Toolkit.Mmc;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    [ServiceContract]
    public partial class RestService
    {
        [OperationContract]
        [WebGet(UriTemplate = "*")]
        Stream AllURIs()
        {
            try
            {
                return StreamHelper.GetStreamFromString(GetAboutInfo(), GOC.Instance.Encoding);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        #region Constructors

        public RestService()
        {
            _settings = GOC.Instance.GetSettings<FiglutWebServiceSettings>();
        }

        #endregion //Constructors

        #region Constants

        internal const string CONFIGURATION_MANAGER_INSTALLATION_DIRECTORY_REG_NAME = "FiglutConfigurationManagerInstallationDirectory";
        internal const string WEB_SERVICE_INSTALLATION_DIRECTORY_REG_NAME = "FiglutWebServiceInstallationDirectory";
        internal const string DESKTOP_DATABOX_INSTALLATION_DIRECTORY_REG_NAME = "FiglutDesktopDataBoxInstallationDirectory";
        internal const string SERVER_TOOLKIT_INSTALLATION_DIRECTORY_REG_NAME = "FiglutServerToolkitInstallationDirectory";
        internal const string MOBILE_TOOLKIT_INSTALLATION_DIRECTRORY_REG_NAME = "FiglutMobileToolkitWMInstallationDirectory";

        #endregion //Constants

        #region Fields

        private FiglutWebServiceSettings _settings;

        #endregion //Fields

        #region Methods

        private string GetAboutInfo()
        {
            string authenticationType = "No security context.";
            if (OperationContext.Current.ServiceSecurityContext != null)
            {
                authenticationType =
                    OperationContext.Current.ServiceSecurityContext.IsAnonymous ?
                    "Anonymous" :
                    OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.AuthenticationType;
            }
            string configManagerDirectory = GetConfigurationManagerDirectory();
            string configManagerExePath = null;
            string configManagerMscFile = null;
            if (!string.IsNullOrEmpty(configManagerDirectory))
            {
                configManagerExePath = GetConfigurationManagerFileName(configManagerDirectory, "*.exe");
                //configManagerMscFile = GetConfigurationManagerFileName(configManagerDirectory, "*.msc");
            }

            StringBuilder infoMessage = new StringBuilder();
            infoMessage.AppendLine(AboutInfo.AssemblyTitle);
            infoMessage.AppendLine();
            infoMessage.AppendLine(string.Format("\tProduct: {0}", AboutInfo.AssemblyProduct));
            infoMessage.AppendLine(string.Format("\tVersion: {0}", AboutInfo.AssemblyVersion));
            infoMessage.AppendLine(string.Format("\tCopyright: {0}", AboutInfo.AssemblyCopyright));
            infoMessage.AppendLine(string.Format("\tCompany : {0}", AboutInfo.AssemblyCompany));
            infoMessage.AppendLine(string.Format("\tDescription: {0}", AboutInfo.AssemblyDescription));
            infoMessage.AppendLine();
            infoMessage.AppendLine(string.Format("Authentication Type: {0}", authenticationType));
            infoMessage.AppendLine();
            infoMessage.AppendLine("Configuration:");
            infoMessage.AppendLine();
            if (!string.IsNullOrEmpty(configManagerDirectory))
            {
                infoMessage.AppendLine(string.Format("\tFiglut Configuration Manager: {0}", configManagerDirectory));
                infoMessage.AppendLine("\tExecute:");
                infoMessage.AppendLine();
                infoMessage.AppendLine(string.Format("\t*    {0} (Windows application)", configManagerExePath));
                //infoMessage.AppendLine(string.Format("\t*    {0} (MMC snap-in)", configManagerMscFile));
            }
            else
            {
                infoMessage.AppendLine("\t*    Figlut Configuration Manager: <not installed>");
            }
            infoMessage.AppendLine();

            #region Settings

            EntityCache<string, SettingItem> databaseSettings = _settings.GetSettingsByCategory(new SettingsCategoryInfo(_settings, "Database"));
            EntityCache<string, SettingItem> serviceSettings = _settings.GetSettingsByCategory(new SettingsCategoryInfo(_settings, "Service"));
            EntityCache<string, SettingItem> loggingSettings = _settings.GetSettingsByCategory(new SettingsCategoryInfo(_settings, "Logging"));
            string settingName = EntityReader<SettingItem>.GetPropertyName(p => p.SettingName, false);
            List<string> hiddenSettings = new List<string>()
            {
                EntityReader<FiglutWebServiceSettings>.GetPropertyName(p => p.DatabaseUserName, false),
                EntityReader<FiglutWebServiceSettings>.GetPropertyName(p => p.DatabaseUserPassword, false),
                EntityReader<FiglutWebServiceSettings>.GetPropertyName(p => p.DatabaseUserPassword, false),
                EntityReader<FiglutWebServiceSettings>.GetPropertyName(p => p.DatabaseConnectionString, false)
            };
            infoMessage.AppendLine("Settings:");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\tDatabase:");
            infoMessage.AppendLine();
            foreach (SettingItem s in databaseSettings)
            {
                if (!hiddenSettings.Contains(s.SettingName))
                {
                    infoMessage.AppendLine(string.Format("\t*    {0} : {1}", s.SettingDisplayName, s.SettingValue.ToString()));
                }
            }
            infoMessage.AppendLine();
            infoMessage.AppendLine("\tService:");
            infoMessage.AppendLine();
            foreach (SettingItem s in serviceSettings)
            {
                if (!hiddenSettings.Contains(s.SettingName))
                {
                    infoMessage.AppendLine(string.Format("\t*    {0} : {1}", s.SettingDisplayName, s.SettingValue.ToString()));
                }
            }
            infoMessage.AppendLine();
            infoMessage.AppendLine("\tLogging:");
            infoMessage.AppendLine();
            foreach (SettingItem s in loggingSettings)
            {
                if (!hiddenSettings.Contains(s.SettingName))
                {
                    infoMessage.AppendLine(string.Format("\t*    {0} : {1}", s.SettingDisplayName, s.SettingValue.ToString()));
                }
            }

            #endregion //Settings

            #region Usage

            infoMessage.AppendLine();
            infoMessage.AppendLine("Usage:");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\tBasic SQL Table CRUD: A list/array of entities must be used with the entity & property names matching those of a table and its columns.");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t*    Create: POST entities to '/sqltable/{tableName}' where {tableName} is the name of DB table.");
            infoMessage.AppendLine("\t*    Read: GET entities from '/sqltable/{tableName}'.");
            infoMessage.AppendLine("\t*    Update: PUT entities to to '/sqltable/{tableName}'.");
            infoMessage.AppendLine("\t*    Delete: DELETE entities to '/sqltable/{tableName}'.");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t     e.g. http://localhost:2983/Figlut/sqltable/Order");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\tFiltered Read:");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t*    Read: GET entities from '{sqltable/{tableName}/{filters}' where {filters} applies a where clause to the generated SELECT statements.");
            infoMessage.AppendLine("\t*    Filter Format (comma  separated): {columnName}_{comparisonOperator}_{columnName}_{logicalOperator}, ...");
            infoMessage.AppendLine("\t*    Comparison Operators: =, >, <, >=, <=, <>, !=, !<, !>, IS, IS_NOT");
            infoMessage.AppendLine("\t*    Logical Operators: ALL, AND, ANY, BETWEEN, EXISTS, IN, LIKE, NOT, OR, SOME");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t     e.g. http://localhost:2983/Figlut/Order/OrderNumber_=_KMI1983_AND,UserStatusCode_<_5");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\tRaw SQL: raw sql statements/scripts may be sent to the service to be executed against the target DB.");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t*    Execute: PUT SQL to '/sql' where no results are returned e.g. INSERT/UPDATE/DELETE statements.");
            infoMessage.AppendLine("\t*    Execute: PUT SQL to '/sql/{typeName}' where {typeName} is the name of the entities to be generated and returned e.g. SELECT statements.");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\tContent Types: The content type can be specified either in the web request's 'Content-Type' and 'Accept' parameters or as the last parameter in the URL.");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t\tSpecified in web request:");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t\t*    XML : text/plain/xml");
            infoMessage.AppendLine("\t\t*    JSON : text/plain/json");
            infoMessage.AppendLine("\t\t*    CSV : text/plain/csv");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t\t     e.g (request header)    User-Agent: Fiddler");
            infoMessage.AppendLine("\t\t                             Host: localhost:2983");
            infoMessage.AppendLine("\t\t                             Content-Type: text/plain/xml");
            infoMessage.AppendLine("\t\t                             Accept: text/plain/xml");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t\tAppended in URL (overrides 'Content-Type' and 'Accept' if also specified in web request):");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t\t*    XML : /xml");
            infoMessage.AppendLine("\t\t*    JSON : /json");
            infoMessage.AppendLine("\t\t*    CSV : /csv");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t\t     e.g. http://localhost:2983/Figlut/sqltable/Order/xml");
            infoMessage.AppendLine();
            infoMessage.AppendLine("\t\tN.B. 'Content-Type' relates to the content being sent to the service and 'Accept' relates to the content it responds with.");
            infoMessage.AppendLine("\t\t     'Content-Type' and 'Accept' default to JSON if either is not specified.");

            #endregion //Usage
            
            return infoMessage.ToString();
        }

        private HttpVerb GetHttpVerb()
        {
            HttpVerb result;
            if (!Enum.TryParse<HttpVerb>(WebOperationContext.Current.IncomingRequest.Method, true, out result))
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.MethodNotAllowed;
                throw new Exception(string.Format("Invalid METHOD {0}.", WebOperationContext.Current.IncomingRequest.Method));
            }
            return result;
        }

        private string GetConfigurationManagerDirectory()
        {
            using (RegistryKey figlutKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Figlut", RegistryKeyPermissionCheck.ReadSubTree, System.Security.AccessControl.RegistryRights.ReadKey))
            {
                if (figlutKey == null)
                {
                    //throw new UserThrownException("No Figlut components installed.", LoggingLevel.Minimum, true);
                    return null;
                }
                object value = figlutKey.GetValue(CONFIGURATION_MANAGER_INSTALLATION_DIRECTORY_REG_NAME);
                return value == null ? null : value.ToString();
            }
        }

        private string GetConfigurationManagerFileName(string configManagerDirectory, string extensionSearchPattern)
        {
            FileSystemHelper.ValidateDirectoryExists(configManagerDirectory);
            string[] files = Directory.GetFiles(configManagerDirectory, extensionSearchPattern);
            if (files.Length < 1)
            {
                throw new UserThrownException(
                    string.Format("Could not find any files with the {0} extension in directory {1}.", extensionSearchPattern, configManagerDirectory),
                    LoggingLevel.Minimum,
                    false);
            }
            if (files.Length > 1)
            {
                throw new UserThrownException(
                    string.Format("More than one file with the extension {0} in directory {1}.", extensionSearchPattern, configManagerDirectory),
                    LoggingLevel.Minimum,
                    false);
            }
            return Path.GetFileName(files[0]);
        }

        private void UpdateHttpStatusOnException(Exception ex)
        {
            WebOperationContext context = WebOperationContext.Current;
            if (context.OutgoingResponse.StatusCode == HttpStatusCode.OK ||
                context.OutgoingResponse.StatusCode == HttpStatusCode.Created)
            {
                context.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            /*A status description with escape sequences causes the server to not respond to the request 
            causing the client to not get a response therefore not know what the exception was.*/
            string errorMessage = ex.Message.Replace("\r", string.Empty);
            errorMessage = errorMessage.Replace("\n", string.Empty);
            errorMessage = errorMessage.Replace("\t", string.Empty);
            context.OutgoingResponse.StatusDescription = errorMessage;
        }

        private void LogRequest()
        {
            string requestUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
            string accept = WebOperationContext.Current.IncomingRequest.Accept;
            string userAgent = WebOperationContext.Current.IncomingRequest.UserAgent;
            GOC.Instance.Logger.LogMessage(new LogMessage(
                string.Format("Request URI: {0}, Accept: {1}, User Agent: {2}", requestUri, accept, userAgent),
                LogMessageType.Information,
                LoggingLevel.Maximum));
            FiglutClientManager.Instance.ManageClient(userAgent, requestUri);
        }

        private void LogResponse(string responseText)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Response:");
            message.AppendLine(responseText);
            GOC.Instance.Logger.LogMessage(new LogMessage(
                message.ToString(), 
                LogMessageType.Information, 
                LoggingLevel.Maximum));
        }

        private void ValidateRequestMethod(HttpVerb verb)
        {
            if (WebOperationContext.Current.IncomingRequest.Method != verb.ToString())
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
                throw new UserThrownException(
                    string.Format(
                    "Unexpected Method of {0} on incoming POST Request {1}.",
                    WebOperationContext.Current.IncomingRequest.Method,
                    WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString()),
                    LoggingLevel.Normal);
            }
        }

        private void ValidateTable(DatabaseTable table, string tableName, string databaseName)
        {
            if (table == null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                throw new UserThrownException(
                    string.Format("Could not find table {0} in database {1}.", tableName, databaseName),
                    LoggingLevel.Maximum);
            }
        }

        private void GetSerialisers(out ISerializer inputSerializer, out ISerializer outputSerializer)
        {
            if (string.IsNullOrEmpty(WebOperationContext.Current.IncomingRequest.ContentType))
            {
                inputSerializer = GOC.Instance.GetSerializer(SerializerType.JSON);
            }
            else
            {
                switch (WebOperationContext.Current.IncomingRequest.ContentType)
                {
                    case MimeContentType.TEXT_PLAIN_XML:
                        inputSerializer = GOC.Instance.GetSerializer(SerializerType.XML);
                        break;
                    case MimeContentType.TEXT_PLAIN_JSON:
                        inputSerializer = GOC.Instance.GetSerializer(SerializerType.JSON);
                        break;
                    case MimeContentType.TEXT_PLAIN_CSV:
                        inputSerializer = GOC.Instance.GetSerializer(SerializerType.CSV);
                        break;
                    default:
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.UnsupportedMediaType;
                        throw new UserThrownException(string.Format(
                            "Invalid Content type: {0}. Only {1} or {2} allowed.",
                            WebOperationContext.Current.IncomingRequest.ContentType,
                            MimeContentType.TEXT_PLAIN_XML,
                            MimeContentType.TEXT_PLAIN_JSON));
                }
            }
            if (string.IsNullOrEmpty(WebOperationContext.Current.IncomingRequest.Accept))
            {
                outputSerializer = GOC.Instance.GetSerializer(SerializerType.JSON);
            }
            else
            {
                switch (WebOperationContext.Current.IncomingRequest.Accept)
                {
                    case MimeContentType.TEXT_PLAIN_XML:
                        outputSerializer = GOC.Instance.GetSerializer(SerializerType.XML);
                        break;
                    case MimeContentType.TEXT_PLAIN_JSON:
                        outputSerializer = GOC.Instance.GetSerializer(SerializerType.JSON);
                        break;
                    case MimeContentType.TEXT_PLAIN_CSV:
                        outputSerializer = GOC.Instance.GetSerializer(SerializerType.CSV);
                        break;
                    default:
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotAcceptable;
                        throw new UserThrownException(string.Format(
                            "Invalid Accept type: {0}. Only {1}, {2} or {3} allowed.",
                            WebOperationContext.Current.IncomingRequest.Accept,
                            MimeContentType.TEXT_PLAIN_XML,
                            MimeContentType.TEXT_PLAIN_JSON,
                            MimeContentType.TEXT_PLAIN_CSV));
                }
            }
        }

        public Stream GetStreamFromObjects(List<object> objects, Type dotNetType, ISerializer serializer)
        {
            Array result = DataHelper.ConvertObjectsToTypedArray(dotNetType, objects);
            WebOperationContext.Current.OutgoingResponse.ContentType = MimeContentType.TEXT_PLAIN;
            if (serializer is JSerializer)
            {
                ((JSerializer)serializer).IncludeOrmTypeNamesInJsonResponse = _settings.IncludeOrmTypeNamesInJsonResponse;
            }
            string responseText = serializer.SerializeToText(result, new Type[] { dotNetType, result.GetType() });
            LogResponse(responseText);
            return StreamHelper.GetStreamFromString(responseText, GOC.Instance.Encoding);
        }

        #endregion //Methods
    }
}