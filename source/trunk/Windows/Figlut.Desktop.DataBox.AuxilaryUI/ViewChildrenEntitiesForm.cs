namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    #region Using Directives

    using Figlut.Desktop.BaseUI;
    using Figlut.Desktop.DataBox.Configuration;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    #endregion //Using Directives

    public partial class ViewChildrenEntitiesForm : FiglutBaseForm
    {
        #region Constructors

        public ViewChildrenEntitiesForm(
            DatabaseTable childTable,
            EntityCache<string, ForeignKeyInfo> foreignKeys,
            object entity,
            FiglutDesktopDataBoxSettings settings)
        {
            InitializeComponent();
            _childTable = childTable;
            _foreignKeys = foreignKeys;
            _entity = entity;
            _settings = settings;
        }

        #endregion //Constructors


        #region Fields

        private DatabaseTable _childTable;
        private EntityCache<string, ForeignKeyInfo> _foreignKeys;
        private object _entity;
        private FiglutEntityCacheUnique _currentEntityCache;
        private int _entityIdColumnIndex;
        private List<string> _hiddenProperties;
        private FiglutDesktopDataBoxSettings _settings;

        #endregion //Fields

        #region Methods

        private void RefreshChildrenTableDataFromServer()
        {
            using (WaitProcess w = new WaitProcess(this))
            {
                w.ChangeStatus("Refreshing from server ...");
                List<WhereClauseColumn> filters = new List<WhereClauseColumn>();
                int i = 0;
                foreach (ForeignKeyInfo f in _foreignKeys)
                {
                    object primaryKeyValue = EntityReader.GetPropertyValue(f.ParentTablePrimaryKeyName, _entity, false);
                    WhereClauseLogicalOperator logicalOperator = null;
                    if (i < (_foreignKeys.Count - 1))
                    {
                        logicalOperator = new WhereClauseLogicalOperator(LogicalOperator.AND);
                    }
                    filters.Add(new WhereClauseColumn(
                        f.ChildTableForeignKeyName,
                        new WhereClauseComparisonOperator(ComparisonOperator.EQUALS),
                        primaryKeyValue,
                        false,
                        false,
                        logicalOperator));
                    i++;
                }
                FiglutWebServiceFilterString filterString = new FiglutWebServiceFilterString(_childTable.TableName, filters);
                string rawOutput = null;
                IList result = DataHelper.GetListOfType(_childTable.MappedType);
                result = (IList)GOC.Instance.FiglutWebServiceClient.Query(result.GetType(), filterString, out rawOutput, true);
                _currentEntityCache = new FiglutEntityCacheUnique(_childTable.MappedType);
                _currentEntityCache.OverrideFromList(result);
            }
        }

        private void RefreshChildrenTableData(bool fromServer)
        {
            if (fromServer)
            {
                RefreshChildrenTableDataFromServer();
            }
            Status = string.Format("{0}s", DataShaper.ShapeCamelCaseString(_childTable.TableName));
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
                _childTable.MappedType,
                hiddenTypes,
                _settings.ShapeColumnNames).ForEach(p => _hiddenProperties.Add(p));

            _hiddenProperties.ForEach(c => grdData.Columns[c].Visible = false);
            grdData.Refresh();
            if (selectedRowIndex < grdData.Rows.Count && selectedRowIndex > -1)
            {
                grdData.Rows[selectedRowIndex].Selected = true;
            }
            grdData.Refresh();
            if (selectedRowIndex < dataSourceTable.Rows.Count && selectedRowIndex > -1)
            {
                grdData.Rows[selectedRowIndex].Selected = true;
            }
            grdData.Focus();
        }

        #endregion //Methods

        #region Event Handlers

        private void ViewChildrenEntitiesForm_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action<bool>(RefreshChildrenTableData), true);
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ViewChildrenEntitiesForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void ViewChildrenEntitiesForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void ViewChildrenEntitiesForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void ViewChildrenEntitiesForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}
