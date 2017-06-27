namespace Figlut.Server.Toolkit.Mmc.SettingEditors
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using Microsoft.ManagementConsole;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public partial class EditBoolSettingControl : UserControl, IEditSetting
    {
        public EditBoolSettingControl(EditSettingPage parentPropertyPage)
        {
            InitializeComponent();
            _parentPropertyPage = parentPropertyPage;
        }

        #region Fields

        private EditSettingPage _parentPropertyPage;
        private SettingItem _selectedSettingItem;

        #endregion //Fields

        #region Methods

        /// <summary>
        /// Populate control values from the SelectionObject (set in UserListView.SelectionOnChanged) The node is the node being updated.
        /// This method sets the default values on the controls.
        /// </summary>
        public void RefreshData(object selectionObject)
        {
            _selectedSettingItem = (SettingItem)selectionObject;
            if (!_selectedSettingItem.SettingType.Equals(typeof(Boolean)))
            {
                throw new ArgumentException(string.Format(
                    "Excepeted setting of type {0} and received instead a setting of type {1} on {2}.",
                    typeof(Boolean).FullName,
                    _selectedSettingItem.SettingType.FullName,
                    this.GetType().FullName));
            }
            bool settingValueBool = Convert.ToBoolean(_selectedSettingItem.SettingValue);
            txtSetting.Text = _selectedSettingItem.SettingDisplayName;
            chkValue.Checked = settingValueBool;
            chkValue.Focus();
            _parentPropertyPage.Dirty = false;
        }

        /// <summary>
        /// Check during UserProptertyPage.OnApply to ensure that changes can be Applied i.e. verify values entered from the user.
        /// </summary>
        /// <returns>returns true if changes are valid</returns>
        public bool CanApplyChanges()
        {
            return true;
        }

        /// <summary>
        /// Update the node with the controls values.
        /// </summary>
        /// <param name="userNode">Node being updated by property page</param>
        public void UpdateData(object selectionObject)
        {
            SettingItem selectedSetting = (SettingItem)selectionObject;

            //Save the settings file.
            EntityReader.SetPropertyValue(selectedSetting.SettingName, selectedSetting.SettingsCategoryInfo.Settings, chkValue.Checked);
            selectedSetting.RefreshSettingsByCategory(chkValue.Checked.ToString());
            selectedSetting.SettingsCategoryInfo.Settings.SaveToFile();
            _parentPropertyPage.Dirty = false;
        }

        #endregion //Methods

        #region Event Handlers

        /// <summary>
        /// Notifies/Flags the PropertyPage that info has changed and that the PropertySheet can change the 
        /// buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkValue_CheckedChanged(object sender, EventArgs e)
        {
            _parentPropertyPage.Dirty = true;
        }

        #endregion //Event Handlers
    }
}
