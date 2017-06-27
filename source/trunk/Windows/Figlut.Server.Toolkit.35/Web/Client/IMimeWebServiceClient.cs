namespace Figlut.Server.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Server.Toolkit.Web.Client;

    #endregion //Using Directives

    public interface IMimeWebServiceClient
    {
        #region Methods

        void ConnectionTest(int timeout);

        T CallService<T>(
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string rawOutput,
            bool deserializeToDotNetToObject,
            int timeout);

        object CallService(
            Type returnType,
            string queryString,
            object requestPostObject,
            HttpVerb verb,
            out string rawOutput,
            bool deserializeToDotNetObject,
            int timeout);

        #endregion //Methods
    }
}
