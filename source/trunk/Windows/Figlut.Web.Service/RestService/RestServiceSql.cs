namespace Figlut.Server
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.IO;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Web.Client;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Data.ORM;
    using Figlut.Server.Toolkit.Data;
    using System.Reflection;
    using System.Data.SqlClient;
    using System.Reflection.Emit;
    using System.Data;
    using Figlut.Server.Toolkit.Web;
    using Figlut.Server.Toolkit.Data.QueryRunners;
    using Figlut.Web.Service.Configuration;
    using System.Net;
    using Figlut.Web.Service.Utilities;
    using Figlut.Server.Toolkit.Extensions.WebService.Events.Crud;

    #endregion //Using Directives

    /// <summary>
    /// Collectible Assemblies: 
    /// http://msdn.microsoft.com/en-us/library/dd554932.aspx
    /// http://msdn.microsoft.com/en-us/library/system.reflection.emit.assemblybuilderaccess.aspx
    /// </summary>
    public partial class RestService
    {
        #region Methods

        #region REST Service Methods

        #region No Results Returned

        [OperationContract]
        [WebInvoke(UriTemplate = "/sql", Method = "PUT")]
        Stream ExecuteSqlRootNoResults(Stream inputStream)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return ExecuteQuery(
                    null,
                    inputStream,
                    inputSerializer,
                    outputSerializer,
                    WebOperationContext.Current.IncomingRequest.Accept);
            }
            catch (Exception ex)
            {
                Figlut.Server.Toolkit.Utilities.ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sql/xml", Method = "PUT")]
        Stream ExecuteSqlXmlNoResults(Stream inputStream)
        {
            try
            {
                return ExecuteQuery(
                    null,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.XML),
                    GOC.Instance.GetSerializer(SerializerType.XML),
                    MimeContentType.TEXT_PLAIN_XML);
            }
            catch (Exception ex)
            {
                Figlut.Server.Toolkit.Utilities.ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sql/json", Method = "PUT")]
        Stream ExecuteSqlJsonNoResults(Stream inputStream)
        {
            try
            {
                return ExecuteQuery(
                    null,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.JSON),
                    GOC.Instance.GetSerializer(SerializerType.JSON),
                    MimeContentType.TEXT_PLAIN_JSON);
            }
            catch (Exception ex)
            {
                Figlut.Server.Toolkit.Utilities.ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sql/csv", Method = "PUT")]
        Stream ExecuteSqlCsvNoResults(Stream inputStream)
        {
            try
            {
                return ExecuteQuery(
                    null,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.CSV),
                    GOC.Instance.GetSerializer(SerializerType.CSV),
                    MimeContentType.TEXT_PLAIN_CSV);
            }
            catch (Exception ex)
            {
                Figlut.Server.Toolkit.Utilities.ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        #endregion //No Results Returned

        #region Results Returned

        [OperationContract]
        [WebInvoke(UriTemplate = "/sql/{typeName}", Method = "PUT")]
        Stream ExecuteSqlRoot(string typeName, Stream inputStream)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return ExecuteQuery(
                    typeName, 
                    inputStream, 
                    inputSerializer, 
                    outputSerializer, 
                    WebOperationContext.Current.IncomingRequest.Accept);
            }
            catch (Exception ex)
            {
                Figlut.Server.Toolkit.Utilities.ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sql/{typeName}/xml", Method = "PUT")]
        Stream ExecuteSqlXml(string typeName, Stream inputStream)
        {
            try
            {
                return ExecuteQuery(
                    typeName,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.XML),
                    GOC.Instance.GetSerializer(SerializerType.XML),
                    MimeContentType.TEXT_PLAIN_XML);
            }
            catch (Exception ex)
            {
                Figlut.Server.Toolkit.Utilities.ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sql/{typeName}/json", Method = "PUT")]
        Stream ExecuteSqlJson(string typeName, Stream inputStream)
        {
            try
            {
                return ExecuteQuery(
                    typeName,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.JSON),
                    GOC.Instance.GetSerializer(SerializerType.JSON),
                    MimeContentType.TEXT_PLAIN_JSON);
            }
            catch (Exception ex)
            {
                Figlut.Server.Toolkit.Utilities.ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sql/{typeName}/csv", Method = "PUT")]
        Stream ExecuteSqlCsv(string typeName, Stream inputStream)
        {
            try
            {
                return ExecuteQuery(
                    typeName,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.CSV),
                    GOC.Instance.GetSerializer(SerializerType.CSV),
                    MimeContentType.TEXT_PLAIN_CSV);
            }
            catch (Exception ex)
            {
                Figlut.Server.Toolkit.Utilities.ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        #endregion //Results Returned

        #endregion //REST Service Methods

        Stream ExecuteQuery(
            string typeName,
            Stream inputStream,
            ISerializer inputSerializer,
            ISerializer outputSerializer,
            string acceptContentType)
        {
            LogRequest();
            ValidateRequestMethod(HttpVerb.PUT);
            string sqlQueryString = StreamHelper.GetStringFromStream(inputStream, GOC.Instance.Encoding);

            #region Interceptor Before Event

            BeforeWebInvokeSqlArgs eBefore = new BeforeWebInvokeSqlArgs(
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString(),
                GetHttpVerb(),
                WebOperationContext.Current.IncomingRequest.ContentType,
                WebOperationContext.Current.IncomingRequest.Accept,
                WebOperationContext.Current.IncomingRequest.UserAgent,
                sqlQueryString,
                inputSerializer,
                outputSerializer,
                typeName,
                sqlQueryString);
            FiglutWebServiceApplication.Instance.CrudInterceptor.PerformOnBeforeWebInvokeSql(eBefore);
            if (eBefore.Cancel)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = eBefore.HttpStatusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = eBefore.HttpStatusMessage;
                return StreamHelper.GetStreamFromString(eBefore.OutputString, GOC.Instance.Encoding);
            }

            #endregion //Interceptor Before Event

            string responseText = (new SqlQueryRunnerDispatcher(GOC.Instance.GetByTypeName<SqlQueryRunnerConfig>())).DispatchSqlQueryRunner(
                GOC.Instance.GetDatabase<SqlDatabase>().Name,
                typeName,
                sqlQueryString,
                acceptContentType,
                GOC.Instance.GetSettings<FiglutWebServiceSettings>().DatabaseConnectionString,
                _settings.IncludeOrmTypeNamesInJsonResponse);

            #region Interceptor After Event

            AfterWebInvokeSqlArgs eAfter = new AfterWebInvokeSqlArgs(
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString(),
                GetHttpVerb(),
                WebOperationContext.Current.IncomingRequest.ContentType,
                WebOperationContext.Current.IncomingRequest.Accept,
                WebOperationContext.Current.IncomingRequest.UserAgent,
                sqlQueryString,
                inputSerializer,
                outputSerializer,
                typeName,
                responseText);
            FiglutWebServiceApplication.Instance.CrudInterceptor.PerformOnAfterWebInvokeSql(eAfter);
            if (eAfter.OverrideHttpStatusResult)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = eAfter.HttpStatusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = eAfter.HttpStatusMessage;
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = "SQL query executed successfuly.";
            }
            if (eAfter.OverrideResponseOutputContent)
            {
                responseText = eAfter.OutputString;
            }

            #endregion //Interceptor After Event

            WebOperationContext.Current.OutgoingResponse.ContentType = MimeContentType.TEXT_PLAIN;
            LogResponse(responseText);
            return StreamHelper.GetStreamFromString(responseText, GOC.Instance.Encoding);
        }

        #endregion //Methods
    }
}
