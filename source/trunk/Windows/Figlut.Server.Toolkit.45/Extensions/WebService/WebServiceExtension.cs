namespace Figlut.Server.Toolkit.Extensions.WebService
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Extensions.WebService.Handlers;

    #endregion //Using Directives

    public abstract class WebServiceExtension
    {
        #region Constructors

        public WebServiceExtension()
        {
            _webRequestToExtensionHandlerCache = new WebRequestToExtensionHandlerCache();
            AddWebRequestToExtensionHandlers();
        }

        #endregion //Constructors

        #region Fields

        protected WebRequestToExtensionHandlerCache _webRequestToExtensionHandlerCache;

        #endregion //Fields

        #region Methods

        public abstract void AddWebRequestToExtensionHandlers();

        public WebRequestToExtensionHandler GetHandler(string handler)
        {
            if (!_webRequestToExtensionHandlerCache.Exists(handler))
            {
                throw new NullReferenceException(string.Format(
                    "No {0} named {1} exists in {2}.",
                    typeof(WebRequestToExtensionHandler).FullName,
                    handler,
                    this.GetType().FullName));
            }
            return _webRequestToExtensionHandlerCache[handler];
        }

        public void AddHandler(WebRequestToExtensionHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(string.Format(
                    "The {0} to be added to {1} may not null.",
                    typeof(WebRequestToExtensionHandler).FullName,
                    this.GetType().FullName));
            }
            _webRequestToExtensionHandlerCache.Add(handler.Name, handler);
        }

        #endregion //Methods
    }
}
