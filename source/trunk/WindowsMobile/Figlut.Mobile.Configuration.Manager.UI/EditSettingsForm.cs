namespace Figlut.Mobile.Configuration.Manager.UI
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
    using Figlut.Mobile.Toolkit.Data;
    using Figlut.Mobile.Configuration.Manager.Utilities;
using Figlut.Mobile.Configuration.Manager.UI.AuxilaryUI;

    #endregion //Using Directives

    public partial class EditSettingsForm : BaseForm
    {
        #region Constructors

        public EditSettingsForm(EntityCache<string, SettingItem> currentCategorySettings, List<string> hiddenProperties)
        {
            InitializeComponent();
            _currentCategorySettings = currentCategorySettings;
            _hiddenProperties = hiddenProperties;
            grdSettingValues.TableStyles.Clear();
            grdSettingValues.TableStyles.Add(UIHelper.GetDataGridTableStyle<SettingItem>(250, _hiddenProperties, true));
        }

        #endregion //Constructors

        #region Fields

        private EntityCache<string, SettingItem> _currentCategorySettings;
        private Settings _currentSettings;
        private List<string> _hiddenProperties;
        private DataTable _currentTable;
        private int _settingNameColumnIndex;

        #endregion //Fields

        #region Methods

        private void RefresGrid(bool handleExceptions)
        {
            try
            {
                int selectedRowIndex = grdSettingValues.CurrentRowIndex;
                using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
                {
                    w.ChangeStatus("Refreshing settings ...");
                    _currentSettings = _currentCategorySettings.Count > 0 ? _currentCategorySettings.Entities[0].SettingsCategoryInfo.Settings : null;
                    if (_currentSettings == null)
                    {
                        throw new InvalidCastException(string.Format(
                            "Expected a {0} in the current category settings, but none was found.", 
                            typeof(Settings).FullName));
                    }
                    _currentTable = _currentCategorySettings.GetDataTable(null, false, true);
                    foreach (SettingItem s in _currentCategorySettings)
                    {
                        if (s.PasswordChar != '\0')
                        {
                            foreach (DataRow row in _currentTable.Rows)
                            {
                                if (row[EntityReader<SettingItem>.GetPropertyName(p => p.SettingName, true)].ToString() == 
                                    s.SettingName)
                                {
                                    row[EntityReader<SettingItem>.GetPropertyName(p => p.SettingValue, true)] = DataShaper.MaskPasswordString(s.SettingValue.ToString(), s.PasswordChar);
                                }
                            }
                        }
                    }
                    _settingNameColumnIndex = DataHelper.GetColumnIndex(_currentTable, EntityReader<SettingItem>.GetPropertyName(p => p.SettingName, true));
                    grdSettingValues.DataSource = _currentTable;
                    grdSettingValues.Refresh();
                    if (selectedRowIndex < _currentTable.Rows.Count && selectedRowIndex > -1)
                    {
                        grdSettingValues.CurrentRowIndex = selectedRowIndex;
                    }
                }
                statusMain.Text = _currentCategorySettings.Name;
            }
            catch (Exception ex)
            {
                grdSettingValues.DataSource = null;
                grdSettingValues.Refresh();
                if (!handleExceptions)
                {
                    throw ex;
                }
                ExceptionHandler.HandleException(ex, true, true, this, EditSettingsForm_KeyDown);
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void EditSettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!GOC.Instance.GetSettings<FiglutConfigurationManagerSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    grdSettingValues.Height += 50;
                }
                this.BeginInvoke(new Action<bool>(RefresGrid), true);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditSettingsForm_KeyDown);
            }
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditSettingsForm_KeyDown);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditSettingsForm_KeyDown);
            }
        }

        private void grdSettingValues_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridCell selectedCell = grdSettingValues.CurrentCell;
                object selectedCellValue = UIHelper.GetSelectedGridRowCellValue<object>(grdSettingValues, selectedCell.ColumnNumber);
                string currentColumnName = _currentTable.Columns[selectedCell.ColumnNumber].ColumnName;
                statusMain.Text = _hiddenProperties.Contains(currentColumnName) ? string.Empty : selectedCellValue.ToString();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditSettingsForm_KeyDown);
            }
        }

        private void mnuEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedSettingName = UIHelper.GetSelectedGridRowCellValue<string>(grdSettingValues, _settingNameColumnIndex);
                SettingItem selectedSetting = _currentCategorySettings[selectedSettingName];
                if (selectedSetting == null)
                {
                    throw new NullReferenceException(string.Format(
                        "Could not find setting with Setting Name {0}.",
                        selectedSettingName));
                }
                            Form editSettingForm = null;
                if (selectedSetting.SettingType.Equals(typeof(string)))
                {
                    editSettingForm = new EditTextSettingForm(selectedSetting);
                }
                if (selectedSetting.SettingType.IsEnum)
                {
                    editSettingForm = new EditEnumSettingForm(selectedSetting);
                }
                if(selectedSetting.SettingType.Equals(typeof(Boolean)))
                {
                    editSettingForm = new EditBoolSettingForm(selectedSetting);
                }
                if(selectedSetting.SettingType.Equals(typeof(Int64)) ||
                    selectedSetting.SettingType.Equals(typeof(Int32)) ||
                    selectedSetting.SettingType.Equals(typeof(Int16)))
                {
                    editSettingForm = new EditLongSettingForm(selectedSetting);
                }
                using (editSettingForm)
                {
                    if (editSettingForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }
                using(WaitProcess w = new WaitProcess(mnuMain, statusMain))
                {
                    w.ChangeStatus("Saving setting ...");
                    EntityReader.SetPropertyValue(
                        selectedSetting.SettingName, 
                        selectedSetting.SettingsCategoryInfo.Settings, 
                        selectedSetting.SettingValue);
                    selectedSetting.SettingsCategoryInfo.Settings.SaveToFile();
                }
                RefresGrid(false);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditSettingsForm_KeyDown);
            }
        }

        private void EditSettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    mnuEdit_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, EditSettingsForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}