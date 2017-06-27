namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    #region Using Directives

    using Figlut.Desktop.BaseUI;
    using Figlut.Desktop.DataBox.Configuration;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Winforms;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    #endregion //Using Directives

    public partial class SelectParentDataBoxForm : FiglutBaseForm
    {
        #region Inner Types

        private class ParentTableListItem
        {
            #region Constructors

            public ParentTableListItem(DatabaseTableColumn foreignKeyColumn)
            {
                _foreignKeyColumn = foreignKeyColumn;
            }

            #endregion //Constructors

            #region Fields

            private DatabaseTableColumn _foreignKeyColumn;

            #endregion //Fields

            #region Properties

            public DatabaseTableColumn ForeignKeyColumn
            {
                get { return _foreignKeyColumn; }
            }

            #endregion //Properties

            #region Methods

            public override string ToString()
            {
                return ForeignKeyColumn.ParentTableName;
            }

            #endregion //Methods
        }

        #endregion //Inner Types

        #region Constructors

        public SelectParentDataBoxForm(
            EntityOperation entityOperation,
            object entity,
            Nullable<Guid> entityId,
            FiglutDesktopDataBoxSettings settings,
            SqlDatabaseTable currentTable,
            FiglutEntityCacheUnique currentEntityCache)
        {
            InitializeComponent();
            _entityOperation = entityOperation;
            _entity = entity;
            _entityId = entityId;
            _settings = settings;
            _currentTable = currentTable;
            _currentEntityCache = currentEntityCache;
            _database = GOC.Instance.GetDatabase<SqlDatabase>();
        }

        #endregion //Constructors

        #region Fields

        private EntityOperation _entityOperation;
        private object _entity;
        private Nullable<Guid> _entityId;
        private FiglutDesktopDataBoxSettings _settings;
        private SqlDatabaseTable _currentTable;
        private FiglutEntityCacheUnique _currentEntityCache;
        private EntityCache<string, DatabaseTableColumn> _foreignKeyColumns;
        private SqlDatabase _database;

        #endregion //Fields

        #region Methods

        private void RefreshParentTables()
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
            {
                w.ChangeStatus("Refreshing parent tables ...");
                lstParentDataBoxes.Items.Clear();
                _foreignKeyColumns = _currentTable.GetForeignKeyColumns();
                foreach (DatabaseTableColumn c in _foreignKeyColumns)
                {
                    ParentTableListItem listItem = new ParentTableListItem(c);
                    foreach (object o in lstParentDataBoxes.Items)
                    {
                        if (o.ToString() == listItem.ToString())
                        {
                            continue;
                        }
                    }
                    lstParentDataBoxes.Items.Add(listItem);
                }
            }
        }

        private void ViewParent(ParentTableListItem selectedParentTableListItem, DatabaseTable parentTable)
        {
            using (ViewParentEntityForm f = new ViewParentEntityForm(parentTable, _foreignKeyColumns.ToList(), _entity, _settings))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void LoadParentTable(ParentTableListItem selectedParentTableListItem, DatabaseTable parentTable)
        {
            using (SelectParentEntityForm f = new SelectParentEntityForm(parentTable, selectedParentTableListItem.ForeignKeyColumn, _entity, _settings))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void SelectParentDataBoxForm_Load(object sender, EventArgs e)
        {
            RefreshParentTables();
        }

        private void lstParentDataBoxes_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void lstParentDataBoxes_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void lstParentDataBoxes_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            if (_foreignKeyColumns.Count < 1)
            {
                return;
            }
            if (lstParentDataBoxes.SelectedIndex < 0)
            {
                lstParentDataBoxes.Focus();
                throw new UserThrownException("No DataBox selected.", LoggingLevel.Maximum);
            }
            ParentTableListItem selectedParentTableListItem = (ParentTableListItem)lstParentDataBoxes.SelectedItem;
            DatabaseTable parentTable = _database.Tables[selectedParentTableListItem.ForeignKeyColumn.ParentTableName];
            if (_entityOperation == EntityOperation.ReadOnly)
            {
                ViewParent(selectedParentTableListItem, parentTable);
            }
            else
            {
                LoadParentTable(selectedParentTableListItem, parentTable);
            }
        }

        private void SelectParentDataBoxForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                mnuSelect.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}
