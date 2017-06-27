#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Figlut.MonoDroid.Toolkit.Utilities;
using Figlut.MonoDroid.DataBox.Configuration;
using Figlut.MonoDroid.DataBox.Utilities;
using Figlut.MonoDroid.Toolkit.Data;
using Figlut.MonoDroid.Toolkit.Data.DB.SQLServer;
using System.Reflection;
using Figlut.MonoDroid.Toolkit.Utilities.Logging;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "Manage Entity")]			
	public class ManageEntityActivity : BaseActivity
	{
		#region Fields

		private FiglutMonoDroidDataBoxSettings _settings;

		private View _contentView;
		private LinearLayout _layoutInputControls;
		private TextView _txtManageEntityTitle;

		private TextView _txtDateDialogTitle;
		private TextView _txtEditTextDialogTitle;

		private EntityOperation _entityOperation;
		private EditText _inputControlDialogMappped;

		#endregion //Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			try 
			{
				base.OnCreate (bundle);
				_contentView = UIHelper.AddContentLayoutToViewStub (this, Resource.Layout.manage_entity_content_layout, Resource.Id.content_stub);
				ApplyTheme (true, true);

				_layoutInputControls = (LinearLayout)_contentView.FindViewById<LinearLayout> (Resource.Id.layout_manage_entity_input_controls);
				_txtManageEntityTitle = _contentView.FindViewById<TextView> (Resource.Id.text_manage_entity_title);
				_entityOperation = (EntityOperation)Intent.GetIntExtra (FiglutDataBoxApplication.EXTRA_ENTITY_OPERATION, -1);
				_txtManageEntityTitle.Text = string.Format ("{0} {1}", _entityOperation.ToString (), DataShaper.ShapeCamelCaseString(DataBoxData.Instance.CurrentTable.TableName));
				_txtManageEntityTitle = new TextView (this);
				_settings = GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings> ();
				RefreshInputControls ();
				if(_entityOperation == EntityOperation.Update)
				{
					RefreshInputControlValuesFromEntity ();
				}
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		/// <summary>
		/// This method only gets called once per dialog ID to determine what dialog is needed and then it gets created.
		/// While OnPrepareDialog gets called everytime the ShowDialog method is called.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="args">Arguments.</param>
		protected override Dialog OnCreateDialog (int id, Bundle args)
		{
			try 
			{
				switch (id) {
				case FiglutDataBoxApplication.DIALOG_INPUT_DATE:
					return CreateDateInputDialog ();
				case FiglutDataBoxApplication.DIALOG_INPUT_TEXT:
					return CreateEditTextInputDialog ();
				default:
					throw new Exception (string.Format ("Unexpected dialog ID {0}.", id.ToString ()));
				}
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return null;
			}
		}

		/// <summary>
		/// OnPrepareDialog gets called everytime the ShowDialog method is called.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="dialog">Dialog.</param>
		/// <param name="args">Arguments.</param>
		protected override void OnPrepareDialog (int id, Dialog dialog, Bundle args)
		{
			try 
			{
				string inputControlName = args.GetString (FiglutDataBoxApplication.EXTRA_INPUT_CONTROL_NAME);
				Android.Text.InputTypes inputType = (Android.Text.InputTypes)args.GetInt (FiglutDataBoxApplication.EXTRA_INPUT_TYPE);
				if(id == FiglutDataBoxApplication.DIALOG_INPUT_DATE)
				{
					_txtDateDialogTitle.Text = DataShaper.ShapeCamelCaseString (inputControlName);
					DatePickerDialog datePickerDialog = (DatePickerDialog)dialog;
					string dateString = ((EditText)DataBoxData.Instance.InputControls [inputControlName]).Text;
					if (!string.IsNullOrEmpty (dateString)) 
					{
						DateTime date = DateTime.Parse (dateString);
						datePickerDialog.UpdateDate (date);
					}
				}
				else if(id == FiglutDataBoxApplication.DIALOG_INPUT_TEXT)
				{
					_txtEditTextDialogTitle.Text = DataShaper.ShapeCamelCaseString (inputControlName);
					EditText inputControl = (EditText)DataBoxData.Instance.InputControls [inputControlName];
					_inputControlDialogMappped.Text = inputControl.Text;
					_inputControlDialogMappped.InputType = inputType;
					if (!_inputControlDialogMappped.HasFocus) 
					{
						_inputControlDialogMappped.RequestFocus ();
					} 
					_inputControlDialogMappped.SelectAll ();
				}
				DataBoxData.Instance.CurrentInputControlName = inputControlName;	
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}


		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			try 
			{
				base.OnCreateOptionsMenu (menu);
				MenuInflater menuInflator = new Android.Views.MenuInflater (this);
				menuInflator.Inflate (Resource.Layout.manage_entity_main_menu_layout, menu);
				return true;	
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return false;
			}
		}

		public override bool OnMenuItemSelected (int featureId, IMenuItem item)
		{
			try
			{
				base.OnMenuItemSelected (featureId, item);
				if (item.ItemId != Resource.Id.menu_item_apply)
				{
					throw new ArgumentException (string.Format ("Invalid menu item resource ID {0}.", item.ItemId.ToString ()));
				}
				switch (_entityOperation) 
				{
				case EntityOperation.Update:
					UpdateEntity ();
					break;
				case EntityOperation.Add:
					AddEntity ();
					break;
				default:
					break;
				}
				return true;
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return false;
			}
		}

		#endregion //Event Handlers

		#region Methods

		protected override void ApplyTheme (bool colors, bool bannerImage)
		{
			base.ApplyTheme (true, true);
			using (TextView textManageEntityTitle = _contentView.FindViewById<TextView> (Resource.Id.text_manage_entity_title)) {
				textManageEntityTitle.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
		}

		private void RefreshInputControls()
		{
			DataBoxData.Instance.InputControls = UIHelper.GetControlsForEntity (
				this, 
				DataBoxData.Instance.CurrentTable.MappedType,
				true,
				Android.Graphics.Color.WhiteSmoke,
				DataBoxData.Instance.HiddenProperties,
				new List<string> (),
				_settings.ShapeColumnNames);
			foreach (string inputControlName in DataBoxData.Instance.InputControls.Keys) //Hook onto the Click event if a EditText is clicked which is managing a DateTime field.
			{
				EditText editText = DataBoxData.Instance.InputControls [inputControlName] as EditText;
				if(editText == null)
				{
					continue;
				}
				editText.Focusable = false;
				editText.Tag = new InputControlInfoHolder () { InputControlName = inputControlName, InputType = editText.InputType };
				editText.InputType = Android.Text.InputTypes.Null; //Makes the EditText read only.
				editText.FocusChange += delegate(object sender, View.FocusChangeEventArgs e) {
					if(e.HasFocus){ ShowDialogForInputControl((EditText)sender); }
				};
				editText.Click += delegate(object sender, EventArgs e) {
					ShowDialogForInputControl((EditText)sender);
				};
			}
			DataBoxData.Instance.InputControls.Values.ToList ().ForEach (p => _layoutInputControls.AddView (p));
		}

		private void ShowDialogForInputControl(EditText inputControl)
		{
			InputControlInfoHolder inputControlInfo = (InputControlInfoHolder)inputControl.Tag;
			Bundle args = new Bundle();
			args.PutString(FiglutDataBoxApplication.EXTRA_INPUT_CONTROL_NAME, inputControlInfo.InputControlName);
			args.PutInt (FiglutDataBoxApplication.EXTRA_INPUT_TYPE, (int)inputControlInfo.InputType);
			int dialogId = 
				inputControlInfo.InputType == Android.Text.InputTypes.ClassDatetime ?
					FiglutDataBoxApplication.DIALOG_INPUT_DATE :
					FiglutDataBoxApplication.DIALOG_INPUT_TEXT;
			ShowDialog (dialogId, args); //This will call the OnCreateDialog call back method asking what dialog we want to show.
		}

		private AlertDialog CreateDateInputDialog()
		{
			if (_txtDateDialogTitle == null) {
				_txtDateDialogTitle = new TextView (this){ TextSize = FiglutDataBoxApplication.TEXT_SIZE_DIALOG_TITLE };
			}
			return UIHelper.CreateDatePickerDialog (
				this,
				_txtDateDialogTitle,
				Resources.GetString (Resource.String.cancel),
				delegate(object sender, DatePickerDialog.DateSetEventArgs e) {
					((EditText)DataBoxData.Instance.InputControls[DataBoxData.Instance.CurrentInputControlName]).Text = e.Date.ToString();
					DataBoxData.Instance.CurrentInputControlName = null;
				},
				delegate(object sender, DialogClickEventArgs e) {
					DataBoxData.Instance.CurrentInputControlName = null;
				});
		}

		private AlertDialog CreateEditTextInputDialog()
		{
			if (_txtEditTextDialogTitle == null) {
				_txtEditTextDialogTitle = new TextView (this){ TextSize = FiglutDataBoxApplication.TEXT_SIZE_DIALOG_TITLE };
			}
			if (_inputControlDialogMappped == null) {
				_inputControlDialogMappped = new EditText (this);
			}
			return UIHelper.CreateInputDialog (
				this,
				_txtEditTextDialogTitle,
				_inputControlDialogMappped, 
				Resources.GetString (Resource.String.set), 
				Resources.GetString (Resource.String.cancel),
				delegate(object sender, DialogClickEventArgs e) {
					((EditText)DataBoxData.Instance.InputControls[DataBoxData.Instance.CurrentInputControlName]).Text = _inputControlDialogMappped.Text;
					DataBoxData.Instance.CurrentInputControlName = null;
				},
				delegate(object sender, DialogClickEventArgs e) {
					DataBoxData.Instance.CurrentInputControlName = null;
				});
		}

		private void ClearInputControls()
		{
			DataBoxData.Instance.InputControls.Values.ToList ().ForEach (p => _layoutInputControls.RemoveView (p));
		}

		private void RefreshInputControlValuesFromEntity()
		{
			UIHelper.PopulateControlsFromEntity (
				this,
				DataBoxData.Instance.InputControls,
				DataBoxData.Instance.EntityUnderUpdate,
				DataBoxData.Instance.HiddenProperties,
				new List<string> (),
				_settings.ShapeColumnNames);
		}

		private void UpdateEntity()
		{
			UIHelper.PopulateEntityFromControls (
				DataBoxData.Instance.InputControls,
				DataBoxData.Instance.EntityUnderUpdate,
				DataBoxData.Instance.HiddenProperties,
				new List<string> (),
				_settings.ShapeColumnNames,
				_settings.TreatZeroAsNull);

			Type entityType = DataBoxData.Instance.EntityUnderUpdate.GetType();
			foreach (SqlDatabaseTableColumn column in DataBoxData.Instance.CurrentTable.Columns) //Check if all properties, that are not nullable, have been set.
			{
				PropertyInfo p = entityType.GetProperty(column.ColumnName);
				object value = p.GetValue(DataBoxData.Instance.EntityUnderUpdate, null);
				if ((value == null ||
					string.IsNullOrEmpty(value.ToString()) ||
					(p.PropertyType == typeof(Guid) && ((Guid)value) == Guid.Empty) ||
					(_settings.TreatZeroAsNull && value.ToString() == "0")) &&
					!column.IsNullable)
				{
					if (!column.IsForeignKey)
					{
						DataBoxData.Instance.InputControls [p.Name].RequestFocus ();
					}
					throw new UserThrownException(string.Format("{0} must be specified.", DataShaper.ShapeCamelCaseString(column.ColumnName)), LoggingLevel.Maximum);
				}
			}
			UIHelper.ClearControls (DataBoxData.Instance.InputControls);
			DataBoxData.Instance.CurrentEntityCache.NotifyEntityUpdated (
				DataBoxData.Instance.EntityIdUnderUpdate.Value, 
				DataBoxData.Instance.EntityUnderUpdate);
			DataBoxData.Instance.ManageEntityChangesMade = true;
			DataBoxData.Instance.EntityUnderUpdate = null;
			DataBoxData.Instance.EntityIdUnderUpdate = null;
			SetResult (Result.Ok);
			Finish ();
		}

		private void AddEntity()
		{
			UIHelper.PopulateEntityFromControls (
				DataBoxData.Instance.InputControls,
				DataBoxData.Instance.EntityUnderUpdate,
				DataBoxData.Instance.HiddenProperties,
				new List<string> (),
				_settings.ShapeColumnNames,
				_settings.TreatZeroAsNull);

			Type type = DataBoxData.Instance.EntityUnderUpdate.GetType ();
			foreach (SqlDatabaseTableColumn column in DataBoxData.Instance.CurrentTable.Columns)
			{
				PropertyInfo p = type.GetProperty(column.ColumnName);
				if (column.IsKey && (p.PropertyType == typeof(Guid)))
				{
					p.SetValue(DataBoxData.Instance.EntityUnderUpdate, Guid.NewGuid(), null); //Set the GUID primary key.
					continue;
				}
				object value = p.GetValue(DataBoxData.Instance.EntityUnderUpdate, null);
				if ((
					(((value == null) || string.IsNullOrEmpty(value.ToString())) || ((p.PropertyType == typeof(Guid)) && (((Guid)value) == Guid.Empty))) || 
					(_settings.TreatZeroAsNull && (value.ToString() == "0"))) && 
					!column.IsNullable)
				{
					if (!column.IsForeignKey)
					{
						DataBoxData.Instance.InputControls [p.Name].RequestFocus ();
					}
					throw new UserThrownException(string.Format("{0} must be specified.", DataShaper.ShapeCamelCaseString(column.ColumnName)), LoggingLevel.Maximum);
				}
			}
			DataBoxData.Instance.CurrentEntityCache.Add(DataBoxData.Instance.EntityUnderUpdate);
			DataBoxData.Instance.ManageEntityChangesMade = true;
			UIHelper.ClearControls(DataBoxData.Instance.InputControls);
			if (_settings.CloseAddWindowAfterAdd)
			{
				SetResult (Result.Ok);
				Finish ();
			}
			else
			{
				DataBoxData.Instance.EntityUnderUpdate = null;
				DataBoxData.Instance.EntityUnderUpdate = Activator.CreateInstance(DataBoxData.Instance.CurrentTable.MappedType);
			}
		}

		#endregion //Methods
	}
}