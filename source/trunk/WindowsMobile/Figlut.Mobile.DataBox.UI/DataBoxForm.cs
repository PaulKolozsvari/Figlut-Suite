namespace Figlut.Mobile.DataBox.UI
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
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;
    using Figlut.Mobile.Toolkit.Web.Client.FiglutWebService;
    using Figlut.Mobile.Toolkit.Data;
    using System.Collections;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.Toolkit.Extensions.DataBox.Managers;
    using Figlut.Mobile.DataBox.Utilities;
    using Figlut.Mobile.Toolkit.Extensions.ExtensionManaged;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using Figlut.Mobile.Toolkit.Extensions.DataBox.Events.MainMenu;
    using Figlut.Mobile.Toolkit.Extensions.DataBox.Events.Crud;
    using Figlut.Mobile.Toolkit.WM.Data.DB.SQLCE;
    using System.Reflection;
    using System.IO;
    using System.Diagnostics;
    using Figlut.Mobile.Toolkit.Data.DB;

    #endregion //Using Directives

    public partial class DataBoxForm : BaseForm, IDataBox
    {
        #region Inner Types

        public class OnDataBoxControlStateChangedArgs : EventArgs
        {
            public bool Enabled { get; set; }
        }

        public class OnDataBoxAfterRefreshArgs
        {
            public bool PresentLoadDataBoxForm { get; set; }

            public SqlCeDatabaseTable CurrentTable { get; set; }

            public bool FromServer { get; set; }
        }

        #endregion //Inner Types

        #region Constructors

        public DataBoxForm()
        {
            InitializeComponent();
            _settings = GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>();
            InitializeCustomizationParameters();
            InputControls = new Dictionary<string, Control>();
            ExtensionManagedProperties = new List<string>();
            _dataBoxManager = new DataBoxManager(this);
            FiglutDataBoxApplication.Instance.CrudInterceptor.SetDataBoxManager(_dataBoxManager);
            FiglutDataBoxApplication.Instance.MainMenuExtension.SetDataBoxManager(_dataBoxManager);
        }

        #endregion //Constructors

        #region Fields

        private DataBoxManager _dataBoxManager;
        private FiglutMobileDataBoxSettings _settings;

        private int _entityIdColumnIndex;
        private SqlCeDatabaseTable _currentTable;
        private FiglutEntityCacheUnique _currentEntityCache;
        private List<string> _hiddenProperties;
        private List<string> _extensionManagedProperties;
        private Dictionary<string, Control> _inputControls;
        private List<LinkLabel> _resetLinks;
        private Control _firstInputControl;
        private Control _inputControlAfterAdd;
        private object _entityUnderUpdate;
        private Nullable<Guid> _entityIdUnderUpdate;
        private string _currentDataBoxFilePath;
        private bool _filtersEnabled;
        private bool _unsavedChanges = false;
        private bool _readOnlyMode = false;
        private bool _inUpdateMode;

        public delegate void OnControlStateChanged(object sender, OnDataBoxControlStateChangedArgs e);
        public event OnControlStateChanged OnUpdateModeChanged;
        public event OnControlStateChanged OnFiltersEnabledChanged;

        public delegate void OnAfterRefresh(object sender, OnDataBoxAfterRefreshArgs e);
        public event OnAfterRefresh OnDataBoxRefresh;

        public event OnDataBoxPropertiesChanged OnDataBoxPropertiesChanged;

        #endregion //Fields

        #region Properties

        internal SqlCeDatabaseTable CurrentTable
        {
            get { return _currentTable; }
            set
            {
                _currentTable = value;
                PerformOnPropertiesChanged();
            }
        }

        internal FiglutEntityCacheUnique CurrentEntityCache
        {
            get { return _currentEntityCache; }
            set
            {
                _currentEntityCache = value;
                PerformOnPropertiesChanged();
            }
        }

        internal List<string> HiddenProperties
        {
            get { return _hiddenProperties; }
            set
            {
                _hiddenProperties = value;
                PerformOnPropertiesChanged();
            }
        }

        internal List<string> ExtensionManagedProperties
        {
            get { return _extensionManagedProperties; }
            set
            {
                _extensionManagedProperties = value;
                PerformOnPropertiesChanged();
            }
        }

        internal Dictionary<string, Control> InputControls
        {
            get { return _inputControls; }
            set
            {
                _inputControls = value;
                PerformOnPropertiesChanged();
            }
        }

        internal List<LinkLabel> ResetLinks
        {
            get { return _resetLinks; }
            set { _resetLinks = value; }
        }

        internal Control FirstInputControl
        {
            get { return _firstInputControl; }
            set
            {
                _firstInputControl = value;
                PerformOnPropertiesChanged();
            }
        }

        internal Control InputControlAfterAdd
        {
            get
            {
                return _inputControlAfterAdd;
            }
            set
            {
                _inputControlAfterAdd = value;
                PerformOnPropertiesChanged();
            }
        }

        internal object EntityUnderUpdate
        {
            get { return _entityUnderUpdate; }
            set
            {
                _entityUnderUpdate = value;
                PerformOnPropertiesChanged();
            }
        }

        internal Nullable<Guid> EntityIdUnderUpdate
        {
            get { return _entityIdUnderUpdate; }
            set
            {
                _entityIdUnderUpdate = value;
                PerformOnPropertiesChanged();
            }
        }

        internal string CurrentDataBoxFilePath
        {
            get { return _currentDataBoxFilePath; }
            set
            {
                _currentDataBoxFilePath = value;
                PerformOnPropertiesChanged();
            }
        }

        internal bool FiltersEnabled
        {
            get { return _filtersEnabled; }
            set
            {
                _filtersEnabled = value;
                PerformOnPropertiesChanged();
            }
        }

        internal bool UnsavedChanges
        {
            get { return _unsavedChanges; }
            set
            {
                _unsavedChanges = value;
                PerformOnPropertiesChanged();
            }
        }

        internal bool ReadOnlyMode
        {
            get { return _readOnlyMode; }
            set
            {
                _readOnlyMode = value;
                PerformOnPropertiesChanged();
            }
        }

        internal bool InUpdateMode
        {
            get { return _inUpdateMode; }
            set
            {
                _inUpdateMode = value;
                PerformOnPropertiesChanged();
            }
        }

        #endregion //Properties

        #region Methods

        private void InitializeCustomizationParameters()
        {
            if (FiglutDataBoxApplication.Instance.ThemeColor != null)
            {
                grdData.GridLineColor = grdData.HeaderBackColor =
                    FiglutDataBoxApplication.Instance.ThemeColor;
            }
            if (FiglutDataBoxApplication.Instance.DataBoxSelectRowColor != null)
            {
                grdData.SelectionBackColor = 
                    FiglutDataBoxApplication.Instance.DataBoxSelectRowColor;
            }
        }

        public void PerformOnPropertiesChanged()
        {
            if (OnDataBoxPropertiesChanged != null)
            {
                OnDataBoxPropertiesChanged(this, new DataBoxPropertiesChangedArgs(
                    CurrentTable,
                    CurrentEntityCache,
                    HiddenProperties,
                    ExtensionManagedProperties,
                    InputControls,
                    FirstInputControl,
                    EntityUnderUpdate,
                    EntityIdUnderUpdate,
                    CurrentDataBoxFilePath,
                    FiltersEnabled,
                    UnsavedChanges,
                    ReadOnlyMode,
                    InUpdateMode));
            }
        }

        private void RefreshExtensionManagedProperties(bool shapeColumnNames)
        {
            ExtensionManagedProperties.Clear();
            if (FiglutDataBoxApplication.Instance.CrudInterceptor.ExtensionManagedEntities.Exists(CurrentTable.MappedType.FullName))
            {
                foreach (ExtensionManagedEntityProperty p in FiglutDataBoxApplication.Instance.CrudInterceptor.ExtensionManagedEntities[CurrentTable.MappedType.FullName])
                {
                    string propertyName = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.PropertyName) : p.PropertyName;
                    ExtensionManagedProperties.Add(propertyName);
                }
            }
        }

        private void CleanExtensionsMainMenu()
        {
            foreach (MenuItem menuItem in mnuOptions.MenuItems)
            {
                CleanExtensionsMenuItem(menuItem);
            }
            mnuOptions.MenuItems.Clear();
        }

        private void CleanExtensionsMenuItem(MenuItem menuItem)
        {
            menuItem.Click -= menuItem_Click;
            foreach (MenuItem childMenuItem in menuItem.MenuItems)
            {
                CleanExtensionsMenuItem(childMenuItem);
            }
        }

        private void RefreshExtensionsMainMenu()
        {
            CleanExtensionsMainMenu();
            foreach (ExtensionManagedMenuItem extensionManagedMenuItem in
                FiglutDataBoxApplication.Instance.MainMenuExtension.ExtensionManagedMainMenu)
            {
                if (extensionManagedMenuItem.ExtensionManagedEntities.Exists(CurrentTable.MappedType.FullName))
                {
                    MenuItem menuItem = new MenuItem() { Text = extensionManagedMenuItem.Name };
                    mnuOptions.MenuItems.Add(menuItem);
                    BuildExtensionManagedMenuItem(extensionManagedMenuItem, menuItem);
                }
            }
            mnuOptions.Enabled = mnuOptions.MenuItems.Count > 0;
        }

        private void BuildExtensionManagedMenuItem(
            ExtensionManagedMenuItem extensionManagedMenuItem,
            MenuItem menuItem)
        {
            menuItem.Click += menuItem_Click;
            foreach (ExtensionManagedMenuItem childExtensionManagedMenuItem in extensionManagedMenuItem)
            {
                if (childExtensionManagedMenuItem.ExtensionManagedEntities.Exists(CurrentTable.MappedType.FullName))
                {
                    MenuItem childMenuItem = new MenuItem() { Text = childExtensionManagedMenuItem.Name };
                    menuItem.MenuItems.Add(childMenuItem);
                    BuildExtensionManagedMenuItem(childExtensionManagedMenuItem, childMenuItem);
                }
            }
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem == null)
            {
                return;
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedGridRowCellValue<Nullable<Guid>>(grdData, _entityIdColumnIndex);
            object selectedEntity = selectedEntityId.HasValue ? CurrentEntityCache[selectedEntityId.Value] : null;
            ExtensionManagedMenuItem extensionManagedMenuItem = GetExtensionManagedMenuItem(menuItem.Text);
            if (extensionManagedMenuItem == null)
            {
                throw new UserThrownException(string.Format("Could not find extension managed menu item with name {0}.", menuItem.Text), LoggingLevel.Minimum, true);
            }
            extensionManagedMenuItem.PerformOnClick(new MenuClickArgs(_currentTable, selectedEntity, InUpdateMode));
        }

        private ExtensionManagedMenuItem GetExtensionManagedMenuItem(string name)
        {
            foreach (ExtensionManagedMenuItem extensionManagedMenuItem in FiglutDataBoxApplication.Instance.MainMenuExtension.ExtensionManagedMainMenu)
            {
                if (extensionManagedMenuItem.Name == name)
                {
                    return extensionManagedMenuItem;
                }
                ExtensionManagedMenuItem result = GetExtensionManagedMenuItem(extensionManagedMenuItem, name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        private ExtensionManagedMenuItem GetExtensionManagedMenuItem(ExtensionManagedMenuItem extensionManagedMenuItem, string name)
        {
            foreach (ExtensionManagedMenuItem childExtensionManagedMenuItem in extensionManagedMenuItem)
            {
                if (childExtensionManagedMenuItem.Name == name)
                {
                    return childExtensionManagedMenuItem;
                }
            }
            return null;
        }

        public void RefreshInputControls(List<string> hiddenProperties, bool shapeColumnNames)
        {
            if (this.ResetLinks == null)
            {
                this.ResetLinks = new List<LinkLabel>();
            }
            InputControls = UIHelper.GetControlsForEntity(
                CurrentTable.MappedType, 
                true, 
                DockStyle.Top, 
                Color.Transparent, 
                hiddenProperties, 
                ExtensionManagedProperties, 
                shapeColumnNames, 
                ResetLinks);
            BeforeAddInputControlsArgs aBeforeAddInputControls = new BeforeAddInputControlsArgs(InputControls, true);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeAddInputControls(aBeforeAddInputControls);
            if (aBeforeAddInputControls.Cancel) return;

            List<Control> inputControlsList = Enumerable.ToList<Control>(InputControls.Values);
            int firstInputControlIndex = inputControlsList.Count - ((_settings.FirstInputControlIndex + 1) * 2);
            FirstInputControl = inputControlsList[firstInputControlIndex];
            int inputControlAfterAddIndex = inputControlsList.Count - ((_settings.InputControlIndexAfterAdd + 1) * 2);
            if (inputControlAfterAddIndex < inputControlsList.Count)
            {
                InputControlAfterAdd = inputControlsList[inputControlAfterAddIndex];
            }
            this.grdData.Focus();
            AfterAddInputControlsArgs eAfterAddInputControls = new AfterAddInputControlsArgs(InputControls, true, FirstInputControl);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterAddInputControls(eAfterAddInputControls);
        }

        public bool RefreshFromServer(bool presentLoadDataBoxForm)
        {
            if (presentLoadDataBoxForm)
            {
                using (LoadDataBoxForm f = new LoadDataBoxForm())
                {
                    if (f.ShowDialog() == DialogResult.Cancel)
                    {
                        return false;
                    }
                    CurrentTable = f.SelectedTable;
                }
            }
            this.Refresh();
            BeforeRefreshFromServerArgs eBeforeRefreshFromServer = new BeforeRefreshFromServerArgs(presentLoadDataBoxForm, CurrentTable);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeRefreshFromServer(eBeforeRefreshFromServer);
            if (eBeforeRefreshFromServer.Cancel) return false;
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
            {
                w.ChangeStatus("Refreshing from server ...");
                FiglutWebServiceFilterString filterString = new FiglutWebServiceFilterString(CurrentTable.TableName, null);
                string rawOutput = null;
                IList result = DataHelper.GetListOfType(CurrentTable.MappedType);
                result = (IList)GOC.Instance.FiglutWebServiceClient.Query(result.GetType(), filterString, out rawOutput, true);
                CurrentEntityCache = new FiglutEntityCacheUnique(CurrentTable.MappedType);
                CurrentEntityCache.OverrideFromList(result);
            }
            RefreshExtensionManagedProperties(true);
            RefreshExtensionsMainMenu();
            AfterRefreshFromServerArgs eAfterRefreshFromServer = new AfterRefreshFromServerArgs(presentLoadDataBoxForm, CurrentTable);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterRefreshFromServer(eAfterRefreshFromServer);
            return true;
        }

        private bool RefreshFromCsvFile(bool presentLoadDataBoxForm, bool presentOpenFileDialogBox)
        {
            if (presentLoadDataBoxForm)
            {
                using (LoadDataBoxForm f = new LoadDataBoxForm())
                {
                    if (f.ShowDialog() == DialogResult.Cancel)
                    {
                        return false;
                    }
                    CurrentTable = f.SelectedTable;
                    CurrentEntityCache = new FiglutEntityCacheUnique(CurrentTable.MappedType);
                }
            }
            if (presentOpenFileDialogBox)
            {
                if (opdImport.ShowDialog() == DialogResult.OK)
                {
                    _currentDataBoxFilePath = opdImport.FileName;
                }
            }
            if (!string.IsNullOrEmpty(_currentDataBoxFilePath))
            {
                ImportFromFile(_currentDataBoxFilePath);
                return true;
            }
            return true;
        }

        #region IDataBox Members

        public bool RefreshDataBox(bool fromServer, bool fromFile, bool presentOpenFileDialogBox, bool presentLoadDataBoxForm, bool refreshInputControls)
        {
            if (!presentLoadDataBoxForm && CurrentTable == null)
            {
                throw new UserThrownException("No DataBox loaded yet to be refreshed. Load DataBox first.", LoggingLevel.Maximum);
            }
            if (fromServer && !RefreshFromServer(presentLoadDataBoxForm))
            {
                return false;
            }
            else if(fromFile && !RefreshFromCsvFile(presentLoadDataBoxForm, presentOpenFileDialogBox))
            {
                return false;
            }
            picParent.Visible = _currentTable.GetForeignKeyColumns().Count > 0;
            picChildren.Visible = _currentTable.ChildrenTables.Count > 0;
            int selectedRowIndex = -1;
            if (grdData.CurrentRowIndex > 0)
            {
                selectedRowIndex = grdData.CurrentRowIndex;
            }
            BeforeGridRefreshArgs eBeforeGridRefresh = new BeforeGridRefreshArgs(selectedRowIndex, grdData, FiltersEnabled, fromServer);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeGridRefresh(eBeforeGridRefresh);
            if (eBeforeGridRefresh.Cancel) return false;
            List<Type> hiddenTypes = null;
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
            {
                w.ChangeStatus("Refreshing grid ...");
                DataTable dataSourceTable = null;
                if (FiltersEnabled)
                {
                    //Dictionary<string, object> properties = new Dictionary<string, object>();
                    //properties.Add(EntityReader<TimesheetItem>.GetPropertyName(p => p.ReferenceNumber, false), txtFilterReferenceNumber.Text);
                    //properties.Add(EntityReader<TimesheetItem>.GetPropertyName(p => p.Client, false), txtFilterClient.Text);
                    //if (_billingFiltersEnabled)
                    //{
                    //    properties.Add(EntityReader<TimesheetItem>.GetPropertyName(p => p.CallOut, false), chkFilterCallOut.Checked);
                    //    properties.Add(EntityReader<TimesheetItem>.GetPropertyName(p => p.Billable, false), chkFilterBillable.Checked);
                    //    properties.Add(EntityReader<TimesheetItem>.GetPropertyName(p => p.SunkCost, false), chkFilterSunkCost.Checked);
                    //    properties.Add(EntityReader<TimesheetItem>.GetPropertyName(p => p.Paid, false), chkFilterPaid.Checked);
                    //    properties.Add(EntityReader<TimesheetItem>.GetPropertyName(p => p.BillableTo, false), filterBillableTo);
                    //    properties.Add(EntityReader<TimesheetItem>.GetPropertyName(p => p.PONumber, false), txtFilterPONumber.Text);
                    //}
                    //grdTimesheet.DataSource = _timesheetItemCache.GetDataTable(
                    //    properties,
                    //    false,
                    //    true,
                    //    dtpFilterStartDate.Value,
                    //    dtpFilterEndDate.Value,
                    //    _filteredTimesheetItemCache); //After this call the filtered timesheet item cache will only contain the items included while the filtered was applied.
                }
                else
                {
                    //grdTimesheet.DataSource = _timesheetItemCache.GetDataTable(null, false, true);
                    //_filteredTimesheetItemCache.Clear();
                    //_timesheetItemCache.ToList().ForEach(p => _filteredTimesheetItemCache.Add(p));
                    dataSourceTable = CurrentEntityCache.GetDataTable(null, false, _settings.ShapeColumnNames, _settings.EntityIdColumnName);
                    grdData.DataSource = dataSourceTable;
                }
                _entityIdColumnIndex = DataHelper.GetColumnIndex(dataSourceTable, _settings.EntityIdColumnName);
                hiddenTypes = _settings.GetHiddenTypes();
                if (HiddenProperties == null)
                {
                    HiddenProperties = new List<string>();
                }
                HiddenProperties.Clear();
                if (_settings.HideEntityIdColumn)
                {
                    HiddenProperties.Add(_settings.EntityIdColumnName);
                }
                EntityReader.GetPropertyNamesByType(
                    CurrentTable.MappedType,
                    hiddenTypes,
                    _settings.ShapeColumnNames).ForEach(p => HiddenProperties.Add(p));

                DataGridTableStyle tableStyle = UIHelper.GetDataGridTableStyle(
                    CurrentTable.MappedType,
                    _settings.DataBoxColumnWidth, 
                    HiddenProperties, 
                    _settings.ShapeColumnNames);
                grdData.TableStyles.Clear();
                grdData.TableStyles.Add(tableStyle);
                grdData.Refresh();
                if (selectedRowIndex < dataSourceTable.Rows.Count && selectedRowIndex > -1)
                {
                    grdData.CurrentRowIndex = selectedRowIndex;
                }
            }
            AfterGridRefreshArgs eAfterGridRefresh = new AfterGridRefreshArgs(selectedRowIndex, grdData, FiltersEnabled, fromServer, HiddenProperties, hiddenTypes);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterGridRefresh(eAfterGridRefresh);
            if (refreshInputControls)
            {
                RefreshInputControls(HiddenProperties, _settings.ShapeColumnNames);
            }
            if (OnDataBoxRefresh != null)
            {
                OnDataBoxRefresh(this, new OnDataBoxAfterRefreshArgs()
                {
                    PresentLoadDataBoxForm = presentLoadDataBoxForm,
                    CurrentTable = CurrentTable,
                    FromServer = fromServer
                });
            }
            grdData.Focus();
            return true;
        }

        public void CancelEntityUpdate()
        {
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(EntityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeCancelUpdate(eBefore);
            if (eBefore.Cancel)
            {
                return;
            }
            if (!eBefore.CancelDefaultBindingBehaviour)
            {
                UIHelper.ClearControls(InputControls);
            }
            EntityUnderUpdate = null;
            EntityIdUnderUpdate = null;
            InUpdateMode = false;
            grdData.Focus();
            if (OnUpdateModeChanged != null)
            {
                OnUpdateModeChanged(this, new OnDataBoxControlStateChangedArgs() { Enabled = false }); //No longer in update mode.
            }
            AfterCrudOperationArgs eAfter = new AfterCrudOperationArgs(EntityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterCancelUpdate(eAfter);
        }

        public void PrepareForEntityUpdate()
        {
            if (grdData.CurrentRowIndex < 0)
            {
                UIHelper.DisplayError("No row selected.", this, DataBoxForm_KeyDown);
                return;
            }
            bool cancelDefaultBindingBehaviour = false;
            EntityIdUnderUpdate = UIHelper.GetSelectedGridRowCellValue<Nullable<Guid>>(grdData, _entityIdColumnIndex);
            if (!EntityIdUnderUpdate.HasValue)
            {
                throw new UserThrownException("No row selected.", LoggingLevel.None);
            }
            object selectedEntity = CurrentEntityCache[EntityIdUnderUpdate.Value];
            BeforeCrudOperationArgs eBeforePrepareForUpdate = new BeforeCrudOperationArgs(selectedEntity);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforePrepareForUpdate(eBeforePrepareForUpdate);
            if (eBeforePrepareForUpdate.Cancel)
            {
                return;
            }
            if (!cancelDefaultBindingBehaviour)
            {
                UIHelper.PopulateControlsFromEntity(
                    InputControls,
                    selectedEntity,
                    HiddenProperties,
                    ExtensionManagedProperties,
                    _settings.ShapeColumnNames);
            }
            EntityUnderUpdate = selectedEntity;
            FirstInputControl.Focus();
            if (OnUpdateModeChanged != null)
            {
                OnUpdateModeChanged(this, new OnDataBoxControlStateChangedArgs() { Enabled = true }); //Entered update mode.
            }
            AfterCrudOperationArgs eAfterPrepapreForUpdate = new AfterCrudOperationArgs(EntityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterPrepareForUpdate(eAfterPrepapreForUpdate);
            InUpdateMode = true;
            using (ManageEntityForm f = new ManageEntityForm(
                EntityOperation.Update,
                _entityUnderUpdate,
                _entityIdUnderUpdate,
                _inputControls,
                _firstInputControl,
                _inputControlAfterAdd,
                ResetLinks,
                _hiddenProperties,
                _extensionManagedProperties,
                _settings,
                _currentTable,
                _currentEntityCache))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    RefreshDataBox(false, false, false, false, false);
                    grdData.Focus();

                    UnsavedChanges = true;
                    AfterCrudOperationArgs eAfterUpdate = new AfterCrudOperationArgs(EntityUnderUpdate);
                    FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterUpdate(eAfterUpdate);
                    if (OnUpdateModeChanged != null)
                    {
                        OnUpdateModeChanged(this, new OnDataBoxControlStateChangedArgs() { Enabled = false }); //No longer in update mode.
                    }
                    InUpdateMode = false;
                }
                else
                {
                    CancelEntityUpdate();
                }
            }
        }

        public void AddEntity()
        {
            if (CurrentEntityCache == null)
            {
                throw new UserThrownException("No DataBox loaded yet. Load DataBox first.", LoggingLevel.Maximum);
            }
            object entity = Activator.CreateInstance(CurrentTable.MappedType);
            bool cancelDefaultBindingBehaviour = false;
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(entity);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeAdd(eBefore);
            if (eBefore.Cancel)
            {
                return;
            }
            if (!cancelDefaultBindingBehaviour)
            {
                using (ManageEntityForm f = new ManageEntityForm(
                    EntityOperation.Add,
                    entity,
                    null,
                    _inputControls,
                    _firstInputControl,
                    _inputControlAfterAdd,
                    ResetLinks,
                    _hiddenProperties,
                    _extensionManagedProperties,
                    _settings,
                    _currentTable,
                    _currentEntityCache))
                {
                    f.ShowDialog();
                    if (!f.ChangesMade)
                    {
                        return;
                    }
                }
                RefreshDataBox(false, false, false, false, false);
                UnsavedChanges = true;
            }
            AfterCrudOperationArgs eAfter = new AfterCrudOperationArgs(entity);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterAdd(eAfter);
        }

        public void DeleteEntity()
        {
            if (grdData.CurrentRowIndex < 0)
            {
                grdData.Focus();
                throw new UserThrownException("No row selected.", LoggingLevel.Maximum);
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedGridRowCellValue<Nullable<Guid>>(grdData, _entityIdColumnIndex);
            Debug.Assert(selectedEntityId.HasValue);
            object selectedEntity = CurrentEntityCache[selectedEntityId.Value];
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(selectedEntity);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeDelete(eBefore);
            if (eBefore.Cancel)
            {
                return;
            }
            CurrentEntityCache.Delete(selectedEntityId.Value);
            RefreshDataBox(false, false, false, false, false);
            UnsavedChanges = true;
            AfterCrudOperationArgs eAfter = new AfterCrudOperationArgs(selectedEntity);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterDelete(eAfter);
        }

        public void ManageChildren()
        {
            if (!picChildren.Visible)
            {
                return;
            }
            if (grdData.CurrentRowIndex < 0)
            {
                grdData.Focus();
                throw new UserThrownException("No row selected.", LoggingLevel.Maximum);
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedGridRowCellValue<Nullable<Guid>>(grdData, _entityIdColumnIndex);
            Debug.Assert(selectedEntityId.HasValue);
            object selectedEntity = CurrentEntityCache[selectedEntityId.Value];
            using (SelectChildDataBoxForm f = new SelectChildDataBoxForm(
                selectedEntity,
                selectedEntityId,
                _settings,
                _currentTable))
            {
                f.ShowDialog();
            }
        }

        public void ViewParent()
        {
            if (!picParent.Visible)
            {
                return;
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedGridRowCellValue<Nullable<Guid>>(grdData, _entityIdColumnIndex);
            Debug.Assert(selectedEntityId.HasValue);
            object selectedEntity = CurrentEntityCache[selectedEntityId.Value];
            MobileDatabaseTable parentTable = GOC.Instance.GetDatabase<SqlCeDatabase>().Tables[_currentTable.TableName];
            using (SelectParentDataBoxForm f = new SelectParentDataBoxForm(
                EntityOperation.ReadOnly,
                selectedEntity,
                selectedEntityId,
                _settings,
                _currentTable,
                _currentEntityCache))
            {
                f.ShowDialog();
            }   
        }

        public void Save()
        {
            BeforeSaveArgs eBefore = new BeforeSaveArgs();
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeSave(eBefore);
            if (eBefore.Cancel)
            {
                return;
            }
            if (!_settings.OfflineMode)
            {
                string messageResult = null;
                using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
                {
                    w.ChangeStatus("Saving changes to server ...");
                    bool saveSuccessful = CurrentEntityCache.SaveToServer(out messageResult, true);
                    AfterSaveArgs eAfter = new AfterSaveArgs(messageResult, saveSuccessful);
                    FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterSave(eAfter);
                    UnsavedChanges = false;
                }
                UIHelper.DisplayInformation(messageResult, this, DataBoxForm_KeyDown);
            }
            else
            {
                if (string.IsNullOrEmpty(_currentDataBoxFilePath) ||
                    !File.Exists(_currentDataBoxFilePath) ||
                    UIHelper.AskQuestion("Save to new file?") == DialogResult.Yes)
                {
                    if (svdExport.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    _currentDataBoxFilePath = svdExport.FileName;
                }
                Debug.Assert(!string.IsNullOrEmpty(_currentDataBoxFilePath));
                using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
                {
                    w.ChangeStatus(string.Format("Saving {0} ...", _currentDataBoxFilePath));
                    ExportToFile(_currentDataBoxFilePath);
                    UIHelper.DisplayInformation(string.Format("Saved to {0}.", _currentDataBoxFilePath));
                }
            }
        }

        public void SetFiltersEnabled(bool enable)
        {
            throw new NotImplementedException();
        }

        public void SelectEntity(Guid dataBoxEntityId)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void ImportFromFile(string filePath)
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
            {
                w.ChangeStatus("Importing from file ...");
                _currentEntityCache.ImportFromCsv(filePath, false);
                _unsavedChanges = false;
            }
        }

        private void ExportToFile(string filePath)
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
            {
                w.ChangeStatus("Exporting to file ...");
                _currentEntityCache.ExportToCsv(filePath, null, false, false, null);
                _unsavedChanges = false;
            }
        }

        private void ForceFullDataBoxLoad(bool handleExceptions, bool closeFormOnCancel)
        {
            try
            {
                if (!_settings.OfflineMode)
                {
                    if (!RefreshDataBox(true, false, false, true, true) && closeFormOnCancel)
                    {
                        this.DialogResult = DialogResult.Cancel;
                        Close();
                    }
                }
                else
                {
                    if (!RefreshDataBox(false, true, true, true, true) && closeFormOnCancel)
                    {
                        this.DialogResult = DialogResult.Cancel;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!handleExceptions)
                {
                    throw ex;
                }
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_Load);
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void DataBoxForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Manage data in databox.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    grdData.Height += 50;
                }
                this.BeginInvoke(new Action<bool, bool>(ForceFullDataBoxLoad), true, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (UnsavedChanges && 
                    UIHelper.AskQuestion("There are unsaved changes that will be lost. Are you sure?") == DialogResult.No)
                {
                    return;
                }
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }

        private void picAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddEntity();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }

        private void picUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareForEntityUpdate();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }

        private void picDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteEntity();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }

        private void picImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (opdImport.ShowDialog() == DialogResult.OK)
                {
                    ImportFromFile(opdImport.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ForceFullDataBoxLoad(false, false);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }


        private void picChildren_Click(object sender, EventArgs e)
        {
            try
            {
                ManageChildren();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }

        private void picParent_Click(object sender, EventArgs e)
        {
            try
            {
                ViewParent();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }

        private void DataBoxForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
                else if (e.KeyCode == Keys.A)
                {
                    picAdd_Click(sender, e);
                }
                else if (e.KeyCode == Keys.U)
                {
                    picUpdate_Click(sender, e);
                }
                else if (e.KeyCode == Keys.D)
                {
                    picDelete_Click(sender, e);
                }
                else if (e.KeyCode == Keys.I)
                {
                    picImport_Click(sender, e);
                }
                else if (e.KeyCode == Keys.C)
                {
                    picChildren_Click(sender, e);
                }
                else if (e.KeyCode == Keys.P)
                {
                    picParent_Click(sender, e);
                }
                else if (e.KeyCode == Keys.L)
                {
                    picRefresh_Click(sender, e);
                }
                else if (e.KeyCode == Keys.S)
                {
                    picSave_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, DataBoxForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}