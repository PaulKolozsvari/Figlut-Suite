namespace Figlut.Mobile.Toolkit.Web.Client
{
    /// <summary>
    /// The method/verb of the web request made to the web service.
    /// Normally corresponds to typical CRUD (Create/Read/Update/Delete) operations
    /// although you should not assume that it directly corresponds to SQL operations i.e. 
    /// it depends on how the web service was implementated to interpret the verb.
    /// </summary>
    public enum HttpVerb
    {
        /// <summary>
        /// A get request. Normally used to query data through the web service.
        /// </summary>
        GET,
        /// <summary>
        /// A post request. Normally used the update data through the web service.
        /// However, PUT and POST are normally interchangeable depending on the
        /// web service implementation.
        /// </summary>
        POST,
        /// <summary>
        /// A put request. Normally used to update data through the web service.
        /// However, PUT and POST are normally interchangeable depending on the
        /// web service implementation.
        /// </summary>
        PUT,
        /// <summary>
        /// A delete request. Normally used to create data through the web service.
        /// </summary>
        DELETE,
        /// <summary>
        /// A head request.
        /// </summary>
        HEAD,
        /// <summary>
        /// An options request.
        /// </summary>
        OPTIONS
    }
}
