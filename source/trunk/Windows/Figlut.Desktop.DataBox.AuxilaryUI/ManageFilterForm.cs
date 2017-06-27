namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.BaseUI;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Winforms;

    #endregion //Using Directives

    public partial class ManageFilterForm : FiglutBaseForm
    {
        #region Constructors

        public ManageFilterForm(
            SqlDatabaseTable table, 
            List<string> hiddenProperties, 
            List<string> unmanagedProperties,
            bool shapeColumnNames,
            bool treatZeroAsNull,
            WhereClauseColumn whereClauseColumnToUpdate)
        {
            InitializeComponent();
            _table = table;
            _hiddenProperties = hiddenProperties;
            _unmanagedProperties = unmanagedProperties;
            _shapeColumnNames = shapeColumnNames;
            _treatZeroAsNull = treatZeroAsNull;
            _whereClauseColumn = new WhereClauseColumn();
            if (whereClauseColumnToUpdate != null)
            {
                EntityReader.CopyProperties(whereClauseColumnToUpdate, _whereClauseColumn, false);
                mnuApply.Text = "Update";
            }
            else
            {
                mnuApply.Text = "Add";
            }
            UnhookComboBoxEvents();
        }

        #endregion //Constructors

        #region Fields

        private SqlDatabaseTable _table;
        private List<string> _hiddenProperties;
        private List<string> _unmanagedProperties;
        private bool _shapeColumnNames;
        private bool _treatZeroAsNull;
        private WhereClauseColumn _whereClauseColumn;
        private Control _columnValueInputControl;
        private PropertyInfo _columnValuePropertyType;

        #endregion //Fields

        #region Properties

        public WhereClauseColumn WhereClauseColumn
        {
            get { return _whereClauseColumn; }
        }

        #endregion //Properties

        #region Methods

        private void UnhookComboBoxEvents()
        {
            this.cboColumnName.SelectedIndexChanged -= new System.EventHandler(this.cboColumnName_SelectedIndexChanged);
            this.cboComparisonOperator.SelectedIndexChanged -= new System.EventHandler(this.cboComparisonOperator_SelectedIndexChanged);
            this.cboLogicalOperator.SelectedIndexChanged -= new System.EventHandler(this.cboLogicalOperator_SelectedIndexChanged);
        }

        private void HookComboBoxEvents()
        {
            this.cboColumnName.SelectedIndexChanged += new System.EventHandler(this.cboColumnName_SelectedIndexChanged);
            this.cboComparisonOperator.SelectedIndexChanged += new System.EventHandler(this.cboComparisonOperator_SelectedIndexChanged);
            this.cboLogicalOperator.SelectedIndexChanged += new System.EventHandler(this.cboLogicalOperator_SelectedIndexChanged);
        }
        
        private void RefreshColumnNames()
        {
            cboColumnName.Items.Clear();
            List<string> propertyNames = EntityReader.GetAllPropertyNames(
                _shapeColumnNames,
                _table.MappedType,
                _hiddenProperties,
                _unmanagedProperties);
            propertyNames.Reverse();
            propertyNames.ForEach(p => cboColumnName.Items.Add(p));

            cboColumnName.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(_whereClauseColumn.ColumnName))
            {
                cboColumnName.SelectedItem = DataShaper.ShapeCamelCaseString(_whereClauseColumn.ColumnName);
            }
        }

        private void RefreshComparisonOperators()
        {
            cboComparisonOperator.Items.Clear();
            Array comparisonOperators = EnumHelper.GetEnumValues(typeof(ComparisonOperator));
            foreach (Enum e in comparisonOperators)
            {
                cboComparisonOperator.Items.Add(e.ToString());
            }
            cboComparisonOperator.SelectedIndex = 0;
            if (_whereClauseColumn.ComparisonOperator != null)
            {
                ComparisonOperator comparisonOperator = WhereClauseComparisonOperator.GetComparisonOperatorFromString(_whereClauseColumn.ComparisonOperator.Value);
                cboComparisonOperator.SelectedItem = comparisonOperator.ToString();
            }
        }

        private void RefreshLogicalOperators()
        {
            cboLogicalOperator.Items.Clear();
            Array logicalOperators = EnumHelper.GetEnumValues(typeof(LogicalOperator));
            foreach (Enum e in logicalOperators)
            {
                cboLogicalOperator.Items.Add(e.ToString());
            }
            cboLogicalOperator.SelectedIndex = -1;
            if (_whereClauseColumn.LogicalOperatorAgainstNextColumn != null)
            {
                cboLogicalOperator.SelectedItem = _whereClauseColumn.LogicalOperatorAgainstNextColumn.Value;
            }
        }

        private void RefreshColumnValueInputControl(bool clearColumnValue)
        {
            pnlColumnValue.Controls.Clear();
            if (cboColumnName.SelectedIndex < 0)
            {
                _columnValueInputControl = null;
                return;
            }
            string propertyName = DataShaper.RestoreStringToCamelCase(cboColumnName.SelectedItem.ToString());
            _columnValueInputControl = UIHelper.GetInputControlForEntityProperty(propertyName, _table.MappedType, DockStyle.Top, Color.White);
            HookInputControlValueChangedEvent();
            _columnValuePropertyType = _table.MappedType.GetProperty(propertyName);
            pnlColumnValue.Controls.Add(_columnValueInputControl);
            if (clearColumnValue)
            {
                _whereClauseColumn.ColumnValue = null;
            }
            if (_whereClauseColumn.ColumnValue != null)
            {
                UIHelper.PopulateControl(_columnValueInputControl, _whereClauseColumn.ColumnValue);
            }
        }

        private void UpdateWhereClause()
        {
            if (_columnValueInputControl == null)
            {
                return; //Initialization has not completed.
            }
            _whereClauseColumn.ColumnName =
                cboColumnName.SelectedIndex < 0 ?
                string.Empty :
                DataShaper.RestoreStringToCamelCase(cboColumnName.SelectedItem.ToString());
            _whereClauseColumn.ComparisonOperator = 
                cboComparisonOperator.SelectedIndex < 0 ?
                null :
                new WhereClauseComparisonOperator((ComparisonOperator)Enum.Parse(typeof(ComparisonOperator), cboComparisonOperator.SelectedItem.ToString()));
            _whereClauseColumn.ColumnValue = UIHelper.GetControlValue(_columnValueInputControl, _columnValuePropertyType, _treatZeroAsNull); ;
            _whereClauseColumn.LogicalOperatorAgainstNextColumn =
                cboLogicalOperator.SelectedIndex < 0 ?
                null :
                new WhereClauseLogicalOperator((LogicalOperator)Enum.Parse(typeof(LogicalOperator), cboLogicalOperator.SelectedItem.ToString()));

            Status = _whereClauseColumn.ToString();
        }

        private void HookInputControlValueChangedEvent()
        {
            Type controlType = _columnValueInputControl.GetType();
            string controlTypeName = controlType.FullName;
            if (controlTypeName.Equals(typeof(TextBox).FullName))
            {
                TextBox textBox = (TextBox)_columnValueInputControl;
                textBox.TextChanged += delegate(object sender, EventArgs e)
                {
                    UpdateWhereClause();
                };
            }
            else if (controlTypeName.Equals(typeof(NumericTextBox).FullName))
            {
                NumericTextBox numericTextBox = (NumericTextBox)_columnValueInputControl;
                numericTextBox.TextChanged += delegate(object sender, EventArgs e)
                {
                    UpdateWhereClause();
                };
            }
            else if (controlTypeName.Equals(typeof(CheckBox).FullName))
            {
                CheckBox checkBox = (CheckBox)_columnValueInputControl;
                checkBox.CheckedChanged += delegate(object sender, EventArgs e)
                {
                    UpdateWhereClause();
                };
            }
            else if (controlTypeName.Equals(typeof(NumericUpDown).FullName))
            {
                NumericUpDown numericUpDown = (NumericUpDown)_columnValueInputControl;
                numericUpDown.ValueChanged += delegate(object sender, EventArgs e)
                {
                    UpdateWhereClause();
                };
            }
            else if (controlTypeName.Equals(typeof(DateTimePicker).FullName))
            {
                DateTimePicker dateTimePicker = (DateTimePicker)_columnValueInputControl;
                dateTimePicker.ValueChanged += delegate(object sender, EventArgs e)
                {
                    UpdateWhereClause();
                };
            }
            else if (controlTypeName.Equals(typeof(ComboBox).FullName))
            {
                ComboBox comboBox = (ComboBox)_columnValueInputControl;
                comboBox.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    UpdateWhereClause();
                };
            }
            else
            {
                throw new Exception(string.Format("Unexpected controltype {0} to hook onto value changed event", controlTypeName));
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void ManageFilterForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void ManageFilterForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void ManageFilterForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void ManageFilterForm_Load(object sender, EventArgs e)
        {
            RefreshColumnNames();
            RefreshComparisonOperators();
            RefreshLogicalOperators();
            RefreshColumnValueInputControl(false);
            UpdateWhereClause();
            HookComboBoxEvents();
        }

        private void mnuApply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_whereClauseColumn.ColumnName))
            {
                throw new UserThrownException(
                    string.Format("{0} not selected.", EntityReader<WhereClauseColumn>.GetPropertyName(p => p.ColumnName, true)),
                    LoggingLevel.None);
            }
            if (_whereClauseColumn.ComparisonOperator == null)
            {
                throw new UserThrownException(
                    string.Format("{0} not selected.", EntityReader<WhereClauseColumn>.GetPropertyName(p => p.ComparisonOperator, true)),
                    LoggingLevel.None);
            }
            if ((_whereClauseColumn.ColumnValue == null) || (string.IsNullOrEmpty(_whereClauseColumn.ColumnValue.ToString())))
            {
                throw new UserThrownException(
                    string.Format("{0} not set.", EntityReader<WhereClauseColumn>.GetPropertyName(p => p.ColumnValue, true),
                    LoggingLevel.None));
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cboColumnName_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshColumnValueInputControl(true);
            UpdateWhereClause();
        }

        private void cboComparisonOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWhereClause();
        }

        private void cboLogicalOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWhereClause();
        }

        private void ManageFilterForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                mnuApply.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}
