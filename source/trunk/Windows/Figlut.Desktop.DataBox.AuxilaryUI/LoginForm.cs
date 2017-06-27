namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.BaseUI;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public partial class LoginForm : FiglutBaseForm
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

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void LoginForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void LoginForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void mnuLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                txtUserName.Focus();
                throw new UserThrownException("User Name not entered.", LoggingLevel.Maximum, false);
            }
            _userName = txtUserName.Text;
            _password = txtPassword.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void LoginForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                mnuLogin.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}
