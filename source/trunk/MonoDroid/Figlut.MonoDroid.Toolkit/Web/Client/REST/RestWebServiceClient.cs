namespace Figlut.MonoDroid.Toolkit.Web.Client.REST
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
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
