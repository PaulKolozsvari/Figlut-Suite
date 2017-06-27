using Android.Views;
using Android.App;
using Android.Content;
using Android.Widget;

namespace Figlut.MonoDroid.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using Figlut.MonoDroid.Toolkit.Data;
    using Figlut.MonoDroid.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    /// <summary>
    /// A helper class for displaying message boxes.
    /// </summary>
    public class UIHelper
    {
		#region Inner Types

		public enum MessageBoxResult
		{
			None,
			OK,
			Cancel,
			Yes,
			No
		}

		public enum MessageBoxButton
		{
			OK,
			OKCancel,
			YesNo,
			YesNoCancel,
		}

		public enum MessageBoxButtonText
		{
			OK,
			Cancel,
			Yes,
			No
		}

		#endregion //Inner Types

        #region Methods

		public static void DisplayMessage(
			Activity context, 
			Action<MessageBoxResult> callback, 
			string messageBoxText, 
			string caption, 
			MessageBoxButton button)
		{
			AlertDialog.Builder alerBuilder = new AlertDialog.Builder (context);
			alerBuilder.SetTitle (caption);
			alerBuilder.SetMessage (messageBoxText);
			switch (button)
			{
				case MessageBoxButton.OK:
					alerBuilder.SetPositiveButton (MessageBoxButtonText.OK.ToString (), delegate(object sender, DialogClickEventArgs e) {
						if(callback != null) callback(MessageBoxResult.OK);
					});
					break;
				case MessageBoxButton.OKCancel:
					alerBuilder.SetPositiveButton (MessageBoxButtonText.OK.ToString (), delegate(object sender, DialogClickEventArgs e) {
						if(callback != null) callback (MessageBoxResult.OK);
					});
					alerBuilder.SetNegativeButton (MessageBoxButtonText.Cancel.ToString (), delegate(object sender, DialogClickEventArgs e) {
						if(callback != null) callback(MessageBoxResult.Cancel);
					});
					break;
				case MessageBoxButton.YesNo:
					alerBuilder.SetPositiveButton (MessageBoxButtonText.Yes.ToString (), delegate(object sender, DialogClickEventArgs e) {
						if(callback != null) callback (MessageBoxResult.Yes);
					});
					alerBuilder.SetNegativeButton (MessageBoxButtonText.No.ToString (), delegate(object sender, DialogClickEventArgs e) {
						if(callback != null) callback(MessageBoxResult.No);
					});
					break;
				case MessageBoxButton.YesNoCancel:
					alerBuilder.SetPositiveButton (MessageBoxButtonText.Yes.ToString (), delegate(object sender, DialogClickEventArgs e) {
						if(callback != null) callback (MessageBoxResult.Yes);
					});
					alerBuilder.SetNegativeButton (MessageBoxButtonText.No.ToString (), delegate(object sender, DialogClickEventArgs e) {
						if(callback != null) callback (MessageBoxResult.No);
					});
					alerBuilder.SetNeutralButton (MessageBoxButtonText.Cancel.ToString (), delegate(object sender, DialogClickEventArgs e) {
						if(callback != null) callback(MessageBoxResult.Cancel);	
					});
					break;
				default:
					break;
			}
			alerBuilder.Show ();
		}

		public static void DisplayMessage(Activity context, string messageBoxText)
		{
			DisplayMessage (context, null, messageBoxText, string.Empty, MessageBoxButton.OK);
		}

		public static void DisplayMessage(Activity context, string messageBoxText, string caption)
		{
			DisplayMessage (context, null, messageBoxText, caption, MessageBoxButton.OK);
		}

		public static View AddContentLayoutToViewStub(Activity currentActivity, int contentLayoutId, int contentStubId)
		{
			ViewStub viewStub  = currentActivity.FindViewById<ViewStub> (contentStubId);
			viewStub.LayoutResource = contentLayoutId;
			return viewStub.Inflate();
		}

        public static Dictionary<string, Type> GetTypeNameToControlTypeMappings()
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();
            result.Add(typeof(Boolean).FullName, typeof(CheckBox));
            result.Add(typeof(Char).FullName, typeof(EditText));
            result.Add(typeof(String).FullName, typeof(EditText));
            result.Add(typeof(Byte).FullName, typeof(EditText));
            result.Add(typeof(Int16).FullName, typeof(EditText));
            result.Add(typeof(Int32).FullName, typeof(EditText));
            result.Add(typeof(Int64).FullName, typeof(EditText));
            result.Add(typeof(UInt16).FullName, typeof(EditText));
            result.Add(typeof(UInt32).FullName, typeof(EditText));
            result.Add(typeof(UInt64).FullName, typeof(EditText));
			result.Add(typeof(Single).FullName, typeof(EditText));
			result.Add(typeof(Double).FullName, typeof(EditText));
			result.Add(typeof(Decimal).FullName, typeof(EditText));
            result.Add(typeof(DateTime).FullName, typeof(EditText));
            result.Add(typeof(Enum).FullName, typeof(Spinner));
            return result;
        }

        public static Dictionary<string, View> GetControlsForEntity(
			Context context,
            Type entityType,
            bool includeLabels,
            Android.Graphics.Color backColor,
            List<string> hiddenProperties,
            List<string> unmanagedProperties,
            bool shapeColumnNames)
        {
            Dictionary<string, View> controls = new Dictionary<string, View>();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                PropertyInfo p = entityProperties[i];
                string propertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                if (hiddenProperties.Contains(propertyNameMatch) || unmanagedProperties.Contains(propertyNameMatch))
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
				View control = CreateInputControl (context, propertyType);
				if(control.LayoutParameters == null)
				{
					control.LayoutParameters = new ViewGroup.LayoutParams (ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent);
				}
				control.LayoutParameters.Width = Android.Views.ViewGroup.LayoutParams.FillParent;
				control.LayoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
				control.SetBackgroundColor (backColor);

                string controlName = string.Format("{0}", p.Name);
				control.Tag = new PropertyTypeHolder () { PropertyType = p.PropertyType };
                if (includeLabels)
                {
					TextView textView = new TextView (context);
					string textViewName = string.Format ("lbl{0}", p.Name);
					textView.Text = string.Format ("{0}:", DataShaper.ShapeCamelCaseString (p.Name));
					controls.Add (textViewName, textView);
                }
				controls.Add (controlName, control);
            }
			return controls;
        }

		private static View CreateInputControl(Context context, Type propertyType)
		{
			View control = null;
			if (propertyType.FullName == typeof(Boolean).FullName) 
			{
				CheckBox checkBox = new CheckBox (context);
				control = checkBox;
			} 
			else if (propertyType.FullName == typeof(Enum).FullName) 
			{
				Spinner spinner = new Spinner (context);
				control = spinner;
			} 
			else 
			{
				EditText editText = new EditText (context);
				if (propertyType.FullName == typeof(Int16).FullName ||
				    propertyType.FullName == typeof(Int32).FullName ||
				    propertyType.FullName == typeof(Int64).FullName ||
				    propertyType.FullName == typeof(UInt16).FullName ||
				    propertyType.FullName == typeof(UInt32).FullName ||
				    propertyType.FullName == typeof(UInt64).FullName) 
				{ 
					editText.InputType = Android.Text.InputTypes.ClassNumber;
				} 
				else if (propertyType.FullName == typeof(Single).FullName ||
				         propertyType.FullName == typeof(Double).FullName ||
				         propertyType.FullName == typeof(Decimal).FullName) 
				{
					editText.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal | Android.Text.InputTypes.NumberFlagSigned;
				}
				else if(propertyType.FullName == typeof(DateTime).FullName)
				{
					/*This ClassDatetime InputType is only applied so that the calling application knows that this EditText 
						 * is meant to be for DateTime capture. The calling application should open a DatePickerDialog on the OnClick
						 * event of the EditText to capture a Date and populate it back into the EditText.
						 Furthermore, after the calling application has hooked onto the Click/FocusChanged event it should
						 change the InputType to InputTypes.Null in order to not allow the user to edit the text without the
						 DatePicker dialog i.e. it will set the EditText to read only mode. */
					editText.InputType = Android.Text.InputTypes.ClassDatetime;
					editText.SetCursorVisible (false);
				}
				control = editText;
			}
			return control;
		}

		public class PropertyTypeHolder : Java.Lang.Object
		{
			public Type PropertyType{get;set;}
		}

        public static void PopulateControlsFromEntity(
			Context context,
            Dictionary<string, View> controls,
            object entity,
            List<string> hiddenProperties,
            List<string> unmanagedProperties,
            bool shapeColumnNames)
        {
            Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
            Type entityType = entity.GetType();
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                string propertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                if (hiddenProperties.Contains(propertyNameMatch) || unmanagedProperties.Contains(propertyNameMatch))
                {
                    continue; //Don't populate any control for hidden and unmanaged properties.
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
				View control = controls [p.Name];
                if (control.GetType() != expectedControlType)
                {
                    throw new ArgumentException(string.Format(
                        "Expected to populate a {0} control for property {1} of type {2} for entity {3}, but received a {4} control to populate.",
                        expectedControlType.FullName,
                        p.Name,
                        propertyType.FullName,
                        entityType.FullName,
                        control.GetType().FullName));
                }
                PopulateControl(context, control, propertyValue);
            }
        }
