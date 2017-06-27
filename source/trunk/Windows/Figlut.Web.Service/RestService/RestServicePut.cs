namespace Figlut.Server
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using System.ServiceModel.Web;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Web;
    using System.ServiceModel;
    using System.IO;
    using Figlut.Server.Toolkit.Web.Client;
    using Figlut.Server.Toolkit.Extensions.WebService.Events.Crud;
    using Figlut.Web.Service.Utilities;

    #endregion //Using Directives

    public partial class RestService
    {
        #region Methods

        #region No Column Name Update

        [OperationContract]
        [WebInvoke(UriTemplate = "/sqltable/{tableName}", Method = "PUT")]
        Stream PutEntityRoot(string tableName, Stream inputStream)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return PutEntity(tableName, null, inputStream, inputSerializer, outputSerializer);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sqltable/{tableName}/xml", Method = "PUT")]
        Stream PutEntityXml(string tableName, Stream inputStream)
        {
            try
            {
                return PutEntity(
                    tableName,
                    null,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.XML),
                    GOC.Instance.GetSerializer(SerializerType.XML));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sqltable/{tableName}/json", Method = "PUT")]
        Stream PutEntityJson(string tableName, Stream inputStream)
        {
            try
            {
                return PutEntity(
                    tableName,
                    null,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.JSON),
                    GOC.Instance.GetSerializer(SerializerType.JSON));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sqltable/{tableName}/csv", Method = "PUT")]
        Stream PutEntityCsv(string tableName, Stream inputStream)
        {
            try
            {
                return PutEntity(
                    tableName,
                    null,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.CSV),
                    GOC.Instance.GetSerializer(SerializerType.CSV));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        #endregion //No Column Name Update

        #region Column Name Update

        [OperationContract]
        [WebInvoke(UriTemplate = "/sqltable/{tableName}/{columnName}", Method = "PUT")]
        Stream PutEntityByColumnRoot(string tableName, string columnName, Stream inputStream)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return PutEntity(tableName, columnName, inputStream, inputSerializer, outputSerializer);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sqltable/{tableName}/{columnName}/xml", Method = "PUT")]
        Stream PutEntityByColumnXml(string tableName, string columnName, Stream inputStream)
        {
            try
            {
                return PutEntity(
                    tableName,
                    columnName,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.XML),
                    GOC.Instance.GetSerializer(SerializerType.XML));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sqltable/{tableName}/{columnName}/json", Method = "PUT")]
        Stream PutEntityByColumnJson(string tableName, string columnName, Stream inputStream)
        {
            try
            {
                return PutEntity(
                    tableName,
                    columnName,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.JSON),
                    GOC.Instance.GetSerializer(SerializerType.JSON));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/sqltable/{tableName}/{columnName}/csv", Method = "PUT")]
        Stream PutEntityByColumnCsv(string tableName, string columnName, Stream inputStream)
        {
            try
            {
                return PutEntity(
                    tableName,
                    columnName,
                    inputStream,
                    GOC.Instance.GetSerializer(SerializerType.CSV),
                    GOC.Instance.GetSerializer(SerializerType.CSV));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        #endregion //Column Name Update

        Stream PutEntity(
            string tableName, 
            string columnName, 
            Stream inputStream, 
            ISerializer inputSerializer,
            ISerializer outputSerializer)
        {
            LogRequest();
            ValidateRequestMethod(HttpVerb.PUT);
            string inputString = StreamHelper.GetStringFromStream(inputStream, GOC.Instance.Encoding);
            SqlDatabase db = GOC.Instance.GetDatabase<SqlDatabase>();
            DatabaseTable table = db.GetDatabaseTable(tableName);
            ValidateTable(table, tableName, db.Name);
            List<object> inputEntities = DataHelper.GetEntitiesFromSerializedText(inputString, table.MappedType, inputSerializer);

            #region Interceptor Before Event

            BeforeWebInvokeSqlTableArgs eBefore = new BeforeWebInvokeSqlTableArgs(
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString(),
                GetHttpVerb(),
                WebOperationContext.Current.IncomingRequest.ContentType,
                WebOperationContext.Current.IncomingRequest.Accept,
                WebOperationContext.Current.IncomingRequest.UserAgent,
                inputString,
                inputSerializer,
                outputSerializer,
                tableName,
                inputEntities,
                table.MappedType);
            FiglutWebServiceApplication.Instance.CrudInterceptor.PerformOnBeforeWebInvokeSqlTable(eBefore);
            if (eBefore.Cancel)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = eBefore.HttpStatusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = eBefore.HttpStatusMessage;
                return StreamHelper.GetStreamFromString(eBefore.OutputString, GOC.Instance.Encoding);
            }

            #endregion //Interceptor Before Event

            table.Update(inputEntities, columnName, true);
            string responseText = null;

            #region Interceptor After Event

            AfterWebInvokeSqlTableArgs eAfter = new AfterWebInvokeSqlTableArgs(
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString(),
                GetHttpVerb(),
                WebOperationContext.Current.IncomingRequest.ContentType,
                WebOperationContext.Current.IncomingRequest.Accept,
                WebOperationContext.Current.IncomingRequest.UserAgent,
                inputString,
                inputSerializer,
                outputSerializer,
                tableName,
                inputEntities);
            FiglutWebServiceApplication.Instance.CrudInterceptor.PerformOnAfterWebInvokeSqlTable(eAfter);
            if (eAfter.OverrideHttpStatusResult)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = eAfter.HttpStatusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = eAfter.HttpStatusMessage;
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = string.Format("{0} records updated in {1}.", inputEntities.Count, table.TableName);
            }
            if (eAfter.OverrideResponseOutputContent)
            {
                responseText = eAfter.OutputString;
            }
            else
            {
                responseText = string.Empty;
            }

            #endregion //Interceptor After Event

            WebOperationContext.Current.OutgoingResponse.ContentType = MimeContentType.TEXT_PLAIN;
            LogResponse(responseText);
            return StreamHelper.GetStreamFromString(responseText, GOC.Instance.Encoding);
        }

        #endregion //Methods
    }
}