namespace Figlut.ReModem
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.Linq;
    using System.Threading.Tasks;

    #endregion //Using Directives

    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        #region Constructors

        public ProjectInstaller()
        {
            InitializeComponent();
        }

        #endregion //Constructors

        #region Methods

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
            if (!System.Diagnostics.EventLog.SourceExists("Figlut.ReModem.Source"))
            {
                System.Diagnostics.EventLog.CreateEventSource("Figlut.ReModem.Source", "Figlut.ReModem.Log");
            }
        }

        protected override void OnAfterUninstall(IDictionary savedState)
        {
            base.OnAfterUninstall(savedState);
            if (System.Diagnostics.EventLog.SourceExists("Figlut.ReModem.Source"))
            {
                System.Diagnostics.EventLog.DeleteEventSource("Figlut.ReModem.Source");
            }
            if (System.Diagnostics.EventLog.Exists("Figlut.ReModem.Log"))
            {
                System.Diagnostics.EventLog.Delete("Figlut.ReModem.Log");
            }
        }

        #endregion //Methods
    }
}
