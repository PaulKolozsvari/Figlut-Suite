namespace Figlut.Configuration.Manager
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
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
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _mainForm = new MainForm();
            Application.Run(_mainForm);
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
