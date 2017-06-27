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

    public partial class ViewParentEntityForm : FiglutBaseForm
    {
        #region Constructors

        public ViewParentEntityForm(
            DatabaseTable parentTable,
            List<DatabaseTableColumn> foreignKeyColumns,
            object entity,
            FiglutDesktopDataBoxSettings settings)
        {
            InitializeComponent();
            _parentTable = parentTable;
            _foreignKeyColumns = foreignKeyColumns;
            _entity = entity;
            _settings = settings;
        }

        #endregion //Constructors

        #region Fields

        private DatabaseTable _parentTable;
        private List<DatabaseTableColumn> _foreignKeyColumns;
        private object _entity;
        private FiglutEntityCacheUnique _currentEntityCache;
        private int _entityIdColumnIndex;
        private List<string> _hiddenProperties;
        private object _parentEntity;
        private FiglutDesktopDataBoxSettings _settings;

        #endregion //Fields

        #region Methods

        private Dictionary<string, object> GetParentTableKeysAndValues()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (DatabaseTableColumn foreignKeyColumn in _foreignKeyColumns.Where(p => p.ParentTableName == _parentTable.TableName)) //Foreign keys of the child table mapped to the parent table.
            {
                object childForeignKeyValue = EntityReader.GetPropertyValue(foreignKeyColumn.ColumnName, _entity, false);
                result.Add(foreignKeyColumn.ParentTablePrimaryKeyName, childForeignKeyValue);
            }
            return result;
        }

        private void RefreshParentFromServer()
        {
            using (WaitProcess w = new WaitProcess(this))
            {
                w.ChangeStatus("Refreshing parent ...");
                Dictionary<string, object> parentTableKeysAndValues = GetParentTableKeysAndValues();
                List<WhereClauseColumn> filters = new List<WhereClauseColumn>();
                int i = 0;
                foreach (string pk in parentTableKeysAndValues.Keys)
                {
                    WhereClauseLogicalOperator logicalOperator = null;
                    if (i < (parentTableKeysAndValues.Count - 1))
                    {
                        logicalOperator = new WhereClauseLogicalOperator(LogicalOperator.AND);
                    }
                    filters.Add(new WhereClauseColumn(
                        pk,
                        new WhereClauseComparisonOperator(ComparisonOperator.EQUALS),
                        parentTableKeysAndValues[pk],
                        false,
                        false,
                        logicalOperator));
                    i++;
                }
                FiglutWebServiceFilterString filterString = new FiglutWebServiceFilterString(_parentTable.TableName, filters);
                string rawOutput = null;
                IList result = DataHelper.GetListOfType(_parentTable.MappedType);
                result = (IList)GOC.Instance.FiglutWebServiceClient.Query(result.GetType(), filterString, out rawOutput, true);
                if (result.Count < 1)
                {
                    throw new NullReferenceException(string.Format("Could not find parent for filter string {0}.", filterString.ToString()));
                }
                _parentEntity = result[0];
            }
        }

        private void RefreshParent()
        {
            RefreshParentFromServer();
            Status = string.Format("{0}", _parentTable.TableName);
            StringBuilder result = new StringBuilder();
            List<Type> hiddenTypes = _settings.GetHiddenTypes();
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
            foreach (string p in EntityReader.GetAllPropertyNames(_settings.ShapeColumnNames, _parentEntity.GetType()))
            {
                if (_hiddenProperties.Contains(p))
                {
                    continue;
                }
                object value = EntityReader.GetPropertyValue(DataShaper.RestoreStringToCamelCase(p), _parentEntity, false);
                if (value == null)
                {
                    value = string.Empty;
                }
                result.AppendLine(string.Format("{0}: {1}", p, value));
            }
            txtInfo.Text = result.ToString();
        }

        #endregion //Methods

        #region Event Handlers

        private void ViewParentEntityForm_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(RefreshParent));
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ViewParentEntityForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void ViewParentEntityForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void ViewParentEntityForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void ViewParentEntityForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}
