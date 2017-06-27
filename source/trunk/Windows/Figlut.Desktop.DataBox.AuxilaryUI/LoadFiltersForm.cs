namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.BaseUI;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;

    #endregion //Using Directives

    public partial class LoadFiltersForm : FiglutBaseForm
    {
        #region Constructors

        public LoadFiltersForm(
            SqlDatabaseTable table, 
            List<string> hiddenProperties,
            List<string> unmanagedProperties,
            bool shapeColumnNames,
            bool treatZeroAsNull)
        {
            InitializeComponent();
            _table = table;
            _filters = new List<WhereClauseColumn>();
            _hiddenProperties = hiddenProperties;
            _unmanagedProperties = unmanagedProperties;
            _shapeColumnNames = shapeColumnNames;
            _treatZeroAsNull = treatZeroAsNull;
        }

        #endregion //Constructors

        #region Fields

        private SqlDatabaseTable _table;
        private List<WhereClauseColumn> _filters;
        private FiglutWebServiceFilterString _filterString;
        private List<string> _hiddenProperties;
        private List<string> _unmanagedProperties;
        private bool _shapeColumnNames;
        private bool _treatZeroAsNull;

        #endregion //Fields

        #region Properties

        public FiglutWebServiceFilterString FilterString
        {
            get { return _filterString; }
        }

        #endregion //Properties

        #region Methods

        private void RefreshWhereClauseColumns()
        {
            using (WaitProcess w = new WaitProcess(this))
            {
                w.ChangeStatus("Refreshing list ...");
                lstFilters.BeginUpdate();
                lstFilters.Items.Clear();
                _filters.ToList().ForEach(p => lstFilters.Items.Add(p));
                lstFilters.EndUpdate();
                if (lstFilters.Items.Count > 0)
                {
                    lstFilters.SelectedIndex = 0;
                }
            }
            _filterString = new FiglutWebServiceFilterString(_table.TableName, _filters);
            Status = _filterString.ToString();
        }

        #endregion //Methods

        #region Event Handlers

        private void LoadFiltersForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void LoadFiltersForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void LoadFiltersForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void LoadFiltersForm_Load(object sender, EventArgs e)
        {
            RefreshWhereClauseColumns();
        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            if (_filters.Count > 0 && _filters[0].LogicalOperatorAgainstNextColumn == null)
            {
                throw new UserThrownException(
                    "Last filter in the list does not have a logical operator to link to a new filter which you want to add. Each filter must be linked with a logical operator. Update the last filter and apply a logical operator to it first.",
                    LoggingLevel.None);
            }
            WhereClauseColumn whereClauseColumn = null;
            using (ManageFilterForm f = new ManageFilterForm(
                _table,
                _hiddenProperties,
                _unmanagedProperties,
                _shapeColumnNames,
                _treatZeroAsNull,
                null))
            {
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                whereClauseColumn = f.WhereClauseColumn;
            }
            _filters.Add(whereClauseColumn);
            RefreshWhereClauseColumns();
        }

        private void tsUpdate_Click(object sender, EventArgs e)
        {
            if (lstFilters.SelectedIndex < 0)
            {
                throw new UserThrownException("No filter selected.", LoggingLevel.None);
            }
            WhereClauseColumn w = (WhereClauseColumn)lstFilters.SelectedItem;
            using (ManageFilterForm f = new ManageFilterForm(
                _table,
                _hiddenProperties,
                _unmanagedProperties,
                _shapeColumnNames,
                _treatZeroAsNull,
                w))
            {
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                EntityReader.CopyProperties(f.WhereClauseColumn, w, false);
            }
            RefreshWhereClauseColumns();
        }

        private void tsDelete_Click(object sender, EventArgs e)
        {
            if (lstFilters.SelectedIndex < 0)
            {
                throw new UserThrownException("No filter selected.", LoggingLevel.None);
            }
            WhereClauseColumn w = (WhereClauseColumn)lstFilters.SelectedItem;
            _filters.Remove(w);
            RefreshWhereClauseColumns();
        }

        private void mnuLoad_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void LoadFiltersForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                mnuLoad.PerformClick();
            }
            else if ((e.KeyCode == Keys.A) & e.Control & e.Shift)
            {
                tsAdd.PerformClick();
            }
            else if ((e.KeyCode == Keys.U) & e.Control & e.Shift)
            {
                tsUpdate.PerformClick();
            }
            else if ((e.KeyCode == Keys.D) & e.Control & e.Shift)
            {
                tsDelete.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}
