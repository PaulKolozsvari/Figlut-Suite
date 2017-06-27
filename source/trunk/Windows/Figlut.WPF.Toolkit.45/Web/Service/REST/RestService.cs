namespace Figlut.Server.Toolkit.Web.Service.REST
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.LINQ;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Web;
    using Figlut.Server.Toolkit.Web.Client;
    using Figlut.Server.Toolkit.Web.Service.REST.Events;

    #endregion //Using Directives

    public partial class RestService : IRestService
    {
        #region Constructors

        public RestService()
        {
        }

        #endregion //Constructors

        #region Events

        public event OnBeforeGetEntitiesHandler OnBeforeGetEntities;
        public event OnAfterGetEntitiesHandler OnAfterGetEntities;

        public event OnBeforeGetEntityByIdHandler OnBeforeGetEntityById;
        public event OnAfterGetEntityByIdHandler OnAfterGetEntityById;

        public event OnBeforeGetEntitiesByFieldHandler OnBeforeGetEntitiesByField;
        public event OnAfterGetEntitiesByFieldHandler OnAfterGetEntitiesByField;

        public event OnBeforePutEntityHandler OnBeforePut;
        public event OnAfterPutEntityHandler OnAfterPut;

        public event OnBeforeDeleteEntityHandler OnBeforeDelete;
        public event OnAfterDeleteEntityHandler OnAfterDelete;

        #endregion //Events

        #region Methods

        public Stream AllURIs()
        {
            try
            {
                return StreamHelper.GetStreamFromString("TekShed Web Service", GOC.Instance.Encoding);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        #region REST Service Methods

        public Stream GetEntities(string entityName)
        {
            try
            {
                ValidateRequestMethod(HttpVerb.GET);
                EntityContext context = GetEntityContext();
                Nullable<Guid> userId = null;
                string userName = null;
                GetUserDetails(context, out userId, out userName);
                Type entityType = GetEntityType(entityName);
                if (OnBeforeGetEntities != null)
                {
                    OnBeforeGetEntities(this, new RestServiceGetEntitiesEventArgs(
                        entityName, userId, userName, context, entityType, null));
                }
                List<object> outputEntities = context.GetAllEntities(
                    entityType,
                    false,
                    userId,
                    userName).Contents;
                if (OnAfterGetEntities != null)
                {
                    OnAfterGetEntities(this, new RestServiceGetEntitiesEventArgs(
                        entityName, userId, userName, context, entityType, outputEntities));
                }
                return GetStreamFromObject(outputEntities);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        public Stream GetEntityById(string entityName, string entityId)
        {
            try
            {
                ValidateRequestMethod(HttpVerb.GET);
                EntityContext context = GetEntityContext();
                Nullable<Guid> userId = null;
                string userName = null;
                GetUserDetails(context, out userId, out userName);
                Type entityType = GetEntityType(entityName);
                if (OnBeforeGetEntityById != null)
                {
                    OnBeforeGetEntityById(this, new RestServiceGetEntityByIdEventArgs(
                        entityName, userId, userName, context, entityType, entityId, null));
                }
                object outputEntity = context.GetEntitybySurrogateKey(
                    entityType,
                    entityId,
                    false,
                    userId,
                    userName).Contents;
                if (OnAfterGetEntityById != null)
                {
                    OnAfterGetEntityById(this, new RestServiceGetEntityByIdEventArgs(
                        entityName, userId, userName, context, entityType, entityId, outputEntity));
                }
                return GetStreamFromObject(outputEntity);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        public Stream GetEntitiesByField(string entityName, string fieldName, string fieldValue)
        {
            try
            {
                ValidateRequestMethod(HttpVerb.GET);
                EntityContext context = GetEntityContext();
                Nullable<Guid> userId = null;
                string userName = null;
                GetUserDetails(context, out userId, out userName);
                Type entityType = GetEntityType(entityName);
                if (OnBeforeGetEntitiesByField != null)
                {
                    OnBeforeGetEntitiesByField(this, new RestServiceGetEntitiesByFieldEventArgs(
                        entityName, userId, userName, context, entityType, fieldName, fieldValue, null));
                }
                List<object> outputEntities = context.GetEntitiesByField(
                    entityType,
                    fieldName,
                    fieldValue,
                    false,
                    userId,
                    userName).Contents;
                if(OnAfterGetEntitiesByField != null)
                {
                    OnAfterGetEntitiesByField(this, new RestServiceGetEntitiesByFieldEventArgs(
                        entityName, userId, userName, context, entityType, fieldName, fieldValue, outputEntities));
                }
                return GetStreamFromObject(outputEntities);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        public Stream PutEntity(string entityName, Stream inputStream)
        {
            try
            {
                ValidateRequestMethod(HttpVerb.PUT);
                EntityContext context = GetEntityContext();
                Nullable<Guid> userId = null;
                string userName = null;
                Type entityType = GetEntityType(entityName);
                GetUserDetails(context, out userId, out userName);
                object inputEntity = GetObjectFromStream(entityType, inputStream);
                if (OnBeforePut != null)
                {
                    OnBeforePut(this, new RestServicePutEntityEventArgs(
                        entityName, userId, userName, context, entityType, inputEntity));
                }
                context.Save(
                    entityType,
                    new List<object>() { inputEntity },
                    userId,
                    userName, 
                    false);
                if (OnAfterPut != null)
                {
                    OnAfterPut(this, new RestServicePutEntityEventArgs(
                        entityName, userId, userName, context, entityType, inputEntity));
                }
                return StreamHelper.GetStreamFromString(string.Format("{0} saved successfully.", entityName), GOC.Instance.Encoding);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        public Stream DeleteEntity(string entityName, string entityId)
        {
            try
            {
                ValidateRequestMethod(HttpVerb.DELETE);
                EntityContext context = GetEntityContext();
                Nullable<Guid> userId = null;
                string userName = null;
                GetUserDetails(context, out userId, out userName);
                Type entityType = GetEntityType(entityName);
                if (OnBeforeDelete != null)
                {
                    OnBeforeDelete(this, new RestServiceDeleteEntityEventArgs(
                        entityName, userId, userName, context, entityType, entityId));
                }
                context.DeleteBySurrogateKey(
                    entityType,
                    new List<object>() { entityId },
                    userId,
                    userName);
                if (OnAfterDelete != null)
                {
                    OnAfterDelete(this, new RestServiceDeleteEntityEventArgs(
                        entityName, userId, userName, context, entityType, entityId));
                }
                return StreamHelper.GetStreamFromString(string.Format("{0} deleted successfully.", entityName), GOC.Instance.Encoding);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        #endregion //REST Service Methods

        #region Utility Methods

        protected void UpdateHttpStatusOnException(Exception ex)
        {
            WebOperationContext context = WebOperationContext.Current;
            if (context.OutgoingResponse.StatusCode == HttpStatusCode.OK ||
                context.OutgoingResponse.StatusCode == HttpStatusCode.Created)
            {
                context.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            /*A status description with escape sequences causes the server to not respond to the request 
            causing the client to not get a response therefore not know what the exception was.*/
            string errorMessage = ex.Message.Replace("\r", string.Empty);
            errorMessage = errorMessage.Replace("\n", string.Empty);
            errorMessage = errorMessage.Replace("\t", string.Empty);
            context.OutgoingResponse.StatusDescription = errorMessage;
        }

        protected void ValidateRequestMethod(HttpVerb verb)
        {
            ValidateRequestMethod(verb.ToString());
        }

        protected void ValidateRequestMethod(string verb)
        {
            if (WebOperationContext.Current.IncomingRequest.Method != verb)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
                throw new UserThrownException(
                    string.Format(
                    "Unexpected Method of {0} on incoming POST Request {1}.",
                    WebOperationContext.Current.IncomingRequest.Method,
                    WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString()),
                    LoggingLevel.Normal);
            }
        }

        protected Stream GetStreamFromObject(object obj)
        {
            return StreamHelper.GetStreamFromString(GOC.Instance.JsonSerializer.SerializeToText(obj), GOC.Instance.Encoding);
        }

        protected E GetObjectFromStream<E>(Stream stream) where E : class
        {
            return (E)GetObjectFromStream(typeof(E), stream);
        }

        protected object GetObjectFromStream(Type entityType, Stream stream)
        {
            string inputText = StreamHelper.GetStringFromStream(stream, GOC.Instance.Encoding);
            object result = GOC.Instance.JsonSerializer.DeserializeFromText(entityType, inputText);
            if (result == null)
            {
                throw new Exception(string.Format("The following text could not be deserialized to a {0} : {1}", entityType.FullName, inputText));
            }
            return result;
        }

        protected void GetUserDetails(EntityContext context, out Nullable<Guid> userId, out string userName)
        {
            userId = null;
            userName = null;
            if (ServiceSecurityContext.Current != null && !string.IsNullOrEmpty(ServiceSecurityContext.Current.WindowsIdentity.Name))
            {
                userId = context.GetUserId(ServiceSecurityContext.Current.WindowsIdentity.Name);
                userName = ServiceSecurityContext.Current.WindowsIdentity.Name;
            }
        }

        protected Type GetEntityType(string entityName)
        {
            Type result = AssemblyReader.FindType(
                GOC.Instance.LinqToClassesAssembly,
                GOC.Instance.LinqToSQLClassesNamespace,
                entityName,
                false);
            if (result == null)
            {
                throw new NullReferenceException(string.Format("Could not find entity with name {0}.", entityName));
            }
            return result;
        }

        public static EntityContext GetEntityContext()
        {
            return new EntityContext(
                GOC.Instance.GetNewLinqToSqlDataContext(),
                GOC.Instance.GetByTypeName<LinqFunnelSettings>(),
                true,
                GOC.Instance.UserLinqToSqlType,
                GOC.Instance.ServerActionLinqToSqlType,
                GOC.Instance.ServerErrorLinqToSqlType);
        }

        #endregion //Utility Methods

        #endregion //Methods
    }
}
