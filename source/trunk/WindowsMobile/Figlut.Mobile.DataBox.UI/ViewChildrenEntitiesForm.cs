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
using Figlut.Mobile.Toolkit.Data;
using Figlut.Mobile.Toolkit.Data.DB;
    using Figlut.Mobile.Toolkit.Web.Client.FiglutWebService;
    using Figlut.Mobile.Toolkit.Data.DB.SQLQuery;
    using System.Collections;
using Figlut.Mobile.DataBox.Configuration;

    #endregion //Using Directives

    public partial class ViewChildrenEntitiesForm : BaseForm
    {
        #region Constructors

        public ViewChildrenEntitiesForm(
            MobileDatabaseTable childTable,
            EntityCache<string, ForeignKeyInfo> foreignKeys,
            object entity,
            FiglutMobileDataBoxSettings settings)
        {
            InitializeComponent();
            _childTable = childTable;
            _foreignKeys = foreignKeys;
            _entity = entity;
            _settings = settings;
        }

        #endregion //Constructors

        #region Fields

        private MobileDatabaseTable _childTable;
        private EntityCache<string, ForeignKeyInfo> _foreignKeys;
        private object _entity;
        private FiglutEntityCacheUnique _currentEntityCache;
        private int _entityIdColumnIndex;
        private List<string> _hiddenProperties;
        private FiglutMobileDataBoxSettings _settings;

        #endregion //Fields

        #region Methods

        private void RefreshChildrenTableDataFromServer()
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
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

        private void RefreshChildrenTableData(bool fromServer, bool handleExceptions)
        {
            try
            {
                if (fromServer)
                {
                    RefreshChildrenTableDataFromServer();
                }
                lblChildTableName.Text = string.Format("{0}s:", DataShaper.ShapeCamelCaseString(_childTable.TableName));
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
                    _childTable.MappedType, 
                    hiddenTypes, 
                    _settings.ShapeColumnNames).ForEach(p => _hiddenProperties.Add(p));
                DataGridTableStyle tableStyle = UIHelper.GetDataGridTableStyle(
                    _childTable.MappedType,
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
                ExceptionHandler.HandleException(ex, true, true, this, ViewChildrenForm_KeyDown);
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void ViewChildrenForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "View children entities.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    grdData.Height += 50;
                }
                this.BeginInvoke(new Action<bool, bool>(RefreshChildrenTableData), true, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ViewChildrenForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, ViewChildrenForm_KeyDown);
            }
        }

        private void mnuApply_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ViewChildrenForm_KeyDown);
            }
        }

        private void ViewChildrenForm_KeyDown(object sender, KeyEventArgs e)
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
                ExceptionHandler.HandleException(ex, true, true, this, ViewChildrenForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}