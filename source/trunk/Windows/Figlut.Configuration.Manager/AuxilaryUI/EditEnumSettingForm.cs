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
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public partial class EditEnumSettingForm : FiglutBaseForm
    {
        #region Constructors

        public EditEnumSettingForm(SettingItem settingItem)
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

        private void EditEnumSettingForm_Load(object sender, EventArgs e)
        {
            txtSetting.Text = _settingItem.SettingDisplayName;
            cboValue.Items.Clear();
            Array enumValues = EnumHelper.GetEnumValues(_settingItem.SettingType);
            Enum selectedItem = null;
            foreach (Enum en in enumValues)
            {
                if (en.ToString() == _settingItem.SettingValue.ToString())
                {
                    selectedItem = en;
                }
                cboValue.Items.Add(en);
            }
            if (selectedItem != null)
            {
                cboValue.SelectedItem = selectedItem;
            }
            else
            {
                cboValue.SelectedIndex = -1;
            }
            cboValue.Focus();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void mnuUpdate_Click(object sender, EventArgs e)
        {
            if (cboValue.SelectedIndex > -1)
            {
                _settingItem.SettingValue = cboValue.SelectedItem;
            }
            else
            {
                cboValue.SelectedValue = null;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void EditEnumSettingForm_KeyUp(object sender, KeyEventArgs e)
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
