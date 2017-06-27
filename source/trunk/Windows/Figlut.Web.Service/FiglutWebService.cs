namespace Figlut.Web.Service
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using System.IO;
    using System.ServiceModel.Description;
    using Figlut.Server.Toolkit.Data.QueryRunners;
    using Figlut.Web.Service.Configuration;
    using Figlut.Web.Service.Utilities;

    #endregion //Using Directives

    public partial class FiglutWebService : ServiceBase
    {
        #region Constructors

        public FiglutWebService()
        {
            InitializeComponent();
        }

        #endregion //Constructors

        #region Methods

        protected override void OnStart(string[] args)
        {
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                Stop();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                throw;
            }
        }

        internal static void Start()
        {
            FiglutWebServiceSettings settings = GOC.Instance.GetSettings<FiglutWebServiceSettings>(true, true);
            FiglutWebServiceApplication.Instance.Initialize(settings);
        }

        internal static new void Stop()
        {
            SqlDatabase db = GOC.Instance.GetDatabase<SqlDatabase>();
            db.Dispose();
            string message = GOC.Instance.GetSettings<FiglutWebServiceSettings>().SaveOrmAssembly ?
                string.Format("Database {0} disposed. ORM assembly deleted from {1}", db.Name, db.GetOrmAssembly().AssemblyFilePath) :
                string.Format("Database {0} disposed.", db.Name);
            GOC.Instance.Logger.LogMessage(new LogMessage(
                message,
                LogMessageType.Information, 
                LoggingLevel.Maximum));
            GOC.Instance.GetByTypeName<ServiceHost>().Close();
            GOC.Instance.Logger.LogMessage(new LogMessage("Figlut Web Service stopped.", LogMessageType.Information, LoggingLevel.Minimum));
            if (GOC.Instance.GetSettings<FiglutWebServiceSettings>().DeleteClientDataWhenServiceStops)
            {
                FiglutClientManager.Instance.Stop();
            }
        }

        #endregion //Methods
    }
}