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
    using Figlut.Mobile.Toolkit.Data;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;
    using Figlut.Mobile.Toolkit.Data.DB;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public partial class SelectParentDataBoxForm : BaseForm
    {
        #region Inner Types

        private class ParentTableListItem
        {
            #region Constructors

            public ParentTableListItem(MobileDatabaseTableColumn foreignKeyColumn)
            {
                _foreignKeyColumn = foreignKeyColumn;
            }

            #endregion //Constructors

            #region Fields

            private MobileDatabaseTableColumn _foreignKeyColumn;

            #endregion //Fields

            #region Properties

            public MobileDatabaseTableColumn ForeignKeyColumn
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
            FiglutMobileDataBoxSettings settings,
            SqlCeDatabaseTable currentTable,
            FiglutEntityCacheUnique currentEntityCache)
        {
            InitializeComponent();
            _entityOperation = entityOperation;
            _entity = entity;
            _entityId = entityId;
            _settings = settings;
            _currentTable = currentTable;
            _currentEntityCache = currentEntityCache;
            _database = GOC.Instance.GetDatabase<SqlCeDatabase>();
        }

        #endregion //Constructors

        #region Fields

        private EntityOperation _entityOperation;
        private object _entity;
        private Nullable<Guid> _entityId;
        private FiglutMobileDataBoxSettings _settings;
        private SqlCeDatabaseTable _currentTable;
        private FiglutEntityCacheUnique _currentEntityCache;
        private EntityCache<string, MobileDatabaseTableColumn> _foreignKeyColumns;
        private SqlCeDatabase _database;

        #endregion //Fields

        #region Methods

        private void RefreshParentTables()
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
            {
                w.ChangeStatus("Refreshing parent tables ...");
                lstParentDataBoxes.Items.Clear();
                _foreignKeyColumns = _currentTable.GetForeignKeyColumns();
                foreach (MobileDatabaseTableColumn c in _foreignKeyColumns)
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

        private void ViewParent(ParentTableListItem selectedParentTableListItem, MobileDatabaseTable parentTable)
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

        private void LoadParentTable(ParentTableListItem selectedParentTableListItem, MobileDatabaseTable parentTable)
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

        private void SelectParentTableForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Select parent DataBox.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    lstParentDataBoxes.Height += 50;
                }
                RefreshParentTables();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentTableForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentTableForm_KeyDown);
            }
        }

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            try
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
                MobileDatabaseTable parentTable = _database.Tables[selectedParentTableListItem.ForeignKeyColumn.ParentTableName];
                if (_entityOperation == EntityOperation.ReadOnly)
                {
                    ViewParent(selectedParentTableListItem, parentTable);
                }
                else
                {
                    LoadParentTable(selectedParentTableListItem, parentTable);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentTableForm_KeyDown);
            }
        }

        private void SelectParentTableForm_KeyDown(object sender, KeyEventArgs e)
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
                ExceptionHandler.HandleException(ex, true, true, this, SelectParentTableForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}