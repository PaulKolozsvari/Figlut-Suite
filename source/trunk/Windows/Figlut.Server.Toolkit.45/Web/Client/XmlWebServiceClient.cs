namespace Figlut.Server.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using System.Net;

    #endregion //Using Directives

    public class XmlWebServiceClient : WebServiceClient, IMimeWebServiceClient
    {
        #region Constructors

        public XmlWebServiceClient()
        {
        }

        public XmlWebServiceClient(string webServiceBaseUrl)
            : base(webServiceBaseUrl)
        {
        }

        #endregion //Constructors

        #region Methods

        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            bool serializePostObject,
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            string rawOutput;
            return CallService<T>(
                queryString,
                requestPostObject,
                verb,
                out rawOutput,
                serializePostObject,
                true,
                MimeContentType.TEXT_PLAIN_XML,
                timeout,
                MimeContentType.TEXT_PLAIN_XML,
                out statusCode,
                out statusDescription,
                wrapWebException,
                requestHeaders);
        }

        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            bool serializePostObject,
            string postContentType,
            int timeout,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            string rawOutput;
            return CallService<T>(
                queryString,
                requestPostObject,
                verb,
                out rawOutput,
                serializePostObject,
                true,
                postContentType,
                timeout,
                accept,
                out statusCode,
                out statusDescription,
                wrapWebException,
                requestHeaders);
        }

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
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            return CallService<T>(
                queryString,
                requestPostObject,
                verb,
                out rawOutput,
                serializePostObject,
                deserializeToDotNetObject,
                MimeContentType.TEXT_PLAIN_XML,
                timeout,
                MimeContentType.TEXT_PLAIN_XML,
                out statusCode,
                out statusDescription,
                wrapWebException,
                requestHeaders);
        }

        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string rawOutput,
            bool serializePostObject,
            bool deserializeToDotNetTObject,
            string postContentType,
            int timeout,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string inputText = null;
            if (serializePostObject && requestPostObject != null)
            {
                inputText = GOC.Instance.GetSerializer(SerializerType.XML).SerializeToText(requestPostObject);
            }
            else if(requestPostObject != null)
            {
                inputText = requestPostObject.ToString();
            }
            rawOutput = base.CallService(
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
            T result = default(T);
            if (deserializeToDotNetTObject)
            {
                result = (T)GOC.Instance.GetSerializer(SerializerType.XML).DeserializeFromText(typeof(T), rawOutput);
            }
            return result;
        }

        public object CallService(
            Type returnType, 
            string queryString, 
            object requestPostObject, 
            HttpVerb verb, 
            out string rawOutput,
            bool serializePostObject,
            bool deserializeToDotNetObject, 
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string inputText = null;
            if (serializePostObject && requestPostObject != null)
            {
                inputText = GOC.Instance.GetSerializer(SerializerType.XML).SerializeToText(requestPostObject);
            }
            else if (requestPostObject != null)
            {
                inputText = requestPostObject.ToString();
            }
            rawOutput = base.CallService(
                queryString,
                inputText,
                verb,
                MimeContentType.TEXT_PLAIN_XML,
                timeout,
                MimeContentType.TEXT_PLAIN_XML,
                out statusCode,
                out statusDescription,
                wrapWebException,
                requestHeaders);
            object result = null;
            if (deserializeToDotNetObject)
            {
                result = GOC.Instance.GetSerializer(SerializerType.XML).DeserializeFromText(returnType, rawOutput);
            }
            return result;
        }

        #endregion //Methods
    }
}
