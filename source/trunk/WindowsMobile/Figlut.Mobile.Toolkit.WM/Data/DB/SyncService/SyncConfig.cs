namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    #endregion //Using Directives

    public class SyncConfig
    {
        #region Constructors

        public SyncConfig(
            bool handleLocalDbConnection,
            bool showPercentageInProgressSummary,
            bool logSyncStatisticsSummary)
        {
            _handleLocalDbConnection = handleLocalDbConnection;
            _showPercentageInProgressSummary = showPercentageInProgressSummary;
            _logSyncStatisticsSummary = logSyncStatisticsSummary;
        }

        public SyncConfig(
            bool handleLocalDbConnection,
            bool showPercentageInProgressSummary,
            bool logSyncStatisticsSummary,
            StatusBar statusBar)
        {
            _handleLocalDbConnection = handleLocalDbConnection;
            _showPercentageInProgressSummary = showPercentageInProgressSummary;
            _logSyncStatisticsSummary = logSyncStatisticsSummary;
            _statusBar = statusBar;
        }

        #endregion //Constructors

        #region Fields

        protected bool _handleLocalDbConnection;
        protected bool _showPercentageInProgressSummary;
        protected bool _logSyncStatisticsSummary;
        protected StatusBar _statusBar;

        #endregion //Fields

        #region Properties

        public bool HandleLocalDbConnection
        {
            get { return _handleLocalDbConnection; }
            set { _handleLocalDbConnection = value; }
        }

        public bool ShowPercentageInProgressSummary
        {
            get { return _showPercentageInProgressSummary; }
            set { _showPercentageInProgressSummary = value; }
        }

        public bool LogSyncStatisticsSummary
        {
            get { return _logSyncStatisticsSummary; }
            set { _logSyncStatisticsSummary = value; }
        }

        public StatusBar StatusBar
        {
            get { return _statusBar; }
            set { _statusBar = value; }
        }

        #endregion //Properties

        #region Methods

        public void ChangeStatus(string status)
        {
            if (_statusBar == null)
            {
                return;
            }
            _statusBar.Text = status;
            Application.DoEvents();
        }

        #endregion //Methods
    }
}