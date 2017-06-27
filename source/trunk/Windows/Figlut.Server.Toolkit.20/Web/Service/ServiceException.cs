namespace Figlut.Server.Toolkit.Web.Service
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    public class ServiceException : Exception
    {
        #region Constructors

        public ServiceException(string message, ServiceResultCode code) : base(message)
        {
            _result = new ServiceResult()
            {
                Message = message,
                Code = code
            };
        }

        #endregion //Constructors

        #region Fields

        private ServiceResult _result;

        #endregion //Fields

        #region Properties

        public ServiceResult Result
        {
            get { return _result; }
        }

        #endregion //Properties
    }
}