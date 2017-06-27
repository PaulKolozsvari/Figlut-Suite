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

    public partial class EditTextSettingForm : BaseForm
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

        private void EditTextSettingForm_Load(object sender, EventArgs e)
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
                    if (_settingItem.PasswordChar != '\0')
                    {
                        txtValue.PasswordChar = _settingItem.PasswordChar;
                    }
                    txtValue.Text = _settingItem.SettingValue.ToString();
                }
                txtValue.Focus();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditTextSettingForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, EditTextSettingForm_KeyDown);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            try
            {
                _settingItem.SettingValue = string.IsNullOrEmpty(txtValue.Text) ? null : txtValue.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditTextSettingForm_KeyDown);
            }
        }

        private void EditTextSettingForm_KeyDown(object sender, KeyEventArgs e)
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
                ExceptionHandler.HandleException(ex, true, true, this, EditTextSettingForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}