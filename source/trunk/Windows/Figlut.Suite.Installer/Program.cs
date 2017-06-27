namespace Figlut.Suite.Installer
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
            Application.Run(new MainForm());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            UIHelper.DisplayException(e.Exception);
            Application.Exit();
        }

        #endregion //Methods
    }
}
