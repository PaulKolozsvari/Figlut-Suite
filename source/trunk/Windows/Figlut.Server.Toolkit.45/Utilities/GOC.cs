namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Data;
    using System.Drawing;
    using Figlut.Server.Toolkit.Web;
    using System.Reflection;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Web.Client;
    using System.Diagnostics;
    using System.ServiceModel;
    using Figlut.Server.Toolkit.Web.Service;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;
using System.Data.Linq;
using System.Windows.Forms;
    using Figlut.Server.Toolkit.Utilities.Email;
    using System.Transactions;

    #endregion //Using Directives

    public class GOC : EntityCache<string, object>
    {
        #region Singleton Setup

        private static GOC _instance;

        public static GOC Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GOC();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private GOC()
        {
            _settingsCache = new SettingsCache();
            _webServiceCache = new WebServiceClientCache();
            _databaseCache = new DatabaseCache();
            _logger = new Logger();
            _xSerializer = new XSerializer();
            _jSerializer = new JSerializer();
            _csvSerializer = new CsvSerializer();
            _encoding = Encoding.Default;
        }

        #endregion //Constructors

        #region Fields

        protected string _applicationName;
        protected string _executableName;
        protected string _version;
        protected string _userAgent;
        protected SettingsCache _settingsCache;
        protected WebServiceClientCache _webServiceCache;
        protected DatabaseCache _databaseCache;
        protected Logger _logger;
        protected Image _logo;
        protected XSerializer _xSerializer;
        protected JSerializer _jSerializer;
        protected CsvSerializer _csvSerializer;
        protected bool _showWindowsMessageBoxOnException;
        protected bool _showWindowsFormsNotificationOnException;
        protected NotifyIcon _notifyIcon;
        protected Encoding _encoding;
        protected FiglutWebServiceClientWrapper _figlutWebServiceClient;
        protected FiglutWebServiceClientWrapper _figlutWebServiceClientSchema;

        protected Assembly _linqToClassesAssembly;
        protected string _linqToSQLClassesNamespace;
        protected Type _linqToSqlDataContextType;
        protected TransactionScopeOption _databaseTransactionScopeOption;
        protected IsolationLevel _databaseTransactionIsolationLevel;
        protected int _databaseTransactionTimeoutSeconds;
        protected int _databaseTransactionDeadlockRetryAttempts;
        protected int _transactionDeadlockRetryWaitPeriod;
        protected Type _userLinqToSqlType;
        protected Type _serverActionLinqToSqlType;
        protected Type _serverErrorLinqtoSqlType;

        #region Email

        protected bool _emailNotificationsEnabled;
        protected bool _throwEmailFailExceptions;
        protected EmailProvider _emailProvider;
        protected string _gMailSMTPServer;
        protected string _gMailSmtpUserName;
        protected string _gMailSmtpPassword;
        protected int _gMailSmtpPort;
        protected string _senderEmailAddress;
        protected string _senderDisplayName;
        protected string _exceptionEmailSubject;
        protected bool _emailExceptions;
        protected bool _appendHostNameToEmailBody;
        protected bool _appendWindowsUserNameToEmailBody;
        protected bool _logEmails;
        protected bool _includeDefaultEmailRecipients;
        protected List<EmailNotificationRecipient> _defaultEmailRecipients;
        protected EmailClient _emailClient;

        #endregion //Email

        #endregion //Fields

        #region Properties

        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        public string ExecutableName
        {
            get { return _executableName; }
            set { _executableName = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        public Logger Logger
        {
            get { return _logger; }
            set{_logger = value;}
        }

        public Image Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }

        public XSerializer XmlSerializer
        {
            get { return _xSerializer; }
            set { _xSerializer = value; }
        }

        public JSerializer JsonSerializer
        {
            get { return _jSerializer; }
            set { _jSerializer = value; }
        }

        public CsvSerializer CSVSerializer
        {
            get { return _csvSerializer; }
            set { _csvSerializer = value; }
        }

        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        public bool ShowMessageBoxOnException
        {
            get { return _showWindowsMessageBoxOnException; }
            set { _showWindowsMessageBoxOnException = value; }
        }

        public bool ShowWindowsFormsNotificationOnException
        {
            get { return _showWindowsFormsNotificationOnException; }
            set { _showWindowsFormsNotificationOnException = value; }
        }

        public NotifyIcon NotifyIcon
        {
            get { return _notifyIcon; }
            set { _notifyIcon = value; }
        }

        public FiglutWebServiceClientWrapper FiglutWebServiceClient
        {
            get { return _figlutWebServiceClient; }
            set { _figlutWebServiceClient = value; }
        }

        public FiglutWebServiceClientWrapper FiglutWebServiceClientSchema
        {
            get { return _figlutWebServiceClientSchema; }
            set { _figlutWebServiceClientSchema = value; }
        }

        public Assembly LinqToClassesAssembly
        {
            get { return _linqToClassesAssembly; }
            set { _linqToClassesAssembly = value; }
        }

        public string LinqToSQLClassesNamespace
        {
            get { return _linqToSQLClassesNamespace; }
            set { _linqToSQLClassesNamespace = value; }
        }

        public TransactionScopeOption DatabaseTransactionScopeOption
        {
            get { return _databaseTransactionScopeOption; }
            set { _databaseTransactionScopeOption = value; }
        }
        public IsolationLevel DatabaseTransactionIsolationLevel
        {
            get { return _databaseTransactionIsolationLevel; }
            set { _databaseTransactionIsolationLevel = value; }
        }

        public int DatabaseTransactionTimeoutSeconds
        {
            get { return _databaseTransactionTimeoutSeconds; }
            set { _databaseTransactionTimeoutSeconds = value; }
        }

        public int DatabaseTransactionDeadlockRetryAttempts
        {
            get { return _databaseTransactionDeadlockRetryAttempts; }
            set { _databaseTransactionDeadlockRetryAttempts = value; }
        }

        public int DatabaseTransactionDeadlockRetryWaitPeriod
        {
            get { return _transactionDeadlockRetryWaitPeriod; }
            set { _transactionDeadlockRetryWaitPeriod = value; }
        }

        public Type UserLinqToSqlType
        {
            get { return _userLinqToSqlType; }
            set { _userLinqToSqlType = value; }
        }

        public Type ServerActionLinqToSqlType
        {
            get { return _serverActionLinqToSqlType; }
            set { _serverActionLinqToSqlType = value; }
        }

        public Type ServerErrorLinqToSqlType
        {
            get { return _serverErrorLinqtoSqlType; }
            set { _serverErrorLinqtoSqlType = value; }
        }

        #region Email

        public bool SendEmailOnException
        {
            get { return _emailExceptions; }
            set { _emailExceptions = value; }
        }

        public bool AppendHostNameToExceptionEmails
        {
            get { return _appendHostNameToEmailBody; }
            set { _appendHostNameToEmailBody = value; }
        }

        public bool AppendWindowsUserNameToEmailBody
        {
            get { return _appendWindowsUserNameToEmailBody; }
            set { _appendWindowsUserNameToEmailBody = value; }
        }

        public EmailClient EmailClient
        {
            get { return _emailClient; }
            set { _emailClient = value; }
        }

        #endregion //Email

        #endregion //Properties

        #region Methods

        public void AddByTypeName(object e)
        {
            string id = e.GetType().Name;
            if (Exists(id))
            {
                throw new ArgumentException(string.Format(
                    "Already added entity with type name {0}.", 
                    id));
            }
            Add(id, e);
        }

        public void DeleteByTypeName<E>()
        {
            string id = typeof(E).Name;
            if (!Exists(id))
            {
                throw new ArgumentException(string.Format(
                    "Could not find entity with type name {0} to delete.",
                    id));
            }
            Delete(id);
        }

        public bool ExistsByTypeName<E>()
        {
            string id = typeof(E).Name;
            return Exists(id);
        }

        public E GetByTypeName<E>()
        {
            string id = typeof(E).Name;
            if (!Exists(id))
            {
                return default(E);
            }
            return (E)this[id];
        }

        public S GetSettings<S>() where S : Settings
        {
            return GetSettings<S>(false, false);
        }

        public S GetSettings<S>(bool refreshFromFile, bool validateAllSettingValuesSet) where S : Settings
        {
            string cacheId = typeof(S).Name;
            S result;
            if (_settingsCache.Exists(cacheId))
            {
                result = (S)_settingsCache[cacheId];
            }
            else
            {
                result = Activator.CreateInstance<S>();
                _settingsCache.Add(cacheId, result);
            }
            if (refreshFromFile)
            {
                //The validateAllSettingValuesSet check is done when when reading the settings file.
                result.RefreshFromFile(true, validateAllSettingValuesSet);
            }
            else if(validateAllSettingValuesSet)
            {
                foreach (PropertyInfo p in typeof(S).GetProperties())
                {
                    object value = p.GetValue(result, null);
                    if (value == null)
                    {
                        throw new NullReferenceException(string.Format(
                            "{0} not set in {1}.",
                            p.Name,
                            result.FilePath));
                    }
                }
            }
            return result;
        }

        public W GetWebService<W>() where W : IWebService
        {
            return GetWebService<W>(null);
        }

        public W GetWebService<W>(bool validateBaseUrlIsSet) where W : IWebService
        {
            W result = GetWebService<W>(null);
            if (validateBaseUrlIsSet && string.IsNullOrEmpty(result.WebServiceBaseUrl))
            {
                throw new NullReferenceException(string.Format("Web Service Base URL for {0} is not set.", typeof(W).Name));
            }
            return result;
        }

        public W GetWebService<W>(string webServiceBaseUrl) where W : IWebService
        {
            string webServiceId = typeof(W).Name;
            W result;
            if (_webServiceCache.Exists(webServiceId))
            {
                result = (W)_webServiceCache[webServiceId];
            }
            else
            {
                result = Activator.CreateInstance<W>();
                _webServiceCache.Add(webServiceId, result);
            }
            if (!string.IsNullOrEmpty(webServiceBaseUrl))
            {
                result.WebServiceBaseUrl = webServiceBaseUrl;
            }
            return result;
        }

        public void AddDatabase<D>(D database) where D : Database
        {
            if (database == null)
            {
                throw new NullReferenceException(string.Format(
                    "Database to be added to {0} may not be null.",
                    this.GetType().FullName));
            }
            string databaseId = typeof(D).Name;
            if (_databaseCache.Exists(databaseId))
            {
                throw new ArgumentException(string.Format(
                    "Database with name {0} cannot be added to {1} as it has already been added.",
                    database.Name,
                    this.GetType().FullName));
            }
            else
            {
                _databaseCache.Add(databaseId, database);
            }
        }

        public void DeleteDatabase(string databaseId)
        {
            _databaseCache.Delete(databaseId);
        }

        public Database GetDatabase(string databaseId)
        {
            return _databaseCache[databaseId];
        }

        public D GetDatabase<D>() where D : Database
        {
            string databaseId = typeof(D).Name;
            D result;
            if (_databaseCache.Exists(databaseId))
            {
                result = (D)_databaseCache[databaseId];
            }
            else
            {
                result = Activator.CreateInstance<D>();
                _databaseCache.Add(result);
            }
            return result;
        }

        public void ClearDatabases()
        {
            _databaseCache.Clear();
        }

        public ISerializer GetSerializer(SerializerType serializerType)
        {
            switch (serializerType)
            {
                case SerializerType.XML:
                    return _xSerializer;
                case SerializerType.JSON:
                    return _jSerializer;
                case SerializerType.CSV:
                    return _csvSerializer;
                default:
                    throw new ArgumentException(string.Format(
                        "{0} is not a valid {1}.",
                        serializerType.ToString(),
                        serializerType.GetType().FullName));
            }
        }

        public void SetEncoding(TextEncodingType textEncodingType)
        {
            switch (textEncodingType)
            {
                case TextEncodingType.UTF8:
                    _encoding = Encoding.UTF8;
                    break;
                case TextEncodingType.ASCII:
                    _encoding = Encoding.ASCII;
                    break;
                case TextEncodingType.BigEndianUnicode:
                    _encoding = Encoding.BigEndianUnicode;
                    break;
                case TextEncodingType.Default:
                    _encoding = Encoding.Default;
                    break;
                case TextEncodingType.Unicode:
                    _encoding = Encoding.Unicode;
                    break;
                case TextEncodingType.UTF32:
                    _encoding = Encoding.UTF32;
                    break;
                case TextEncodingType.UTF7:
                    _encoding = Encoding.UTF7;
                    break;
                default:
                    break;
            }
        }

        public void SetLinqToSqlDataContextType<T>() where T : DataContext
        {
            _linqToSqlDataContextType = typeof(T);
        }

        public DataContext GetNewLinqToSqlDataContext()
        {
            return (DataContext)Activator.CreateInstance(_linqToSqlDataContextType);
        }

        #endregion //Methods
    }
}