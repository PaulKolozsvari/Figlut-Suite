using Figlut.MonoDroid.Toolkit.Data;

namespace Figlut.MonoDroid.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.MonoDroid.Toolkit.Utilities.Serialization;
    using Figlut.MonoDroid.Toolkit.Utilities;
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
            bool wrapWebException)
        {
            string csvOutput;
            return CallService<T>(
                queryString, 
                requestPostObject, 
                verb, 
                out csvOutput, 
                serializePostObject, 
                true, 
                timeout,
                out statusCode,
                out statusDescription,
                wrapWebException);
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
            bool wrapWebException)
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
                wrapWebException);
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
            bool wrapWebException)
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
                wrapWebException);
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