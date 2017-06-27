namespace Figlut.Mobile.Configuration.Manager.UI.AuxilaryUI
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
    using Figlut.Mobile.Toolkit.Utilities.SettingsFile;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Configuration.Manager.Utilities;

    #endregion //Using Directives

    public partial class EditEnumSettingForm : BaseForm
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

        private void EditEnumSettingForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!GOC.Instance.GetSettings<FiglutConfigurationManagerSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                }
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
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditEnumSettingForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, EditEnumSettingForm_KeyDown);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboValue.SelectedIndex > -1)
                {
                    _settingItem.SettingValue = cboValue.SelectedItem;
                }
                else
                {
                    cboValue.SelectedValue = null;
                }
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditEnumSettingForm_KeyDown);
            }
        }

        private void EditEnumSettingForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    mnuSave_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditEnumSettingForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}