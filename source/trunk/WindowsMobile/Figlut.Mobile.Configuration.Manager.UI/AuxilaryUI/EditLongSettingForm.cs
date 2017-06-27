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
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.SettingsFile;
    using Figlut.Mobile.Configuration.Manager.Utilities;

    #endregion //Using Directives

    public partial class EditLongSettingForm : BaseForm
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

        private void EditLongSettingForm_Load(object sender, EventArgs e)
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
                    txtValue.Value = Convert.ToInt64(_settingItem.SettingValue);
                }
                txtValue.Focus();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditLongSettingForm_KeyDown);
            }
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditLongSettingForm_KeyDown);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_settingItem.SettingType.Equals(typeof(Int64)))
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
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditLongSettingForm_KeyDown);
            }
        }

        private void EditLongSettingForm_KeyDown(object sender, KeyEventArgs e)
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
                ExceptionHandler.HandleException(ex, true, true, this, EditLongSettingForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}