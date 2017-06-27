namespace Figlut.Server.Toolkit.Web.Service
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class ServiceFunctionResult<E> : ServiceResult
    {
        #region Constructors

        public ServiceFunctionResult()
        {
            Code = ServiceResultCode.Success;
            Message = null;
        }

        public ServiceFunctionResult(ServiceResult result)
        {
            Code = result.Code;
            Message = result.Message;
        }

        #endregion //Constructors

        #region Properties

        public E Contents { get; set; }

        #endregion //Properties
    }
}