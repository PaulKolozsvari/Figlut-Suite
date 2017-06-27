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
    using Figlut.Web.Service.Utilities;
    using Figlut.Server.Toolkit.Extensions.WebService.Events.Crud;
    using System.Net;

    #endregion //Using Directives

    public partial class RestService
    {
        #region Methods

        #region REST Service Methods

        #region Regular

        [OperationContract]
        [WebGet(UriTemplate = "/sqltable/{tableName}")]
        Stream GetEntitiesRoot(string tableName)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                Stream result = GetEntities(
                    tableName,
                    null, 
                    inputSerializer, 
                    outputSerializer);
                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebGet(UriTemplate = "/sqltable/{tableName}/xml")]
        Stream GetEntitiesXml(string tableName)
        {
            try
            {
                return GetEntities(
                    tableName,
                    null,
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
        [WebGet(UriTemplate = "/sqltable/{tableName}/json")]
        Stream GetEntitiesJson(string tableName)
        {
            try
            {
                return GetEntities(
                    tableName,
                    null,
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
        [WebGet(UriTemplate = "/sqltable/{tableName}/csv")]
        Stream GetEntitiesCsv(string tableName)
        {
            try
            {
                return GetEntities(
                    tableName,
                    null,
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

        #endregion //Regular

        #region Filters

        [OperationContract]
        [WebGet(UriTemplate = "/sqltable/{tableName}/{filters}")]
        Stream GetEntitiesRootFilter(string tableName, string filters)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return GetEntities(
                    tableName, 
                    filters, 
                    inputSerializer, 
                    outputSerializer);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        [OperationContract]
        [WebGet(UriTemplate = "/sqltable/{tableName}/{filters}/xml")]
        Stream GetEntitiesXmlFilter(string tableName, string filters)
        {
            try
            {
                return GetEntities(
                    tableName,
                    filters,
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
        [WebGet(UriTemplate = "/sqltable/{tableName}/{filters}/json")]
        Stream GetEntitiesJsonFilter(string tableName, string filters)
        {
            try
            {
                return GetEntities(
                    tableName,
                    filters,
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
        [WebGet(UriTemplate = "/sqltable/{tableName}/{filters}/csv")]
        Stream GetEntitiesCsvFilter(string tableName, string filters)
        {
            try
            {
                return GetEntities(
                    tableName,
                    filters,
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

        #endregion //Filters

        #endregion //REST Service Methods

        Stream GetEntities(string tableName, string filters, ISerializer inputSerializer, ISerializer outputSerializer)
        {
            LogRequest();
            ValidateRequestMethod(HttpVerb.GET);
            SqlDatabase db = GOC.Instance.GetDatabase<SqlDatabase>();
            DatabaseTable table = db.GetDatabaseTable(tableName);
            ValidateTable(table, tableName, db.Name);

            #region Interceptor Before Event

            BeforeWebGetSqlTableArgs eBefore = new BeforeWebGetSqlTableArgs(
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString(),
                GetHttpVerb(),
                WebOperationContext.Current.IncomingRequest.ContentType,
                WebOperationContext.Current.IncomingRequest.Accept,
                WebOperationContext.Current.IncomingRequest.UserAgent,
                inputSerializer,
                outputSerializer,
                tableName,
                filters,
                table.MappedType);
            FiglutWebServiceApplication.Instance.CrudInterceptor.PerformOnBeforeWebGetSqlTable(eBefore);
            if (eBefore.Cancel)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = eBefore.HttpStatusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = eBefore.HttpStatusMessage;
                return StreamHelper.GetStreamFromString(eBefore.OutputString, GOC.Instance.Encoding);
            }

            #endregion //Interceptor Before Event

            SqlQuery query = new SqlQuery(SqlQueryKeyword.SELECT);
            query.AppendSelectColumn(table.MappedType, true);
            if (!string.IsNullOrEmpty(filters))
            {
                query.AppendWhereColumns(ParseFilterToWhereClauseColumns(filters));
            }
            List<object> queryResult = db.Query(query, table.MappedType);
            Stream result = null;

            #region Interceptor After Event

            AfterWebGetSqlTableArgs eAfter = new AfterWebGetSqlTableArgs(
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString(),
                GetHttpVerb(),
                WebOperationContext.Current.IncomingRequest.ContentType,
                WebOperationContext.Current.IncomingRequest.Accept,
                WebOperationContext.Current.IncomingRequest.Accept,
                inputSerializer,
                outputSerializer,
                tableName,
                filters,
                table.MappedType,
                queryResult);
            FiglutWebServiceApplication.Instance.CrudInterceptor.PerformOnAfterWebGetSqlTable(eAfter);
            if (eAfter.OverrideHttpStatusResult)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = eAfter.HttpStatusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = eAfter.HttpStatusMessage;
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = "Query executed and entities generated successfully.";
            }
            if (eAfter.OverrideResponseOutputContent)
            {
                result = StreamHelper.GetStreamFromString(eAfter.OutputString, GOC.Instance.Encoding);
            }
            else
            {
                result = GetStreamFromObjects(queryResult, table.MappedType, outputSerializer);
            }

            #endregion //Interceptor After Event

            return result;
        }

        private List<WhereClauseColumn> ParseFilterToWhereClauseColumns(string filters)
        {
            List<WhereClauseColumn> result = new List<WhereClauseColumn>();
            if (string.IsNullOrEmpty(filters))
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                throw new UserThrownException("Filters may not be null or empty.", LoggingLevel.Maximum, false);
            }
            string[] filterItems = filters.Trim().Split(',');
            for(int i = 0; i < filterItems.Length; i++)
            {
                string f = filterItems[i];
                string[] filterArguments = f.Trim().Split('_');
                if (filterArguments.Length > 4 || filterArguments.Length < 3)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    throw new ArgumentException();
                    throw new UserThrownException(string.Format("Invalid number of arguments in filter {0}.", filterArguments), LoggingLevel.Maximum, false);
                }
                string filterName = filterArguments[0];
                WhereClauseComparisonOperator whereClauseComparisonOperator = new WhereClauseComparisonOperator(filterArguments[1]);
                string filterValue = filterArguments[2];
                WhereClauseLogicalOperator whereClauseLogicalOperator = null;
                if(filterArguments.Length > 3)
                {
                    LogicalOperator logicalOperator = (LogicalOperator)Enum.Parse(typeof(LogicalOperator), filterArguments[3], true);
                    whereClauseLogicalOperator = new WhereClauseLogicalOperator(logicalOperator);
                }
                result.Add(new WhereClauseColumn(
                    filterName,
                    whereClauseComparisonOperator,
                    filterValue,
                    true,
                    false,
                    whereClauseLogicalOperator));
            }
            return result;
        }

        #endregion //Methods
    }
}