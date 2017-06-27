namespace Figlut.Server
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.Linq;

    #endregion //Using Directives

    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        #region Methods

        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
            if (!System.Diagnostics.EventLog.SourceExists("Figlut.Web.Service.Source"))
            {
                System.Diagnostics.EventLog.CreateEventSource("Figlut.Web.Service.Source", "Figlut.Web.Service.Log");
            }
        }

        protected override void OnAfterUninstall(IDictionary savedState)
        {
            base.OnAfterUninstall(savedState);
            if (System.Diagnostics.EventLog.SourceExists("Figlut.Web.Service.Source"))
            {
                System.Diagnostics.EventLog.DeleteEventSource("Figlut.Web.Service.Source");
            }
            if (System.Diagnostics.EventLog.Exists("Figlut.Web.Service.Log"))
            {
                System.Diagnostics.EventLog.Delete("Figlut.Web.Service.Log");
            }
        }

        #endregion //Methods
    }
}
