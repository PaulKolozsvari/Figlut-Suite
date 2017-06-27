namespace Figlut.Server.Toolkit.Extensions.WebService.Handlers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class WebRequestToExtensionOutput
    {
        #region Constructors

        public WebRequestToExtensionOutput(
            HttpStatusCode outputStatusCode,
            string outputStatusDescription,
            string outputString)
        {
            _outputStatusCode = outputStatusCode;
            _outputStatusDescription = outputStatusDescription;
            _outputString = outputString;
        }

        #endregion //Constructors

        #region Fields

        protected HttpStatusCode _outputStatusCode;
        protected string _outputStatusDescription;
        protected string _outputString;

        #endregion //Fields

        #region Properties

        public HttpStatusCode OutputStatusCode
        {
            get { return _outputStatusCode; }
        }

        public string OutputStatusDescription
        {
            get { return _outputStatusDescription; }
        }

        public string OutputString
        {
            get { return _outputString; }
        }

        #endregion //Properties
    }
}
