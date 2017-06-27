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

    public partial class SelectChildDataBoxForm : FiglutBaseForm
    {
        #region Inner Types

        private class ChildTableListItem
        {
            #region Constructors

            public ChildTableListItem(
                string tableName,
                EntityCache<string, ForeignKeyInfo> foreignKeys)
            {
                _tableName = tableName;
                _foreignKeys = foreignKeys;
            }

            #endregion //Constructors

            #region Fields

            private string _tableName;
            private EntityCache<string, ForeignKeyInfo> _foreignKeys;

            #endregion //Fields

            #region Properties

            public string TableName
            {
                get { return _tableName; }
            }

            public EntityCache<string, ForeignKeyInfo> ForeignKeys
            {
                get { return _foreignKeys; }
            }

            #endregion //Properties

            #region Methods

            public override string ToString()
            {
                return _tableName;
            }

            #endregion //Methods
        }

        #endregion //Inner Types

        #region Constructors

        public SelectChildDataBoxForm(
            object entity,
            Nullable<Guid> entityId,
            FiglutDesktopDataBoxSettings settings,
            SqlDatabaseTable currentTable)
        {
            InitializeComponent();
            _entity = entity;
            _entityId = entityId;
            _settings = settings;
            _currentTable = currentTable;
        }

        #endregion //Constructors

        #region Fields

        private object _entity;
        private Nullable<Guid> _entityId;
        private FiglutDesktopDataBoxSettings _settings;
        private SqlDatabaseTable _currentTable;

        #endregion //Fields

        #region Methods

        private void RefreshChildrenTables()
        {
            using (WaitProcess w = new WaitProcess(this))
            {
                w.ChangeStatus("Refreshing children tables ...");
                lstChildrenDataBoxes.Items.Clear();
                foreach (string tableName in _currentTable.ChildrenTables.GetEntitiesKeys())
                {
                    lstChildrenDataBoxes.Items.Add(new ChildTableListItem(tableName, _currentTable.ChildrenTables[tableName]));
                }
            }
        }

        private void LoadChildren()
        {
            if (lstChildrenDataBoxes.SelectedIndex < 0)
            {
                throw new UserThrownException("No DataBox selected.", LoggingLevel.Maximum);
            }
            string selectedChildTableName = lstChildrenDataBoxes.SelectedItem.ToString();
            EntityCache<string, ForeignKeyInfo> foreignKeys = _currentTable.ChildrenTables[selectedChildTableName];
            DatabaseTable childTable = GOC.Instance.GetDatabase<SqlDatabase>().Tables[selectedChildTableName];
            using (ViewChildrenEntitiesForm f = new ViewChildrenEntitiesForm(
                childTable,
                foreignKeys,
                _entity,
                _settings))
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

        private void SelectChildDataBoxForm_Load(object sender, EventArgs e)
        {
            RefreshChildrenTables();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            LoadChildren();
        }

        private void SelectChildDataBoxForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void SelectChildDataBoxForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void SelectChildDataBoxForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void SelectChildDataBoxForm_KeyUp(object sender, KeyEventArgs e)
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
