namespace Figlut.Server.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Data;
    using System.Diagnostics;
    using System.Collections;

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
            int timeout)
        {
            string csvOutput;
            return CallService<T>(queryString, requestPostObject, verb, out csvOutput, false, timeout);
        }

        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string rawOutput,
            bool deserializeToDotNetObject,
            int timeout)
        {
            if (deserializeToDotNetObject && DataHelper.GetGenericCollectionItemType(typeof(T)) == null)
            {
                throw new ArgumentException(string.Format(
                    "{0} only supports a generic collection (e.g. List<>) as the result type when deserialing the service result to a .NET object.",
                    this.GetType().FullName));
            }
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string csvInput = null;
            if (requestPostObject != null)
            {
                csvInput = GOC.Instance.GetSerializer(SerializerType.CSV).SerializeToText(requestPostObject);
            }
            rawOutput = base.CallService(
                queryString,
                csvInput,
                verb,
                MimeContentType.TEXT_PLAIN_CSV,
                timeout,
                MimeContentType.TEXT_PLAIN_CSV);
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
            bool deserializeToDotNetObject, 
            int timeout)
        {
            if (deserializeToDotNetObject && DataHelper.GetGenericCollectionItemType(returnType) == null)
            {
                throw new ArgumentException(string.Format(
                    "{0} only supports a generic collection (e.g. List<>) as the result type when deserialing the service result to a .NET object.",
                    this.GetType().FullName));
            }
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string csvInput = null;
            if (requestPostObject != null)
            {
                csvInput = GOC.Instance.GetSerializer(SerializerType.CSV).SerializeToText(requestPostObject);
            }
            rawOutput = base.CallService(
                queryString,
                csvInput,
                verb,
                MimeContentType.TEXT_PLAIN_CSV,
                timeout,
                MimeContentType.TEXT_PLAIN_CSV);
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