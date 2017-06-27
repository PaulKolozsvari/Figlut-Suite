namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Winforms;

    #endregion //Using Directives

    /// <summary>
    /// A helper class for displaying message boxes.
    /// </summary>
    public class UIHelper
    {
        #region Methods

        /// <summary>
        /// Displays a message box containing the message of the provided Exception
        /// as well as the message of the inner exception if it exists.
        /// </summary>
        /// <param name="ex">The exception whose message will be displayed.</param>
        public static void DisplayException(Exception ex)
        {
            StringBuilder message = new StringBuilder(ex.Message);
            if (ex.InnerException != null)
            {
                string innerExeptionMessage = ex.InnerException.Message;
                if (innerExeptionMessage.Length > 100)
                {
                    innerExeptionMessage = innerExeptionMessage.Substring(0, 100);
                }
                message.Append(string.Format("\r\nInner Exception : {0}", innerExeptionMessage));
            }
            MessageBox.Show(
                message.ToString(),
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Hand,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Displays an error message box with the given error message.
        /// </summary>
        /// <param name="errorMessage">The error message to be displayed.</param>
        public static void DisplayError(string errorMessage)
        {
            MessageBox.Show(
                errorMessage,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Hand,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Displays an information message box with the given info message.
        /// </summary>
        /// <param name="infoMessage">The information message to be displayed.</param>
        public static void DisplayInformation(string infoMessage)
        {
            MessageBox.Show(
                infoMessage, 
                "Information", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Asterisk, 
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Displays a warning message box with the given warning message.
        /// </summary>
        /// <param name="warningMessage">The warning message to be displayed.</param>
        public static void DisplayWarning(string warningMessage)
        {
            MessageBox.Show(
                warningMessage,
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Displays question message box containing the question message and then
        /// returns a Dialog result of either Yes or No based on the user's selection/response.
        /// </summary>
        /// <param name="question">The question message to be displayed to the user.</param>
        /// <returns>Returns either a Yes or No response based on the user's selection/response.</returns>
        public static DialogResult AskQuestion(string questionMessage)
        {
            return MessageBox.Show(
                questionMessage,
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
        }

        public static void MenuEnabled(MenuStrip mainMenu, bool enabled)
        {
            foreach (ToolStripMenuItem menuItem in mainMenu.Items)
            {
                menuItem.Enabled = enabled;
            }
        }

        /// <summary>
        /// Gets the cell value of a specific column in the currently selected row of a DataGrid.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static T GetSelectedGridRowCellValue<T>(DataGrid grid, int columIndex)
        {
            return (T)grid[grid.CurrentRowIndex, columIndex];
        }

        /// <summary>
        /// Gets the cell value of a specific column in the currently selected row of a DataGrid.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static T GetSelectedDataGridViewRowCellValue<T>(DataGridView grid, int columIndex) 
        {
            if (grid.SelectedRows.Count < 1)
            {
                return default(T);
            }
            DataRow row = ((DataRowView)(grid.SelectedRows[0].DataBoundItem)).Row;
            return (T)row[columIndex];
        }

        public static T GetSelectedDataGridViewRowCellValue<T>(DataGridView grid, string columnName)
        {
            if (grid.SelectedRows.Count < 1)
            {
                return default(T);
            }
            DataRow row = ((DataRowView)(grid.SelectedRows[0].DataBoundItem)).Row;
            return (T)row[columnName];
        }

        public static DataGridTableStyle GetDataGridTableStyle<T>(int width, List<string> hiddenColumns, bool shapeColumnNames)
        {
            DataGridTableStyle result = new DataGridTableStyle();
            result.MappingName = shapeColumnNames ? DataShaper.ShapeCamelCaseString(typeof(T).Name) : typeof(T).Name;
            List<string> columns = EntityReader<T>.GetAllPropertyNames(shapeColumnNames);
            foreach (string c in columns)
            {
                int modifiedWidth = width;
                if (hiddenColumns.Contains(c))
                {
                    modifiedWidth = -1;
                }
                result.GridColumnStyles.Add(GetColumnStyle(c, c, modifiedWidth));
            }
            return result;
        }

        public static DataGridColumnStyle GetColumnStyle(string mappingName, string headerText, int width)
        {
            DataGridColumnStyle result = new DataGridTextBoxColumn();
            result.MappingName = mappingName;
            if (!string.IsNullOrEmpty(headerText))
            {
                result.HeaderText = headerText;
            }
            result.Width = width;
            return result;
        }

        public static Dictionary<string, Type> GetTypeNameToControlTypeMappings()
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();
            result.Add(typeof(Boolean).FullName, typeof(CheckBox));
            result.Add(typeof(Char).FullName, typeof(TextBox));
            result.Add(typeof(String).FullName, typeof(TextBox));
            result.Add(typeof(Int16).FullName, typeof(NumericUpDown));
            result.Add(typeof(Int32).FullName, typeof(NumericUpDown));
            result.Add(typeof(Int64).FullName, typeof(NumericTextBox));
            result.Add(typeof(UInt16).FullName, typeof(NumericUpDown));
            result.Add(typeof(UInt32).FullName, typeof(NumericUpDown));
            result.Add(typeof(UInt64).FullName, typeof(NumericUpDown));
            result.Add(typeof(Single).FullName, typeof(NumericUpDown));
            result.Add(typeof(Double).FullName, typeof(NumericUpDown));
            result.Add(typeof(Decimal).FullName, typeof(NumericUpDown));
            result.Add(typeof(DateTime).FullName, typeof(DateTimePicker));
            result.Add(typeof(Enum).FullName, typeof(ComboBox));
            return result;
        }

        public static Dictionary<string, Control> GetControlsForEntity(
            Type entityType, 
            bool includeLabels, 
            DockStyle dockStyle,
            Color backColor, 
            List<string> hiddenProperties, 
            bool shapeColumnNames)
        {
            Dictionary<string, Control> controls = new Dictionary<string, Control>();
            Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            for(int i = 0; i < entityProperties.Length; i++)
            {
                PropertyInfo p = entityProperties[i];
                string hiddenPropertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                if (hiddenProperties.Contains(hiddenPropertyNameMatch))
                {
                    continue;
                }
                Type propertyType = null;
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = p.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    propertyType = p.PropertyType;
                }
                if (!controlMappings.ContainsKey(propertyType.FullName))
                {
                    //throw new NullReferenceException(string.Format(
                    //    "There is not control mapping to type {0} of property {1} of entity type {2}.",
                    //    propertyType.FullName,
                    //    p.Name,
                    //    entityType.FullName));
                    continue;
                }
                Type controlType = controlMappings[propertyType.FullName];
                Control control = (Control)Activator.CreateInstance(controlType);
                control.Name = string.Format("{0}", p.Name);
                control.Tag = p.PropertyType;
                control.Dock = dockStyle;
                control.TabStop = true;
                control.TabIndex = i;
                if (!(control is TextBox) && 
                    !(control is NumericTextBox) && 
                    !(control is NumericUpDown) &&
                    !(control is DateTimePicker) &&
                    !(control is ComboBox))
                {
                    control.BackColor = backColor;
                }
                if (includeLabels)
                {
                    Label label = new Label();
                    label.Name = string.Format("lbl{0}", p.Name);
                    label.Text = string.Format("{0}:", DataShaper.ShapeCamelCaseString(p.Name));
                    label.Dock = dockStyle;
                    label.BackColor = backColor;
                    controls.Add(label.Name, label);
                }
                controls.Add(control.Name, control);
            }
            return DataHelper.ReverseDictionaryOrder<string, Control>(controls);
        }

        public static void PopulateControlsFromEntity(
            Dictionary<string, Control> controls, 
            object entity,
            List<string> hiddenProperties,
            bool shapeColumnNames)
        {
            Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
            Type entityType = entity.GetType();
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                string hiddenPropertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                if (hiddenProperties.Contains(hiddenPropertyNameMatch))
                {
                    continue; //Don't populate any control for hidden properties.
                }
                Type propertyType = null;
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = p.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    propertyType = p.PropertyType;
                }
                if (!controlMappings.ContainsKey(propertyType.FullName))
                {
                    //throw new NullReferenceException(string.Format(
                    //    "No mapping to control exists for property with name {0} and of type {1}.",
                    //    p.Name,
                    //    propertyType.FullName));
                    continue;
                }
                Type expectedControlType = controlMappings[propertyType.FullName];
                object propertyValue = p.GetValue(entity, null);
                Control c = controls[p.Name];
                if (c.GetType() != expectedControlType)
                {
                    throw new ArgumentException(string.Format(
                        "Expected to populate a {0} control for property {1} of type {2} for entity {3}, but received a {4} control to populate.",
                        expectedControlType.FullName,
                        p.Name,
                        propertyType.FullName,
                        entityType.FullName,
                        c.GetType().FullName));
                }
                PopulateControl(c, propertyValue);
            }
        }

        public static void PopulateControl(Control control, object value)
        {
            if (value == null)
            {
                return;
            }
            Type controlType = control.GetType();
            string controlTypeName = controlType.FullName;
            if (controlTypeName.Equals(typeof(TextBox).FullName))
            {
                TextBox textBox = (TextBox)control;
                textBox.Text = value.ToString();
            }
            else if (controlTypeName.Equals(typeof(CheckBox).FullName))
            {
                CheckBox checkBox = (CheckBox)control;
                checkBox.Checked = Convert.ToBoolean(value);
            }
            else if (controlTypeName.Equals(typeof(NumericUpDown).FullName))
            {
                NumericUpDown numericUpDown = (NumericUpDown)control;
                numericUpDown.Value = Convert.ToDecimal(value);
            }
            else if (controlTypeName.Equals(typeof(DateTimePicker).FullName))
            {
                DateTimePicker dateTimePicker = (DateTimePicker)control;
                dateTimePicker.Value = Convert.ToDateTime(value);
            }
            else if (controlTypeName.Equals(typeof(ComboBox).FullName))
            {
                ComboBox comboBox = (ComboBox)control;
                Array enumValues = EnumHelper.GetEnumValues(value.GetType());
                for (int i = 0; i < enumValues.Length; i++) //Populate the ComboBox with all the enum values.
                {
                    object e = enumValues.GetValue(i);
                    comboBox.Items.Add(e);
                }
                comboBox.SelectedItem = value; //Set the selected item based on the passed in value.
            }
            else
            {
                throw new Exception(string.Format(
                    "Unexpected controltype {0} to populate with value {1}.",
                    controlTypeName,
                    value));
            }
        }

        public static void PopulateEntityFromControls(
            Dictionary<string, Control> controls,
            object entity,
            List<string> hiddenProperties,
            bool shapeColumnNames)
        {
            Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
            Type entityType = entity.GetType();
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                string hiddenPropertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                if (hiddenProperties.Contains(hiddenPropertyNameMatch))
                {
                    continue; //Don't populate a hidden properties from a control.
                }
                Type propertyType = null;
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = p.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    propertyType = p.PropertyType;
                }
                if (!controlMappings.ContainsKey(propertyType.FullName))
                {
                    //throw new NullReferenceException(string.Format(
                    //    "No mapping to control exists for property with name {0} and of type {1}.",
                    //    p.Name,
                    //    propertyType.FullName));
                    continue;
                }
                Type expectedControlType = controlMappings[propertyType.FullName];
                object propertyValue = p.GetValue(entity, null);
                Control c = controls[p.Name];
                if (c.GetType() != expectedControlType)
                {
                    throw new ArgumentException(string.Format(
                        "Expected to populate a {0} control for property {1} of type {2} for entity {3}, but received a {4} control to populate.",
                        expectedControlType.FullName,
                        p.Name,
                        propertyType.FullName,
                        entityType.FullName,
                        c.GetType().FullName));
                }
                PopulateEntityProperty(c, p, entity);
            }
        }

        public static void PopulateEntityProperty(Control control, PropertyInfo p, object entity)
        {
            bool isNullable = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

            Type controlType = control.GetType();
            string controlTypeName = controlType.FullName;
            if (controlTypeName.Equals(typeof(TextBox).FullName))
            {
                TextBox textBox = (TextBox)control;
                p.SetValue(entity, textBox.Text, null);
            }
            else if (controlTypeName.Equals(typeof(CheckBox).FullName))
            {
                CheckBox checkBox = (CheckBox)control;
                p.SetValue(entity, checkBox.Checked, null);
            }
            else if (controlTypeName.Equals(typeof(NumericUpDown).FullName))
            {
                NumericUpDown numericUpDown = (NumericUpDown)control;
                p.SetValue(entity, numericUpDown.Value, null);
            }
            else if (controlTypeName.Equals(typeof(DateTimePicker).FullName))
            {
                DateTimePicker dateTimePicker = (DateTimePicker)control;
                p.SetValue(entity, dateTimePicker.Value, null);
            }
            else if (controlTypeName.Equals(typeof(ComboBox).FullName))
            {
                ComboBox comboBox = (ComboBox)control;
                Array enumValues = EnumHelper.GetEnumValues(p.PropertyType);
                p.SetValue(entity, comboBox.SelectedItem, null);
            }
            else
            {
                throw new Exception(string.Format(
                    "Unexpected controltype {0} to be used to populate property {1} on {2}.",
                    controlTypeName,
                    p.Name,
                    entity.GetType().FullName));
            }
        }

        public static void ClearControls(Dictionary<string, Control> controls)
        {
            foreach (Control control in controls.Values)
            {
                Type controlType = control.GetType();
                string controlTypeName = controlType.FullName;
                if (controlTypeName.Equals(typeof(TextBox).FullName))
                {
                    TextBox textBox = (TextBox)control;
                    textBox.Text = string.Empty;
                }
                else if (controlTypeName.Equals(typeof(CheckBox).FullName))
                {
                    CheckBox checkBox = (CheckBox)control;
                    checkBox.Checked = false;
                }
                else if (controlTypeName.Equals(typeof(NumericUpDown).FullName))
                {
                    NumericUpDown numericUpDown = (NumericUpDown)control;
                    numericUpDown.Value = 0;
                }
                else if (controlTypeName.Equals(typeof(DateTimePicker).FullName))
                {
                    DateTimePicker dateTimePicker = (DateTimePicker)control;
                    dateTimePicker.Value = DateTime.Now;
                }
                else if (controlTypeName.Equals(typeof(ComboBox).FullName))
                {
                    ComboBox comboBox = (ComboBox)control;
                    comboBox.SelectedIndex = -1;
                    comboBox.Items.Clear();
                }
                else
                {
                    //Do nothing
                }
            }
        }

        #endregion //Methods
    }
}