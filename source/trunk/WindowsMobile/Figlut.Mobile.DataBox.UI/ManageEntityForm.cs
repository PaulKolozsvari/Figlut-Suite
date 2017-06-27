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
    using Figlut.Mobile.Toolkit.Extensions.DataBox.Events.Crud;
    using Figlut.Mobile.DataBox.Utilities;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.Toolkit.WM.Data.DB.SQLCE;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;
    using System.Reflection;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using Figlut.Mobile.Toolkit.Data;
    using Figlut.Mobile.Toolkit.Data.DB;
    using Figlut.Mobile.Toolkit.Tools;

    #endregion //Using Directives

    public partial class ManageEntityForm : BaseForm
    {
        #region Constructors

        public ManageEntityForm(
            EntityOperation entityOperation,
            object entity,
            Nullable<Guid> entityId,
            Dictionary<string, Control> inputControls, 
            Control firstControl,
            Control controlAfterAdd,
            List<LinkLabel> resetLinks,
            List<string> hiddenProperties,
            List<string> extensionManagedProperties,
            FiglutMobileDataBoxSettings settings,
            SqlCeDatabaseTable currentTable,
            FiglutEntityCacheUnique currentEntityCache)
        {
            InitializeComponent();
            _entityOperation = entityOperation;
            _entityUnderUpdate = entity;
            _entityId = entityId;
            _inputControls = inputControls;
            _firstControl = firstControl;
            _controlAfterAdd = controlAfterAdd;
            _resetLinks = resetLinks;
            _hiddenProperties = hiddenProperties;
            _extensionManagedProperties = extensionManagedProperties;
            _settings = settings;
            _currentTable = currentTable;
            _currentEntityCache = currentEntityCache;
        }

        #endregion //Constructors

        #region Fields

        private EntityOperation _entityOperation;
        private object _entityUnderUpdate;
        private Nullable<Guid> _entityId;
        private Dictionary<string, Control> _inputControls;
        private Control _firstControl;
        private Control _firstInputControl;
        private Control _controlAfterAdd;
        private Control _inputControlAfterAdd;
        private List<LinkLabel> _resetLinks;
        private List<string> _hiddenProperties;
        private List<string> _extensionManagedProperties;
        private FiglutMobileDataBoxSettings _settings;
        private SqlCeDatabaseTable _currentTable;
        private FiglutEntityCacheUnique _currentEntityCache;
        private bool _unsavedChanges;

        #endregion //Fields

        #region Properties

        public bool ChangesMade
        {
            get { return _unsavedChanges; }
        }

        #endregion //Properties

        #region Methods

        private void SubscribeToResetLinksClickEvents()
        {
            foreach (LinkLabel lnkReset in _resetLinks)
            {
                lnkReset.Click += new EventHandler(lnkReset_Click);
            }
        }

        public void UnsubscribeFromResetLinksClickEvents()
        {
            foreach (LinkLabel lnkReset in _resetLinks)
            {
                lnkReset.Click -= new EventHandler(lnkReset_Click);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            ClearInputControls();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void RefreshInputControls()
        {
            BeforeAddInputControlsArgs eBeforeAddInputControls = new BeforeAddInputControlsArgs(this._inputControls, true);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeAddInputControls(eBeforeAddInputControls);
            if (eBeforeAddInputControls.Cancel) return;

            pnlInputControls.Controls.Clear();
            int tabIndex = _inputControls.Count - 1;
            foreach (Control control in _inputControls.Values)
            {
                control.TabIndex = tabIndex;
                pnlInputControls.Controls.Add(control);
                tabIndex--;
            }
            pnlInputControls.Refresh();
            _firstInputControl = UIHelper.ExtractInputControlFromInputPanel((Panel)_firstControl);
            _firstInputControl.Focus();
            if (_controlAfterAdd != null)
            {
                _inputControlAfterAdd = UIHelper.ExtractInputControlFromInputPanel((Panel)_controlAfterAdd);
            }
            AfterAddInputControlsArgs eAfterAddInputControls = new AfterAddInputControlsArgs(_inputControls, true, _firstControl);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterAddInputControls(eAfterAddInputControls);
            lnkSelectParent.Visible = _currentTable.GetForeignKeyColumns().Count > 0;
        }

        private void ClearInputControls()
        {
            UnsubscribeFromResetLinksClickEvents();
            _inputControls.Values.ToList().ForEach(c => pnlInputControls.Controls.Remove(c));
            UIHelper.ClearControls(_inputControls);
        }

        public void UpdateEntity()
        {
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(_entityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeUpdate(eBefore);
            if (eBefore.Cancel) return;
            if (!eBefore.CancelDefaultBindingBehaviour)
            {
                UIHelper.PopulateEntityFromControls(
                    _inputControls,
                    _entityUnderUpdate,
                    _hiddenProperties,
                    _extensionManagedProperties,
                    _settings.ShapeColumnNames,
                    _settings.TreatZeroAsNull);
                _unsavedChanges = true;
            }
            Type entityType = _entityUnderUpdate.GetType();
            foreach (SqlCeDatabaseTableColumn column in _currentTable.Columns) //Check if all properties, that are not nullable, have been set.
            {
                PropertyInfo p = entityType.GetProperty(column.ColumnName);
                object value = p.GetValue(_entityUnderUpdate, null);
                if ((value == null ||
                    string.IsNullOrEmpty(value.ToString()) ||
                    (p.PropertyType == typeof(Guid) && ((Guid)value) == Guid.Empty) ||
                    (_settings.TreatZeroAsNull && value.ToString() == "0")) &&
                    !column.IsNullable)
                {
                    if (!column.IsForeignKey)
                    {
                        string inputPanelName = string.Format("pnl{0}", column.ColumnName);
                        UIHelper.ExtractInputControlFromInputPanel((Panel)_inputControls[inputPanelName]).Focus();
                    }
                    throw new UserThrownException(string.Format("{0} must be specified.", DataShaper.ShapeCamelCaseString(column.ColumnName)), LoggingLevel.Maximum);
                }
            }
            UIHelper.ClearControls(_inputControls);
            _currentEntityCache.NotifyEntityUpdated(_entityId.Value, _entityUnderUpdate);
            _entityUnderUpdate = null;
            _entityId = null;

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void AddEntity()
        {
            BeforeCrudOperationArgs eBefore = new BeforeCrudOperationArgs(this._entityUnderUpdate);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeAdd(eBefore); //Before populating the entity properties.
            if (eBefore.Cancel) return;
            if (!eBefore.CancelDefaultBindingBehaviour)
            {
                UIHelper.PopulateEntityFromControls(
                    _inputControls, 
                    _entityUnderUpdate, 
                    _hiddenProperties, 
                    _extensionManagedProperties, 
                    _settings.ShapeColumnNames, 
                    _settings.TreatZeroAsNull);
            }
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnBeforeAdd(eBefore); //After populating the entity properties.
            if (eBefore.Cancel) return;
            Type type = _entityUnderUpdate.GetType();
            foreach (SqlCeDatabaseTableColumn column in _currentTable.Columns)
            {
                PropertyInfo property = type.GetProperty(column.ColumnName);
                if (column.IsKey && (property.PropertyType == typeof(Guid)))
                {
                    property.SetValue(this._entityUnderUpdate, Guid.NewGuid(), null); //Set the GUID primary key.
                    continue;
                }
                object value = property.GetValue(_entityUnderUpdate, null);
                if ((
                    (((value == null) || string.IsNullOrEmpty(value.ToString())) || ((property.PropertyType == typeof(Guid)) && (((Guid)value) == Guid.Empty))) || 
                    (_settings.TreatZeroAsNull && (value.ToString() == "0"))) && 
                    !column.IsNullable)
                {
                    if (!column.IsForeignKey)
                    {
                        string inputPanelName = string.Format("pnl{0}", column.ColumnName);
                        UIHelper.ExtractInputControlFromInputPanel((Panel)_inputControls[inputPanelName]).Focus();
                    }
                    throw new UserThrownException(string.Format("{0} must be specified.", DataShaper.ShapeCamelCaseString(column.ColumnName)), LoggingLevel.Maximum);
                }
            }
            _currentEntityCache.Add(_entityUnderUpdate);
            _unsavedChanges = true;
            UIHelper.ClearControls(_inputControls);
            FiglutDataBoxApplication.Instance.CrudInterceptor.PerformOnAfterAdd(new AfterCrudOperationArgs(_entityUnderUpdate));
            if (_settings.CloseAddWindowAfterAdd)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                _entityUnderUpdate = null;
                _entityUnderUpdate = Activator.CreateInstance(_currentTable.MappedType);
                ((_inputControlAfterAdd == null) ? _firstInputControl : _inputControlAfterAdd).Focus();
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void lnkReset_Click(object sender, EventArgs e)
        {
            try
            {
                LinkLabel lnkClear = (LinkLabel)sender;
                Control control = (Control)lnkClear.Tag;
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = false;
                }
                else if (control is NumericTextBox)
                {
                    ((NumericTextBox)control).Text = string.Empty;
                }
                else if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
                else if (control is NumericUpDown)
                {
                    ((NumericUpDown)control).Value = 0;
                }
                else if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).Value = DateTime.Now;
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
                else
                {
                    throw new UserThrownException(string.Format("Unexpected control of type {0} to be reset.", control.GetType().FullName), LoggingLevel.Minimum);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ManageEntityForm_KeyDown);
            }
        }

        private void ManageEntityForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = _entityOperation.ToString();
                statusMain.Text = string.Format("{0} record.", _entityOperation.ToString());
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    pnlInputControls.Height += 50;
                }
                RefreshInputControls();
                SubscribeToResetLinksClickEvents();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ManageEntityForm_KeyDown);
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
                ExceptionHandler.HandleException(ex, true, true, this, ManageEntityForm_KeyDown);
            }
        }

        private void mnuApply_Click(object sender, EventArgs e)
        {
            try
            {
                switch (_entityOperation)
                {
                    case EntityOperation.Update:
                        UpdateEntity();
                        break;
                    case EntityOperation.Add:
                        AddEntity();
                        break;
                    default:
                        throw new ArgumentException(string.Format("Invalid entity operation {0}.", _entityOperation.ToString()));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ManageEntityForm_KeyDown);
            }
        }

        private void lnkSelectParent_Click(object sender, EventArgs e)
        {
            try
            {
                using (SelectParentDataBoxForm f = new SelectParentDataBoxForm(
                    _entityOperation,
                    _entityUnderUpdate,
                    _entityId,
                    _settings,
                    _currentTable,
                    _currentEntityCache))
                {
                    f.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ManageEntityForm_KeyDown);
            }
        }

        private void ManageEntityForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter && lnkSelectParent.Focused)
                {
                    return;
                }
                foreach (LinkLabel lnkReset in _resetLinks)
                {
                    if (e.KeyCode == Keys.Enter && lnkReset.Focused)
                    {
                        return;
                    }
                }
                if (e.KeyCode == Keys.Escape)
                {
                    mnuCancel_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    mnuApply_Click(sender, e);
                }
                else if (e.KeyCode == Keys.P)
                {
                    lnkSelectParent_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, ManageEntityForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}