namespace Figlut.Mobile.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Mobile.Toolkit.Data;
    using System.Drawing;
    using Figlut.Mobile.Toolkit.Web;
    using System.Reflection;
    using Figlut.Mobile.Toolkit.Data.DB;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using Figlut.Mobile.Toolkit.Utilities.Serialization;
    using Figlut.Mobile.Toolkit.Web.Client;
    using Figlut.Mobile.Toolkit.Utilities.SettingsFile;
    using Figlut.Mobile.Toolkit.Web.Client.FiglutWebService;

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
            _mobileDatabaseCache = new MobileDatabaseCache();
            _logger = new Logger();
            _xSerializer = new XSerializer();
            _jSerializer = new JSerializer();
            _encoding = Encoding.Default;
        }

        #endregion //Constructors

        #region Fields

        protected string _executableName;
        protected string _version;
        protected string _userAgent;
        protected SettingsCache _settingsCache;
        protected WebServiceClientCache _webServiceCache;
        protected MobileDatabaseCache _mobileDatabaseCache;
        protected Logger _logger;
        protected Image _logo;
        protected XSerializer _xSerializer;
        protected JSerializer _jSerializer;
        protected CsvSerializer _csvSerializer;
        protected bool _showWindowsMessageBoxOnException;
        protected Encoding _encoding;
        protected FiglutWebServiceClientWrapper _figlutWebServiceClient;
        protected FiglutWebServiceClientWrapper _figlutWebServiceClientSchema;

        #endregion //Fields

        #region Properties

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
            set { _logger = value; }
        }

        public Image Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }

        public XSerializer XMLSerializer
        {
            get { return _xSerializer; }
            set { _xSerializer = value; }
        }

        public JSerializer JSONSerializer
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
            W result = default(W);
            if (_webServiceCache.Exists(webServiceId))
            {
                result = (W)_webServiceCache[webServiceId];
            }
            else
            {
                if (!string.IsNullOrEmpty(webServiceBaseUrl))
                {
                    result = Instantiator<W>.New(webServiceBaseUrl);
                }
                else
                {
                    result = Activator.CreateInstance<W>();
                }
                _webServiceCache.Add(webServiceId, result);
            }
            return result;
        }

        public void AddDatabase<D>(D database) where D : MobileDatabase
        {
            if (database == null)
            {
                throw new NullReferenceException(string.Format(
                    "Database to be added to {0} may not be null.",
                    this.GetType().FullName));
            }
            string databaseId = typeof(D).Name;
            if (_mobileDatabaseCache.Exists(databaseId))
            {
                throw new ArgumentException(string.Format(
                    "Database with name {0} cannot be added to {1} as it has already been added.",
                    database.Name,
                    this.GetType().FullName));
            }
            else
            {
                _mobileDatabaseCache.Add(databaseId, database);
            }
        }

        public void DeleteDatabase(string databaseId)
        {
            _mobileDatabaseCache.Delete(databaseId);
        }

        public MobileDatabase GetDatabase(string databaseId)
        {
            return _mobileDatabaseCache[databaseId];
        }

        public D GetDatabase<D>() where D : MobileDatabase
        {
            string databaseId = typeof(D).Name;
            D result;
            if (_mobileDatabaseCache.Exists(databaseId))
            {
                result = (D)_mobileDatabaseCache[databaseId];
            }
            else
            {
                result = Activator.CreateInstance<D>();
                _mobileDatabaseCache.Add(result);
            }
            return result;
        }

        public D GetMobileDatabase<D>() where D : MobileDatabase
        {
            string mobileDatabaseId = typeof(D).Name;
            D result;
            if (_mobileDatabaseCache.Exists(mobileDatabaseId))
            {
                result = (D)_mobileDatabaseCache[mobileDatabaseId];
            }
            else
            {
                result = Activator.CreateInstance<D>();
                _mobileDatabaseCache.Add(result);
            }
            return result;
        }

        public void ClearMobileDatabases()
        {
            _mobileDatabaseCache.Clear();
        }

        public ISerializer GetSerializer(SerializerType serializerType)
        {
            switch (serializerType)
            {
                case SerializerType.XML:
                    return _xSerializer;
                case SerializerType.JSON:
                    return _jSerializer;
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
                    throw new ArgumentException(string.Format(
                        ".NET Compact Framework does not support {0}.",
                        textEncodingType.ToString()));
                case TextEncodingType.UTF7:
                    _encoding = Encoding.UTF7;
                    break;
                default:
                    throw new ArgumentException(string.Format(
                        "Invalid encoding {0}.",
                        textEncodingType.ToString()));
            }
        }

        #endregion //Methods
    }
}