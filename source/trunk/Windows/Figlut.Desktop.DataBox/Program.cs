namespace Figlut.Desktop.DataBox
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.DataBox.Configuration;
    using Figlut.Desktop.DataBox.Utilities;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    static class Program
    {
        #region Fields

        private static MainForm _mainForm;

        #endregion //Fields

        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.ThreadException += Application_ThreadException;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                GOC.Instance.ShowMessageBoxOnException = true;
                GOC.Instance.ExecutableName = Path.GetFileName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                GOC.Instance.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                FiglutDesktopDataBoxSettings settings = GOC.Instance.GetSettings<FiglutDesktopDataBoxSettings>(true, true);
                FiglutDataBoxApplication.Instance.InitializeCustomizationParameters(settings);

                _mainForm = new MainForm(settings);
                Application.Run(_mainForm);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (!ExceptionHandler.HandleException(e.Exception))
            {
                _mainForm.ForceClose = true;
                Application.Exit();
            }
        }

        #endregion //Methods
    }
}
