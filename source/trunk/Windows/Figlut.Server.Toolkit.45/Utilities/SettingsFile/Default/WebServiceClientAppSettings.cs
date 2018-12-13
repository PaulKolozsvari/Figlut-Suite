namespace Figlut.Server.Toolkit.Utilities.SettingsFile.Default
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class WebServiceClientAppSettings : AppSettings
    {
        #region Web Service

        /// <summary>
        /// The URL of the web service.
        /// </summary>
        [SettingInfo("Web Service Client", DisplayName = "Base URL", Description = "The Base URL of the web service.", CategorySequenceId = 0)]
        public string WebServiceBaseUrl { get; set; }

        /// <summary>
        /// Whether or not the web service requires clients to authenticate.
        /// </summary>
        [SettingInfo("Web Service Client", AutoFormatDisplayName = true, Description = "Whether or not the web service requires clients to authenticate.", CategorySequenceId = 1)]
        public bool UseAuthentication { get; set; }

        /// <summary>
        /// The domain name (or hostname) to be used in the credentials when authenticating against the web service.
        /// </summary>
        [SettingInfo("Web Service Client", DisplayName = "Domain Name", Description = "The domain name (or hostname) to be used in the credentials when authenticating against the web service.", CategorySequenceId = 2)]
        public string AuthenticationDomainName { get; set; }

        /// <summary>
        /// The windows user name to be used in the credentials when authenticating against the web service. N.B. Only used if the user is not prompted for credentials.
        /// </summary>
        [SettingInfo("Web Service Client", DisplayName = "User Name", Description = "The windows user name to be used in the credentials when authenticating against the web service. N.B. Only used if the user is not prompted for credentials.", CategorySequenceId = 3)]
        public string AuthenticationUserName { get; set; }

        /// <summary>
        /// The password of the windows user to be used in the credentials when authentication against the web service.
        /// </summary>
        [SettingInfo("Web Service Client", DisplayName = "Password", Description = "The password of the windows user to be used in the credentials when authentication against the web service.", CategorySequenceId = 4, PasswordChar = '*')]
        public string AuthenticationPassword { get; set; }

        /// <summary>
        /// The timeout in milliseconds of a web request made to the web service by the application.
        /// </summary>
        [SettingInfo("Web Service Client", DisplayName = "Web Request Timeout", Description = "The timeout in milliseconds of a web request made to the web service by the application.", CategorySequenceId = 5)]
        public int WebServiceWebRequestTimeout { get; set; }

        /// <summary>
        /// The encoding of the text response from web service. The encoding of the application and web service need to be configured to match.
        /// </summary>
        [SettingInfo("Web Service Client", DisplayName = "Text Response Encoding", Description = "The encoding of the text response from web service. The encoding of the application and web service need to be configured to match.", CategorySequenceId = 6)]
        public TextEncodingType WebServiceTextResponseEncoding { get; set; }

        /// <summary>
        /// The format of the messages exchanged between the application and the web service e.g. XML, JSON or CSV.
        /// </summary>
        [SettingInfo("Web Service Client", DisplayName = "Messaging Format", Description = "The format of the messages exchanged between the application and the web service e.g. XML, JSON or CSV.", CategorySequenceId = 7)]
        public SerializerType WebServiceMessagingFormat { get; set; }

        #endregion //Web Service
    }
}
