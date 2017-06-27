namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class WindowsHelper
    {
        #region Methods

        public static string GetCurrentWindowsUserName(bool includeDomainName)
        {
            //return includeDomainName ?
            //    (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name :
            //    System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return includeDomainName ?
                System.Security.Principal.WindowsIdentity.GetCurrent().Name :
                System.Environment.UserName;
        }

        #endregion //Methods
    }
}
