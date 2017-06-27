namespace Figlut.Server
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Extensions.WebService;
    using Figlut.Server.Toolkit.Extensions.WebService.Handlers;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Web.Client;
    using Figlut.Web.Service.Utilities;

    #endregion //Using Directives

    public partial class RestService
    {
        #region Methods

        [OperationContract]
        [WebInvoke(UriTemplate = "/extension/{handlerName}", Method = "PUT")]
        Stream PostToExtension(string handlerName, Stream inputStream)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return PostToExtension(handlerName, null, inputStream, inputSerializer, outputSerializer);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/extension/{handlerName}/{customParameters}", Method = "PUT")]
        Stream PostToExtensionWithCustomParameters(string handlerName, string customParameters, Stream inputStream)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return PostToExtension(handlerName, customParameters, inputStream, inputSerializer, outputSerializer);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw;
            }
        }

        Stream PostToExtension(string handlerName, string customParameters, Stream inputStream, ISerializer inputSerializer, ISerializer outputSerializer)
        {
            LogRequest();
            ValidateRequestMethod(HttpVerb.PUT);
            string inputString = StreamHelper.GetStringFromStream(inputStream, GOC.Instance.Encoding);
            if (FiglutWebServiceApplication.Instance.Extension == null)
            {
                throw new Exception("Web Service has no extension loaded.");
            }
            WebRequestToExtensionHandler handler = FiglutWebServiceApplication.Instance.Extension.GetHandler(handlerName);
            WebRequestToExtensionOutput output = handler.HandleWebRequest(new WebRequestToExtensionInput(
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString(),
                GetHttpVerb(),
                WebOperationContext.Current.IncomingRequest.ContentType,
                WebOperationContext.Current.IncomingRequest.Accept,
                WebOperationContext.Current.IncomingRequest.UserAgent,
                customParameters,
                inputString,
                inputSerializer,
                outputSerializer));
            WebOperationContext.Current.OutgoingResponse.StatusCode = output.OutputStatusCode;
            WebOperationContext.Current.OutgoingResponse.StatusDescription = output.OutputStatusDescription;
            return StreamHelper.GetStreamFromString(output.OutputString, GOC.Instance.Encoding);
        }

        #endregion //Methods
    }
}
