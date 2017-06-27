namespace Figlut.Server.Toolkit.Extensions.WebService.Events
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Web.Client;

    #endregion //Using Directives

    public class BeforeWebServiceRequestArgs : WebServiceRequestArgs
    {
        #region Constructors

        public BeforeWebServiceRequestArgs(
            string uri,
            HttpVerb method,
            string contentType,
            string accept,
            string userAgent,
            string inputString,
            ISerializer inputSerializer,
            ISerializer outputSerializer)
            : base(
                uri,
                method,
                contentType,
                accept,
                userAgent,
                inputString,
                inputSerializer,
                outputSerializer)
        {
            _httpStatusCode = HttpStatusCode.OK;
            _httpStatusMessage = string.Empty;
            _outputString = string.Empty;
        }

        #endregion //Constructors

        #region Fields

        private bool _cancel;
        private HttpStatusCode _httpStatusCode;
        private string _httpStatusMessage;
        private string _outputString;

        #endregion //Fields

        #region Properties

        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        public HttpStatusCode HttpStatusCode
        {
            get { return _httpStatusCode; }
            set { _httpStatusCode = value; }
        }

        public string HttpStatusMessage
        {
            get { return _httpStatusMessage; }
            set { _httpStatusMessage = value; }
        }

        public string OutputString
        {
            get { return _outputString; }
            set { _outputString = value; }
        }

        #endregion //Properties
    }
}