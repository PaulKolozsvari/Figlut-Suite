namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    #region Using Directives

    using Figlut.Desktop.BaseUI;
    using Figlut.Desktop.DataBox.Configuration;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    #endregion //Using Directives

    public partial class SelectParentEntityForm : FiglutBaseForm
    {
        #region Constructors

        public SelectParentEntityForm(
            DatabaseTable parentTable,
            DatabaseTableColumn foreignKeyColumn,
            object entity,
            FiglutDesktopDataBoxSettings settings)
        {
            InitializeComponent();
            _parentTable = parentTable;
            _foreignKeyColumn = foreignKeyColumn;
            _entity = entity;
            _settings = settings;
        } 

        #endregion //Constructors

        #region Fields

        private DatabaseTable _parentTable;
        private DatabaseTableColumn _foreignKeyColumn;
        private object _entity;
        private FiglutEntityCacheUnique _currentEntityCache;
        private int _entityIdColumnIndex;
        private List<string> _hiddenProperties;
        private FiglutDesktopDataBoxSettings _settings;

        #endregion //Fields

        #region Methods

        private void RefreshParentTableDataFromServer()
        {
            using (WaitProcess w = new WaitProcess(this))
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

        private void RefreshParentTableData(bool fromServer)
        {
            if (fromServer)
            {
                RefreshParentTableDataFromServer();
            }
            int selectedRowIndex = -1;
            if (grdData.SelectedRows.Count > 0)
            {
                selectedRowIndex = grdData.SelectedRows[0].Index;
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

            _hiddenProperties.ForEach(c => grdData.Columns[c].Visible = false);
            grdData.Refresh();
            if (selectedRowIndex < grdData.Rows.Count && selectedRowIndex > -1)
            {
                grdData.Rows[selectedRowIndex].Selected = true;
            }
            grdData.Focus();
        }

        private void ApplyParent()
        {
            if (grdData.SelectedRows.Count < 1)
            {
                grdData.Focus();
                throw new UserThrownException("No row selected.", LoggingLevel.Maximum);
            }
            Nullable<Guid> selectedEntityId = UIHelper.GetSelectedDataGridViewRowCellValue<Nullable<Guid>>(grdData, _settings.EntityIdColumnName);
            Debug.Assert(selectedEntityId.HasValue);
            object selectedParentEntity = _currentEntityCache[selectedEntityId.Value];
            object parentForeignKeyValue = EntityReader.GetPropertyValue(_foreignKeyColumn.ColumnName, selectedParentEntity, false);
            EntityReader.SetPropertyValue(_foreignKeyColumn.ColumnName, _entity, parentForeignKeyValue);

            this.DialogResult = DialogResult.OK;
            Close();
        }

        #endregion //Methods

        #region Event Handlers

        private void SelectParentEntityForm_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action<bool>(RefreshParentTableData), true);
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void mnuApply_Click(object sender, EventArgs e)
        {
            ApplyParent();
        }

        private void SelectParentEntityForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void SelectParentEntityForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void SelectParentEntityForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void SelectParentEntityForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
            else if ((e.KeyCode == Keys.Enter) && e.Control & e.Shift)
            {
                mnuApply.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}
