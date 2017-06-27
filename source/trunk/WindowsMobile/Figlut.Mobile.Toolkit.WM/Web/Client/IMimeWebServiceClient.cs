namespace Figlut.Mobile.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Mobile.Toolkit.Web.Client;
    using System.Net;

    #endregion //Using Directives

    public interface IMimeWebServiceClient
    {
        #region Methods

        void ConnectionTest(
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException);

        T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string rawOutput,
            bool serializePostObject,
            bool deserializeToDotNetObject,
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException);

        object CallService(
            Type returnType,
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string rawOutput,
            bool serializePostObject,
            bool deserializeToDotNetObject,
            int timeout,
            out HttpStatusCode statusCode,
            out string statusDescription,
            bool wrapWebException);

        #endregion //Methods
    }
}
