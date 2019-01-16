namespace Figlut.MonoDroid.Toolkit.Utilities.SettingsFile.Default
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class WebServiceAppSettings : DatabaseAppSettings
    {
        #region Properties

        #region Service

        /// <summary>
        /// The suffix to append to the URI on which the web service will be accessed i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:8889/MyService.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The suffix to append to the URI on which the web service will be accessed i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:8889/MyService.", CategorySequenceId = 1)]
        public string HostAddressSuffix { get; set; }

        /// <summary>
        /// The port number on which the web service should listen for requests from clients i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:2984/MyService.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The port number on which the web service should listen for requests from clients i.e. http://localhost:{port_number}/{suffix} e.g. http://localhost:2984/MyService.", CategorySequenceId = 2)]
        public int PortNumber { get; set; }

        /// <summary>
        /// Whether or not the service should authenticate clients attempting to consume the service.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether or not the service should authenticate clients attempting to consume the service.", CategorySequenceId = 3)]
        public bool UseAuthentication { get; set; }

        /// <summary>
        /// Whether or not to include the exception details including the stack trace in the web response when an unhandled exception occurs.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether or not to include the exception details including the stack trace in the web response when an unhandled exception occurs.", CategorySequenceId = 4)]
        public bool IncludeExceptionDetailInResponse { get; set; }

        /// <summary>
        /// Encoding to used on the text response from the service.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Encoding to used on the text response from the service.", CategorySequenceId = 5)]
        public TextEncodingType TextResponseEncoding { get; set; }

        /// <summary>
        /// Whether or not to include in the JSON response the names of the .NET generated ORM types representing each table in the database.
        /// </summary>
        [SettingInfo("Service", DisplayName = "Include ORM Type Names in JSON Response", Description = "Whether or not to include in the JSON response the names of the .NET generated ORM types representing each table in the database.", CategorySequenceId = 6)]
        public bool IncludeOrmTypeNamesInJsonResponse { get; set; }

        /// <summary>
        /// The maximum amount of memory allocated, in bytes, for the buffer manager that manages the buffers required by endpoints that use this binding.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The maximum amount of memory allocated, in bytes, for the buffer manager that manages the buffers required by endpoints that use this binding.", CategorySequenceId = 7)]
        public long MaxBufferPoolSize { get; set; }

        /// <summary>
        /// The maximum amount of memory allocated, in bytes, for use by the manager of the message buffers that receive messages from the channel.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The maximum amount of memory allocated, in bytes, for use by the manager of the message buffers that receive messages from the channel.", CategorySequenceId = 8)]
        public long MaxBufferSize { get; set; }

        /// <summary>
        /// The maximum size, in bytes, for a message that can be processed by the binding.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "The maximum size, in bytes, for a message that can be processed by the binding.", CategorySequenceId = 9)]
        public long MaxReceivedMessageSize { get; set; }

        /// <summary>
        /// Whether to trace HTTP messages
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether to trace HTTP messages.", CategorySequenceId = 10)]
        public bool TraceHttpMessages { get; set; }

        /// <summary>
        /// Whether to trace HTTP message headers
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Whether to trace HTTP message headers.", CategorySequenceId = 11)]
        public bool TraceHttpMessageHeaders { get; set; }

        /// <summary>
        /// Directory where files get uploaded to by the mobile app via FTP.
        /// </summary>
        [SettingInfo("Service", AutoFormatDisplayName = true, Description = "Directory where files get uploaded to by the mobile app via FTP.", CategorySequenceId = 13)]
        public string ServiceFtpDirectory { get; set; }

        #endregion //Service

        #endregion //Properties
    }
}
