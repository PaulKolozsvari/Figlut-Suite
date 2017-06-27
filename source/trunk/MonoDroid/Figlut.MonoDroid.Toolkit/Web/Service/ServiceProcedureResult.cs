namespace Figlut.MonoDroid.Toolkit.Web.Service
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class ServiceProcedureResult : ServiceResult
    {
        #region Constructors

        public ServiceProcedureResult()
        {
            Code = ServiceResultCode.Success;
            Message = null;
        }

        public ServiceProcedureResult(ServiceResult result)
        {
            Code = result.Code;
            Message = result.Message;
        }

        #endregion //Constructors
    }
}