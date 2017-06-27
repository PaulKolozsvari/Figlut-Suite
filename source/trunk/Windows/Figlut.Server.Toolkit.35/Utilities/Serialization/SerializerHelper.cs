namespace Figlut.Server.Toolkit.Utilities.Serialization
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Web;

    #endregion //Using Directives

    public class SerializerHelper
    {
        #region Methods

        public static void GetSerialisers(string contentType, string acceptContentType, out ISerializer inputSerializer, out ISerializer outputSerializer)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                inputSerializer = GOC.Instance.GetSerializer(SerializerType.JSON);
            }
            else
            {
                switch (contentType)
                {
                    case MimeContentType.TEXT_PLAIN_XML:
                        inputSerializer = GOC.Instance.GetSerializer(SerializerType.XML);
                        break;
                    case MimeContentType.TEXT_PLAIN_JSON:
                        inputSerializer = GOC.Instance.GetSerializer(SerializerType.JSON);
                        break;
                    case MimeContentType.TEXT_PLAIN_CSV:
                        inputSerializer = GOC.Instance.GetSerializer(SerializerType.CSV);
                        break;
                    default:
                        throw new UserThrownException(string.Format(
                            "Invalid Content type: {0}. Only {1} or {2} allowed.",
                            contentType,
                            MimeContentType.TEXT_PLAIN_XML,
                            MimeContentType.TEXT_PLAIN_JSON));
                }
            }
            if (string.IsNullOrEmpty(acceptContentType))
            {
                outputSerializer = GOC.Instance.GetSerializer(SerializerType.JSON);
            }
            else
            {
                switch (acceptContentType)
                {
                    case MimeContentType.TEXT_PLAIN_XML:
                        outputSerializer = GOC.Instance.GetSerializer(SerializerType.XML);
                        break;
                    case MimeContentType.TEXT_PLAIN_JSON:
                        outputSerializer = GOC.Instance.GetSerializer(SerializerType.JSON);
                        break;
                    case MimeContentType.TEXT_PLAIN_CSV:
                        outputSerializer = GOC.Instance.GetSerializer(SerializerType.CSV);
                        break;
                    default:
                        throw new UserThrownException(string.Format(
                            "Invalid Accept type: {0}. Only {1}, {2} or {3} allowed.",
                            acceptContentType,
                            MimeContentType.TEXT_PLAIN_XML,
                            MimeContentType.TEXT_PLAIN_JSON,
                            MimeContentType.TEXT_PLAIN_CSV));
                }
            }
        }

        public static string SerializeEntities(
            List<object> entities,
            Type dotNetType,
            ISerializer serializer,
            bool includeOrmTypeNamesInJsonResponse)
        {
            Array result = DataHelper.ConvertObjectsToTypedArray(dotNetType, entities);
            if (serializer is JSerializer)
            {
                ((JSerializer)serializer).IncludeOrmTypeNamesInJsonResponse = includeOrmTypeNamesInJsonResponse;
            }
            return serializer.SerializeToText(result, new Type[] { dotNetType, result.GetType() });
        }

        #endregion //Methods
    }
}
