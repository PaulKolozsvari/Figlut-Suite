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
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.DataBox.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using System.Diagnostics;
    using Figlut.Mobile.DataBox.Configuration;
    using System.IO;

    #endregion //Using Directives

    public partial class LoadDataBoxForm : BaseForm
    {
        #region Constructors

        public LoadDataBoxForm()
        {
            InitializeComponent();
        }

        #endregion //Constructors

        #region Fields

        private SqlCeDatabaseTable _selectedTable;

        #endregion //Fields

        #region Properties

        public SqlCeDatabaseTable SelectedTable
        {
            get { return _selectedTable; }
        }

        #endregion //Properties

        #region Methods

        private Dictionary<string, object> GetSearchProperties()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add(EntityReader<SqlCeDatabaseTable>.GetPropertyName(p => p.TableName, false), txtDataBoxTable.Text);
            return result;
        }

        private void RefreshTables(bool fromServer, bool handleException)
        {
            try
            {
                using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
                {
                    if (fromServer)
                    {
                        w.ChangeStatus("Acquiring schema from server ...");
                        FiglutDataBoxApplication.Instance.AcquireSchema(
                            true,
                            Path.Combine(Information.GetExecutingDirectory(), GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().DatabaseSchemaFileName));
                    }
                    w.ChangeStatus("Refreshing tables ...");
                    SqlCeDatabase db = GOC.Instance.GetDatabase<SqlCeDatabase>();
                    lstDataBoxTable.Items.Clear();
                    db.Tables.GetEntitiesByProperties(GetSearchProperties(), false).OrderBy(p => p.TableName).ToList().ForEach(p => lstDataBoxTable.Items.Add(p));
                    if (lstDataBoxTable.Items.Count > 0)
                    {
                        lstDataBoxTable.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!handleException)
                {
                    throw ex;
                }
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void LoadDataBoxForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Select DataBox to load.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    lstDataBoxTable.Height += 50;
                }
                this.BeginInvoke(new Action<bool, bool>(RefreshTables), false, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, LoadDataBoxForm_KeyDown);
            }
        }

        private void txtDataBoxTable_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshTables(false, false);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, LoadDataBoxForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, LoadDataBoxForm_KeyDown);
            }
        }

        private void mnuLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstDataBoxTable.SelectedIndex < 0)
                {
                    lstDataBoxTable.Focus();
                    UIHelper.DisplayError("No DataBox selected.", this, LoadDataBoxForm_KeyDown);
                    return;
                }
                SqlCeDatabaseTable table = lstDataBoxTable.SelectedItem as SqlCeDatabaseTable;
                Debug.Assert(table != null);
                _selectedTable = table;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, LoadDataBoxForm_KeyDown);
            }
        }

        private void LoadDataBoxForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    mnuLoad_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, LoadDataBoxForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}