namespace Figlut.ReModem
{
    #region Using Directives

    using Figlut.ReModem.Configuration;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public partial class FiglutReModemService : ServiceBase
    {
        #region Constructors

        public FiglutReModemService()
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
            FiglutReModemSettings settings = GOC.Instance.GetSettings<FiglutReModemSettings>(true, false);
            FiglutReModemApplication.Instance.Start(settings);
            GOC.Instance.Logger.LogMessage(new LogMessage("ReModem SERVICE STARTED.", LogMessageType.Information, LoggingLevel.Minimum));
        }

        internal static new void Stop()
        {
            FiglutReModemApplication.Instance.Stop();
            GOC.Instance.Logger.LogMessage(new LogMessage("ReModem SERVICE STOPPED.", LogMessageType.Information, LoggingLevel.Minimum));
        }

        #endregion //Methods
    }
}
