namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System.Diagnostics;
    using System.Runtime.InteropServices;

    #endregion //Using Directives

    public class ConsoleApplication
    {
        #region Methods


        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        public static void Maximize()
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }

        #endregion //Methods
    }
}
