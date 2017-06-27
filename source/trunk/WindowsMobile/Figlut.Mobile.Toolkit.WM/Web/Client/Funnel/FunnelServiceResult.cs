namespace Figlut.Mobile.Toolkit.Web.Client.Funnel
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Data;

    #endregion //Using Directives

    public class FunnelServiceResult
    {
        #region Constructors

        public FunnelServiceResult()
        {
        }

        public FunnelServiceResult(FunnelServiceResultCode code, string message)
        {
            Code = code;
            Message = message;
        }

        #endregion //Constructors

        #region Properties

        public FunnelServiceResultCode Code { get; set; }

        public string Message { get; set; }

        #endregion //Properties
    }
}