namespace Figlut.Server.Toolkit.Web.Service
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class ServiceResult
    {
        public ServiceResultCode Code { get; set; }

        public string Message { get; set; }
    }
}