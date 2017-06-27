namespace Figlut.Mobile.DataBox
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Figlut.Mobile.DataBox.UI;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.DataBox.Utilities;
    using System.IO;
    using System.Reflection;

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
                GOC.Instance.ExecutableName = Path.GetFileName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                GOC.Instance.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                FiglutMobileDataBoxSettings settings = GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>(true, true);
                FiglutDataBoxApplication.Instance.InitializeCustomizationParameters(settings);
                Application.Run(new MainMenuForm(settings));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Methods
    }
}