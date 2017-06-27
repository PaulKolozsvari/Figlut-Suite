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

    public class WebServiceRequestArgs : EventArgs
    {
        public WebServiceRequestArgs(
            string uri,
            HttpVerb method,
            string contentType,
            string accept,
            string userAgent,
            string inputString,
            ISerializer inputSerializer,
            ISerializer outputSerializer)
        {
            _uri = uri;
            _method = method;
            _contentType = contentType;
            _accept = accept;
            _userAgent = userAgent;
            _inputString = inputString;
            _inputSerializer = inputSerializer;
            _outputSerializer = outputSerializer;
        }


        #region Fields

        protected string _uri;
        protected HttpVerb _method;
        protected string _contentType;
        protected string _accept;
        protected string _userAgent;
        protected string _inputString;
        protected ISerializer _inputSerializer;
        protected ISerializer _outputSerializer;

        #endregion //Fields

        #region Properties

        public string Uri
        {
            get { return _uri; }
        }

        public HttpVerb Method
        {
            get { return _method; }
        }

        public string ContentType
        {
            get { return _contentType; }
        }

        public string Accept
        {
            get { return _accept; }
        }

        public string UserAgent
        {
            get { return _userAgent; }
        }

        public string InputString
        {
            get { return _inputString; }
        }

        public ISerializer InputSerializer
        {
            get { return _inputSerializer; }
        }

        public ISerializer OutputSerializer
        {
            get { return _outputSerializer; }
        }

        #endregion //Properties
    }
}