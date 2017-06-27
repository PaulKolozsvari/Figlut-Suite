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

    public partial class EditTextSettingControl : UserControl, IEditSetting
    {
        public EditTextSettingControl(EditSettingPage parentPropertyPage)
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
        /// Populate control values from the SelectionObject (set in UserListView.SelectionOnChanged). The node is the node being updated.
        /// This method sets the default values on the controls.
        /// </summary>
        public void RefreshData(object selectionObject)
        {
            _selectedSettingItem = (SettingItem)selectionObject;
            if (!_selectedSettingItem.SettingType.Equals(typeof(string)))
            {
                throw new ArgumentException(string.Format(
                    "Excepeted setting of type {0} and received instead a setting of type {1} on {2}.",
                    typeof(string).FullName,
                    _selectedSettingItem.SettingType.FullName,
                    this.GetType().FullName));
            }
            txtSetting.Text = _selectedSettingItem.SettingDisplayName;
            if (_selectedSettingItem.PasswordChar != '\0')
            {
                txtValue.PasswordChar = _selectedSettingItem.PasswordChar;
            }
            txtValue.Text = _selectedSettingItem.SettingValue.ToString();
            txtValue.Focus();
            _parentPropertyPage.Dirty = false;
        }

        /// <summary>
        /// Check during UserProptertyPage.OnApply to ensure that changes can be Applied i.e. verify values entered from the user.
        /// </summary>
        /// <returns>returns true if changes are valid</returns>
        public bool CanApplyChanges()
        {
            return !string.IsNullOrEmpty(txtValue.Text);
        }

        /// <summary>
        /// Update the node with the controls values.
        /// </summary>
        /// <param name="userNode">Node being updated by property page</param>
        public void UpdateData(object selectionObject)
        {
            SettingItem selectedSetting = (SettingItem)selectionObject;
            EntityReader.SetPropertyValue(selectedSetting.SettingName, selectedSetting.SettingsCategoryInfo.Settings, txtValue.Text);
            selectedSetting.RefreshSettingsByCategory(txtValue.Text);
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
        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            _parentPropertyPage.Dirty = true;
        }

        #endregion //Event Handlers
    }
}
