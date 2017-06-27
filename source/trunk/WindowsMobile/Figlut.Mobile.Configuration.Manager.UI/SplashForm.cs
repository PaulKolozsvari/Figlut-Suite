namespace Figlut.Mobile.Configuration.Manager.UI
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
    using System.IO;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.DataBox.UI.Base;
    using Figlut.Mobile.DataBox.Configuration;
    using System.Net;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using Figlut.Mobile.Configuration.Manager.Utilities;

    #endregion //Using Directives

    public partial class SplashForm : BaseForm
    {
        #region Constructors

        public SplashForm()
        {
            InitializeComponent();
            TopMost = true;
            tmrMain.Enabled = true;
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            Cursor.Current = Cursors.Default;
            base.Dispose(disposing);
        }

        #endregion //Methods

        #region Event Handlers

        private void SplashForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Please wait ...";
                Cursor.Current = Cursors.WaitCursor;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (progressMain.Value < progressMain.Maximum)
                    {
                        progressMain.Value++;
                        return;
                    }
                    tmrMain.Enabled = false;
                    Initialize();
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    tmrMain.Enabled = false;
                    ExceptionHandler.HandleException(ex, true, true);
                    this.DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void Initialize()
        {
            using (WaitProcess w = new WaitProcess(statusMain))
            {
                w.ChangeStatus("Initializing settings ...");
                FiglutConfigurationManagerSettings settings = GOC.Instance.GetSettings<FiglutConfigurationManagerSettings>(true, true);
                try
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                catch (UserThrownException uex)
                {
                    throw uex;
                }
                catch (Exception ex)
                {
                    throw new UserThrownException(ex.Message, LoggingLevel.Minimum, true);
                }
            }
        }

        #endregion //Event Handlers
    }
}