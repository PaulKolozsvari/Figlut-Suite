namespace Figlut.Server.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Data;
    using System.Diagnostics;
    using System.Collections;
using System.Net;

    #endregion //Using Directives

    public class CsvWebServiceClient : WebServiceClient, IMimeWebServiceClient
    {
        #region Constructors

        public CsvWebServiceClient()
        {
        }

        public CsvWebServiceClient(string webServiceBaseUrl)
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
                timeout,
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
            bool deserializeToDotNetObject,
            string postContentType,
            int timeout,
            string accept,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException,
            Dictionary<string, string> requestHeaders)
        {
            if (deserializeToDotNetObject && DataHelper.GetGenericCollectionItemType(typeof(T)) == null)
            {
                throw new ArgumentException(string.Format(
                    "{0} only supports a generic collection (e.g. List<>) as the result type when deserialing the service result to a .NET object.",
                    this.GetType().FullName));
            }
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string inputText = null;
            if (serializePostObject && requestPostObject != null)
            {
                inputText = GOC.Instance.GetSerializer(SerializerType.CSV).SerializeToText(requestPostObject);
            }
            else if (requestPostObject != null)
            {
                inputText = requestPostObject.ToString();
            }
            rawOutput = base.CallService(
                queryString,
                inputText,
                verb,
                MimeContentType.TEXT_PLAIN_CSV,
                timeout,
                MimeContentType.TEXT_PLAIN_CSV,
                out statusCode,
                out statusDescription,
                wrapWebException,
                requestHeaders);
            T result = default(T);
            if (deserializeToDotNetObject)
            {
                List<object> deserializedObject = DataHelper.GetEntitiesFromSerializedText(
                    rawOutput, 
                    DataHelper.GetGenericCollectionItemType(typeof(T)), 
                    GOC.Instance.GetSerializer(SerializerType.CSV));
                return (T)DataHelper.GetListOfTypeFromObjectList(
                    (List<object>)deserializedObject,
                    DataHelper.GetGenericCollectionItemType(typeof(T)));
                
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
            if (deserializeToDotNetObject && DataHelper.GetGenericCollectionItemType(returnType) == null)
            {
                throw new ArgumentException(string.Format(
                    "{0} only supports a generic collection (e.g. List<>) as the result type when deserialing the service result to a .NET object.",
                    this.GetType().FullName));
            }
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string inputText = null;
            if (serializePostObject && requestPostObject != null)
            {
                inputText = GOC.Instance.GetSerializer(SerializerType.CSV).SerializeToText(requestPostObject);
            }
            else if (requestPostObject != null)
            {
                inputText = requestPostObject.ToString();
            }
            rawOutput = base.CallService(
                queryString,
                inputText,
                verb,
                MimeContentType.TEXT_PLAIN_CSV,
                timeout,
                MimeContentType.TEXT_PLAIN_CSV,
                out statusCode,
                out statusDescription,
                wrapWebException,
                requestHeaders);
            object result = null;
            if (deserializeToDotNetObject)
            {
                List<object> deserializedObject = DataHelper.GetEntitiesFromSerializedText(
                    rawOutput,
                    DataHelper.GetGenericCollectionItemType(returnType),
                    GOC.Instance.GetSerializer(SerializerType.CSV));

                result = DataHelper.GetListOfTypeFromObjectList(
                    (List<object>)deserializedObject,
                    DataHelper.GetGenericCollectionItemType(returnType));
            }
            return result;
        }

        #endregion //Methods
    }
}