//
//        public static Control ExtractInputControlFromInputPanel(Panel inputPanel)
//        {
//            foreach (Control control in inputPanel.Controls)
//            {
//                if (!(control is LinkLabel))
//                {
//                    return control;
//                }
//            }
//            throw new NullReferenceException(string.Format("Could not find an input control inside {0} with name {1}.", inputPanel.GetType().FullName, inputPanel.Name));
//        }
//
        public static void PopulateControl(Context context, View control, object value)
        {
            if (value == null)
            {
                return;
            }
            Type controlType = control.GetType();
            string controlTypeName = controlType.FullName;
			if (controlTypeName.Equals (typeof(EditText).FullName)) 
			{
				EditText editText = (EditText)control;
				editText.Text = value.ToString ();
			} 
			else if (controlTypeName.Equals (typeof(CheckBox).FullName)) 
			{
				CheckBox checkBox = (CheckBox)control;
				checkBox.Checked = Convert.ToBoolean (value);
			} 
			else if (controlTypeName.Equals (typeof(Spinner).FullName)) 
			{
				Spinner spinner = (Spinner)control;
				ArrayAdapter<object> spinnerAdapter = new ArrayAdapter<object> (context, Android.Resource.Layout.SimpleSpinnerDropDownItem);
				spinner.Adapter = spinnerAdapter;
				Array enumValues = EnumHelper.GetEnumValues (value.GetType ());
				int selectedItemIndex = -1;
				for (int i = 0; i < enumValues.Length; i++) 
				{
					object e = enumValues.GetValue (i);
					spinnerAdapter.Add (e);
					if (e == value) 
					{
						selectedItemIndex = i;
					}
				}
				spinner.SetSelection (selectedItemIndex);
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
			Dictionary<string, View> controls,
            object entity,
            List<string> hiddenProperties,
            List<string> unmanagedProperties,
            bool shapeColumnNames,
            bool treatZeroAsNull)
        {
            Dictionary<string, Type> controlMappings = GetTypeNameToControlTypeMappings();
            Type entityType = entity.GetType();
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                string propertyNameMatch = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                if (hiddenProperties.Contains(propertyNameMatch) || unmanagedProperties.Contains(propertyNameMatch))
                {
                    continue; //Don't populate a hidden or unmanaged property from a control.
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
                    continue;
                }
                Type expectedControlType = controlMappings[propertyType.FullName];
                object propertyValue = p.GetValue(entity, null);
				View control = controls [p.Name];
                if (control.GetType() != expectedControlType)
                {
                    throw new ArgumentException(string.Format(
                        "Expected to populate a {0} control for property {1} of type {2} for entity {3}, but received a {4} control to populate.",
                        expectedControlType.FullName,
                        p.Name,
                        propertyType.FullName,
                        entityType.FullName,
                        control.GetType().FullName));
                }
                PopulateEntityProperty(control, p, entity, treatZeroAsNull);
            }
        }

		public static void PopulateEntityProperty(View control, PropertyInfo p, object entity, bool treatZeroAsNull)
        {
            bool isNullable = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

            Type controlType = control.GetType();
            string controlTypeName = controlType.FullName;
            string propertyoTypeName = p.PropertyType.FullName;

			if (controlTypeName.Equals (typeof(EditText).FullName)) 
			{
				EditText editText = (EditText)control;
				EntityReader.SetPropertyValue (p.Name, entity, editText.Text, true);
			} 
			else if (controlTypeName.Equals (typeof(CheckBox).FullName))
			{
				CheckBox checkBox = (CheckBox)control;
				p.SetValue (entity, checkBox.Checked);
			}
			else if (controlTypeName.Equals (typeof(Spinner).FullName))
			{
				Spinner spinner = (Spinner)control;
//				Array enumValues = EnumHelper.GetEnumValues (p.PropertyType);
				p.SetValue (entity, spinner.SelectedItem);
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

        public static void ClearControls(Dictionary<string, View> controls)
        {
            foreach (View control in controls.Values)
            {
                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    checkBox.Checked = false;
                }
                else if (control is EditText)
                {
                    EditText editText = (EditText)control;
                    editText.Text = string.Empty;
                }
                else if (control is Spinner)
                {
                    Spinner spinner = (Spinner)control;
					((ArrayAdapter<object>)spinner.Adapter).Clear ();
					spinner.Adapter = null;
                }
                else
                {
                    //Do nothing
                }
            }
        }
//
//        public static bool AllControlsPopulated(
//            System.Windows.Forms.Control.ControlCollection controls,
//            bool focusOnBlankControl,
//            bool displayErrorMessage)
//        {
//            string errorMessage;
//            return AllControlsPopulated(controls, focusOnBlankControl, displayErrorMessage, out errorMessage);
//        }
//
//        public static bool AllControlsPopulated(
//            System.Windows.Forms.Control.ControlCollection controls,
//            bool focusOnBlankControl,
//            bool displayErrorMessage,
//            out string errorMessage)
//        {
//            List<Control> controlsList = new List<Control>();
//            foreach (Control c in controls)
//            {
//                controlsList.Add(c);
//            }
//            return AllControlsPopulated(controlsList, focusOnBlankControl, displayErrorMessage, out errorMessage);
//        }
//
//        public static bool AllControlsPopulated(
//            List<Control> controls,
//            bool focusOnBlankControl,
//            bool displayErrorMessage)
//        {
//            string errorMessage;
//            return AllControlsPopulated(controls, focusOnBlankControl, displayErrorMessage, out errorMessage);
//        }
//
//        public static bool AllControlsPopulated(
//            List<Control> controls,
//            bool focusOnBlankControl,
//            bool displayErrorMessage,
//            out string errorMessage)
//        {
//            Control blankControl = null;
//            errorMessage = null;
//            foreach (Control c in controls)
//            {
//                Type type = c.GetType();
//                if (c is TextBox && string.IsNullOrEmpty(((TextBox)c).Text))
//                {
//                    blankControl = c;
//                    break;
//                }
//                else if (c is NumericTextBox && string.IsNullOrEmpty(((TextBox)c).Text))
//                {
//                    blankControl.Focus();
//                }
//            }
//            if (blankControl != null)
//            {
//                string friendlyControlName = blankControl.Name.Substring(DataShaper.GetIndexOfFirstUpperCaseLetter(blankControl.Name));
//                errorMessage = string.Format("{0} not entered.", DataShaper.ShapeCamelCaseString(friendlyControlName));
//                if (focusOnBlankControl)
//                {
//                    blankControl.Focus();
//                    return false;
//                }
//            }
//            return true;
//        }

		public static DatePickerDialog CreateDatePickerDialog(
			Context context, 
			TextView titleTextView,
			string cancelButtonText,
			EventHandler<DatePickerDialog.DateSetEventArgs> setButtonCallback,
			EventHandler<DialogClickEventArgs> cancelButtonCallback)
		{
			DateTime date = DateTime.Now;
			DatePickerDialog dialog = new DatePickerDialog(context, setButtonCallback, date.Year, date.Month - 1, date.Day); 
			dialog.SetButton ((int)DialogButtonType.Negative, cancelButtonText, cancelButtonCallback);
			dialog.SetCustomTitle (titleTextView);
			return dialog;
		}

		public static AlertDialog CreateInputDialog(
			Context context,
			TextView titleTextView,
			View inputControlDialogMappped,
			string setButtonText,
			string cancelButtonText,
			EventHandler<DialogClickEventArgs> setButtonCallback,
			EventHandler<DialogClickEventArgs>  cancelButtonCallback)
		{
			AlertDialog.Builder dialogBuilder = new AlertDialog.Builder (context);
			dialogBuilder.SetView (inputControlDialogMappped);
			dialogBuilder.SetPositiveButton (setButtonText, setButtonCallback);
			dialogBuilder.SetNegativeButton (cancelButtonText,  cancelButtonCallback);
			dialogBuilder.SetCustomTitle (titleTextView);
			return dialogBuilder.Create ();
		}

		public static void HideKeyboard(Activity context, EditText inputControl)
		{
			Android.Views.InputMethods.InputMethodManager inputMethodManager = 
				(Android.Views.InputMethods.InputMethodManager)context.GetSystemService (Context.InputMethodService);
			inputMethodManager.HideSoftInputFromWindow (inputControl.WindowToken, Android.Views.InputMethods.HideSoftInputFlags.None);
		}

        #endregion //Methods
    }
}