namespace Figlut.Server.Toolkit.Extensions.WebService.Handlers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Extensions.WebService.Events;

    #endregion //Using Directives

    public abstract class WebRequestToExtensionHandler
    {
        #region Constructors

        public WebRequestToExtensionHandler(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be null or empty when constructing a {1}.",
                    EntityReader<WebRequestToExtensionHandler>.GetPropertyName(p => p.Name, false),
                    typeof(WebRequestToExtensionHandler).FullName));
            }
            _name = name;
        }

        #endregion //Constructors

        #region Fields

        protected string _name;

        #endregion //Fields

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        #endregion //Properties

        #region Methods

        public abstract WebRequestToExtensionOutput HandleWebRequest(WebRequestToExtensionInput input);

        #endregion //Methods
    }
}