﻿namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Xceed.Wpf.Toolkit;

    #endregion //Using Directives

    /// <summary>
    /// A helper class for displaying message boxes.
    /// </summary>
    public class UIHelper
    {
        #region Methods

        #region Message Boxes

        public static void DisplayException(Exception ex)
        {
            StringBuilder message = new StringBuilder(ex.Message);
            if (ex.InnerException != null)
            {
                message.AppendLine(ex.InnerException.Message);
            }
            if (!string.IsNullOrEmpty(ex.HelpLink))
            {
                message.AppendLine();
                message.AppendLine();
                message.AppendLine(string.Format("Support: {0}", ex.HelpLink));
            }
            MessageBox.Show(
                message.ToString(),
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error,
                System.Windows.MessageBoxResult.OK);
        }

        public static void DisplayError(string message)
        {
            MessageBox.Show(
                message.ToString(),
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error,
                System.Windows.MessageBoxResult.OK);
        }

        public static void DisplayInformation(string info)
        {
            MessageBox.Show(
                info,
                "Information",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information,
                System.Windows.MessageBoxResult.OK);
        }

        public static System.Windows.MessageBoxResult AskQuestion(string question)
        {
            return MessageBox.Show(
                question,
                "Question",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question,
                System.Windows.MessageBoxResult.No);
        }

        public static void DisplayWarning(string warning)
        {
            MessageBox.Show(
                warning,
                "Warning",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning,
                System.Windows.MessageBoxResult.OK);
        }

        #endregion //Message Boxes

        //public static Dictionary<string, Type> GetTypeNameToControlTypeMappings()
        //{
        //    Dictionary<string, Type> result = new Dictionary<string, Type>();
        //    result.Add(typeof(Boolean).FullName, typeof(CheckBox));
        //    result.Add(typeof(Char).FullName, typeof(TextBox));
        //    result.Add(typeof(String).FullName, typeof(TextBox));
        //    result.Add(typeof(Byte).FullName, typeof(NumericUpDown));
        //    result.Add(typeof(Int16).FullName, typeof(NumericUpDown));
        //    result.Add(typeof(Int32).FullName, typeof(NumericUpDown));
        //    result.Add(typeof(Int64).FullName, typeof(NumericTextBox));
        //    result.Add(typeof(UInt16).FullName, typeof(NumericUpDown));
        //    result.Add(typeof(UInt32).FullName, typeof(NumericUpDown));
        //    result.Add(typeof(UInt64).FullName, typeof(NumericTextBox));
        //    result.Add(typeof(Single).FullName, typeof(NumericTextBox));
        //    result.Add(typeof(Double).FullName, typeof(NumericTextBox));
        //    result.Add(typeof(Decimal).FullName, typeof(NumericTextBox));
        //    result.Add(typeof(DateTime).FullName, typeof(DateTimePicker));
        //    result.Add(typeof(Enum).FullName, typeof(ComboBox));
        //    return result;
        //}

        //public static Control GetInputControlForEntityProperty(
        //    string propertyName,
        //    Type entityType,
        //    DockStyle dockStyle,
        //    Color backColor)
        //{
        //    Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
        //    PropertyInfo p = entityType.GetProperty(propertyName);
        //    Type propertyType = null;
        //    if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //    {
        //        propertyType = p.PropertyType.GetGenericArguments()[0];
        //    }
        //    else
        //    {
        //        propertyType = p.PropertyType;
        //    }
        //    if (!controlMappings.ContainsKey(propertyType.FullName))
        //    {
        //        throw new NullReferenceException(string.Format("No control mapped for property type 0.", propertyType.FullName));
        //    }
        //    Type controlType = controlMappings[propertyType.FullName];
        //    Control result = (Control)Activator.CreateInstance(controlType);
        //    result.Name = string.Format("{0}", p.Name);
        //    result.Tag = p.PropertyType;
        //    result.Dock = dockStyle;
        //    result.TabStop = true;
        //    if (!(result is TextBox) && !(result is NumericTextBox) && !(result is NumericUpDown) && !(result is DateTimePicker) && !(result is ComboBox))
        //    {
        //        result.BackColor = backColor;
        //    }
        //    if (result is NumericUpDown)
        //    {
        //        ((NumericUpDown)result).Maximum = int.MaxValue;
        //    }
        //    if (result is NumericTextBox && (propertyType == typeof(Single)) || propertyType == typeof(Double) || propertyType == typeof(Decimal))
        //    {
        //        NumericTextBox numericTextBox = (NumericTextBox)result;
        //        numericTextBox.AllowDecimal = true;
        //    }
        //    return result;
        //}

        //public static Dictionary<string, Control> GetControlsForEntity(
        //    Type entityType,
        //    bool includeLabels,
        //    DockStyle dockStyle,
        //    Color backColor,
        //    List<string> hiddenProperties,
        //    List<string> unmanagedProperties,
        //    bool shapeColumnNames,
        //    List<LinkLabel> resetLinks,
        //    Color resetLinksBackColor)
        //{
        //    if (resetLinks != null)
        //    {
        //        resetLinks.Clear();
        //    }
        //    Dictionary<string, Control> controls = new Dictionary<string, Control>();
        //    Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
        //    PropertyInfo[] entityProperties = entityType.GetProperties();
        //    for (int i = 0; i < entityProperties.Length; i++)
        //    {
        //        PropertyInfo p = entityProperties[i];
        //        string propertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
        //        if (hiddenProperties.Contains(propertyNameMatch) || unmanagedProperties.Contains(propertyNameMatch))
        //        {
        //            continue;
        //        }
        //        Type propertyType = null;
        //        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //        {
        //            propertyType = p.PropertyType.GetGenericArguments()[0];
        //        }
        //        else
        //        {
        //            propertyType = p.PropertyType;
        //        }
        //        if (!controlMappings.ContainsKey(propertyType.FullName))
        //        {
        //            continue;
        //        }
        //        Type controlType = controlMappings[propertyType.FullName];
        //        Control control = (Control)Activator.CreateInstance(controlType);
        //        control.Name = string.Format("{0}", p.Name);
        //        control.Tag = p.PropertyType;
        //        control.Dock = DockStyle.Fill;
        //        control.TabStop = true;
        //        //control.TabIndex = i;
        //        //control.Height = 40;

        //        Panel pnlInputControl = new Panel() { Name = string.Format("pnl{0}", p.Name), Dock = dockStyle, Height = control.Height };
        //        pnlInputControl.Controls.Add(control);
        //        if (resetLinks != null)
        //        {
        //            LinkLabel lnkReset = new LinkLabel() { Name = string.Format("lnk{0}", p.Name), Text = "Reset", Tag = control, Dock = DockStyle.Right, Width = 60, BackColor = resetLinksBackColor};
        //            pnlInputControl.Controls.Add(lnkReset);
        //            resetLinks.Add(lnkReset);
        //        }
        //        if (!(control is TextBox) && !(control is NumericTextBox) && !(control is NumericUpDown) && !(control is DateTimePicker) && !(control is ComboBox))
        //        {
        //            control.BackColor = backColor;
        //        }
        //        if (control is NumericUpDown)
        //        {
        //            ((NumericUpDown)control).Maximum = int.MaxValue;
        //        }
        //        if (control is NumericTextBox && (propertyType == typeof(Single)) || propertyType == typeof(Double) || propertyType == typeof(Decimal))
        //        {
        //            NumericTextBox numericTextBox = (NumericTextBox)control;
        //            numericTextBox.AllowDecimal = true;
        //        }
        //        if (includeLabels)
        //        {
        //            Label label = new Label()
        //            {
        //                Name = string.Format("lbl{0}", p.Name),
        //                Text = string.Format("{0}:", DataShaper.ShapeCamelCaseString(p.Name)),
        //                Dock = dockStyle,
        //                BackColor = backColor,
        //                //Height = 40,
        //            };
        //            controls.Add(label.Name, label);
        //        }
        //        controls.Add(pnlInputControl.Name, pnlInputControl);
        //    }
        //    return DataHelper.ReverseDictionaryOrder<string, Control>(controls);
        //}

        //public static void PopulateControlsFromEntity(
        //    Dictionary<string, Control> controls,
        //    object entity,
        //    List<string> hiddenProperties,
        //    List<string> unmanagedProperties,
        //    bool shapeColumnNames)
        //{
        //    Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
        //    Type entityType = entity.GetType();
        //    foreach (PropertyInfo p in entityType.GetProperties())
        //    {
        //        string propertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
        //        if (hiddenProperties.Contains(propertyNameMatch) || unmanagedProperties.Contains(propertyNameMatch))
        //        {
        //            continue; //Don't populate any control for hidden and unmanaged properties.
        //        }
        //        Type propertyType = null;
        //        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //        {
        //            propertyType = p.PropertyType.GetGenericArguments()[0];
        //        }
        //        else
        //        {
        //            propertyType = p.PropertyType;
        //        }
        //        if (!controlMappings.ContainsKey(propertyType.FullName))
        //        {
        //            //throw new NullReferenceException(string.Format(
        //            //    "No mapping to control exists for property with name {0} and of type {1}.",
        //            //    p.Name,
        //            //    propertyType.FullName));
        //            continue;
        //        }
        //        Type expectedControlType = controlMappings[propertyType.FullName];
        //        object propertyValue = p.GetValue(entity, null);
        //        Panel pnlInputControl = (Panel)controls[string.Format("pnl{0}", p.Name)]; //S5hould be a panel containing the input control.
        //        Control control = null;
        //        foreach (Control c in pnlInputControl.Controls)
        //        {
        //            if (!(c is LinkLabel))
        //            {
        //                control = c;
        //            }
        //        }
        //        if (control.GetType() != expectedControlType)
        //        {
        //            throw new ArgumentException(string.Format(
        //                "Expected to populate a {0} control for property {1} of type {2} for entity {3}, but received a {4} control to populate.",
        //                expectedControlType.FullName,
        //                p.Name,
        //                propertyType.FullName,
        //                entityType.FullName,
        //                control.GetType().FullName));
        //        }
        //        PopulateControl(control, propertyValue);
        //    }
        //}

        //public static Control ExtractInputControlFromInputPanel(Panel inputPanel)
        //{
        //    foreach (Control control in inputPanel.Controls)
        //    {
        //        if (!(control is LinkLabel))
        //        {
        //            return control;
        //        }
        //    }
        //    throw new NullReferenceException(string.Format("Could not find an input control inside {0} with name {1}.", inputPanel.GetType().FullName, inputPanel.Name));
        //}

        //public static void PopulateControl(Control control, object value)
        //{
        //    if (value == null)
        //    {
        //        return;
        //    }
        //    Type controlType = control.GetType();
        //    string controlTypeName = controlType.FullName;
        //    if (controlTypeName.Equals(typeof(TextBox).FullName))
        //    {
        //        TextBox textBox = (TextBox)control;
        //        textBox.Text = value.ToString();
        //    }
        //    else if (controlTypeName.Equals(typeof(NumericTextBox).FullName))
        //    {
        //        NumericTextBox numericTextBox = (NumericTextBox)control;
        //        numericTextBox.Value = Convert.ToDouble(value);
        //    }
        //    else if (controlTypeName.Equals(typeof(CheckBox).FullName))
        //    {
        //        CheckBox checkBox = (CheckBox)control;
        //        checkBox.Checked = Convert.ToBoolean(value);
        //    }
        //    else if (controlTypeName.Equals(typeof(NumericUpDown).FullName))
        //    {
        //        NumericUpDown numericUpDown = (NumericUpDown)control;
        //        numericUpDown.Value = Convert.ToDecimal(value);
        //    }
        //    else if (controlTypeName.Equals(typeof(DateTimePicker).FullName))
        //    {
        //        DateTimePicker dateTimePicker = (DateTimePicker)control;
        //        dateTimePicker.Value = Convert.ToDateTime(value);
        //    }
        //    else if (controlTypeName.Equals(typeof(ComboBox).FullName))
        //    {
        //        ComboBox comboBox = (ComboBox)control;
        //        Array enumValues = EnumHelper.GetEnumValues(value.GetType());
        //        for (int i = 0; i < enumValues.Length; i++) //Populate the ComboBox with all the enum values.
        //        {
        //            object e = enumValues.GetValue(i);
        //            comboBox.Items.Add(e);
        //        }
        //        comboBox.SelectedItem = value; //Set the selected item based on the passed in value.
        //    }
        //    else
        //    {
        //        throw new Exception(string.Format(
        //            "Unexpected controltype {0} to populate with value {1}.",
        //            controlTypeName,
        //            value));
        //    }
        //}

        //public static void PopulateEntityFromControls(
        //    Dictionary<string, Control> controls,
        //    object entity,
        //    List<string> hiddenProperties,
        //    List<string> unmanagedProperties,
        //    bool shapeColumnNames,
        //    bool treatZeroAsNull)
        //{
        //    Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
        //    Type entityType = entity.GetType();
        //    foreach (PropertyInfo p in entityType.GetProperties())
        //    {
        //        string propertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
        //        if (hiddenProperties.Contains(propertyNameMatch) || unmanagedProperties.Contains(propertyNameMatch))
        //        {
        //            continue; //Don't populate a hidden or unmanaged property from a control.
        //        }
        //        Type propertyType = null;
        //        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //        {
        //            propertyType = p.PropertyType.GetGenericArguments()[0];
        //        }
        //        else
        //        {
        //            propertyType = p.PropertyType;
        //        }
        //        if (!controlMappings.ContainsKey(propertyType.FullName))
        //        {
        //            continue;
        //        }
        //        Type expectedControlType = controlMappings[propertyType.FullName];
        //        object propertyValue = p.GetValue(entity, null);
        //        Panel pnlInputControl = (Panel)controls[string.Format("pnl{0}", p.Name)]; //S5hould be a panel containing the input control.
        //        Control control = null;
        //        foreach (Control c in pnlInputControl.Controls)
        //        {
        //            if (!(c is LinkLabel))
        //            {
        //                control = c;
        //            }
        //        }
        //        if (control.GetType() != expectedControlType)
        //        {
        //            throw new ArgumentException(string.Format(
        //                "Expected to populate a {0} control for property {1} of type {2} for entity {3}, but received a {4} control to populate.",
        //                expectedControlType.FullName,
        //                p.Name,
        //                propertyType.FullName,
        //                entityType.FullName,
        //                control.GetType().FullName));
        //        }
        //        PopulateEntityProperty(control, p, entity, treatZeroAsNull);
        //    }
        //}

        //public static object GetControlValue(Control control, PropertyInfo p, bool treatZeroAsNull)
        //{
        //    bool isNullable = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

        //    Type controlType = control.GetType();
        //    string controlTypeName = controlType.FullName;
        //    string propertyTypeName = p.PropertyType.FullName;
        //    if (controlTypeName.Equals(typeof(TextBox).FullName))
        //    {
        //        TextBox textBox = (TextBox)control;
        //        return textBox.Text;
        //    }
        //    else if (controlTypeName.Equals(typeof(NumericTextBox).FullName))
        //    {
        //        NumericTextBox numericTextBox = (NumericTextBox)control;
        //        object value = DataHelper.ChangeType(numericTextBox.Value, p.PropertyType);
        //        if (treatZeroAsNull && value != null && value.ToString() == "0")
        //        {
        //            value = null;
        //        }
        //        return value;
        //    }
        //    else if (controlTypeName.Equals(typeof(CheckBox).FullName))
        //    {
        //        CheckBox checkBox = (CheckBox)control;
        //        return checkBox.Checked;
        //    }
        //    else if (controlTypeName.Equals(typeof(NumericUpDown).FullName))
        //    {
        //        NumericUpDown numericUpDown = (NumericUpDown)control;
        //        object value = DataHelper.ChangeType(numericUpDown.Value, p.PropertyType);
        //        if (treatZeroAsNull && value != null && value.ToString() == "0")
        //        {
        //            value = null;
        //        }
        //        return value;
        //    }
        //    else if (controlTypeName.Equals(typeof(DateTimePicker).FullName))
        //    {
        //        DateTimePicker dateTimePicker = (DateTimePicker)control;
        //        return dateTimePicker.Value;
        //    }
        //    else if (controlTypeName.Equals(typeof(ComboBox).FullName))
        //    {
        //        ComboBox comboBox = (ComboBox)control;
        //        Array enumValues = EnumHelper.GetEnumValues(p.PropertyType);
        //        return comboBox.SelectedItem;
        //    }
        //    else
        //    {
        //        throw new Exception(string.Format(
        //            "Unexpected controltype {0} to be used to retrieve control value using property type {1}.",
        //            controlTypeName,
        //            p.Name));
        //    }
        //}

        //public static void PopulateEntityProperty(Control control, PropertyInfo p, object entity, bool treatZeroAsNull)
        //{
        //    bool isNullable = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

        //    Type controlType = control.GetType();
        //    string controlTypeName = controlType.FullName;
        //    string propertyTypeName = p.PropertyType.FullName;
        //    if (controlTypeName.Equals(typeof(TextBox).FullName))
        //    {
        //        TextBox textBox = (TextBox)control;
        //        p.SetValue(entity, textBox.Text, null);
        //    }
        //    else if (controlTypeName.Equals(typeof(NumericTextBox).FullName))
        //    {
        //        NumericTextBox numericTextBox = (NumericTextBox)control;
        //        object value = DataHelper.ChangeType(numericTextBox.Value, p.PropertyType);
        //        if (treatZeroAsNull && value != null && value.ToString() == "0")
        //        {
        //            value = null;
        //        }
        //        p.SetValue(entity, value, null);
        //    }
        //    else if (controlTypeName.Equals(typeof(CheckBox).FullName))
        //    {
        //        CheckBox checkBox = (CheckBox)control;
        //        p.SetValue(entity, checkBox.Checked, null);
        //    }
        //    else if (controlTypeName.Equals(typeof(NumericUpDown).FullName))
        //    {
        //        NumericUpDown numericUpDown = (NumericUpDown)control;
        //        object value = DataHelper.ChangeType(numericUpDown.Value, p.PropertyType);
        //        if (treatZeroAsNull && value != null && value.ToString() == "0")
        //        {
        //            value = null;
        //        }
        //        p.SetValue(entity, value, null);
        //    }
        //    else if (controlTypeName.Equals(typeof(DateTimePicker).FullName))
        //    {
        //        DateTimePicker dateTimePicker = (DateTimePicker)control;
        //        p.SetValue(entity, dateTimePicker.Value, null);
        //    }
        //    else if (controlTypeName.Equals(typeof(ComboBox).FullName))
        //    {
        //        ComboBox comboBox = (ComboBox)control;
        //        Array enumValues = EnumHelper.GetEnumValues(p.PropertyType);
        //        p.SetValue(entity, comboBox.SelectedItem, null);
        //    }
        //    else
        //    {
        //        throw new Exception(string.Format(
        //            "Unexpected controltype {0} to be used to populate property {1} on {2}.",
        //            controlTypeName,
        //            p.Name,
        //            entity.GetType().FullName));
        //    }
        //}

        //public static void ClearControls(System.Windows.Forms.Control.ControlCollection controls)
        //{
        //    ClearControls(controls, null);
        //}

        //public static void ClearControls(System.Windows.Forms.Control.ControlCollection controls, List<string> excludedControls)
        //{
        //    Dictionary<string, Control> controlsDictionary = new Dictionary<string, Control>();
        //    foreach (Control c in controls)
        //    {
        //        if ((excludedControls == null) ||
        //            (excludedControls != null && !excludedControls.Contains(c.Name)))
        //        {
        //            controlsDictionary.Add(c.Name, c);
        //        }
        //    }
        //    ClearControls(controlsDictionary);
        //}

        //public static void ClearControls(List<Control> controls)
        //{
        //    Dictionary<string, Control> controlsDictionary = new Dictionary<string, Control>();
        //    foreach (Control c in controls)
        //    {
        //        controlsDictionary.Add(c.Name, c);
        //    }
        //    ClearControls(controlsDictionary);
        //}

        //public static void ClearControls(Dictionary<string, Control> controls)
        //{
        //    foreach (Control control in controls.Values)
        //    {
        //        if (control is Label)
        //        {
        //            continue;
        //        }
        //        if (!(control is Panel))
        //        {
        //            throw new UserThrownException(string.Format("Expected control of type {0} and got control of type {1}.", typeof(Panel).FullName, control), LoggingLevel.Minimum);
        //        }
        //        Panel pnlInputControl = (Panel)control;
        //        Control inputControl = null;
        //        foreach (Control c in pnlInputControl.Controls)
        //        {
        //            if (!(c is LinkLabel))
        //            {
        //                inputControl = c;
        //                break;
        //            }
        //        }
        //        if (inputControl is CheckBox)
        //        {
        //            CheckBox checkBox = (CheckBox)inputControl;
        //            checkBox.Checked = false;
        //        }
        //        else if (inputControl is NumericTextBox)
        //        {
        //            NumericTextBox numericTextBox = (NumericTextBox)inputControl;
        //            numericTextBox.Text = string.Empty;
        //        }
        //        else if (inputControl is TextBox)
        //        {
        //            TextBox textBox = (TextBox)inputControl;
        //            textBox.Text = string.Empty;
        //        }
        //        else if (inputControl is NumericUpDown)
        //        {
        //            NumericUpDown numericUpDown = (NumericUpDown)inputControl;
        //            numericUpDown.Value = 0;
        //        }
        //        else if (inputControl is DateTimePicker)
        //        {
        //            DateTimePicker dateTimePicker = (DateTimePicker)inputControl;
        //            dateTimePicker.Value = DateTime.Now;
        //        }
        //        else if (inputControl is ComboBox)
        //        {
        //            ComboBox comboBox = (ComboBox)inputControl;
        //            if (comboBox.Items.Count > 0)
        //            {
        //                //comboBox.Items.Clear();
        //                comboBox.SelectedIndex = -1;
        //            }
        //        }
        //        else
        //        {
        //            //Do nothing
        //        }
        //    }
        //}

        //public static bool AllControlsPopulated(
        //    System.Windows.Forms.Control.ControlCollection controls,
        //    bool focusOnBlankControl,
        //    bool displayErrorMessage)
        //{
        //    string errorMessage;
        //    return AllControlsPopulated(controls, focusOnBlankControl, displayErrorMessage, out errorMessage);
        //}

        //public static bool AllControlsPopulated(
        //    System.Windows.Forms.Control.ControlCollection controls,
        //    bool focusOnBlankControl,
        //    bool displayErrorMessage,
        //    out string errorMessage)
        //{
        //    List<Control> controlsList = new List<Control>();
        //    foreach (Control c in controls)
        //    {
        //        controlsList.Add(c);
        //    }
        //    return AllControlsPopulated(controlsList, focusOnBlankControl, displayErrorMessage, out errorMessage);
        //}

        //public static bool AllControlsPopulated(
        //    List<Control> controls,
        //    bool focusOnBlankControl,
        //    bool displayErrorMessage)
        //{
        //    string errorMessage;
        //    return AllControlsPopulated(controls, focusOnBlankControl, displayErrorMessage, out errorMessage);
        //}

        //public static bool AllControlsPopulated(
        //    List<Control> controls,
        //    bool focusOnBlankControl,
        //    bool displayErrorMessage,
        //    out string errorMessage)
        //{
        //    Control blankControl = null;
        //    errorMessage = null;
        //    foreach (Control c in controls)
        //    {
        //        Type type = c.GetType();
        //        if (c is TextBox && string.IsNullOrEmpty(((TextBox)c).Text))
        //        {
        //            blankControl = c;
        //            break;
        //        }
        //        else if (c is NumericTextBox && string.IsNullOrEmpty(((TextBox)c).Text))
        //        {
        //            blankControl.Focus();
        //        }
        //    }
        //    if (blankControl != null)
        //    {
        //        string friendlyControlName = blankControl.Name.Substring(DataShaper.GetIndexOfFirstUpperCaseLetter(blankControl.Name));
        //        errorMessage = string.Format("{0} not entered.", DataShaper.ShapeCamelCaseString(friendlyControlName));
        //        if (focusOnBlankControl)
        //        {
        //            blankControl.Focus();
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        #endregion //Methods
    }
}