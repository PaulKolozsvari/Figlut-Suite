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
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public partial class EditEnumSettingControl : UserControl, IEditSetting
    {
        #region Constructors

        public EditEnumSettingControl(EditSettingPage parentPropertyPage)
        {
            InitializeComponent();
            _parentPropertyPage = parentPropertyPage;
        }

        #endregion //Constructors

        #region Fields

        private EditSettingPage _parentPropertyPage;
        private SettingItem _selectedSettingItem;

        #endregion //Fields

        #region Methods

        public void RefreshData(object selectionObject)
        {
            _selectedSettingItem = (SettingItem)selectionObject;
            if (!_selectedSettingItem.SettingType.IsEnum)
            {
                throw new ArgumentException(string.Format(
                    "Excepeted setting of type {0} and received instead a setting of type {1} on {2}.",
                    typeof(Enum).FullName,
                    _selectedSettingItem.SettingType.FullName,
                    this.GetType().FullName));
            }
            txtSetting.Text = _selectedSettingItem.SettingDisplayName;
            Array enumValues = EnumHelper.GetEnumValues(_selectedSettingItem.SettingType);
            cboValue.Items.Clear();
            Enum selectedItem = null;
            foreach (Enum en in enumValues)
            {
                if (en.ToString() == _selectedSettingItem.SettingValue.ToString())
                {
                    selectedItem = en;
                }
                cboValue.Items.Add(en);
            }
            cboValue.SelectedItem = selectedItem;
            cboValue.Focus();
            _parentPropertyPage.Dirty = false;
        }

        public bool CanApplyChanges()
        {
            return cboValue.SelectedIndex > -1;
        }

        public void UpdateData(object selectionObject)
        {
            SettingItem selectedSetting = (SettingItem)selectionObject;
            EntityReader.SetPropertyValue(selectedSetting.SettingName, selectedSetting.SettingsCategoryInfo.Settings, cboValue.SelectedItem);
            selectedSetting.RefreshSettingsByCategory(cboValue.SelectedItem.ToString());
            selectedSetting.SettingsCategoryInfo.Settings.SaveToFile();
            _parentPropertyPage.Dirty = false;
        }

        #endregion //Methods

        #region Event Handlers

        private void cboValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            _parentPropertyPage.Dirty = true;
        }

        #endregion //Event Handlers
    }
}
