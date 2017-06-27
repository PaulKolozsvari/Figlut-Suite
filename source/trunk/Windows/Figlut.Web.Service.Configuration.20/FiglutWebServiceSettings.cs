namespace Figlut.Web.Service.Configuration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public class FiglutWebServiceSettings : Settings
    {
        #region Constructors

        public FiglutWebServiceSettings()
            : base()
        {
        }

        public FiglutWebServiceSettings(string filePath)
            : base(filePath)
        {
        }

        public FiglutWebServiceSettings(string name, string filePath)
            : base(name, filePath)
        {
        }

        #endregion //Constructors

        #region Fields

        private bool _logToFile;
        private bool _logToWindowsEventLog;
        private bool _logToConsole;
        private string _logFileName;
        private string _eventSourceName;
        private string _eventLogName;
        private LoggingLevel _loggingLevel;
        private Dbms _dbms;
        private string _databaseConnectionString;
        private string _databaseServerName;
        private string _databaseName;
        private string _databaseUserName;
        private string _databaseUserPassword;
        private bool _saveOrmAssembly;
        private string _ormAssemblyOutputDirectory;
        private string _queryRunnerAssemblyName;
        private string _sqlQueryRunnerFullTypeName;
        private string _hostAddressSuffix;
        private long _portNumber;
        private bool _useAuthentication;
        private bool _includeExceptionDetailInResponse;
        private TextEncodingType _textResponseEncoding;
        private bool _includeOrmTypeNamesInJsonResponse;
        private long _maxBufferPoolSize;
        private long _maxBufferSize;
        private long _maxReceivedMessageSize;
        private bool _deleteClientDataWhenServiceStops;
        private bool _enableClientTrace;
        private bool _useExtensions;
        private string _extensionAssemblyName;
        private string _crudInterceptorFullTypeName;
        private string _extensionFullTypeName;

        private bool _firstLoad = true;

        #endregion //Fields

        #region Properties

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

        [SettingInfo("Database", Description = "Database Management System being utilized (MS SQL Server is currently the only supported DBMS).", CategorySequenceId = 0)]
        public Dbms DBMS
        {
            get { return _dbms; }
            set { _dbms = value; }
        }

        [SettingInfo("Database", DisplayName = "Server Name", Description = "Name of the DBMS server hosting the database.", CategorySequenceId = 1)]
        public string DatabaseServerName
        {
            get { return _databaseServerName; }
            set
            {
                _databaseServerName = value;
                GenerateDatabaseConnectionString();
            }
        }

        [SettingInfo("Database", AutoFormatDisplayName = true, Description = "Name of the target database.", CategorySequenceId = 2)]
        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                _databaseName = value;
                GenerateDatabaseConnectionString();
            }
        }

        [SettingInfo("Database", DisplayName = "User Name", Description = "User name of the user that has permission to read and write to the database.", CategorySequenceId = 3)]
        public string DatabaseUserName
        {
            get { return _databaseUserName; }
            set
            {
                _databaseUserName = value;
                GenerateDatabaseConnectionString();
            }
        }

        [SettingInfo("Database", DisplayName = "Password", Description = "Password of the user accessing the database.", CategorySequenceId = 4, PasswordChar = '*')]
        public string DatabaseUserPassword
        {
            get { return _databaseUserPassword; }
            set
            {
                _databaseUserPassword = value;
                GenerateDatabaseConnectionString();
            }
        }

        [SettingInfo("Database", DisplayName = "Connection String", Description = "The connection string to the target database (this setting overrides the below database credentials).", CategorySequenceId = 5)]
        public string DatabaseConnectionString
        {
            get { return _databaseConnectionString; }
            set
            {
                _databaseConnectionString = value;
                if (!_firstLoad)
                {
                    _databaseServerName = string.Empty;
                    _databaseName = string.Empty;
                    _databaseUserName = string.Empty;
                    _databaseUserPassword = string.Empty;
                }
            }
        }

        [SettingInfo("Database", DisplayName = "Save ORM Assembly", Description = "Whether or not to save generated assembly (.dll) that contains the ORM classes mapped to the database tables.", CategorySequenceId = 6)]
        public bool SaveOrmAssembly
        {
            get { return _saveOrmAssembly; }
            set { _saveOrmAssembly = value; }
        }

        [SettingInfo("Database", DisplayName = "ORM Assembly Copy Directory", Description = "The directory to which the ORM assembly should be savedd to if saving configured to happen. N.B. This directory must exist.", CategorySequenceId = 7)]
        public string OrmAssemblyOutputDirectory
        {
            get { return _ormAssemblyOutputDirectory; }
            set { _ormAssemblyOutputDirectory = value; }
        }

        [SettingInfo("Database", AutoFormatDisplayName = true, Description = "The name of the assembly living in the executing directory which is responsible for running queries. e.g. Figlut.Server.QueryRunner.dll", CategorySequenceId = 8)]
        public string QueryRunnnerAssemblyName
        {
            get { return _queryRunnerAssemblyName; }
            set { _queryRunnerAssemblyName = value; }
        }

        [SettingInfo("Database", AutoFormatDisplayName = true, Description = "The full type name of the type in the query runner assembly responsible for executing sql queries.", CategorySequenceId = 9)]
        public string SqlQueryRunnerFullTypeName
        {
            get { return _sqlQueryRunnerFullTypeName; }
            set { _sqlQueryRunnerFullTypeName = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The suffix to append to the URI on which the Figlut Server will be accessed i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:8889/Figlut.", CategorySequenceId = 0)]
        public string HostAddressSuffix
        {
            get { return _hostAddressSuffix; }
            set { _hostAddressSuffix = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The port number on which the Figlut Server should listen for requests from clients i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:2983/Figlut.", CategorySequenceId = 1)]
        public long PortNumber
        {
            get { return _portNumber; }
            set { _portNumber = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether or not the service should authenticate clients attempting to consume the service.", CategorySequenceId = 2)]
        public bool UseAuthentication
        {
            get { return _useAuthentication; }
            set { _useAuthentication = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether or not to include the exception details including the stack trace in the web response when an unhandled exception occurs.", CategorySequenceId = 3)]
        public bool IncludeExceptionDetailInResponse
        {
            get { return _includeExceptionDetailInResponse; }
            set { _includeExceptionDetailInResponse = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Encoding to used on the text response from the service.", CategorySequenceId = 4)]
        public TextEncodingType TextResponseEncoding
        {
            get { return _textResponseEncoding; }
            set { _textResponseEncoding = value; }
        }

        [SettingInfo("Service", DisplayName = "Include ORM Type Names in JSON Response", Description = "Whether or not to include in the JSON response the names of the .NET generated ORM types representing each table in the database.", CategorySequenceId = 5)]
        public bool IncludeOrmTypeNamesInJsonResponse
        {
            get { return _includeOrmTypeNamesInJsonResponse; }
            set { _includeOrmTypeNamesInJsonResponse = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The maximum amount of memory allocated, in bytes, for the buffer manager that manages the buffers required by endpoints that use this binding.", CategorySequenceId = 6)]
        public long MaxBufferPoolSize
        {
            get { return _maxBufferPoolSize; }
            set { _maxBufferPoolSize = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The maximum amount of memory allocated, in bytes, for use by the manager of the message buffers that receive messages from the channel.", CategorySequenceId = 7)]
        public long MaxBufferSize
        {
            get { return _maxBufferSize; }
            set { _maxBufferSize = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The maximum size, in bytes, for a message that can be processed by the binding.", CategorySequenceId = 8)]
        public long MaxReceivedMessageSize
        {
            get { return _maxReceivedMessageSize; }
            set { _maxReceivedMessageSize = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether to delete all client related data including trace when the service is stoppped.", CategorySequenceId = 9)]
        public bool DeleteClientDataWhenServiceStops
        {
            get { return _deleteClientDataWhenServiceStops; }
            set { _deleteClientDataWhenServiceStops = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether to trace client transactions against the service.", CategorySequenceId = 10)]
        public bool EnableClientTrace
        {
            get { return _enableClientTrace; }
            set { _enableClientTrace = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether or not the extension should be used. N.B. If it is to be used, then both ExtensionAssemblyName and ExtensionFullTypeName need to be configured.", CategorySequenceId = 11)]
        public bool UseExtensions
        {
            get { return _useExtensions; }
            set { _useExtensions = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The name of the assembly (DLL file) containing the Figlut Web Service extension e.g. FiglutExtension.dll. The assembly needs to be placed in the Figlut Web Service executing directory.", CategorySequenceId = 12)]
        public string ExtensionAssemblyName
        {
            get { return _extensionAssemblyName; }
            set { _extensionAssemblyName = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The full type name of the class acting as the CRUD interceptor e.g. FiglutExtension.MyWebServiceCrudInterceptor.", CategorySequenceId = 13)]
        public string CrudInterceptorFullTypeName
        {
            get { return _crudInterceptorFullTypeName; }
            set { _crudInterceptorFullTypeName = value; }
        }

        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The full type name of the class acting as the Web Service extension e.g. FiglutExtension.MyWebServiceExtension.", CategorySequenceId = 14)]
        public string ExtensionFullTypeName
        {
            get { return _extensionFullTypeName; }
            set { _extensionFullTypeName = value; }
        }

        #endregion //Properties

        #region Methods

        private void GenerateDatabaseConnectionString()
        {
            if (!_firstLoad)
            {
                _databaseConnectionString = string.Format(
                    "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
                    _databaseServerName,
                    _databaseName,
                    _databaseUserName,
                    _databaseUserPassword);
            }
        }

        public override void RefreshFromFile(bool saveIfFileDoesNotExist, bool validateAllSettingValuesSet)
        {
            base.RefreshFromFile(saveIfFileDoesNotExist, validateAllSettingValuesSet);
            _firstLoad = false;
        }

        #endregion //Methods
    }
}