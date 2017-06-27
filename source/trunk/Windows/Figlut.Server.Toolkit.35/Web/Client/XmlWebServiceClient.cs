namespace Figlut.Server.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Serialization;

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
            int timeout)
        {
            string xmlOutput;
            return CallService<T>(queryString, requestPostObject, verb, out xmlOutput, true, timeout);
        }

        public T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string rawOutput,
            bool deserializeToDotNetTObject,
            int timeout)
        {
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string xmlInput = null;
            if (requestPostObject != null)
            {
                xmlInput = GOC.Instance.GetSerializer(SerializerType.XML).SerializeToText(requestPostObject);
            }
            rawOutput = base.CallService(
                queryString,
                xmlInput,
                verb,
                MimeContentType.TEXT_PLAIN_XML,
                timeout,
                MimeContentType.TEXT_PLAIN_XML);
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
            bool deserializeToDotNetObject, 
            int timeout)
        {
            string uri = string.Format("{0}/{1}", _webServiceBaseUrl, queryString);
            string xmlInput = null;
            if (requestPostObject != null)
            {
                xmlInput = GOC.Instance.GetSerializer(SerializerType.XML).SerializeToText(requestPostObject);
            }
            rawOutput = base.CallService(
                queryString,
                xmlInput,
                verb,
                MimeContentType.TEXT_PLAIN_XML,
                timeout,
                MimeContentType.TEXT_PLAIN_XML);
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
