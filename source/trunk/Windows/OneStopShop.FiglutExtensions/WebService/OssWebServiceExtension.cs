namespace OneStopShop.FiglutExtensions.WebService
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Extensions.WebService;
    using OneStopShop.FiglutExtensions.Biometric;

    #endregion //Using Directives

    public class OssWebServiceExtension : WebServiceExtension
    {
        #region Constructors

        public OssWebServiceExtension()
        {
        }

        #endregion //Constructors

        #region Fields

        private UserHandler _userHandler;

        #endregion //Fields

        #region Methods

        public override void AddWebRequestToExtensionHandlers()
        {
            OssWebServiceApplication.Instance.InitializeFingerMatcher();
            _userHandler = new UserHandler();
            this.AddHandler(_userHandler);
        }

        #endregion //Methods
    }
}
