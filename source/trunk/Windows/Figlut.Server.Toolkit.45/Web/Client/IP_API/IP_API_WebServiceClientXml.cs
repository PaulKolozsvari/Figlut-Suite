namespace Figlut.Server.Toolkit.Web.Client.IP_API
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class IP_API_WebServiceClientXml : XmlWebServiceClient
    {
        #region Constructors

        public IP_API_WebServiceClientXml(string webServiceBaseUrl)
            : base(webServiceBaseUrl)
        {
        }

        #endregion //Constructors

        #region Methods

        public query GetWhoIsInfo(string ipAddress, int timeout, bool handleExceptions)
        {
            query result = null;
            try
            {
                if (string.IsNullOrEmpty(ipAddress))
                {
                    return null;
                }
                if (ipAddress == "::1") //Local machine i.e. during development.
                {
                    ipAddress = "197.242.157.9"; //The figlut.com public IP address - used for testing purposes during development.
                }
                HttpStatusCode httpStatusCode;
                string httpStatusDescription = null;
                result = CallService<query>(
                    ipAddress,
                    null,
                    HttpVerb.GET,
                    false,
                    timeout,
                    out httpStatusCode,
                    out httpStatusDescription,
                    false,
                    null);
                int stop = 0;
            }
            catch (Exception ex)
            {
                if (!handleExceptions)
                {
                    throw ex;
                }
                ExceptionHandler.HandleException(ex);
            }
            return result;
        }

        #endregion //Methods
    }
}
