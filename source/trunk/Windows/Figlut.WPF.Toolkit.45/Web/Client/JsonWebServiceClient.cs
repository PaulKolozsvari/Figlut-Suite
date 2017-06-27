namespace Figlut.Server.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Net;
    using Newtonsoft.Json;
    using System.IO;
    using Figlut.Server.Toolkit.Web;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    /// <summary>
    /// A wrapper class for performing web service calls to a JSON Rest based web service.
    /// </summary>
    public class JsonWebServiceClient : WebServiceClient, IMimeWebServiceClient
    {
        #region Constructors

        /// <summary>
        /// A wrapper class for performing web service calls to a JSON Rest based web service.
        /// </summary>
        public JsonWebServiceClient()
        {
        }

        /// <summary>
        /// A wrapper class for performing web service calls to a JSON Rest based web service.
        /// </summary>
        /// <param name="webServiceBaseUrl">The base URL for the REST web service e.g. http://www.mydomains.com/api</param>
        public JsonWebServiceClient(string webServiceBaseUrl)
            : base(webServiceBaseUrl)
        {
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// Performs a web request to a JSON REST based web service by appending the 
        /// given query string to the WebServiceBaseUrl and applying the given HTTP verb.
        /// If the object to be posted is not null it will be serialized to a JSON string
        /// and then included in the web request to the service. Lastly it attempts to 
        /// deserialize the JSON text it receives from the service into an object of 
        /// the specified type T.
        /// </summary>
        /// <typeparam name="T">The .NET type of the object to be returned from the web service call.</typeparam>
        /// <param name="queryString">The string that will be appended to the end of the Web Service Base URL.</param>
        /// <param name="requestPostObject">The object to be serialized to JSON and be posted to the service.</param>
        /// <param name="verb">The HTTP verb to be applied to the web request.</verb>
        /// <param name="timeout">The timeout of the web request</param>
        /// <returns>Returns JSON result deserialized into an object of the specified type T.</returns>
        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            bool serializePostObject,
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException)
        {
            string jsonOutput;
            return CallService<T>(
                queryString, 
                requestPostObject, 
                verb, 
                out jsonOutput, 
                serializePostObject, 
                true, timeout,
                out statusCode,
                out statusDescription,
                wrapWebException);
        }


        /// <summary>
        /// Performs a web request to a JSON REST based web service by appending the 
        /// given query string to the WebServiceBaseUrl and applying the given HTTP verb.
        /// If the object to be posted is not null it will be serialized to a JSON string
        /// and then included in the web request to the service. Lastly it attempts to 
        /// deserialize the JSON text it receives from the service into an object of 
        /// the specified type T.
        /// </summary>
        /// <typeparam name="T">The .NET type of the object to be returned from the web service call.</typeparam>
        /// <param name="queryString">The string that will be appended to the end of the Web Service Base URL.</param>
        /// <param name="requestPostObject">The object to be serialized to JSON and be posted to the service.</param>
        /// <param name="verb">The HTTP verb to be applied to the web request.</verb>
        /// <param name="jsonOutput">The JSON text returned by the web service call.</param>
        /// <param name="timeout">The timeout of the web request</param>
        /// <returns>Returns JSON result deserialized into an object of the specified type T.</returns>
        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string rawOutput,
            bool serializePostObject,
            bool deserializeToDotNetObject,
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException)
        {
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string inputText = null;
            if (serializePostObject && requestPostObject != null)
            {
                inputText = GOC.Instance.GetSerializer(SerializerType.JSON).SerializeToText(requestPostObject);
            }
            else if(requestPostObject != null)
            {
                inputText = requestPostObject.ToString();
            }
            rawOutput = base.CallService(
                queryString,
                inputText,
                verb,
                MimeContentType.TEXT_PLAIN_JSON,
                timeout,
                MimeContentType.TEXT_PLAIN_JSON,
                out statusCode,
                out statusDescription,
                wrapWebException);
            T result = default(T);
            if (deserializeToDotNetObject)
            {
                result = (T)GOC.Instance.GetSerializer(SerializerType.JSON).DeserializeFromText(typeof(T), rawOutput);
            }
            return result;
        }

        public object CallService(
            Type returnType, 
            string queryString, 
            object requestPostObject, 
            HttpVerb verb, out string 
            rawOutput, 
            bool serializePostObject,
            bool deserializeToDotNetObject,
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException)
        {
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string inputText = null;
            if (serializePostObject && requestPostObject != null)
            {
                inputText = GOC.Instance.GetSerializer(SerializerType.JSON).SerializeToText(requestPostObject);
            }
            else if(requestPostObject != null)
            {
                inputText = requestPostObject.ToString();
            }
            rawOutput = base.CallService(
                queryString,
                inputText,
                verb,
                MimeContentType.TEXT_PLAIN_JSON,
                timeout,
                MimeContentType.TEXT_PLAIN_JSON,
                out statusCode,
                out statusDescription,
                wrapWebException);
            object result = null;
            if (deserializeToDotNetObject)
            {
                result = GOC.Instance.GetSerializer(SerializerType.JSON).DeserializeFromText(returnType, rawOutput);
            }
            return result;
        }

        #endregion //Methods
    }
}