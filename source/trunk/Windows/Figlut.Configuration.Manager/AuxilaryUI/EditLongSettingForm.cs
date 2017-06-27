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

    public partial class EditLongSettingForm : FiglutBaseForm
    {
        #region Constructors

        public EditLongSettingForm(SettingItem settingItem)
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

        private void EditLongSettingForm_Load(object sender, EventArgs e)
        {
            txtSetting.Text = _settingItem.SettingDisplayName;
            if (_settingItem.SettingValue != null)
            {
                txtValue.Value = Convert.ToInt64(_settingItem.SettingValue);
            }
            txtValue.Focus();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void mnuUpdate_Click(object sender, EventArgs e)
        {
            if(_settingItem.SettingType.Equals(typeof(Int64)))
            {
                _settingItem.SettingValue = Convert.ToInt64(txtValue.Value);
            }
            if (_settingItem.SettingType.Equals(typeof(Int32)))
            {
                _settingItem.SettingValue = Convert.ToInt32(txtValue.Value);
            }
            if (_settingItem.SettingType.Equals(typeof(Int16)))
            {
                _settingItem.SettingValue = Convert.ToInt16(txtValue.Value);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void EditLongSettingForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel_Click(sender, e);
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                mnuUpdate_Click(sender, e);
            }
        }

        #endregion //Event Handlers
    }
}
