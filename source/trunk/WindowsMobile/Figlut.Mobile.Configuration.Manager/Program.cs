namespace Figlut.Mobile.Configuration.Manager
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Figlut.Mobile.Configuration.Manager.UI;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.DataBox.Utilities;
    using System.Drawing;

    #endregion //Using Directives

    static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            try
            {
                GOC.Instance.ShowMessageBoxOnException = true;
                FiglutDataBoxApplication.Instance.ThemeColor = Color.SteelBlue; //Just to set the theme color in the FiglutDataBoxApplication to be used by the base form for the configuration manager.
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Methods
    }
}