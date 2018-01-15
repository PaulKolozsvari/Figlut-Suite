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
    using System.ServiceModel.Channels;

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

        public event OnBeforePostEntityHandler OnBeforePost;
        public event OnAfterPostEntityHandler OnAfterPost;

        public event OnBeforeDeleteEntityHandler OnBeforeDelete;
        public event OnAfterDeleteEntityHandler OnAfterDelete;

        #endregion //Events

        #region Methods

        public string GetCurrentRequestUri()
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            string result = request.UriTemplateMatch.RequestUri.OriginalString;
            return result;
        }

        public string GetAllHeadersFullString()
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            return request.Headers.ToString();
        }

        public string GetAllHeadersFormatted()
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            StringBuilder result = new StringBuilder();
            foreach (var key in request.Headers.AllKeys)
            {
                result.AppendLine(string.Format("{0}={1}", key, request.Headers[key]));
            }
            return result.ToString();
        }

        public string GetHeader(string key, bool throwExceptionOnNotFound)
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            string result = string.Empty;
            if (request != null && request.Headers.HasKeys())
            {
                result = request.Headers.Get(key);
            }
            if (result == null && throwExceptionOnNotFound)
            {
                throw new NullReferenceException(string.Format("Could not find HTTP Header with key {0}.", key));
            }
            return result;
        }

        public virtual Stream AllURIs()
        {
            try
            {
                string allHeadersFullString = GetAllHeadersFullString();
                StringBuilder message = new StringBuilder();
                string applicationName = string.IsNullOrEmpty(GOC.Instance.ApplicationName) ? "REST Web Service" : GOC.Instance.ApplicationName;
                message.AppendLine(applicationName);
                message.AppendLine();
                message.AppendLine("HTTP Headers:");
                message.AppendLine();
                message.AppendLine(allHeadersFullString);
                message.AppendLine();
                return StreamHelper.GetStreamFromString(
                    message.ToString(), 
                    GOC.Instance.Encoding);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        #region REST Service Methods

        public virtual Stream GetEntities(string entityName)
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

        public virtual Stream GetEntityById(string entityName, string entityId)
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

        public virtual Stream GetEntitiesByField(string entityName, string fieldName, string fieldValue)
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

        public virtual Stream PutEntity(string entityName, Stream inputStream)
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

        public virtual Stream PostEntity(string entityName, Stream inputStream)
        {
            try
            {
                ValidateRequestMethod(HttpVerb.POST);
                EntityContext context = GetEntityContext();
                Nullable<Guid> userId = null;
                string userName = null;
                Type entityType = GetEntityType(entityName);
                GetUserDetails(context, out userId, out userName);
                object inputEntity = GetObjectFromStream(entityType, inputStream);
                if (OnBeforePost != null)
                {
                    OnBeforePost(this, new RestServicePostEntityEventArgs(
                        entityName, userId, userName, context, entityType, inputEntity));
                }
                context.Insert(
                    entityType,
                    new List<object>() { inputEntity },
                    userId,
                    userName,
                    false);
                if (OnAfterPost != null)
                {
                    OnAfterPost(this, new RestServicePostEntityEventArgs(
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

        public virtual Stream DeleteEntity(string entityName, string entityId)
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

        //http://blogs.msdn.com/b/webapps/archive/2012/09/06/wcf-chunking.aspx
        public Stream FileUpload(string fileName, Stream inputStream)
        {
            try
            {
                ValidateRequestMethod(HttpVerb.POST);
                byte[] fileBytes = StreamHelper.GetBytesFromStream(inputStream);
                FileStream fs = null;
                lock (FileUploadSessions.Instance.FileStreams)
                {
                    FileUploadSessions.Instance.FileStreams.TryGetValue(fileName, out fs);
                    if (fs == null)
                    {
                        fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
                        FileUploadSessions.Instance.FileStreams.Add(fileName, fs);
                    }
                    fs.Write(fileBytes, 0, fileBytes.Length);
                    fs.Flush();
                }
                return StreamHelper.GetStreamFromString(
                    string.Format("{0} bytes written to {1}", fileBytes.Length, fileName),
                    GOC.Instance.Encoding);
            }
            catch (Exception ex)
            {
                FileUploadCompleted(fileName);
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw ex;
            }
        }

        public Stream FileUploadCompleted(string fileName)
        {
            try
            {
                ValidateRequestMethod(HttpVerb.POST);
                FileStream fs = null;
                lock (FileUploadSessions.Instance.FileStreams)
                {
                    FileUploadSessions.Instance.FileStreams.TryGetValue(fileName, out fs);
                    if (fs == null)
                    {
                        throw new Exception(string.Format("Could not find {0} to closse for file {1}", typeof(FileStream).Name, fileName));
                    }
                    fs.Close();
                    fs.Dispose();
                    FileUploadSessions.Instance.FileStreams.Remove(fileName);
                }
                return StreamHelper.GetStreamFromString(string.Format("{0} for {1} closed.", typeof(FileStream).Name, fileName), GOC.Instance.Encoding);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw;
            }
        }

        public Stream DownloadFile(string fileName)
        {
            try
            {
                FileSystemHelper.ValidateDirectoryExists(fileName);
                return File.OpenRead(fileName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                UpdateHttpStatusOnException(ex);
                throw;
            }
        }

        #endregion //REST Service Methods

        #region Utility Methods

        protected virtual void UpdateHttpStatusOnException(Exception ex)
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

        protected virtual void ValidateRequestMethod(HttpVerb verb)
        {
            ValidateRequestMethod(verb.ToString());
        }

        protected virtual void ValidateRequestMethod(string verb)
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

        protected virtual Stream GetStreamFromObject(object obj)
        {
            return StreamHelper.GetStreamFromString(GOC.Instance.JsonSerializer.SerializeToText(obj), GOC.Instance.Encoding);
        }

        protected virtual Stream GetStreamFromObject(object obj, Type[] extraTypes)
        {
            return StreamHelper.GetStreamFromString(GOC.Instance.JsonSerializer.SerializeToText(obj, extraTypes), GOC.Instance.Encoding);
        }

        protected virtual Stream GetStreamFromObject(object obj, ISerializer serializer)
        {
            return StreamHelper.GetStreamFromString(serializer.SerializeToText(obj), GOC.Instance.Encoding);
        }

        protected virtual Stream GetStreamFromObject(object obj, ISerializer serializer, Type[] extraTypes)
        {
            return StreamHelper.GetStreamFromString(serializer.SerializeToText(obj, extraTypes), GOC.Instance.Encoding);
        }

        protected virtual E GetObjectFromStream<E>(Stream stream) where E : class
        {
            return (E)GetObjectFromStream(typeof(E), stream);
        }

        protected virtual object GetObjectFromStream(Type entityType, Stream stream)
        {
            string inputText = StreamHelper.GetStringFromStream(stream, GOC.Instance.Encoding);
            object result = GOC.Instance.JsonSerializer.DeserializeFromText(entityType, inputText);
            if (result == null)
            {
                throw new Exception(string.Format("The following text could not be deserialized to a {0} : {1}", entityType.FullName, inputText));
            }
            return result;
        }

        protected virtual void GetUserDetails(EntityContext context, out Nullable<Guid> userId, out string userName)
        {
            userId = null;
            userName = null;
            if (ServiceSecurityContext.Current != null && !string.IsNullOrEmpty(ServiceSecurityContext.Current.WindowsIdentity.Name))
            {
                if (context.UserLinqToSqlType != null)
                {
                    userId = context.GetUserId(ServiceSecurityContext.Current.WindowsIdentity.Name);
                }
                userName = ServiceSecurityContext.Current.WindowsIdentity.Name;
            }
        }

        protected virtual Type GetEntityType(string entityName)
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
                false,
                GOC.Instance.UserLinqToSqlType,
                GOC.Instance.ServerActionLinqToSqlType,
                GOC.Instance.ServerErrorLinqToSqlType);
        }

        protected string GetCurrentRequestClientIpAddress()
        {
            MessageProperties props = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpointProperty = props[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string result = null;
            if (endpointProperty != null)
            {
                result = endpointProperty.Address;
            }
            return result;
        }

        #endregion //Utility Methods

        #endregion //Methods
    }
}
