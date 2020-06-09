namespace Figlut.Server.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Net;
    using System.IO;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Web;
    using System.Security.Principal;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using System.Web;
    using System.Collections.Specialized;

    #endregion //Using Directives

    /// <summary>
    /// A wrapper class for performing web requests/calls calls to a service (or any other web resource).
    /// </summary>
    public class WebServiceClient : IWebService
    {
        #region Constructors

        /// <summary>
        /// A wrapper class for performing web requests/calls calls to a service (or any other web resource).
        /// </summary>
        public WebServiceClient()
        {
            Type type = this.GetType();
            _name = type.Name;
        }

        /// <summary>
        /// A wrapper class for performing web requests/calls calls to a service (or any other web resource).
        /// </summary>
        /// <param name="webServiceBaseUrl">The base URL to be used for web requests e.g. http://www.mydomains.com/api</param>
        public WebServiceClient(string webServiceBaseUrl)
        {
            Type type = this.GetType();
            _name = type.Name;
            _webServiceBaseUrl = webServiceBaseUrl;
        }

        /// <summary>
        /// A wrapper class for performing web requests/calls calls to a service (or any other web resource).
        /// </summary>
        /// <param name="name">The name of the web service to used to identify it in perhaps an entity cache.</param>
        /// <param name="webServiceBaseUrl">The base URL to be used for web requests e.g. http://www.mydomains.com/api</param>
        public WebServiceClient(string name, string webServiceBaseUrl)
        {
            _name = name;
            _webServiceBaseUrl = webServiceBaseUrl;
        }

        #endregion //Constructors

        #region Fields

        protected string _name;
        protected string _webServiceBaseUrl;
        protected NetworkCredential _networkCredential;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// The name of the web service to used to identify it in perhaps an entity cache.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The base URL to be used for web requests e.g. http://www.mydomains.com/api
        /// </summary>
        public string WebServiceBaseUrl
        {
            get { return _webServiceBaseUrl; }
            set { _webServiceBaseUrl = value; }
        }

        /// <summary>
        /// The network credential (domain, user name, password) to use if using basic authentication to authenticate against a web service."
        /// Setting this property will cause the web request made to the service to attempt to auhenticate, otherwise if this property is null, no authentication will be applied to the web request.
        /// </summary>
        public NetworkCredential NetworkCredential
        {
            get { return _networkCredential; }
            set { _networkCredential = value; }
        }

        #endregion //Properties

        #region Methods

        #region Utility Methods

        /// <summary>
        /// Returns the query string to populate form-data fields, from a passed in a dictionary of field names and their values.
        /// </summary>
        /// <param name="formDataFields"></param>
        /// <returns>Returns query string for form-data post requests.</returns>
        public static string GetFormDataQueryString(Dictionary<string, string> formDataFields)
        {
            NameValueCollection result = HttpUtility.ParseQueryString(String.Empty);
            foreach (KeyValuePair<string, string> field in formDataFields)
            {
                result.Add(field.Key, field.Value);
            }
            return result.ToString();
        }

        #endregion //Utility Methods

        /// <summary>
        /// Performs a GET operation to the server at the base URL to determine if it can reach the server.
        /// If a WebException is thrown and its status is connection failure or if the inner exception is a socket exception 
        /// then the exception is thrown i.e. could not connect to server. Otherwise the WebException is swallowed e.g. a 404 
        /// (page not found) WebException could have be thrown, but it still indicates that the web server is online.
        /// </summary>
        /// <param name="timeout">The timeout of the web request</param>
        public void ConnectionTest(
            int timeout, 
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            string textOutput;
            ConnectionTest(
                out textOutput, 
                timeout, 
                out statusCode, 
                out statusDescription,
                wrapWebException,
                requestHeaders);
        }

        /// <summary>
        /// Performs a GET operation to the server at the base URL to determine if it can reach the server
        /// If a WebException is thrown and its status is connection failure or if the inner exception is a socket exception 
        /// then the exception is thrown i.e. could not connect to server. Otherwise the WebException is swallowed e.g. a 404 
        /// (page not found) WebException could have be thrown, but it still indicates that the web server is online.
        /// It also returns the text output from the web request.
        /// </summary>
        /// <param name="textOutput">The text output from the web request will be placed in this output parameter.</param>
        /// <param name="timeout">The timeout of the web request</param>
        public void ConnectionTest(
            out string textOutput, 
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            ConnectionTest(
                string.Empty, 
                out textOutput, 
                timeout, 
                out statusCode, 
                out statusDescription,
                wrapWebException,
                requestHeaders);
        }

        /// <summary>
        /// Performs a GET operation to the server at the query string appended to the base URL to
        /// determine if it can reach the server.
        /// If a WebException is thrown and its status is connection failure or if the inner exception is a socket exception 
        /// then the exception is thrown i.e. could not connect to server. Otherwise the WebException is swallowed e.g. a 404 
        /// (page not found) WebException could have be thrown, but it still indicates that the web server is online.
        /// It also returns the text output from the web request.
        /// </summary>
        /// <param name="queryString">The string to append to the base url making up the complete URL where the web request will be made to.</param>
        /// <param name="timeout">The timeout of the web request</param>
        public void ConnectionTest(
            string queryString, 
            int timeout, 
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            string textOutput;
            ConnectionTest(
                queryString, 
                out textOutput, 
                timeout, 
                out statusCode, 
                out statusDescription,
                wrapWebException,
                requestHeaders);
        }

        /// <summary>
        /// Performs a GET operation to the server at the query string appended to the base URL to
        /// determine if it can reach the server.
        /// If a WebException is thrown and its status is connection failure or if the inner exception is a socket exception 
        /// then the exception is thrown i.e. could not connect to server. Otherwise the WebException is swallowed e.g. a 404 
        /// (page not found) WebException could have be thrown, but it still indicates that the web server is online.
        /// </summary>
        /// <param name="queryString">The string to append to the base url making up the complete URL where the web request will be made to.</param>
        /// <param name="textOutput">The text output from the web request will be placed in this output parameter.</param>
        /// <param name="timeout">The timeout of the web request</param>
        public virtual void ConnectionTest(
            string queryString, 
            out string textOutput,
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            textOutput = null;
            statusCode = HttpStatusCode.OK;
            statusDescription = null;
            try
            {

                textOutput = CallService(
                    queryString, 
                    null, 
                    HttpVerb.HEAD, 
                    null, 
                    timeout, 
                    null, 
                    out statusCode, 
                    out statusDescription,
                    wrapWebException,
                    requestHeaders);
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ConnectFailure ||
                    (ex.InnerException != null && ex.InnerException.GetType() == typeof(System.Net.Sockets.SocketException)))
                {
                    HttpWebResponse response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        statusCode = response.StatusCode;
                        statusDescription = response.StatusDescription;
                    }
                    throw ex;
                }
            }
        }

        public virtual string CallService(
            string queryString,
            string requestPostString,
            HttpVerb verb,
            string postContentType,
            int timeout,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException)
        {
            return CallService(
                queryString,
                requestPostString,
                verb,
                postContentType,
                timeout,
                accept,
                out statusCode,
                out statusDescription,
                wrapWebException,
                null);
        }

        /// <summary>
        /// Performs a web request to a URL made of the query string appended to the base URL
        /// and applying the given HTTP verb. If requestPostString to be posted is not null 
        /// it will be included in the web request to the service. It then returns the text it
        /// receives from the web service.
        /// </summary>
        /// <param name="queryString">The string that will be appended to the end of the Web Service Base URL.</param>
        /// <param name="requestPostString">The text to be posted to the service.</param>
        /// <param name="verb">The HTTP verb to be applied to the web request.</param>
        /// <param name="setContentType">Whether the Content Type to the wb request should be set to the next parameter i.e. contentType</param>
        /// <param name="contentType">The type of content for the web request.</param>
        /// <param name="timeout">The timeout of the web request</param>
        /// <returns></returns>
        public virtual string CallService(
            string queryString,
            string requestPostString,
            HttpVerb verb,
            string postContentType,
            int timeout,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            string uri = !string.IsNullOrEmpty(queryString) ? string.Format("{0}/{1}", _webServiceBaseUrl, queryString) : _webServiceBaseUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (requestHeaders != null)
            {
                requestHeaders.ToList().ForEach(p => request.Headers.Add(p.Key, p.Value));
            }
            request.Method = verb.ToString();
            if (!string.IsNullOrEmpty(accept))
            {
                request.Accept = accept;
            }
            if (!string.IsNullOrEmpty(GOC.Instance.UserAgent))
            {
                request.UserAgent = GOC.Instance.UserAgent;
            }
            if (_networkCredential != null)
            {
                request.UseDefaultCredentials = false;
                request.Credentials = _networkCredential;
                request.AllowAutoRedirect = true;
            }
            if (!string.IsNullOrEmpty(postContentType))
            {
                request.ContentType = postContentType;
            }
            if (!string.IsNullOrEmpty(requestPostString))
            {
                request.ContentLength = Encoding.UTF8.GetByteCount(requestPostString);
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(requestPostString);
                    writer.Flush();
                    writer.Close();
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            if (timeout != 0)
            {
                request.Timeout = timeout;
            }
            string result = string.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    
                    if (response.StatusCode != HttpStatusCode.OK && 
                        response.StatusCode != HttpStatusCode.Created &&
                        response.StatusCode != HttpStatusCode.Accepted)
                    {
                        throw new UserThrownException(
                            string.Format("{0} {1} : {2}",
                                response.StatusCode.ToString(),
                                (int)response.StatusCode,
                                response.StatusDescription),
                            LoggingLevel.Normal);
                    }
                    statusCode = response.StatusCode;
                    statusDescription = response.StatusDescription;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            result = reader.ReadToEnd();
                            reader.Close();
                        }
                    }
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse response = (HttpWebResponse)wex.Response;
                if (response != null)
                {
                    statusCode = response.StatusCode;
                    statusDescription = response.StatusDescription;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            result = reader.ReadToEnd();
                            reader.Close();
                        }
                    }
                }
                if (response == null || !wrapWebException)
                {
                    throw wex;
                }
                throw new UserThrownException(
                    string.Format("{0} {1} : {2}",
                        response.StatusCode.ToString(),
                        (int)response.StatusCode,
                        response.StatusDescription),
                    LoggingLevel.Normal);
            }
            return result;
        }

        public virtual string PostBytes(
            string queryString,
            byte[] requestPostBytes,
            int timeout,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException)
        {
            HttpVerb verb = HttpVerb.POST;
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = verb.ToString();
            request.Accept = accept;
            if (!string.IsNullOrEmpty(GOC.Instance.UserAgent))
            {
                request.UserAgent = GOC.Instance.UserAgent;
            }
            if (_networkCredential != null)
            {
                request.UseDefaultCredentials = false;
                request.Credentials = _networkCredential;
                request.AllowAutoRedirect = true;
            }
            request.ContentType = MimeContentType.BINARY;
            if (requestPostBytes != null && requestPostBytes.Length > 0)
            {
                request.ContentLength = requestPostBytes.Length;
                using (BinaryWriter writer = new BinaryWriter(request.GetRequestStream()))
                {
                    writer.Write(requestPostBytes);
                    writer.Flush();
                    writer.Close();
                }
            }
            if (timeout != 0)
            {
                request.Timeout = timeout;
            }
            string result = string.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                    {
                        throw new UserThrownException(
                            string.Format("{0} {1} : {2}",
                                response.StatusCode.ToString(),
                                (int)response.StatusCode,
                                response.StatusDescription),
                            LoggingLevel.Normal);
                    }
                    statusCode = response.StatusCode;
                    statusDescription = response.StatusDescription;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            result = reader.ReadToEnd();
                            reader.Close();
                        }
                    }
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse response = (HttpWebResponse)wex.Response;
                if (response == null || !wrapWebException)
                {
                    throw wex;
                }
                string responseText = null;
                using (Stream stream = wex.Response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                throw new UserThrownException(
                    string.Format("{0} {1} : {2}",
                        response.StatusCode.ToString(),
                        (int)response.StatusCode,
                        response.StatusDescription),
                    LoggingLevel.Normal);
            }
            return result;
        }

        public object CallService(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string textOutput,
            int timeout,
            ISerializer serializer,
            Type[] postExtraSerializationTypes,
            string postContentType,
            Type resultEntityType,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException)
        {
            return CallService(
                queryString,
                requestPostObject,
                verb,
                out textOutput,
                timeout,
                serializer,
                postExtraSerializationTypes,
                postContentType,
                resultEntityType,
                accept,
                out statusCode,
                out statusDescription,
                wrapWebException,
                null);
        }

        public object CallService(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string textOutput,
            int timeout,
            ISerializer serializer,
            Type[] postExtraSerializationTypes,
            string postContentType,
            Type resultEntityType,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string inputText = null;
            if (requestPostObject != null)
            {
                if (postExtraSerializationTypes != null)
                {
                    inputText = serializer.SerializeToText(requestPostObject, postExtraSerializationTypes);
                }
                inputText = serializer.SerializeToText(requestPostObject);
            }
            textOutput = CallService(
                queryString,
                inputText,
                verb,
                postContentType,
                timeout,
                accept,
                out statusCode,
                out statusDescription,
                wrapWebException,
                requestHeaders);
            if (resultEntityType != null)
            {
                return serializer.DeserializeFromText(resultEntityType, textOutput);
            }
            return null;
        }

        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string textOutput,
            int timeout,
            ISerializer serializer,
            Type[] postExtraSerializationTypes,
            string postContentType,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException)
        {
            return CallService<T>(
                queryString,
                requestPostObject,
                verb,
                out textOutput,
                timeout,
                serializer,
                postExtraSerializationTypes,
                postContentType,
                accept,
                out statusCode,
                out statusDescription,
                wrapWebException,
                null);
        }

        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string textOutput,
            int timeout,
            ISerializer serializer,
            Type[] postExtraSerializationTypes,
            string postContentType,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            T result = (T)CallService(
                queryString,
                requestPostObject,
                verb,
                out textOutput,
                timeout,
                serializer,
                postExtraSerializationTypes,
                postContentType,
                typeof(T),
                accept,
                out statusCode,
                out statusDescription,
                wrapWebException,
                requestHeaders);
            return result;
        }

        #endregion //Methods
    }
}