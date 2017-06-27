namespace Figlut.Server.Toolkit.Web.Client
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    public interface IWebService
    {
        #region Properties

        string Name { get; set; }

        string WebServiceBaseUrl { get; set; }

        #endregion //Properties
    }
}