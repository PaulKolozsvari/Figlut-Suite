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

    public partial class EditBoolSettingForm : BaseForm
    {
        #region Constructors

        public EditBoolSettingForm(SettingItem settingItem)
        {
            InitializeComponent();
            _settingItem = settingItem;
        }

        #endregion //Constructors

        #region Fields

        private SettingItem _settingItem;

        #endregion //Fields

        #region Event Handlers

        private void EditBoolSettingForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!GOC.Instance.GetSettings<FiglutConfigurationManagerSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                }
                txtSetting.Text = _settingItem.SettingDisplayName;
                if (_settingItem.SettingValue != null)
                {
                    chkValue.Checked = Convert.ToBoolean(_settingItem.SettingValue);
                }
                chkValue.Focus();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditBoolSettingForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, EditBoolSettingForm_KeyDown);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            try
            {
                _settingItem.SettingValue = chkValue.Checked;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditBoolSettingForm_KeyDown);
            }
        }

        private void EditBoolSettingForm_KeyDown(object sender, KeyEventArgs e)
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
                ExceptionHandler.HandleException(ex, true, true, this, EditBoolSettingForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}