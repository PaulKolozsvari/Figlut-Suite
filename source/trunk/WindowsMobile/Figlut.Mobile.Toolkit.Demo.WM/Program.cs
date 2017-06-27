namespace Figlut.Mobile.Toolkit.Demo.WM
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Forms;

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
            Application.Run(new MainForm());
        }

        #endregion //Methods
    }
}