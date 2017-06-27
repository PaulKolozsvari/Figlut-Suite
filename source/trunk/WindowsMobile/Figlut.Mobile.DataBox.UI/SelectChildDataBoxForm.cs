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
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;
    using Figlut.Mobile.Toolkit.Data;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.Toolkit.Data.DB;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public partial class SelectChildDataBoxForm : BaseForm
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
            FiglutMobileDataBoxSettings settings,
            SqlCeDatabaseTable currentTable)
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
        private FiglutMobileDataBoxSettings _settings;
        private SqlCeDatabaseTable _currentTable;

        #endregion //Fields

        #region Methods

        private void RefreshChildrenTables()
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
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
            MobileDatabaseTable childTable = GOC.Instance.GetDatabase<SqlCeDatabase>().Tables[selectedChildTableName];
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

        private void SelectChildTableForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Select child DataBox.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    lstChildrenDataBoxes.Height += 50;
                }
                RefreshChildrenTables();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, SelectChildTableForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, SelectChildTableForm_KeyDown);
            }
        }

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            try
            {
                LoadChildren();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, SelectChildTableForm_KeyDown);
            }
        }

        private void SelectChildTableForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    mnuSelect_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, SelectChildTableForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}