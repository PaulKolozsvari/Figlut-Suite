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
    using Figlut.Mobile.Toolkit.Data.DB;
    using Figlut.Mobile.Toolkit.Data;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.Toolkit.Web.Client.FiglutWebService;
    using System.Collections;
    using System.Diagnostics;
    using Figlut.Mobile.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public partial class SelectParentEntityForm : BaseForm
    {
        #region Constructors

        public SelectParentEntityForm(
            MobileDatabaseTable parentTable,
            MobileDatabaseTableColumn foreignKeyColumn,
            object entity,
            FiglutMobileDataBoxSettings settings)
        {
            InitializeComponent();
            _parentTable = parentTable;
            _foreignKeyColumn = foreignKeyColumn;
            _entity = entity;
            _settings = settings;
        }

        #endregion //Constructors

        #region Fields

        private MobileDatabaseTable _parentTable;
        private MobileDatabaseTableColumn _foreignKeyColumn;
        private object _entity;
        private FiglutEntityCacheUnique _currentEntityCache;
        private int _entityIdColumnIndex;
        private List<string> _hiddenProperties;
        private FiglutMobileDataBoxSettings _settings;

        #endregion //Fields

        #region Methods

        private void RefreshParentTableDataFromServer()
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
            {
                w.ChangeStatus("Refreshing from server ...");
                FiglutWebServiceFilterString filterString = new FiglutWebServiceFilterString(_parentTable.TableName, null);
                string rawOutput = null;
                IList result = DataHelper.GetListOfType(_parentTable.MappedType);
                result = (IList)GOC.Instance.FiglutWebServiceClient.Query(result.GetType(), filterString, out rawOutput, true);
                _currentEntityCache = new FiglutEntityCacheUnique(_parentTable.MappedType);
                _currentEntityCache.OverrideFromList(result);
            }
        }

        private void RefreshParentTableData(bool fromServer, bool handleExceptions)
        {
            try
            {
                if (fromServer)
                {
                    RefreshParentTableDataFromServer();
                }
                int selectedRowIndex = -1;
                if (grdData.CurrentRowIndex > 0)
                {
                    selectedRowIndex = grdData.CurrentRowIndex;
                }
                List<Type> hiddenTypes = null;
                DataTable dataSourceTable = null;
                using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
                {
                    w.ChangeStatus("Refreshing grid ...");
                    dataSourceTable = _currentEntityCache.GetDataTable(null, false, _settings.ShapeColumnNames, _settings.EntityIdColumnName);
                    grdData.DataSource = dataSourceTable;
                }
                _entityIdColumnIndex = DataHelper.GetColumnIndex(dataSourceTable, _settings.EntityIdColumnName);
                hiddenTypes = _settings.GetHiddenTypes();
                if (_hiddenProperties == null)
                {
                    _hiddenProperties = new List<string>();
                }
                _hiddenProperties.Clear();
                if (_settings.HideEntityIdColumn)
                {
                    _hiddenProperties.Add(_settings.EntityIdColumnName);
                }
                EntityReader.GetPropertyNamesByType(
                    _parentTable.MappedType,
                    hiddenTypes,
                    _settings.ShapeColumnNames).ForEach(p => _hiddenProperties.Add(p));

                DataGridTableStyle tableStyle = UIHelper.GetDataGridTableStyle(
                    _parentTable.MappedType,
                    _settings.DataBoxColumnWidth,
                    _hiddenProperties,
                    _settings.ShapeColumnNames);
                grdData.TableStyles.Clear();
                grdData.TableStyles.Add(tableStyle);
                grdData.Refresh();
                if (selectedRowIndex < dataSourceTable.Rows.Count && selectedRowIndex > -1)
                {
                    grdData.CurrentRowIndex = selectedRowIndex;
                }
                grdData.Focus();
            }
            catch (Exception ex)
            {
                if (!handleExceptions)
                {
                    throw ex;
                }
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentForm_KeyDown);
            }
        }

        private void ApplyParent()
        {
            if (grdData.CurrentRowIndex < 0)
            {
                grdData.Focus();
                throw new UserThrownException("No row selected.", LoggingLevel.Maximum);
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedGridRowCellValue<Nullable<Guid>>(grdData, _entityIdColumnIndex);
            Debug.Assert(selectedEntityId.HasValue);
            object selectedParentEntity = _currentEntityCache[selectedEntityId.Value];
            object parentForeignKeyValue = EntityReader.GetPropertyValue(_foreignKeyColumn.ColumnName, selectedParentEntity, false);
            EntityReader.SetPropertyValue(_foreignKeyColumn.ColumnName, _entity, parentForeignKeyValue);

            this.DialogResult = DialogResult.OK;
            Close();
        }

        #endregion //Methods

        #region Event Handlers

        private void SelectParentForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Select parent entity.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    grdData.Height += 50;
                }
                this.BeginInvoke(new Action<bool, bool>(RefreshParentTableData), true, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentForm_KeyDown);
            }
        }

        private void mnuApply_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyParent();   
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentForm_KeyDown);
            }
        }

        private void SelectParentForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    mnuApply_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}