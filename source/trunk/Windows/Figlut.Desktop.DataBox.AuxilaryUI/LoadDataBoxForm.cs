namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.BaseUI;
    using Figlut.Desktop.DataBox.Utilities;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using System.IO;

    #endregion //Using Directives

    public partial class LoadDataBoxForm : FiglutBaseForm
    {
        #region Constructors

        public LoadDataBoxForm(string databaseSchemaFileName)
        {
            InitializeComponent();
            _databaseSchemaFileName = databaseSchemaFileName;
        }

        #endregion //Constructors

        #region Fields

        private SqlDatabaseTable _selectedTable;
        private string _databaseSchemaFileName;
        private bool _applyFilters;

        #endregion //Fields

        #region Properties

        public SqlDatabaseTable SelectedTable
        {
            get { return _selectedTable; }
        }

        public bool ApplyFilters
        {
            get { return _applyFilters; }
        }

        #endregion //Properties

        #region Methods

        private Dictionary<string, object> GetSearchProperties()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add(EntityReader<SqlDatabaseTable>.GetPropertyName(p => p.TableName, false), txtDataBoxTable.Text);
            return result;
        }

        private void RefreshTables(bool fromServer)
        {
            using (WaitProcess w = new WaitProcess(mnuMain, this))
            {
                if (fromServer)
                {
                    w.ChangeStatus("Acquiring schema from server ...");
                    FiglutDataBoxApplication.Instance.AcquireSchema(true, Path.Combine(Information.GetExecutingDirectory(), _databaseSchemaFileName));
                }
                w.ChangeStatus("Refreshing tables ...");
                SqlDatabase db = GOC.Instance.GetDatabase<SqlDatabase>();
                lstDataBoxTable.Items.Clear();
                db.Tables.GetEntitiesByProperties(GetSearchProperties(), false).OrderBy(p => p.TableName).ToList().ForEach(p => lstDataBoxTable.Items.Add(p));
                if (lstDataBoxTable.Items.Count > 0)
                {
                    lstDataBoxTable.SelectedIndex = 0;
                }
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void LoadDataBoxForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void LoadDataBoxForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void LoadDataBoxForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void LoadDataBoxForm_Load(object sender, EventArgs e)
        {
            RefreshTables(false);
        }

        private void txtDataBoxTable_TextChanged(object sender, EventArgs e)
        {
            RefreshTables(false);
        }

        private void mnuLoad_Click(object sender, EventArgs e)
        {
            if (lstDataBoxTable.SelectedIndex < 0)
            {
                lstDataBoxTable.Focus();
                throw new UserThrownException("No DataBox selected.", LoggingLevel.None);
            }
            SqlDatabaseTable table = lstDataBoxTable.SelectedItem as SqlDatabaseTable;
            Debug.Assert(table != null);
            _selectedTable = table;
            _applyFilters = chkApplyFilters.Checked;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void LoadDataBoxForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                mnuLoad.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}