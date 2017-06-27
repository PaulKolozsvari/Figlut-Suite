namespace Figlut.Web.Service.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Extensions.WebService;

    #endregion //Using Directives

    public class DefaultWebServiceCrudInterceptor : WebServiceCrudInterceptor
    {
        #region Methods

        public override void AddExtensionManagedEntities()
        {
        }

        public override void SubscribeToCrudEvents()
        {
        }

        #endregion //Methods
    }
}
