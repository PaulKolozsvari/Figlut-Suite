namespace Figlut.Server.Toolkit.Utilities.SettingsFile.Default
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class RestWebServiceAppSettings : DatabaseAppSettings
    {
        #region Constructors

        public RestWebServiceAppSettings() : base()
        {
        }

        public RestWebServiceAppSettings(string filePath) : base(filePath)
        {
        }

        public RestWebServiceAppSettings(string name, string filePath) : base(name, filePath)
        {
        }

        #endregion //Constructors

        #region Properties

        #region Service

        /// <summary>
        /// The suffix to append to the URI on which the web service will be accessed i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:8889/MyService.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The suffix to append to the URI on which the web service will be accessed i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:8889/MyService.", CategorySequenceId = 1)]
        public string RestServiceHostAddressSuffix { get; set; }

        /// <summary>
        /// The port number on which the web service should listen for requests from clients i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:2984/MyService.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The port number on which the web service should listen for requests from clients i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:2984/MyService.", CategorySequenceId = 2)]
        public int RestServicePortNumber { get; set; }

        /// <summary>
        /// Whether or not the service should authenticate clients attempting to consume the service.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Whether or not the service should authenticate clients attempting to consume the service.", CategorySequenceId = 3)]
        public bool RestServiceUseAuthentication { get; set; }

        /// <summary>
        /// Whether or not to include the exception details including the stack trace in the web response when an unhandled exception occurs.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Whether or not to include the exception details including the stack trace in the web response when an unhandled exception occurs.", CategorySequenceId = 4)]
        public bool RestServiceIncludeExceptionDetailInResponse { get; set; }

        /// <summary>
        /// Encoding to used on the text response from the service.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Encoding to used on the text response from the service.", CategorySequenceId = 5)]
        public TextEncodingType RestServiceTextResponseEncoding { get; set; }

        /// <summary>
        /// Whether or not to include in the JSON response the names of the .NET generated ORM types representing each table in the database.
        /// </summary>
        [SettingInfo("REST Service", DisplayName = "Include ORM Type Names in JSON Response", Description = "Whether or not to include in the JSON response the names of the .NET generated ORM types representing each table in the database.", CategorySequenceId = 6)]
        public bool RestServiceIncludeOrmTypeNamesInJsonResponse { get; set; }

        /// <summary>
        /// The maximum amount of memory allocated, in bytes, for the buffer manager that manages the buffers required by endpoints that use this binding.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The maximum amount of memory allocated, in bytes, for the buffer manager that manages the buffers required by endpoints that use this binding.", CategorySequenceId = 7)]
        public long RestServiceMaxBufferPoolSize { get; set; }

        /// <summary>
        /// The maximum amount of memory allocated, in bytes, for use by the manager of the message buffers that receive messages from the channel.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The maximum amount of memory allocated, in bytes, for use by the manager of the message buffers that receive messages from the channel.", CategorySequenceId = 8)]
        public long RestServiceMaxBufferSize { get; set; }

        /// <summary>
        /// The maximum size, in bytes, for a message that can be processed by the binding.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The maximum size, in bytes, for a message that can be processed by the binding.", CategorySequenceId = 9)]
        public long RestServiceMaxReceivedMessageSize { get; set; }

        /// <summary>
        /// Whether to trace HTTP messages
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Whether to trace HTTP messages.", CategorySequenceId = 10)]
        public bool RestServiceTraceHttpMessages { get; set; }

        /// <summary>
        /// Whether to trace HTTP message headers
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Whether to trace HTTP message headers.", CategorySequenceId = 11)]
        public bool RestServiceTraceHttpMessageHeaders { get; set; }

        /// <summary>
        /// Whether to audit (log) calls on this web service.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Whether to audit (log) calls on this web service.", CategorySequenceId = 12)]
        public bool RestServiceAuditServiceCalls { get; set; }

        /// <summary>
        /// Directory where files get uploaded to by the mobile app via FTP.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Directory where files get uploaded to by an FTP client", CategorySequenceId = 13)]
        public string RestServiceServiceFtpDirectory { get; set; }

        /// <summary>
        /// The number of concurrent calls that can be made – under .NET 4 defaults to 16 x number of cores.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The number of concurrent calls that can be made – under .NET 4 defaults to 16 x number of cores.", CategorySequenceId = 14)]
        public int RestServiceMaxConcurrentCalls { get; set; }

        /// <summary>
        /// The number of concurrent sessions that can be in in flight – under .NET 4 defaults to 100 x number of cores.
        /// Using Sessions: https://docs.microsoft.com/en-us/dotnet/framework/wcf/using-sessions
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The number of concurrent sessions that can be in in flight – under .NET 4 defaults to 100 x number of cores.", CategorySequenceId = 15)]
        public int RestServiceMaxConcurrentSessions { get; set; }

        /// <summary>
        /// The number of service implementation objects that are in use – defaults to the sum of MaxConcurrentCalls + MaxConcurrentSessions.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The number of service implementation objects that are in use – defaults to the sum of MaxConcurrentCalls + MaxConcurrentSessions.", CategorySequenceId = 16)]
        public int RestServiceMaxConcurrentInstances { get; set; }

        /// <summary>
        /// The scope of the performance counters to enable on the service in order to view the counters with Windows perfmon.exe.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The scope of the performance counters to enable on the service in order to view the counters with Windows perfmon.exe.", CategorySequenceId = 17)]
        public PerformanceCounterScope RestServicePerformanceCounterScope { get; set; }

        /// <summary>
        /// Whether or not the server will handle exceptions (logging and email notifications) after HTTP client connections have been accepted. If set to true, exception handling is optional in your custom agents.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Whether or not the server will handle exceptions (logging and email notifications) after HTTP client connections have been accepted. If set to true, exception handling is optional in your custom agents.", CategorySequenceId = 18)]
        public bool RestServiceHandleExceptionsOnClientConnectionAccepted { get; set; }

        /// <summary>
        /// Whether or not to return a result to the client when exceptions are thrown by the agents.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "Whether or not to return a result to the client when exceptions are thrown by web agents.", CategorySequenceId = 19)]
        public bool RestServiceReturnResponseOnAgentExceptions { get; set; }

        /// <summary>
        /// The date time format used in the messages being sent by the client in web requests.
        /// </summary>
        [SettingInfo("REST Service", AutoFormatDisplayName = true, Description = "The date time format used in the messages being sent by the client in web requests.", CategorySequenceId = 20)]
        public string RestClientDateTimeFormat { get; set; }

        #endregion //Service

        #endregion //Properties
    }
}
