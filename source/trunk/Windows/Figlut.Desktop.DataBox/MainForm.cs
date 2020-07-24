namespace Figlut.Desktop.DataBox
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Figlut.Server.Toolkit.Winforms;
    using Figlut.Desktop.DataBox.Controls;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Desktop.DataBox.Utilities;
    using Figlut.Desktop.BaseUI;
    using Figlut.Desktop.DataBox.AuxilaryUI;
    using Figlut.Desktop.DataBox.Configuration;
    using System.Net;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Extensions.DataBox.Managers;

    #endregion //Using Directives

    public partial class MainForm : FiglutBaseForm
    {
        #region Constructors

        public MainForm(FiglutDesktopDataBoxSettings settings)
        {
            InitializeComponent();
            SetUserAgent();
            _settings = settings;
            if (!string.IsNullOrEmpty(FiglutDataBoxApplication.Instance.ApplicationTitle))
            {
                this.FormTitle = FiglutDataBoxApplication.Instance.ApplicationTitle;
            }
            picLogo.Image = FiglutDataBoxApplication.Instance.ApplicationBannerImage;
            using (SplashForm f = new SplashForm())
            {
                if (f.ShowDialog() == DialogResult.Cancel)
                {
                    Close();
                }
            }
        }

        #endregion //Constructors

        #region Constants

        private Guid APPLICATION_ID = new Guid("438E0151-F39A-4A4A-BD15-4FD2D842E058");

        #endregion //Constants

        #region Fields

        private FiglutDesktopDataBoxSettings _settings;
        private bool _forceClose;

        #endregion //Fields

        #region Properties

        public bool ForceClose
        {
            get { return _forceClose; }
            set { _forceClose = true; }
        }

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        #endregion //Properties

        #region Methods

        private void SetUserAgent()
        {
            GOC.Instance.UserAgent = new FiglutClientInfo(
                APPLICATION_ID,
                GOC.Instance.ExecutableName,
                GOC.Instance.Version,
                Environment.OSVersion.ToString(),
                Information.GetWindowsDomainAndMachineName()).GetHash();
        }

        private void SetupPreferenceSettings()
        {
            if (_settings.StartFullScreen)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.BorderlessForm_Minimize(sender, e);
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            base.BorderlessForm_Maximize(sender, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            using (WaitProcess w = new WaitProcess(this))
            {
                w.ChangeStatus("Initializing preference settings ...");
                SetupPreferenceSettings();
                this.Refresh();
                dataBoxControl.OnUpdateModeChanged += new DataBoxControl.OnControlStateChanged(dataBoxControl_UpdateModeChanged);
                dataBoxControl.OnFiltersEnabledChanged += new DataBoxControl.OnControlStateChanged(dataBoxControl_FiltersEnabledChanged); ;

                string userName = _settings.AuthenticationUserName;
                string password = _settings.AuthenticationPassword;
                while (true)
                {
                    try
                    {
                        if (_settings.UseAuthentication && _settings.PromptForCredentials)
                        {
                            using (LoginForm loginForm = new LoginForm())
                            {
                                if (loginForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                    _forceClose = true;
                                    Application.Exit();
                                    break;
                                }
                                userName = loginForm.UserName;
                                password = loginForm.Password;
                            }
                        }
                        else if (_settings.UseAuthentication &&
                            !_settings.PromptForCredentials &&
                            string.IsNullOrEmpty(_settings.AuthenticationUserName))
                        {
                            throw new UserThrownException(
                                "Application is configured to use authentication and not prompt for credentials (i.e. use configured credentials), but no credentials were configured.",
                                LoggingLevel.Minimum,
                                true);
                        }
                        FiglutDataBoxApplication.Instance.Initialize(userName, password, _settings, false, new DataBoxManager(dataBoxControl), w);
                        break;
                    }
                    catch (UserThrownException uex)
                    {
                        throw uex;
                    }
                    catch (WebException wex)
                    {
                        HttpWebResponse response = (HttpWebResponse)wex.Response;
                        /* Check if the issue is due to invalid credentials entered by the user when prompted.
                         * If so then prompt user to enter credentials again, otherwise throw the exception 
                         * and close the application*/
                        if (_settings.PromptForCredentials &&
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

        private void mnuCurrentSwitchUser_Click(object sender, EventArgs e)
        {
            //if ((timesheetControl.UnsavedChanges) &&
            //    (UIHelper.AskQuestion("There are unsaved changes. Are you sure you want to switch user? All unsaved changes will be lost.") !=
            //    DialogResult.Yes))
            //{
            //    return;
            //}
            //using (LoginForm f = new LoginForm())
            //{
            //    if (f.ShowDialog() != DialogResult.OK)
            //    {
            //        return;
            //    }
            //}
            //if (GlobalDataCache.Instance.CurrentSettingProfile.StartFullScreen)
            //{
            //    base.BorderlessForm_Maximize(this, e);
            //}
            //ConfigureApplicationToCurrentSettingProfile();
            //mnuFileTimesheetNew_Click(this, e);
        }

        private void ConfigureApplicationToCurrentSettingProfile()
        {
            //DesktopRoleId roleId = (DesktopRoleId)GlobalDataCache.Instance.CurrentUser.RoleId;
            //mnuManage.Enabled = (roleId & DesktopRoleId.Administrator) == DesktopRoleId.Administrator;
            //bool showTimesheetPage = (roleId & DesktopRoleId.Desktop) == DesktopRoleId.Desktop;
            //bool showBillablesPage = (roleId & DesktopRoleId.Finance) == DesktopRoleId.Finance;
            //bool showServiceQuotePage = (roleId & DesktopRoleId.Desktop) == DesktopRoleId.Desktop;
            //ShowKaptureTabs(showTimesheetPage, showBillablesPage, showServiceQuotePage);
            //string logFilePath = Path.Combine(Information.GetExecutingDirectory(), GlobalDataCache.Instance.LocalSettings.LogFileName);
            //if (!File.Exists(logFilePath))
            //{
            //    LogEntry entry = new LogEntry()
            //    {
            //        Id = Guid.NewGuid(),
            //        Type = LogEntry.EntryType.Info,
            //        CreationDate = DateTime.Now,
            //        Message = "Log file created.",
            //        ExceptionName = null
            //    };
            //    Logger.Log(entry, logFilePath);
            //}
            //timesheetControl.ConfigureToCurrentSettingProfile();
        }

        private void ShowKaptureTabs(bool showTimesheetPage, bool showBillablesPage, bool showServiceQuotePage)
        {
            tabDataBox.TabPages.Remove(tabPageDataBox);
            if (showTimesheetPage)
            {
                tabDataBox.TabPages.Add(tabPageDataBox);
            }
        }

        public void SelectTimesheetTabPage()
        {
            if (tabDataBox.TabPages.Contains(tabPageDataBox))
            {
                tabDataBox.SelectTab(tabPageDataBox.Name);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataBoxControl_UpdateModeChanged(object sender, DataBoxControl.OnDataBoxControlStateChangedArgs e)
        {
            //if (e.Enabled)
            //{
            //    EnableTimesheetEditControlsForUpdateMode();
            //}
            //else
            //{
            //    EnableTimesheetEditControlsForNonUpdateMode();
            //}
        }

        private void dataBoxControl_FiltersEnabledChanged(object sender, DataBoxControl.OnDataBoxControlStateChangedArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void picResizeWindow_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderLessFormResize(this, e);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_forceClose && UIHelper.AskQuestion("Are you sure you want to exit?") != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                AnimateHideForm();
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if ((e.KeyCode == Keys.F) & e.Control & e.Shift)
            {
                base.BorderlessForm_Maximize(sender, e);
            }
            else if ((e.KeyCode == Keys.U) & e.Control & e.Shift)
            {
                dataBoxControl.HandleUpdateRequest();
            }
            else if ((e.KeyCode == Keys.B) & e.Control & e.Shift)
            {
                dataBoxControl.HandleCancelUpdateRequest();
            }
            else if ((e.KeyCode == Keys.A) & e.Control & e.Shift)
            {
                dataBoxControl.HandleAddRequest();
            }
            else if ((e.KeyCode == Keys.D) & e.Control & e.Shift)
            {
                dataBoxControl.HandleDeleteRequest();
            }
            else if ((e.KeyCode == Keys.C) & e.Control & e.Shift)
            {
                dataBoxControl.ManageChildren();
            }
            else if ((e.KeyCode == Keys.P) & e.Control & e.Shift)
            {
                dataBoxControl.ViewParent();
            }
            else if ((e.KeyCode == Keys.R) & e.Control & e.Shift)
            {
                dataBoxControl.SelectParent();
            }
            else if ((e.KeyCode == Keys.L) & e.Control & e.Shift)
            {
                dataBoxControl.RefreshDataBox(true, true, true);
            }
            else if ((e.KeyCode == Keys.S) & e.Control & e.Shift)
            {
                dataBoxControl.Save();
            }
        }

        #region Current DataBox

        private void mnuCurrentTimesheetLoad_Click(object sender, EventArgs e)
        {
            dataBoxControl.RefreshDataBox(true, true, true);
        }

        #endregion //Current DataBox

        private void mnuCurrentDataBoxRefresh_Click(object sender, EventArgs e)
        {
            dataBoxControl.RefreshDataBox(true, false, false);
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            using (AboutForm f = new AboutForm())
            {
                f.ShowDialog();
            }
        }

        #endregion //Event Handlers
    }
}