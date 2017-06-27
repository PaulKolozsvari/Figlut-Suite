namespace Figlut.Desktop.DataBox.Controls
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Drawing.Imaging;
    using Figlut.Server.Toolkit.Winforms;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Desktop.DataBox.AuxilaryUI;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;
    using System.Collections;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Extensions.DataBox;
    using Figlut.Server.Toolkit.Extensions.DataBox.Events;
    using Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud;
    using Figlut.Server.Toolkit.Extensions.DataBox.Events.MainMenu;
    using Figlut.Server.Toolkit.Extensions;
    using Figlut.Server.Toolkit.Extensions.ExtensionManaged;
    using Figlut.Server.Toolkit.Extensions.DataBox.Managers;
    using Figlut.Desktop.DataBox.Configuration;
    using Figlut.Desktop.DataBox.Utilities;
    using Figlut.Server.Toolkit.Data.DB;

    #endregion //Using Directives

    public partial class DataBoxControl : UserControl, IDataBox
    {
        #region Inner Types

        public class OnDataBoxControlStateChangedArgs : EventArgs
        {
            public bool Enabled { get; set; }
        }

        public class OnDataBoxAfterRefreshArgs
        {
            public bool PresentLoadDataBoxForm { get; set; }

            public SqlDatabaseTable CurrentTable { get; set; }

            public bool FromServer { get; set; }
        }

        #endregion //Inner Types

        #region Constructors

        public DataBoxControl()
        {
            InitializeComponent();
            _settings = GOC.Instance.GetSettings<FiglutDesktopDataBoxSettings>();
            InputControls = new Dictionary<string, Control>();
            ExtensionManagedProperties = new List<string>();
            if (!DesignMode)
            {
                grdData.DefaultCellStyle.SelectionBackColor = string.IsNullOrEmpty(_settings.DataBoxSelectRowColor) ? Color.SteelBlue : Color.FromName(_settings.DataBoxSelectRowColor);
                grdData.RowHeadersDefaultCellStyle.SelectionBackColor = string.IsNullOrEmpty(_settings.DataBoxSelectRowColor) ? Color.SteelBlue : Color.FromName(_settings.DataBoxSelectRowColor);
            }
        }

        #endregion //Constructors

        #region Fields

        private BorderlessForm _parentForm;
        private FiglutDesktopDataBoxSettings _settings;

        private SqlDatabaseTable _currentTable;
        private FiglutWebServiceFilterString _currentFilterString;
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

        private string Status
        {
            get
            {
                if (this.ParentForm == null)
                {
                    return null;
                }
                BorderlessForm f = this.ParentForm as BorderlessForm;
                if (f == null)
                {
                    throw new NullReferenceException("Parent form of TimesheetControl is not of expected type.");
                }
                return f.Status;
            }
            set
            {
                if (this.ParentForm == null)
                {
                    return;
                }
                BorderlessForm f = this.ParentForm as BorderlessForm;
                if (f == null)
                {
                    throw new NullReferenceException("Parent form of TimesheetControl is not of expected type.");
                }
                f.Status = value;
            }
        }

        internal SqlDatabaseTable CurrentTable 
        {
            get { return _currentTable; }
            set 
            { 
                _currentTable = value;
                PerformOnPropertiesChanged();
            }
        }

        internal FiglutWebServiceFilterString CurrentFilterString
        {
            get { return _currentFilterString; }
            set
            {
                _currentFilterString = value;
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

        private void SubscribeToResetLinksClickEvents()
        {
            if (_resetLinks == null)
            {
                return;
            }
            foreach (LinkLabel lnkReset in _resetLinks)
            {
                lnkReset.Click += new EventHandler(lnkReset_Click);
            }
        }

        public void UnsubscribeFromResetLinksClickEvents()
        {
            if (_resetLinks == null)
            {
                return;
            }
            foreach (LinkLabel lnkReset in _resetLinks)
            {
                lnkReset.Click += new EventHandler(lnkReset_Click);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            ClearInputControls();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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

        public void SelectInputsTabPage()
        {
            if (tabData.TabPages.Contains(tabPageDataInput))
            {
                tabData.SelectTab(tabPageDataInput.Name);
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
            foreach (ToolStripMenuItem toolStripMenuItem in mnuExtensionsMainMenu.DropDownItems)
            {
                CleanExtensionsMenuItem(toolStripMenuItem);
            }
            mnuExtensionsMainMenu.DropDownItems.Clear();
        }

        private void CleanExtensionsMenuItem(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.Click -= toolStripMenuItem_Click;
            toolStripMenuItem.Tag = null;
            foreach (ToolStripMenuItem childToolStripMenuItem in toolStripMenuItem.DropDownItems)
            {
                CleanExtensionsMenuItem(childToolStripMenuItem);
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
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(extensionManagedMenuItem.Name);
                    mnuExtensionsMainMenu.DropDownItems.Add(toolStripMenuItem);
                    BuildExtensionManagedMenuItem(extensionManagedMenuItem, toolStripMenuItem);
                }
            }
            mnuExtensionsMainMenu.Visible = mnuExtensionsMainMenu.HasDropDownItems;
        }

        private void BuildExtensionManagedMenuItem(
            ExtensionManagedMenuItem extensionManagedMenuItem, 
            ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.Tag = extensionManagedMenuItem;
            toolStripMenuItem.Click += toolStripMenuItem_Click;
            foreach (ExtensionManagedMenuItem childExtensionManagedMenuItem in extensionManagedMenuItem)
            {
                if (childExtensionManagedMenuItem.ExtensionManagedEntities.Exists(CurrentTable.MappedType.FullName))
                {
                    ToolStripMenuItem childToolStripMenuItem = new ToolStripMenuItem(childExtensionManagedMenuItem.Name);
                    toolStripMenuItem.DropDownItems.Add(childToolStripMenuItem);
                    BuildExtensionManagedMenuItem(childExtensionManagedMenuItem, childToolStripMenuItem);
                }
            }
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            if (toolStripMenuItem == null)
            {
                return;
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedDataGridViewRowCellValue<Nullable<Guid>>(grdData, _settings.EntityIdColumnName);
            object selectedEntity = selectedEntityId.HasValue ? CurrentEntityCache[selectedEntityId.Value] : null;
            ((ExtensionManagedMenuItem)toolStripMenuItem.Tag).PerformOnClick(new MenuClickArgs(
                CurrentTable,
                selectedEntity,
                InUpdateMode));
        }

        public void RefreshInputControls(List<string> hiddenProperties, bool shapeColumnNames)
        {
            if (ResetLinks == null)
            {
                ResetLinks = new List<LinkLabel>();
            }
            UnsubscribeFromResetLinksClickEvents();
            //Dictionary<string, Control> inputControls = UIHelper.GetControlsForEntity(
            //    CurrentTable.MappedType,
            //    true, 
            //    DockStyle.Top,
            //    Color.Transparent,
            //    hiddenProperties,
            //    ExtensionManagedProperties,
            //    shapeColumnNames);
            InputControls = UIHelper.GetControlsForEntity(
                CurrentTable.MappedType,
                true,
                DockStyle.Top,
                Color.Transparent,
                hiddenProperties,
                ExtensionManagedProperties,
                shapeColumnNames,
                ResetLinks,
                Color.Gainsboro);
            BeforeAddInputControlsArgs eBefore = new BeforeAddInputControlsArgs(InputControls, true);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeAddInputControls(eBefore);
            if (eBefore.Cancel) return;

            List<Control> inputControlsList = InputControls.Values.ToList();
            int firstInputControlIndex = inputControlsList.Count - ((_settings.FirstInputControlIndex + 1) * 2);
            FirstInputControl = inputControlsList[firstInputControlIndex];
            int inputControlAfterAddIndex = inputControlsList.Count - ((_settings.InputControlIndexAfterAdd + 1) * 2);
            if (inputControlAfterAddIndex < inputControlsList.Count)
            {
                InputControlAfterAdd = inputControlsList[inputControlAfterAddIndex];
            }
            pnlDataInput.Controls.Clear();
            int tabIndex = _inputControls.Count - 1;
            foreach (Control control in _inputControls.Values)
            {
                control.TabIndex = tabIndex;
                pnlDataInput.Controls.Add(control);
                tabIndex--;
            }
            pnlDataInput.Refresh();
            _firstInputControl = UIHelper.ExtractInputControlFromInputPanel((Panel)FirstInputControl);
            _firstInputControl.Focus();
            if (InputControlAfterAdd != null)
            {
                _inputControlAfterAdd = UIHelper.ExtractInputControlFromInputPanel((Panel)InputControlAfterAdd);
            }
            AfterAddInputControlsArgs eAfterAddInputControls = new AfterAddInputControlsArgs(_inputControls, true, FirstInputControl);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterAddInputControls(eAfterAddInputControls);
            tsSelectParent.Visible = _currentTable.GetForeignKeyColumns().Count > 0;
            SubscribeToResetLinksClickEvents();
        }

        private void ClearInputControls()
        {
            UnsubscribeFromResetLinksClickEvents();
            _inputControls.Values.ToList().ForEach(c => pnlDataInput.Controls.Remove(c));
            UIHelper.ClearControls(_inputControls);
        }

        private bool ApplyLoadDataBoxInfo(
            out SqlDatabaseTable table,
            out FiglutWebServiceFilterString filterString)
        {
            bool applyFilters = false;
            using (LoadDataBoxForm f = new LoadDataBoxForm(_settings.DatabaseSchemaFileName))
            {
                if (f.ShowDialog() != DialogResult.OK) //Cancel out of process.
                {
                    table = null;
                    filterString = null;
                    return false;
                }
                table = f.SelectedTable;
                applyFilters = f.ApplyFilters;
            }
            if (!applyFilters) //Filters should not be applied. Just update the filter string with the name of the newly selected table to be loaded.
            {
                filterString = new FiglutWebServiceFilterString(table.TableName, null);
                return true;
            }
            using (LoadFiltersForm f = new LoadFiltersForm(
                table, 
                GetHiddenProperiesForTable(table),
                ExtensionManagedProperties,
                _settings.ShapeColumnNames, 
                _settings.TreatZeroAsNull))
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    table = null;
                    filterString = null;
                    return false;
                }
                filterString = f.FilterString;
            }
            return true;
        }

        private List<string> GetHiddenProperiesForTable(SqlDatabaseTable table)
        {
            List<Type> hiddenTypes = _settings.GetHiddenTypes();
            List<string> result = EntityReader.GetPropertyNamesByType(
                table.MappedType,
                hiddenTypes,
                _settings.ShapeColumnNames);
            if (_settings.HideEntityIdColumn)
            {
                result.Add(_settings.EntityIdColumnName);
            }
            return result;
        }

        private bool RefreshFromServer(bool presentLoadDataBoxForm)
        {
            SqlDatabaseTable table = CurrentTable;
            FiglutWebServiceFilterString filterString = CurrentFilterString;
            if (presentLoadDataBoxForm)
            {
                if (!ApplyLoadDataBoxInfo(out table, out filterString))
                {
                    return false;
                }
            }
            BeforeRefreshFromServerArgs eBeforeRefreshFromServer = new BeforeRefreshFromServerArgs(presentLoadDataBoxForm, table);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeRefreshFromServer(eBeforeRefreshFromServer);
            if (eBeforeRefreshFromServer.Cancel) return false;
            string rawOutput = null;
            IList result = DataHelper.GetListOfType(table.MappedType);
            result = (IList)GOC.Instance.FiglutWebServiceClient.Query(
                result.GetType(),
                filterString,
                out rawOutput,
                true);
            CurrentEntityCache = new FiglutEntityCacheUnique(table.MappedType);
            CurrentEntityCache.OverrideFromList(result);
            CurrentTable = table;
            CurrentFilterString = filterString;
            RefreshExtensionManagedProperties(true);
            RefreshExtensionsMainMenu();
            AfterRefreshFromServerArgs eAfterRefreshFromServer = new AfterRefreshFromServerArgs(presentLoadDataBoxForm, table);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterRefreshFromServer(eAfterRefreshFromServer);
            return true;
        }

        public bool RefreshDataBox(bool fromServer, bool presentLoadDataBoxForm, bool refreshInputControls)
        {
            string originalStatus = Status;
            try
            {
                if (!presentLoadDataBoxForm && CurrentTable == null)
                {
                    throw new UserThrownException("No DataBox loaded yet to be refreshed. Load DataBox first.", LoggingLevel.Maximum);
                }
                if (fromServer && !RefreshFromServer(presentLoadDataBoxForm))
                {
                    return false;
                }
                tsParent.Visible = _currentTable.GetForeignKeyColumns().Count > 0;
                tsChildren.Visible = _currentTable.ChildrenTables.Count > 0;
                int selectedRowIndex = -1;
                if (grdData.SelectedRows.Count > 0)
                {
                    selectedRowIndex = grdData.SelectedRows[0].Index;
                }
                BeforeGridRefreshArgs eBeforeGridRefresh = new BeforeGridRefreshArgs(selectedRowIndex, grdData, FiltersEnabled, fromServer);
                FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeGridRefresh(eBeforeGridRefresh);
                if (eBeforeGridRefresh.Cancel) return false;
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
                    grdData.DataSource = CurrentEntityCache.GetDataTable(null, false, _settings.ShapeColumnNames, _settings.EntityIdColumnName);
                }
                HiddenProperties = GetHiddenProperiesForTable(CurrentTable);
                HiddenProperties.ForEach(c => grdData.Columns[c].Visible = false);
                grdData.Refresh();
                if (selectedRowIndex < grdData.Rows.Count && selectedRowIndex > -1)
                {
                    grdData.Rows[selectedRowIndex].Selected = true;
                }
                AfterGridRefreshArgs eAfterGridRefresh = new AfterGridRefreshArgs(selectedRowIndex, grdData, FiltersEnabled, fromServer, HiddenProperties, _settings.GetHiddenTypes());
                FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterGridRefresh(eAfterGridRefresh);
                if (refreshInputControls)
                {
                    RefreshInputControls(HiddenProperties, true);
                }
                EntityUnderUpdate = Activator.CreateInstance(CurrentTable.MappedType); //Creates a new entity to be added.
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
            finally
            {
                if (Status != originalStatus)
                {
                    Status = originalStatus;
                }
            }
        }

        public void RefreshDataBox(
            bool fromServer, 
            object rowValueToSelect, 
            string columnNameToSearchOn, 
            bool presentLoadDataBoxForm,
            bool refreshInputControls)
        {
            RefreshDataBox(fromServer, presentLoadDataBoxForm, refreshInputControls);
            foreach (DataGridViewRow r in grdData.Rows)
            {
                r.Selected = false;
            }
            foreach (DataGridViewRow r in grdData.Rows)
            {
                DataRow drv = ((DataRowView)(r.DataBoundItem)).Row;
                object currentValue = drv[columnNameToSearchOn];
                if (currentValue == rowValueToSelect)
                {
                    r.Selected = true;
                    return;
                }
            }
        }

        private void EnableControlsForNonUpdateMode()
        {
            tsAdd.Enabled = true;
            tsUpdate.Enabled = true;
            tsUpdateCancel.Enabled = false;
            tsUpdateCommit.Enabled = false;
            tsDelete.Enabled = true;
        }

        private void EnableControlsForUpdateMode()
        {
            tsAdd.Enabled = false;
            tsUpdate.Enabled = false;
            tsUpdateCancel.Enabled = true;
            tsUpdateCommit.Enabled = true;
            tsDelete.Enabled = false;
        }

        public void SelectEntity()
        {
            throw new NotImplementedException();
        }

        public void UpdateEntity()
        {
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(EntityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeUpdate(eBefore);
            if (eBefore.Cancel) return;
            if (!eBefore.CancelDefaultBindingBehaviour)
            {
                UIHelper.PopulateEntityFromControls(
                    InputControls, 
                    EntityUnderUpdate, 
                    HiddenProperties, 
                    ExtensionManagedProperties, 
                    _settings.ShapeColumnNames, 
                    _settings.TreatZeroAsNull);
                UnsavedChanges = true;
            }
            Type entityType = EntityUnderUpdate.GetType();
            foreach (SqlDatabaseTableColumn column in _currentTable.Columns) //Populate the primary keys if they're GUIDs.
            {
                PropertyInfo p = entityType.GetProperty(column.ColumnName);
                object value = p.GetValue(_entityUnderUpdate, null);
                if ((value == null ||
                    string.IsNullOrEmpty(value.ToString()) ||
                    (p.PropertyType == typeof(Guid) && ((Guid)value) == Guid.Empty) ||
                    (_settings.TreatZeroAsNull && value.ToString() == "0")) &&
                    !column.IsNullable)
                {
                    if (!column.IsForeignKey)
                    {
                        string inputPanelName = string.Format("pnl{0}", column.ColumnName);
                        UIHelper.ExtractInputControlFromInputPanel((Panel)_inputControls[inputPanelName]).Focus();
                    }
                    throw new UserThrownException(string.Format("{0} must be specified.", DataShaper.ShapeCamelCaseString(column.ColumnName)), LoggingLevel.Maximum);
                }
            }
            UIHelper.ClearControls(InputControls);
            CurrentEntityCache.NotifyEntityUpdated(EntityIdUnderUpdate.Value, EntityUnderUpdate);
            RefreshDataBox(false, false, false);
            EntityUnderUpdate = null;
            EntityIdUnderUpdate = null;
            EnableControlsForNonUpdateMode();
            grdData.Focus();
            UnsavedChanges = true;
            AfterCrudOperationArgs eAfter = new AfterCrudOperationArgs(EntityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterUpdate(eAfter);
            if (OnUpdateModeChanged != null)
            {
                OnUpdateModeChanged(this, new OnDataBoxControlStateChangedArgs() { Enabled = false }); //No longer in update mode.
            }
            InUpdateMode = true;
            EntityUnderUpdate = Activator.CreateInstance(CurrentTable.MappedType); //Creates a new entity to be added.
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
            EnableControlsForNonUpdateMode();
            grdData.Focus();
            if (OnUpdateModeChanged != null)
            {
                OnUpdateModeChanged(this, new OnDataBoxControlStateChangedArgs() { Enabled = false }); //No longer in update mode.
            }
            AfterCrudOperationArgs eAfter = new AfterCrudOperationArgs(EntityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterCancelUpdate(eAfter);
            InUpdateMode = false;
            EntityUnderUpdate = Activator.CreateInstance(CurrentTable.MappedType); //Creates a new entity to be added.
        }

        public void PrepareForEntityUpdate()
        {
            bool cancelDefaultBindingBehaviour = false;
            EntityIdUnderUpdate = UIHelper.GetSelectedDataGridViewRowCellValue<Nullable<Guid>>(grdData, _settings.EntityIdColumnName);
            if (!EntityIdUnderUpdate.HasValue)
            {
                throw new UserThrownException("No row selected.", LoggingLevel.None);
            }
            object selectedEntity = CurrentEntityCache[EntityIdUnderUpdate.Value];
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(selectedEntity);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforePrepareForUpdate(eBefore);
            if (eBefore.Cancel)
            {
                return;
            }
            SelectInputsTabPage();
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
            EnableControlsForUpdateMode();
            FirstInputControl.Focus();
            if (OnUpdateModeChanged != null)
            {
                OnUpdateModeChanged(this, new OnDataBoxControlStateChangedArgs() { Enabled = true }); //Entered update mode.
            }
            AfterCrudOperationArgs eAfter = new AfterCrudOperationArgs(EntityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterPrepareForUpdate(eAfter);
            InUpdateMode = true;
        }

        public void AddEntity()
        {
            if (CurrentEntityCache == null)
            {
                throw new UserThrownException("No DataBox loaded yet to be refreshed. Load DataBox first.", LoggingLevel.Maximum);
            }
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(EntityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeAdd(eBefore);
            if (eBefore.Cancel) return;
            if (!eBefore.CancelDefaultBindingBehaviour)
            {
                UIHelper.PopulateEntityFromControls(
                    _inputControls,
                    EntityUnderUpdate,
                    _hiddenProperties,
                    _extensionManagedProperties,
                    _settings.ShapeColumnNames,
                    _settings.TreatZeroAsNull);
            }
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeAdd(eBefore); //After populating the entity properties.
            if (eBefore.Cancel) return;
            Type type = _entityUnderUpdate.GetType();
            foreach (SqlDatabaseTableColumn column in _currentTable.Columns)
            {
                PropertyInfo property = type.GetProperty(column.ColumnName);
                if (column.IsKey && (property.PropertyType == typeof(Guid)))
                {
                    property.SetValue(this._entityUnderUpdate, Guid.NewGuid(), null); //Set the GUID primary key.
                    continue;
                }
                object value = property.GetValue(_entityUnderUpdate, null);
                if ((
                    (((value == null) || string.IsNullOrEmpty(value.ToString())) || ((property.PropertyType == typeof(Guid)) && (((Guid)value) == Guid.Empty))) ||
                    (_settings.TreatZeroAsNull && (value.ToString() == "0"))) &&
                    !column.IsNullable)
                {
                    if (!column.IsForeignKey)
                    {
                        string inputPanelName = string.Format("pnl{0}", column.ColumnName);
                        UIHelper.ExtractInputControlFromInputPanel((Panel)_inputControls[inputPanelName]).Focus();
                    }
                    throw new UserThrownException(string.Format("{0} must be specified.", DataShaper.ShapeCamelCaseString(column.ColumnName)), LoggingLevel.Maximum);
                }
            }
            _currentEntityCache.Add(_entityUnderUpdate);
            UnsavedChanges = true;
            UIHelper.ClearControls(_inputControls);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterAdd(new AfterCrudOperationArgs(EntityUnderUpdate));

            _entityUnderUpdate = null;
            _entityUnderUpdate = Activator.CreateInstance(_currentTable.MappedType);
            ((InputControlAfterAdd == null) ? FirstInputControl : InputControlAfterAdd).Focus();
            RefreshDataBox(false, false, false);
        }

        public void DeleteEntity()
        {
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedDataGridViewRowCellValue<Nullable<Guid>>(grdData, _settings.EntityIdColumnName);
            if (!selectedEntityId.HasValue)
            {
                throw new UserThrownException("No row selected.", LoggingLevel.None);
            }
            object selectedEntity = CurrentEntityCache[selectedEntityId.Value];
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(selectedEntity);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeDelete(eBefore);
            if (eBefore.Cancel)
            {
                return;
            }
            CurrentEntityCache.Delete(selectedEntityId.Value);
            RefreshDataBox(false, false, false);
            UnsavedChanges = true;
            AfterCrudOperationArgs eAfter = new AfterCrudOperationArgs(selectedEntity);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterDelete(eAfter);
        }

        public void Save()
        {
            if (CurrentEntityCache == null)
            {
                throw new UserThrownException("No DataBox loaded yet. Load DataBox first.", LoggingLevel.Maximum);
            }
            BeforeSaveArgs eBefore = new BeforeSaveArgs();
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeSave(eBefore);
            if (eBefore.Cancel)
            {
                return;
            }
            string messageResult = null;
            using (WaitProcess w = new WaitProcess(_parentForm))
            {
                w.ChangeStatus("Saving changes to server ...");
                bool saveSuccessful = CurrentEntityCache.SaveToServer(out messageResult, true);
                AfterSaveArgs eAfter = new AfterSaveArgs(messageResult, saveSuccessful);
                FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterSave(eAfter);
            }
            UIHelper.DisplayInformation(messageResult);
        }

        public void Export()
        {
            if (CurrentEntityCache == null)
            {
                throw new UserThrownException("No DataBox loaded yet. Load DataBox first.", LoggingLevel.Maximum);
            }
            if (svdExport.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            using (WaitProcess w = new WaitProcess(_parentForm))
            {
                w.ChangeStatus(string.Format("Exporting to {0} ...", svdExport.FileName));
                CurrentEntityCache.ExportToCsv(svdExport.FileName, null, false, _settings.ShapeColumnNames, _settings.EntityIdColumnName);
            }
        }

        public void SetFiltersEnabled(bool enable)
        {
            //if (enable)
            //{
            //    tsTimesheetFiltersDisable.Enabled = true;
            //    tsTimesheetFiltersEnable.Enabled = false;
            //    ShowTimesheetInfoTabs(true, true, true, true);
            //    _filtersEnabled = true;
            //    SelectTimesheetFiltersTabPage();
            //    ClearTimesheetFiltersToDefault();
            //    if (FiltersEnabledChanged != null)
            //    {
            //        FiltersEnabledChanged(new TimesheetControlStateChangedArgs() { Enabled = true }); //Now enabled
            //    }
            //}
            //else
            //{
            //    tsTimesheetFiltersDisable.Enabled = false;
            //    tsTimesheetFiltersEnable.Enabled = true;
            //    SetBillingFiltersEnabled(false);
            //    ShowTimesheetInfoTabs(true, true, true, false);
            //    _filtersEnabled = false;
            //    SelectTimesheetClientInputsTabPage();
            //    if (FiltersEnabledChanged != null)
            //    {
            //        FiltersEnabledChanged(new TimesheetControlStateChangedArgs() { Enabled = false });//Now disabled
            //    }
            //}
            //RefreshGrid();
        }

        private void ShowTabs(
            bool showTimesheetInputsPage,
            bool showTimesheetTotalsPage,
            bool showTimesheetInfoPage,
            bool showTimesheetFiltersPage)
        {
            //tabData.TabPages.Remove(tabPageTimesheetInputs);
            //tabData.TabPages.Remove(tabPageTimesheetTotals);
            //tabData.TabPages.Remove(tabPageDataInput);
            //tabData.TabPages.Remove(tabPageTimesheetFilters);
            //if (showTimesheetInputsPage)
            //{
            //    tabData.TabPages.Add(tabPageTimesheetInputs);
            //}
            //if (showTimesheetTotalsPage)
            //{
            //    tabData.TabPages.Add(tabPageTimesheetTotals);
            //}
            //if (showTimesheetInfoPage)
            //{
            //    tabData.TabPages.Add(tabPageDataInput);
            //}
            //if (showTimesheetFiltersPage)
            //{
            //    tabData.TabPages.Add(tabPageTimesheetFilters);
            //}
        }

        public void HandleUpdateRequest()
        {
            if (!InUpdateMode)
            {
                PrepareForEntityUpdate();
            }
            else
            {
                UpdateEntity();
            }
        }

        public void HandleCancelUpdateRequest()
        {
            if (InUpdateMode)
            {
                CancelEntityUpdate();
            }
        }

        public void HandleAddRequest()
        {
            if (!InUpdateMode)
            {
                AddEntity();
            }
        }

        public void HandleDeleteRequest()
        {
            DeleteEntity();
        }

        public void SelectEntity(Guid dataBoxEntityId)
        {
            UIHelper.SelectDataGridViewRow(grdData, _settings.EntityIdColumnName, dataBoxEntityId);
        }

        public void ManageChildren()
        {
            if (!tsChildren.Visible)
            {
                return;
            }
            if (grdData.SelectedRows[0].Index < 0)
            {
                grdData.Focus();
                throw new UserThrownException("No row selected.", LoggingLevel.Maximum);
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedDataGridViewRowCellValue<Nullable<Guid>>(grdData, _settings.EntityIdColumnName);
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
            if (!tsParent.Visible)
            {
                return;
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedDataGridViewRowCellValue<Nullable<Guid>>(grdData, _settings.EntityIdColumnName);
            if (!selectedEntityId.HasValue)
            {
                throw new UserThrownException("No row selected.", LoggingLevel.Maximum);
            }
            object selectedEntity = CurrentEntityCache[selectedEntityId.Value];
            DatabaseTable parentTable = GOC.Instance.GetDatabase<SqlDatabase>().Tables[_currentTable.TableName];
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

        public void SelectParent()
        {
            if (!tsSelectParent.Visible)
            {
                return;
            }
            if (CurrentEntityCache == null)
            {
                throw new UserThrownException("No DataBox loaded yet. Load DataBox first.", LoggingLevel.Maximum);
            }
            EntityOperation entityOperation = InUpdateMode ? EntityOperation.Update : EntityOperation.Add;
            using (SelectParentDataBoxForm f = new SelectParentDataBoxForm(
                entityOperation,
                _entityUnderUpdate,
                _entityIdUnderUpdate,
                _settings,
                _currentTable,
                _currentEntityCache))
            {
                f.ShowDialog();
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void lnkReset_Click(object sender, EventArgs e)
        {
            LinkLabel lnkClear = (LinkLabel)sender;
            Control control = (Control)lnkClear.Tag;
            if (control is CheckBox)
            {
                ((CheckBox)control).Checked = false;
            }
            else if (control is NumericTextBox)
            {
                ((NumericTextBox)control).Text = string.Empty;
            }
            else if (control is TextBox)
            {
                ((TextBox)control).Text = string.Empty;
            }
            else if (control is NumericUpDown)
            {
                ((NumericUpDown)control).Value = 0;
            }
            else if (control is DateTimePicker)
            {
                ((DateTimePicker)control).Value = DateTime.Now;
            }
            else if (control is ComboBox)
            {
                ((ComboBox)control).SelectedIndex = -1;
            }
            else
            {
                throw new UserThrownException(string.Format("Unexpected control of type {0} to be reset.", control.GetType().FullName), LoggingLevel.Minimum);
            }
        }

        private void DataBoxControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }
            _parentForm = this.ParentForm as BorderlessForm;
            if (_parentForm == null)
            {
                throw new InvalidCastException(string.Format(
                    "Expected the parent form of {0} to be a {1} and instead it is a {2}.",
                    this.GetType().FullName,
                    typeof(BorderlessForm).FullName,
                    this.ParentForm.GetType().FullName));
            }
        }

        private void tsSelectParent_Click(object sender, EventArgs e)
        {
            SelectParent();
        }

        private void tsUpdate_Click(object sender, EventArgs e)
        {
            PrepareForEntityUpdate();
        }

        private void tsUpdateCancel_Click(object sender, EventArgs e)
        {
            CancelEntityUpdate();
        }

        private void tsUpdateCommit_Click(object sender, EventArgs e)
        {
            UpdateEntity();
        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            AddEntity();
        }

        private void tsDelete_Click(object sender, EventArgs e)
        {
            DeleteEntity();
        }

        private void tsRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataBox(true, true, true);
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void tsExport_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void tsChildren_Click(object sender, EventArgs e)
        {
            ManageChildren();
        }

        private void tsParent_Click(object sender, EventArgs e)
        {
            ViewParent();
        }

        #endregion //Event Handlers
    }
}