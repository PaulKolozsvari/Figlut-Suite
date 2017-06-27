namespace Figlut.Server.Toolkit.Web.Client.REST
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    /// <summary>
    /// A web service client wrapper that can be used to talk with a REST service on a web server.
    /// N.B. A REST web service needs to exist on the web server with methods matching those expected by this RestWebService.
    /// </summary>
    public class RestWebServiceClient
    {
        #region Constructors

        public RestWebServiceClient(IMimeWebServiceClient webServiceClient, int timeout)
        {
            _webServiceClient = webServiceClient;
            _timeout = timeout;
        }

        #endregion //Constructors

        #region Inner Types

        public delegate void FileTransferProgressHandler(FileTransferProgressResult e);
        public event FileTransferProgressHandler OnFileTransferProgress;

        #endregion //Inner Types

        #region Fields

        protected IMimeWebServiceClient _webServiceClient;
        protected int _timeout;

        #endregion //Fields

        #region Properties

        public IMimeWebServiceClient WebServiceClient
        {
            get { return _webServiceClient; }
        }

        public int Timeout
        {
            get { return _timeout; }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Gets all the entities of a REST web service according to this patern: baseURL/{entityName}
        /// e.g. http://hostname:1983/User to get all users.
        /// </summary>
        /// <typeparam name="E">The name of the entities to get e.g. User</typeparam>
        /// <returns>A list of the specified entities i.e. a list of users.</returns>
        public List<E> GetAllEntities<E>()
        {
            string rawOut = null;
            HttpStatusCode httpStatusCode;
            string httpStatusDescription = null;
            List<E> result = _webServiceClient.CallService<List<E>>(
                typeof(E).Name,
                null,
                HttpVerb.GET,
                out rawOut,
                false,
                true,
                _timeout,
                out httpStatusCode,
                out httpStatusDescription,
                true);
            return result;
        }

        /// <summary>
        /// Gets a specific entity based on its surrogate key according to this pattern: baseURL/{entityName}/{entityId}
        /// e.g. http://hostname:1983/User/3 to get a user with the user ID of 3.
        /// </summary>
        /// <typeparam name="E">The name of the entity to get e.g. User</typeparam>
        /// <param name="entityId"></param>
        /// <returns>The entity.</returns>
        public E GetEntityById<E>(object entityId)
        {
            string rawOut = null;
            HttpStatusCode httpStatusCode;
            string httpStatusDescription = null;
            E result = _webServiceClient.CallService<E>(
                string.Format("{0}/{1}", typeof(E).Name, entityId.ToString()),
                null,
                HttpVerb.GET,
                out rawOut,
                false,
                true,
                _timeout,
                out httpStatusCode,
                out httpStatusDescription,
                true);
            return result;
        }

        /// <summary>
        /// Gets entities (based on specific field's value), from a REST web service according to this pattern. baseURL/{entityName}?searchBy={fieldName}&searchValueOf={fieldValue}.
        /// </summary>
        /// <typeparam name="E">The name of the list entities to get e.g. User</typeparam>
        /// <param name="fieldName">The field of the entity to search by.</param>
        /// <param name="fieldValue">The value of the field of the entity to search on.</param>
        /// <returns></returns>
        public List<E> GetEntitiesByField<E>(string fieldName, object fieldValue)
        {
            string rawOut = null;
            HttpStatusCode httpStatusCode;
            string httpStatusDescription = null;
            List<E> result = _webServiceClient.CallService<List<E>>(
                string.Format("/{0}?searchBy={1}&searchValueOf={2}", typeof(E).Name, fieldName, fieldValue.ToString()),
                null,
                HttpVerb.GET,
                out rawOut,
                false,
                true,
                _timeout,
                out httpStatusCode,
                out httpStatusDescription,
                true);
            return result;
        }

        /// <summary>
        /// Puts an entity to the REST web service according to this pattern: baseURL/{baseURL} with the entity serialized into the body of the web request.
        /// e.g. http://hostname:1983/User 
        /// </summary>
        /// <typeparam name="E">The name of the entity to put e.g. User</typeparam>
        /// <param name="e">The entity to put.</param>
        /// <returns>The server response message.</returns>
        public string PutEntity<E>(E e)
        {
            string rawOut = null;
            HttpStatusCode httpStatusCode;
            string httpStatusDescription = null;
            _webServiceClient.CallService<string>(
                typeof(E).Name,
                e,
                HttpVerb.PUT,
                out rawOut,
                true,
                false,
                _timeout,
                out httpStatusCode,
                out httpStatusDescription,
                true);
            return rawOut;
        }

        /// <summary>
        /// Posts an entity to the REST web service according to this pattern: baseURL/{baseURL} with the entity serialized into the body of the web request.
        /// e.g. http://hostname:1983/User 
        /// </summary>
        /// <typeparam name="E">The name of the entity to post e.g. User</typeparam>
        /// <param name="e">The entity to post.</param>
        /// <returns>The server response message.</returns>
        public string PostEntity<E>(E e)
        {
            string rawOut = null;
            HttpStatusCode httpStatusCode;
            string httpStatusDescription = null;
            _webServiceClient.CallService<string>(
                typeof(E).Name,
                e,
                HttpVerb.POST,
                out rawOut,
                true,
                false,
                _timeout,
                out httpStatusCode,
                out httpStatusDescription,
                true);
            return rawOut;
        }

        public string FileUpload(
            string fileUploadQueryString,
            string filePath,
            int bufferSize,
            string fileUploadCompletedQueryString)
        {
            FileSystemHelper.ValidateFileExists(filePath);
            string fileName = Path.GetFileName(filePath);
            StringBuilder result = new StringBuilder();
            using (FileStream fs = File.Open(filePath, FileMode.Open))
            {
                string rawOutput = null;
                HttpStatusCode httpStatusCode;
                string httpStatusDescription = null;

                byte[] buffer = new byte[bufferSize];
                int bytesRead = fs.Read(buffer, 0, bufferSize);
                long totalTransferredBytes = bytesRead;
                while (bytesRead != 0)
                {
                    //Upload chunks to the service.
                    try
                    {
                        _webServiceClient.PostBytes(
                            fileUploadQueryString,
                            buffer,
                            _timeout,
                            null,
                            out httpStatusCode,
                            out httpStatusDescription,
                            true);
                    }
                    catch (UserThrownException ex)
                    {
                        if (!ex.Message.Contains("400"))
                        {
                            throw ex;
                        }
                        _webServiceClient.PostBytes(
                            fileUploadQueryString,
                            buffer,
                            _timeout,
                            null,
                            out httpStatusCode,
                            out httpStatusDescription,
                            true);
                    }
                    if (OnFileTransferProgress != null)
                    {
                        OnFileTransferProgress(new FileTransferProgressResult(fileName, totalTransferredBytes, fs.Length));
                    }
                    buffer = new byte[bufferSize];
                    bytesRead = fs.Read(buffer, 0, bufferSize);
                    totalTransferredBytes += bytesRead;
                    result.AppendLine(rawOutput);
                }
                //Tell the service to close the file.
                _webServiceClient.CallService<string>(
                    fileUploadCompletedQueryString,
                    null,
                    HttpVerb.POST,
                    out rawOutput,
                    false,
                    false,
                    _timeout,
                    out httpStatusCode,
                    out httpStatusDescription,
                    true);
                result.AppendLine(rawOutput);
            }
            return result.ToString();
        }

        /// <summary>
        /// Deletes an entity with the specified ID on the REST web service according to this pattern: baseURL/{entityName}/{entityId}
        /// e.g. http://hostname:1983/User/3 to delete a user with the user ID of 3.
        /// </summary>
        /// <typeparam name="E">The name of the entity to delete e.g. User.</typeparam>
        /// <param name="entityId">The ID of the entity to be deleted.</param>
        /// <returns>The server response message.</returns>
        public string DeleteById<E>(object entityId)
        {
            string rawOut = null;
            HttpStatusCode httpStatusCode;
            string httpStatusDescription = null;
            _webServiceClient.CallService<string>(
                string.Format("{0}/{1}", typeof(E).Name, entityId.ToString()),
                null,
                HttpVerb.DELETE,
                out rawOut,
                false,
                false,
                _timeout,
                out httpStatusCode,
                out httpStatusDescription,
                true);
            return rawOut;
        }

        #endregion //Methods
    }
}
