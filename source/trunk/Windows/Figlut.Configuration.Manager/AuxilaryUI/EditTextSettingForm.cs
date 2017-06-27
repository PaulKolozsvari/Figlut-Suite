namespace Figlut.Configuration.Manager.AuxilaryUI
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
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public partial class EditTextSettingForm : FiglutBaseForm
    {
        #region Constructors

        public EditTextSettingForm(SettingItem settingItem)
        {
            InitializeComponent();
            _settingItem = settingItem;
        }

        #endregion //Constructors

        #region Fields

        private SettingItem _settingItem;

        #endregion //Fields

        #region Event Handlers

        private void EditSettingForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void EditSettingForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void EditSettingForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void EditTextSettingForm_Load(object sender, EventArgs e)
        {
            txtSetting.Text = _settingItem.SettingDisplayName;
            if (_settingItem.SettingValue != null)
            {
                if (_settingItem.PasswordChar != '\0')
                {
                    txtValue.PasswordChar = _settingItem.PasswordChar;
                }
                txtValue.Text = _settingItem.SettingValue.ToString();
            }
            txtValue.Focus();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            _settingItem.SettingValue = string.IsNullOrEmpty(txtValue.Text) ? null : txtValue.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void EditTextSettingForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel_Click(sender, e);
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                mnuSave_Click(sender, e);
            }
        }

        #endregion //Event Handlers
    }
}
