namespace Figlut.Desktop.Barcode
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.Barcode.Configuration;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;

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
                FiglutDesktopBarcodeSettings defaultSettings = GetDefaultSettings();
                if (!File.Exists(defaultSettings.FilePath))
                {
                    defaultSettings.SaveToFile();
                }
                FiglutDesktopBarcodeSettings settings = GOC.Instance.GetSettings<FiglutDesktopBarcodeSettings>(true, true);
                _mainForm = new MainForm(settings);
                Application.Run(_mainForm);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (!ExceptionHandler.HandleException(e.Exception))
            {
                _mainForm.ForceClose = true;
                Application.Exit();
            }
        }

        private static FiglutDesktopBarcodeSettings GetDefaultSettings()
        {
            return new FiglutDesktopBarcodeSettings()
            {
                LogToFile = true,
                LogToWindowsEventLog = false,
                LogToConsole = false,
                LogFileName = "Figlut.Barcode.Log.txt",
                EventSourceName = "Figlut.Barcode.Source",
                EventLogName = "Figlut.Barcode.Log",
                LoggingLevel = LoggingLevel.Normal
            };
        }

        #endregion //Methods
    }
}
