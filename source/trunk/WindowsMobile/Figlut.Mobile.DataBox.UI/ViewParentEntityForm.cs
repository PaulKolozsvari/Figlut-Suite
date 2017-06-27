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
    using Figlut.Mobile.Toolkit.Data.DB;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Web.Client.FiglutWebService;
    using Figlut.Mobile.Toolkit.Data.DB.SQLQuery;
    using Figlut.Mobile.Toolkit.Data;
    using System.Collections;
    using SystemCF.Reflection;

    #endregion //Using Directives

    public partial class ViewParentEntityForm : BaseForm
    {
        #region Constructors

        public ViewParentEntityForm(MobileDatabaseTable parentTable,
            List<MobileDatabaseTableColumn> foreignKeyColumns,
            object entity,
            FiglutMobileDataBoxSettings settings)
        {
            InitializeComponent();
            _parentTable = parentTable;
            _foreignKeyColumns = foreignKeyColumns;
            _entity = entity;
            _settings = settings;
        }

        #endregion //Constructors

        #region Fields

        private MobileDatabaseTable _parentTable;
        private List<MobileDatabaseTableColumn> _foreignKeyColumns;
        private object _entity;
        private FiglutEntityCacheUnique _currentEntityCache;
        private int _entityIdColumnIndex;
        private List<string> _hiddenProperties;
        private object _parentEntity;
        private FiglutMobileDataBoxSettings _settings;

        #endregion //Fields

        #region Methods

        private Dictionary<string, object> GetParentTableKeysAndValues()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach(MobileDatabaseTableColumn foreignKeyColumn in _foreignKeyColumns.Where(p => p.ParentTableName == _parentTable.TableName)) //Foreign keys of the child table mapped to the parent table.
            {
                object childForeignKeyValue = EntityReader.GetPropertyValue(foreignKeyColumn.ColumnName, _entity, false);
                result.Add(foreignKeyColumn.ParentTablePrimaryKeyName, childForeignKeyValue);
            }
            return result;
        }

        private void RefreshParentFromServer()
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
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

        private void RefreshParent(bool handleExceptions)
        {
            try
            {
                RefreshParentFromServer();
                lblParentTableName.Text = string.Format("{0}:", _parentTable.TableName);
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
                    if(value == null)
                    {
                        value = string.Empty;
                    }
                    result.AppendLine(string.Format("{0}: {1}", p, value));
                }
                txtInfo.Text = result.ToString();
            }
            catch (Exception ex)
            {
                if (!handleExceptions)
                {
                    throw ex;
                }
                ExceptionHandler.HandleException(ex, true, true, this, ViewParentForm_KeyDown);
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void ViewParentForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "View parent entity.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    txtInfo.Height += 50;
                }
                this.BeginInvoke(new Action<bool>(RefreshParent), true);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ViewParentForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, ViewParentForm_KeyDown);
            }
        }

        private void ViewParentForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ViewParentForm_KeyDown);
            }
        } 

        #endregion //Event Handlers
    }
}