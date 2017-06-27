namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using Microsoft.Synchronization;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    public partial class SyncForm : Form
    {
        #region Constructors

        public SyncForm(SyncFormConfig syncFormConfig)
        {
            InitializeComponent();
            _syncFormConfig = syncFormConfig;

            Logo = _syncFormConfig.Logo;
            picLogo.SizeMode = _syncFormConfig.LogoSizeMode;
            _syncFormConfig.MobileDatabase.SyncSessionStateChanged += new MobileDatabase.SyncSessionStateChangedHandler(MobileDatabase_SyncSessionStateChanged);
            _syncFormConfig.MobileDatabase.SyncSessionProgress += new MobileDatabase.SyncSessionProgressHandler(MobileDatabase_SyncSessionProgress);
        }

        #endregion //Constructors

        #region Fields

        protected SyncFormConfig _syncFormConfig;
        protected SyncResult _syncResult;

        #endregion //Fields

        #region Properties

        public Image Logo
        {
            get { return picLogo.Image; }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException(
                        string.Format("{0} for {1} may not be null.",
                        EntityReader<SyncForm>.GetPropertyName(p => p.Logo, false),
                        this.GetType().FullName));
                }
                picLogo.Image = value;
                Application.DoEvents();
            }
        }

        public SyncFormConfig SyncFormConfig
        {
            get { return _syncFormConfig; }
        }

        public SyncResult SyncResult
        {
            get { return _syncResult; }
        }

        #endregion //Properties

        #region Methods

        private void Synchronize(bool handleException)
        {
            try
            {
                using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
                {
                    statusMain.Text = "Synchronizing data ...";
                    Application.DoEvents();
                    txtStatus.Text = string.Empty;
                    _syncResult = _syncFormConfig.MobileDatabase.Synchronize(_syncFormConfig.SyncConfig);
                }
                if (_syncFormConfig.DisplaySyncStatisticsAfterSync)
                {
                    UIHelper.DisplayInformation(_syncResult.SyncStatisticsSummaryMessage);
                }
                if (_syncFormConfig.CloseAfterSync)
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                if (!handleException)
                {
                    throw ex;
                }
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void MobileDatabase_SyncSessionProgress(object sender, SyncSessionProgressEventArgs e)
        {
            try
            {
                if (this.IsDisposed)
                {
                    return;
                }
                string lineFeed = string.IsNullOrEmpty(txtStatus.Text) ? string.Empty : "\r\n";
                txtStatus.Text += string.Format("{0}{1}", lineFeed, e.EventMessage);
                statusMain.Text = string.Format("{0} % complete", e.SessionProgressEventArgs.PercentCompleted);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void MobileDatabase_SyncSessionStateChanged(object sender, SyncSessionStateChangedEventArgs e)
        {
            try
            {
                if (this.IsDisposed)
                {
                    return;
                }
                statusMain.Text = e.EventMessage;
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (_syncFormConfig.SyncOnFormLoad)
                {
                    this.BeginInvoke(new Action<bool>(Synchronize), true);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuSynchronize_Click(object sender, EventArgs e)
        {
            try
            {
                Synchronize(false);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Event Handlers
    }
}