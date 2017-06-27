namespace Figlut.Mobile.DataBox.UI
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
    using Figlut.Mobile.DataBox.Utilities;
    using Figlut.Mobile.DataBox.Configuration;
    using System.Net;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using SystemCF.Reflection;

    #endregion //Using Directives

    public partial class SplashForm : BaseForm
    {
        #region Constructors

        public SplashForm()
        {
            InitializeComponent();
            TopMost = true;
            if (!string.IsNullOrEmpty(FiglutDataBoxApplication.Instance.ApplicationTitle))
            {
                this.Text = lblApplicationTitle.Text = FiglutDataBoxApplication.Instance.ApplicationTitle;
            }
            if (!string.IsNullOrEmpty(FiglutDataBoxApplication.Instance.ApplicationVersion))
            {
                lblVersion.Text = string.Format("Version {0}", FiglutDataBoxApplication.Instance.ApplicationVersion);
            }
            if (FiglutDataBoxApplication.Instance.ApplicationBannerImage != null)
            {
                picLogo.Image = FiglutDataBoxApplication.Instance.ApplicationBannerImage;
            }
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
                Cursor.Current = Cursors.WaitCursor;
                tmrMain.Enabled = true;

                statusMain.Text = "Please wait ...";
                Application.DoEvents();
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
                FiglutMobileDataBoxSettings settings = GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>(true, true);
                string userName = settings.AuthenticationUserName;
                string password = settings.AuthenticationPassword;
                while (true)
                {
                    try
                    {
                        if (settings.UseAuthentication && settings.PromptForCredentials)
                        {
                            using (LoginForm loginForm = new LoginForm())
                            {
                                if (loginForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                    this.Close();
                                    return;
                                }
                                userName = loginForm.UserName;
                                password = loginForm.Password;
                            }
                        }
                        else if (settings.UseAuthentication &&
                            !settings.PromptForCredentials &&
                            string.IsNullOrEmpty(settings.AuthenticationUserName))
                        {
                            throw new UserThrownException(
                                "Application is configured to use authentication and not prompt for credentials (i.e. use configured credentials), but no credentials were configured.",
                                LoggingLevel.Minimum,
                                true);
                        }
                        FiglutDataBoxApplication.Instance.Initialize(userName, password, settings, false, w);
                        break;
                    }
                    catch (UserThrownException uex)
                    {
                        throw uex;
                    }
                    catch (WebException wex)
                    {
                        HttpWebResponse response = wex.Response == null ? null : (HttpWebResponse)wex.Response;
                        /* Check if the issue is due to invalid credentials entered by the user when prompted.
                         * If so then prompt user to enter credentials again, otherwise throw the exception 
                         * and close the application*/
                        if (settings.PromptForCredentials &&
                            response != null &&
                            response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            UIHelper.DisplayError(string.Format("{0} {1} : {2}",
                                response.StatusCode.ToString(),
                                (int)response.StatusCode,
                                response.StatusDescription));
                        }
                        else if (response != null)
                        {
                            throw new UserThrownException(string.Format("{0} {1} : {2}",
                                response.StatusCode.ToString(),
                                (int)response.StatusCode,
                                response.StatusDescription),
                                LoggingLevel.Minimum,
                                true);
                        }
                        else
                        {
                            throw new UserThrownException(wex.Message, LoggingLevel.Minimum, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new UserThrownException(ex.Message, LoggingLevel.Minimum, true);
                    }
                }
            }
        }

        #endregion //Event Handlers
    }
}