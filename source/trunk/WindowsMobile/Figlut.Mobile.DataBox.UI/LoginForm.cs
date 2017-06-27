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
    using Figlut.Mobile.DataBox.UI.Base;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using Figlut.Mobile.DataBox.Configuration;

    #endregion //Using Directives

    public partial class LoginForm : BaseForm
    {
        #region Constructors

        public LoginForm()
        {
            InitializeComponent();
        }

        #endregion //Constructors

        #region Fields

        private string _userName;
        private string _password;

        #endregion //Fields

        #region Properties

        public string UserName
        {
            get { return _userName; }
        }

        public string Password
        {
            get { return _password; }
        }

        #endregion //Properties

        #region Event Handlers

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Enter user name and password to login.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, LoginForm_KeyDown);
            }
        }

        private void mnuLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    txtUserName.Focus();
                    UIHelper.DisplayError("User Name not entered.", this, LoginForm_KeyDown);
                    return;
                }
                _userName = txtUserName.Text;
                _password = txtPassword.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, LoginForm_KeyDown);
            }
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, LoginForm_KeyDown);
            }
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
                else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
                {
                    mnuLogin_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, LoginForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}