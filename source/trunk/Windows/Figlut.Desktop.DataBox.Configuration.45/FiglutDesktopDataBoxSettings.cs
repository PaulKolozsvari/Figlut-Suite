namespace Figlut.Desktop.DataBox.Configuration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public class FiglutDesktopDataBoxSettings : Settings
    {
        #region Constructors

        public FiglutDesktopDataBoxSettings()
            : base()
        {
        }

        public FiglutDesktopDataBoxSettings(string filePath)
            : base(filePath)
        {
        }

        public FiglutDesktopDataBoxSettings(string name, string filePath)
            : base(name, filePath)
        {
        }

        #endregion //Constructors

        #region Fields

        #region Web Service

        private string _figlutWebServiceBaseUrl;
        private bool _useAuthentication;
        private bool _promptForCredentials;
        private string _authenticationDomainName;
        private string _authenticationUserName;
        private string _authenticationPassword;
        private int _figlutWebServiceWebRequestTimeout;
        private TextEncodingType _figlutWebServiceTextResponseEncoding;
        private SerializerType _figlutWebServiceMessagingFormat;
        private SerializerType _figlutWebServiceSchemaAcquisitionMessagingFormat;
        private bool _offlineMode;

        #endregion //Web Service

        #region DataBox

        private string _applicationTitle;
        private string _applicationVersion;
        private string _themeStartColor;
        private string _themeEndColor;
        private string _dataBoxSelectedRowColor;
        private string _splashScreenImageFileName;
        private string _applicationBannerImageFileName;
        private bool _startFullScreen;
        private bool _shapeColumnNames;
        private List<string> _hiddenTypeNames;
        private List<Type> _hiddenTypes;
        private string _entityIdColumnName;
        private bool _hideEntityIdColumn;
        private bool _treatZeroAsNull;
        private int _firstInputControlIndex;
        private int _inputControlIndexAfterAdd;
        private string _databaseSchemaFileName;
        private bool _useExtensions;
        private string _extensionAssemblyName;
        private string _crudInterceptorFullTypeName;
        private string _mainMenuExtensionFullTypeName;

        #endregion //DataBox

        #region Logging

        private bool _logToFile;
        private bool _logToWindowsEventLog;
        private bool _logToConsole;
        private string _logFileName;
        private string _eventSourceName;
        private string _eventLogName;
        private LoggingLevel _loggingLevel;

        #endregion //Logging

        #endregion //Fields

        #region Properties

        #region Web Service

        [SettingInfo("Web Service", DisplayName = "Base URL", Description = "The URL of the Figlut Web Service.", CategorySequenceId = 0)]
        public string FiglutWebServiceBaseUrl
        {
            get { return _figlutWebServiceBaseUrl; }
            set { _figlutWebServiceBaseUrl = value; }
        }

        [SettingInfo("Web Service", AutoFormatDisplayName = true, Description = "Whether or not the Figlut Web Service requires clients to authenticate.", CategorySequenceId = 1)]
        public bool UseAuthentication
        {
            get { return _useAuthentication; }
            set { _useAuthentication = value; }
        }

        [SettingInfo("Web Service", AutoFormatDisplayName = true, Description = "Whether or not to prompt for credentials on application start up or otherwise use the credentials specified in these settings.", CategorySequenceId = 2)]
        public bool PromptForCredentials
        {
            get { return _promptForCredentials; }
            set { _promptForCredentials = value; }
        }

        [SettingInfo("Web Service", DisplayName = "Domain Name", Description = "The domain name (or hostname) to be used in the credentials when authenticating against the Figlut Web Service.", CategorySequenceId = 3)]
        public string AuthenticationDomainName
        {
            get { return _authenticationDomainName; }
            set { _authenticationDomainName = value; }
        }

        [SettingInfo("Web Service", DisplayName = "User Name", Description = "The windows user name to be used in the credentials when authenticating against the Figlut Web Service. N.B. Only used if the user is not prompted for credentials.", CategorySequenceId = 4)]
        public string AuthenticationUserName
        {
            get { return _authenticationUserName; }
            set { _authenticationUserName = value; }
        }

        [SettingInfo("Web Service", DisplayName = "Password", Description = "The password of the windows user to be used in the credentials when authentication against the Figlut Web Service.", CategorySequenceId = 5, PasswordChar = '*')]
        public string AuthenticationPassword
        {
            get { return _authenticationPassword; }
            set { _authenticationPassword = value; }
        }

        [SettingInfo("Web Service", DisplayName = "Web Request Timeout", Description = "The timeout in milliseconds of a web request made to the Figlut Web Service by the DataBox.", CategorySequenceId = 6)]
        public int FiglutWebServiceWebRequestTimeout
        {
            get { return _figlutWebServiceWebRequestTimeout; }
            set { _figlutWebServiceWebRequestTimeout = value; }
        }

        [SettingInfo("Web Service", DisplayName = "Text Response Encoding", Description = "The encoding of the text response from Figlut Web Service. The encoding of the Figlut DataBox and Web Service need to be configured to match.", CategorySequenceId = 7)]
        public TextEncodingType FiglutWebServiceTextResponseEncoding
        {
            get { return _figlutWebServiceTextResponseEncoding; }
            set { _figlutWebServiceTextResponseEncoding = value; }
        }

        [SettingInfo("Web Service", DisplayName = "Messaging Format", Description = "The format of the messages exchanged between the Figlut DataBox and the Web Service e.g. XML, JSON or CSV.", CategorySequenceId = 8)]
        public SerializerType FiglutWebServiceMessagingFormat
        {
            get { return _figlutWebServiceMessagingFormat; }
            set { _figlutWebServiceMessagingFormat = value; }
        }

        [SettingInfo("Web Service", DisplayName = "Schema Acquisition Messaging Format", Description = "The format of the messages exchanged between the Figlut DataBox and the Web Service when acquiring the database schema.", CategorySequenceId = 9)]
        public SerializerType FiglutWebServiceSchemaAcquisitionMessagingFormat
        {
            get { return _figlutWebServiceSchemaAcquisitionMessagingFormat; }
            set { _figlutWebServiceSchemaAcquisitionMessagingFormat = value; }
        }

        //[SettingInfo("Web Service", AutoFormatDisplayName = true, Description = "Whether to download the database schema from the Figlut Web Service when DataBox starts up.", CategorySequenceId = 10)]
        //public bool OfflineMode
        //{
        //    get { return _offlineMode; }
        //    set { _offlineMode = value; }
        //}

        #endregion //Web Service

        #region DataBox

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The name of the application. Leaving this setting blank defaults the title.", CategorySequenceId = 0)]
        public string ApplicationTitle
        {
            get { return _applicationTitle; }
            set { _applicationTitle = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The version of the application. Leaving this setting blank defaults the version.", CategorySequenceId = 1)]
        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set { _applicationVersion = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The theme start color. Leaving this field blank defaults the color.", CategorySequenceId = 2)]
        public string ThemeStartColor
        {
            get { return _themeStartColor; }
            set { _themeStartColor = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The theme end color. Leaving this field blank defaults the color.", CategorySequenceId = 3)]
        public string ThemeEndColor
        {
            get { return _themeEndColor; }
            set { _themeEndColor = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The background color of a selected row in a DataBox.", CategorySequenceId = 4)]
        public string DataBoxSelectRowColor
        {
            get { return _dataBoxSelectedRowColor; }
            set { _dataBoxSelectedRowColor = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The image to be displayed on the splash screen.", CategorySequenceId = 5)]
        public string SplashScreenImageFileName
        {
            get { return _splashScreenImageFileName; }
            set { _splashScreenImageFileName = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The image to be displayed in the application's banner.", CategorySequenceId = 6)]
        public string ApplicationBannerImageFileName
        {
            get { return _applicationBannerImageFileName; }
            set { _applicationBannerImageFileName = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether or not to start the DataBox application in full screen,", CategorySequenceId = 7)]
        public bool StartFullScreen
        {
            get { return _startFullScreen; }
            set { _startFullScreen = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether or not to rename the column names in the DataBox by adding spaces in between upper case letters for camel cased column names.", CategorySequenceId = 8)]
        public bool ShapeColumnNames
        {
            get { return _shapeColumnNames; }
            set { _shapeColumnNames = true; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The types of entity properties to hide from view in the DataBox.", CategorySequenceId = 9)]
        public List<string> HiddenTypeNames
        {
            get { return _hiddenTypeNames; }
            set
            {
                _hiddenTypeNames = value;
                RefreshHiddenTypes();
            }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The name of the dynamically created column used to uniquely identify and lookup rows in the DataBox. This setting should not be changed unless the target database contains a table with a column name matching this setting's value i.e. because column names need to be unique.", CategorySequenceId = 10)]
        public string EntityIdColumnName
        {
            get { return _entityIdColumnName; }
            set { _entityIdColumnName = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether or not to hide the dynamically created entity id column used to uniquely identify and look up rows in the DataBox. Under normal circumstances for presentation purposes this value should be set to 'true' i.e. in order to not display the entity ID.", CategorySequenceId = 11)]
        public bool HideEntityIdColumn
        {
            get { return _hideEntityIdColumn; }
            set { _hideEntityIdColumn = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether to treat zeros as null. N.B. Setting this value to tru will prevent user from entering zero into not nullable numeric fields.", CategorySequenceId = 12)]
        public bool TreatZeroAsNull
        {
            get { return _treatZeroAsNull; }
            set { _treatZeroAsNull = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The index of the input control that should have initial focus.", CategorySequenceId = 13)]
        public int FirstInputControlIndex
        {
            get { return _firstInputControlIndex; }
            set { _firstInputControlIndex = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The index of the input control that should have focus after an add operation. Only relevant if CloseAddWindowAfterAdd is set to true.", CategorySequenceId = 14)]
        public int InputControlIndexAfterAdd
        {
            get { return _inputControlIndexAfterAdd; }
            set { _inputControlIndexAfterAdd = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The name of the file where the database schema info is saved to.", CategorySequenceId = 15)]
        public string DatabaseSchemaFileName
        {
            get { return _databaseSchemaFileName; }
            set { _databaseSchemaFileName = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether or not the extension should be used. N.B. If it is to be used, then the ExtensionAssemblyName as well as the full type names of the extensions need to be configured.", CategorySequenceId = 16)]
        public bool UseExtensions
        {
            get { return _useExtensions; }
            set { _useExtensions = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The name of the assembly (DLL file) containing the Figlut Desktop DataBox extension e.g. FiglutExtension.dll. The assembly needs to be placed in the Figlut Desktop DataBox executing directory.", CategorySequenceId = 17)]
        public string ExtensionAssemblyName
        {
            get { return _extensionAssemblyName; }
            set { _extensionAssemblyName = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The full type name of the class acting as the CRUD interceptor e.g. FiglutExtension.MyDesktopDataBoxCrudInterceptor.", CategorySequenceId = 18)]
        public string CrudInterceptorFullTypeName
        {
            get { return _crudInterceptorFullTypeName; }
            set { _crudInterceptorFullTypeName = value; }
        }

        [SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The full type name of the class acting as the main menu extension e.g. FiglutExtension.MyDesktopDataBoxMainMenuExtension.", CategorySequenceId = 19)]
        public string MainMenuExtentionFullTypeName
        {
            get { return _mainMenuExtensionFullTypeName; }
            set { _mainMenuExtensionFullTypeName = value; }
        }

        #endregion //DataBox

        #region Logging

        [SettingInfo("Logging", DisplayName = "To File", Description = "Whether or not to log to a text log file in the executing directory.", CategorySequenceId = 0)]
        public bool LogToFile
        {
            get { return _logToFile; }
            set { _logToFile = value; }
        }

        [SettingInfo("Logging", DisplayName = "To Windows Event Log", Description = "Whether or not to log to the Windows Event Log.", CategorySequenceId = 1)]
        public bool LogToWindowsEventLog
        {
            get { return _logToWindowsEventLog; }
            set { _logToWindowsEventLog = value; }
        }

        [SettingInfo("Logging", DisplayName = "To Console", Description = "Whether or not to write all log messages to the console. Useful when running the service as a console application i.e. running the executable with the /test_mode switch.", CategorySequenceId = 2)]
        public bool LogToConsole
        {
            get { return _logToConsole; }
            set { _logToConsole = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the text log file to log to. The log file is written in the executing directory.", CategorySequenceId = 3)]
        public string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the event source to use when logging to the Windows Event Log.", CategorySequenceId = 4)]
        public string EventSourceName
        {
            get { return _eventSourceName; }
            set { _eventSourceName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the Windows Event Log to log to.", CategorySequenceId = 5)]
        public string EventLogName
        {
            get { return _eventLogName; }
            set { _eventLogName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The extent of messages being logged: None = logging is disabled, Minimum = logs server start/stop and exceptions, Normal = logs additional information messages, Maximum = logs all requests and responses to the server.", CategorySequenceId = 6)]
        public LoggingLevel LoggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        #endregion //Logging

        #endregion //Properties

        #region Methods

        public List<Type> GetHiddenTypes()
        {
            return _hiddenTypes;
        }

        protected void RefreshHiddenTypes()
        {
            if (_hiddenTypes == null)
            {
                _hiddenTypes = new List<Type>();
            }
            _hiddenTypes.Clear();
            foreach (string typeName in _hiddenTypeNames)
            {
                Type hiddenType = Type.GetType(typeName, false);
                if (hiddenType == null)
                {
                    throw new NullReferenceException(string.Format(
                        "Could not find type {0} to add as a hidden type. Ensure that it is a fully qualified type name.",
                        typeName));
                }
                _hiddenTypes.Add(hiddenType);
            }
        }

        #endregion //Methods
    }
}