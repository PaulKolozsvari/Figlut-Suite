namespace Figlut.Server
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Data.ORM;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Web.Client;

    #endregion //Using Directives

    public partial class RestService
    {
        #region Methods

        #region REST Service Methods

        #region Regular

        [OperationContract]
        [WebGet(UriTemplate = "/sqlschema/{databaseSchemaInfoType}")]
        Stream GetDatabaseSchemaInfoRoot(string databaseSchemaInfoType)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return GetDatabaseSchemaInfo(
                    databaseSchemaInfoType,
                    null,
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
        [WebGet(UriTemplate = "/sqlschema/{databaseSchemaInfoType}/xml")]
        Stream GetDatabaseSchemaInfoXml(string databaseSchemaInfoType)
        {
            try
            {
                return GetDatabaseSchemaInfo(
                    databaseSchemaInfoType,
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
        [WebGet(UriTemplate = "/sqlschema/{databaseSchemaInfoType}/json")]
        Stream GetDatabaseSchemaInfoJson(string databaseSchemaInfoType)
        {
            try
            {
                return GetDatabaseSchemaInfo(
                    databaseSchemaInfoType,
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
        [WebGet(UriTemplate = "/sqlschema/{databaseSchemaInfoType}/csv")]
        Stream GetDatabaseSchemaInfoCsv(string databaseSchemaInfoType)
        {
            try
            {
                return GetDatabaseSchemaInfo(
                    databaseSchemaInfoType,
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
        [WebGet(UriTemplate = "/sqlschema/{databaseSchemaInfoType}/{filters}")]
        Stream GetDatabaseSchemaInfoRootFilter(string databaseSchemaInfoType, string filters)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                GetSerialisers(out inputSerializer, out outputSerializer);
                return GetDatabaseSchemaInfo(
                    databaseSchemaInfoType,
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
        [WebGet(UriTemplate = "/sqlschema/{databaseSchemaInfoType}/{filters}/xml")]
        Stream GetDatabaseSchemaInfoXmlFilter(string databaseSchemaInfoType, string filters)
        {
            try
            {
                return GetDatabaseSchemaInfo(
                    databaseSchemaInfoType,
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
        [WebGet(UriTemplate = "/sqlschema/{databaseSchemaInfoType}/{filters}/json")]
        Stream GetDatabaseSchemaInfoJsonFilter(string databaseSchemaInfoType, string filters)
        {
            try
            {
                return GetDatabaseSchemaInfo(
                    databaseSchemaInfoType,
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
        [WebGet(UriTemplate = "/sqlschema/{databaseSchemaInfoType}/{filters}/csv")]
        Stream GetDatabaseSchemaInfoCsvFilter(string databaseSchemaInfoType, string filters)
        {
            try
            {
                return GetDatabaseSchemaInfo(
                    databaseSchemaInfoType,
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

        Stream GetDatabaseSchemaInfo(
            string databaseSchemaInfoType,
            string filters,
            ISerializer inputSerializer, 
            ISerializer outputSerializer)
        {
            LogRequest();
            ValidateRequestMethod(HttpVerb.GET);
            DatabaseSchemaInfoType schemaInfoType;
            string supportedTypes = EnumHelper.GetEnumValuesAsCsv(typeof(DatabaseSchemaInfoType));
            if (!Enum.TryParse<DatabaseSchemaInfoType>(databaseSchemaInfoType, true, out schemaInfoType))
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                throw new UserThrownException(string.Format(
                    "Invalid databaseSchemaInfoType {0}. Only the following types of info are supported: {1}.",
                    databaseSchemaInfoType,
                    supportedTypes),
                    LoggingLevel.Maximum,
                    false);
            }
            SqlDatabase db = GOC.Instance.GetDatabase<SqlDatabase>();
            object result = null;
            switch (schemaInfoType)
            {
                case DatabaseSchemaInfoType.DatabaseName:
                    result = db.GetDatabaseNameFromSchema(null, true);
                    break;
                case DatabaseSchemaInfoType.Tables:
                    result = db.GetRawTablesSchema(true, null, true);
                    break;
                case DatabaseSchemaInfoType.Columns:
                    if (string.IsNullOrEmpty(filters)) //Filter should contain the name of the table for which columns should retrieved.
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        throw new UserThrownException(string.Format(
                            "The name of the table for which the columns metadata should be retrieved was not supplied in the filters of the the query string : {0}.",
                            WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri),
                            LoggingLevel.Maximum,
                            false);
                    }
                    DatabaseTable table = db.GetDatabaseTable(filters);
                    ValidateTable(table, filters, db.Name);
                    result = table.GetRawColumnsSchema(null, true);
                    break;
                case DatabaseSchemaInfoType.TableKeyColumns:
                    result = db.GetTableKeyColumns();
                    break;
                case DatabaseSchemaInfoType.TableForeignKeyColumns:
                    result = db.GetTableForeignKeyColumns();
                    break;
                default:
                    throw new UserThrownException(string.Format(
                        "Invalid databaseSchemaInfoType {0}. Only the following types of info are supported: {1}.",
                        databaseSchemaInfoType,
                        supportedTypes),
                        LoggingLevel.Maximum,
                        false);
            }

            string responseText = outputSerializer.SerializeToText(result, new Type[] { result.GetType() });
            LogResponse(responseText);
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            WebOperationContext.Current.OutgoingResponse.StatusDescription = "Schema retrieved successfully";
            return StreamHelper.GetStreamFromString(responseText, GOC.Instance.Encoding);
        }

        #endregion //Methods
    }
}