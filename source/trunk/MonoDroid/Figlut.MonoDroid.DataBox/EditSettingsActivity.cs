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
using Figlut.MonoDroid.DataBox.Utilities;
using Figlut.MonoDroid.Toolkit.Utilities.SettingsFile;
using System.IO;
using Figlut.MonoDroid.Toolkit.Data;
using Figlut.MonoDroid.DataBox.Configuration;
using Android.Provider;
using Android.Database;
using Android.Text.Method;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "Edit Settings")]			
	public class EditSettingsActivity : BaseActivity
	{
		#region Fields

		private View _contentView;
		private ListView _listEditSettings;
		private TextView _txtSettingsCategoryTitle;

		private TextView _txtEditTextDialogTitle;
		private TextView _txtCheckBoxDialogTitle;
		private TextView _txtSpinnerDialogTitle;
		private TextView _txtWholeNumberDialogTitle;

		private EditText _inputEditTextDialogMapped;
		private CheckBox _inputCheckBoxDialogMappped;
		private Spinner _inputSpinnerDialogMapped;
		private EditText _inputWholeNumberDialogMapped;

		private string _currentSettingsCategoryName;

		#endregion //Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			try 
			{
				base.OnCreate (bundle);
				_contentView = UIHelper.AddContentLayoutToViewStub (this, Resource.Layout.edit_settings_acitivity_content_layout,  Resource.Id.content_stub);
				ApplyTheme (true, true);

				_listEditSettings = _contentView.FindViewById<ListView> (Resource.Id.list_edit_settings);
				_txtSettingsCategoryTitle = _contentView.FindViewById<TextView> (Resource.Id.text_settings_category_title);
				_currentSettingsCategoryName = Intent.GetStringExtra (FiglutDataBoxApplication.EXTRA_SETTINGS_CATEGORY_NAME);
				_txtSettingsCategoryTitle.Text = DataShaper.ShapeCamelCaseString(_currentSettingsCategoryName);
			} catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnResume ()
		{
			try 
			{
				base.OnResume ();
				_listEditSettings.ItemClick += OnSettingsCategoryItemClick;
				RefreshCategorySettings ();
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnPause ()
		{
			try
			{
				base.OnPause ();
				_listEditSettings.ItemClick -= OnSettingsCategoryItemClick;
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		private void OnSettingsCategoryItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			try 
			{
				Java.Lang.Object o = _listEditSettings.GetItemAtPosition (e.Position);
				SettingsCategoryItem i = (SettingsCategoryItem)o;
				SettingItem selectedSetting = ConfigurationManagerData.Instance.CurrentCategorySettings [i.SettingName];
				if (selectedSetting.SelectFilePath) {
					SelectFilePath (selectedSetting);
					return;
				}
				int dialogId;
				if (selectedSetting.SettingType.IsEnum)
				{
					dialogId = FiglutDataBoxApplication.DIALOG_INPUT_SPINNER;
				} 
				else if (selectedSetting.SettingType.Equals (typeof(Boolean))) 
				{
					dialogId = FiglutDataBoxApplication.DIALOG_INPUT_CHECK_BOX;
				} 
				else if (selectedSetting.SettingType.Equals (typeof(Int16)) ||
					selectedSetting.SettingType.Equals (typeof(Int32)) ||
					selectedSetting.SettingType.Equals (typeof(Int64))) 
				{
					dialogId = FiglutDataBoxApplication.DIALOG_INPUT_NUMBER_WHOLE;
				}
				else 
				{
					dialogId = FiglutDataBoxApplication.DIALOG_INPUT_TEXT;
				}
				Bundle args = new Bundle ();
				args.PutString (FiglutDataBoxApplication.EXTRA_SETTINGS_NAME, selectedSetting.SettingName);
				ShowDialog (dialogId, args);
			} catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			try 
			{
				base.OnActivityResult (requestCode, resultCode, data);
				if (resultCode != Result.Ok) {
					return;
				}
				if (requestCode == FiglutDataBoxApplication.REQUEST_CODE_SELECT_FILE_PATH) {
					UpdateNewSettingFilePath (data);
				}
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override Dialog OnCreateDialog (int id, Bundle args)
		{
			try 
			{
				switch (id) {
				case FiglutDataBoxApplication.DIALOG_INPUT_TEXT:
					return CreateEditTextInputDialog ();
				case FiglutDataBoxApplication.DIALOG_INPUT_NUMBER_WHOLE:
					return CreateWholeNumberInputDialog ();
				case FiglutDataBoxApplication.DIALOG_INPUT_CHECK_BOX:
					return CreateCheckBoxInputDialog ();
				case FiglutDataBoxApplication.DIALOG_INPUT_SPINNER:
					return CreateSpinnerInputDialog ();
				default:
					throw new Exception (string.Format ("Unexpected dialog ID {0}.", id.ToString ()));
				}
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return null;
			}
		}

		protected override void OnPrepareDialog (int id, Dialog dialog, Bundle args)
		{
			try
			{
				string settingName = args.GetString (FiglutDataBoxApplication.EXTRA_SETTINGS_NAME);
				ConfigurationManagerData.Instance.CurrentSettingName = settingName;
				SettingItem setting = ConfigurationManagerData.Instance.CurrentCategorySettings [settingName];
				switch (id) {
				case FiglutDataBoxApplication.DIALOG_INPUT_TEXT:
					if(setting.PasswordChar != '\0')
					{
						_inputEditTextDialogMapped.InputType = Android.Text.InputTypes.TextVariationPassword;
						_inputEditTextDialogMapped.TransformationMethod = PasswordTransformationMethod.Instance;
					}
					_txtEditTextDialogTitle.Text = setting.SettingDisplayName;
					_inputEditTextDialogMapped.Text = ConfigurationManagerData.Instance.CurrentCategorySettings [settingName].SettingValue.ToString ();
					_inputEditTextDialogMapped.RequestFocus ();
					_inputEditTextDialogMapped.SelectAll ();
					break;
				case FiglutDataBoxApplication.DIALOG_INPUT_NUMBER_WHOLE:
					_txtWholeNumberDialogTitle.Text = setting.SettingDisplayName;
					_inputWholeNumberDialogMapped.Text = ConfigurationManagerData.Instance.CurrentCategorySettings [settingName].SettingValue.ToString ();
					_inputWholeNumberDialogMapped.RequestFocus ();
					_inputWholeNumberDialogMapped.SelectAll ();
					break;
				case FiglutDataBoxApplication.DIALOG_INPUT_CHECK_BOX:
					_txtCheckBoxDialogTitle.Text = setting.SettingDisplayName;
					_inputCheckBoxDialogMappped.Checked = Convert.ToBoolean (ConfigurationManagerData.Instance.CurrentCategorySettings [settingName].SettingValue);
					_inputCheckBoxDialogMappped.RequestFocus ();
					break;
				case FiglutDataBoxApplication.DIALOG_INPUT_SPINNER:
					_txtSpinnerDialogTitle.Text = setting.SettingDisplayName;
					PopulateSpinnerInputControl (setting, _inputSpinnerDialogMapped);
					_inputSpinnerDialogMapped.RequestFocus ();
				default:
					break;
				}
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			try 
			{
				base.OnCreateOptionsMenu (menu);
				MenuInflater inflator = new MenuInflater(this);
				inflator.Inflate(Resource.Layout.config_manager_main_menu_layout, menu);
				return true;	
			} catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return false;
			}
		}

		public override bool OnMenuItemSelected (int featureId, IMenuItem item)
		{
			try 
			{
				base.OnMenuItemSelected (featureId, item);
				switch (item.ItemId) {
				case Resource.Id.menu_item_reset_all_settings:
					FiglutMonoDroidDataBoxSettings settings	= GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings>();
					settings.ResetSettingsCategory(_currentSettingsCategoryName);
					settings.SaveToFile();
					ConfigurationManagerData.Instance.CurrentCategorySettings = settings.GetSettingsByCategory (new SettingsCategoryInfo (settings, _currentSettingsCategoryName));
					FiglutDataBoxApplication.Instance.RefreshImageBannerFromSettings();
					FiglutDataBoxApplication.Instance.RefreshThemeColorFromSettings();
					ApplyTheme(true, true);
					RefreshCategorySettings();
					break;
				default:
				break;
				}
				return true;
			}
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return false;	
			}
		}

		#endregion //Event Handlers

		#region Methods

		protected override void ApplyTheme (bool colors, bool bannerImage)
		{
			base.ApplyTheme (colors, bannerImage);
			using (TextView textSettingsCategoryTitle = _contentView.FindViewById<TextView> (Resource.Id.text_settings_category_title)) {
				textSettingsCategoryTitle.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
			using (LinearLayout headerLayout = _contentView.FindViewById<LinearLayout> (Resource.Id.header_layout)) {
				headerLayout.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
			using (TextView textSettingName = _contentView.FindViewById<TextView> (Resource.Id.text_setting_name)) {
				textSettingName.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
			using (TextView textSettingValue = _contentView.FindViewById<TextView> (Resource.Id.text_setting_value)) {
				textSettingValue.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
		}

		private void RefreshCategorySettings()
		{
			ConfigurationManagerData.Instance.CurrentSettingName = null;
			List<SettingsCategoryItem> result = new List<SettingsCategoryItem> ();
			foreach (SettingItem i in ConfigurationManagerData.Instance.CurrentCategorySettings) 
			{
				if (!i.Visible) {
					continue;
				}
				result.Add (new SettingsCategoryItem ()
					{ 
						SettingDisplayName = i.SettingDisplayName, 
						SettingName = i.SettingName,
						SettingValue = i.PasswordChar != '\0' ? DataShaper.MaskPasswordString (i.SettingValue.ToString (), i.PasswordChar) : i.SettingValue
					});
			}
			_listEditSettings.Adapter = new SettingsCategoryItemListAdapter (this, result);
		}

		private void SelectFilePath(SettingItem settingItem)
		{
			ConfigurationManagerData.Instance.CurrentSettingName = settingItem.SettingName;
			Intent intentBrowseFile = new Intent (Intent.ActionGetContent);
			intentBrowseFile.SetType ("image/*");
			intentBrowseFile.AddCategory (Intent.CategoryOpenable);
			StartActivityForResult (intentBrowseFile, (int)FiglutDataBoxApplication.REQUEST_CODE_SELECT_FILE_PATH);
		}

		private void UpdateNewSettingFilePath(Intent data)
		{
			Android.Net.Uri uri = data.Data;
			string selectedFilePath = FileSystemHelper.GetRealFilePathFromUri (uri, this);

			//Update the settings (in memory and file).
			ConfigurationManagerData.Instance.SetSettingValue (ConfigurationManagerData.Instance.CurrentSettingName, selectedFilePath, true);
			if (ConfigurationManagerData.Instance.CurrentSettingName ==
				EntityReader<FiglutMonoDroidDataBoxSettings>.GetPropertyName (p => p.ApplicationBannerImageFilePath, false)) {
				string resultMessage = FiglutDataBoxApplication.Instance.RefreshImageBannerFromSettings ();
				if (!string.IsNullOrEmpty (resultMessage)) {
					Toast.MakeText (this, resultMessage, ToastLength.Long).Show ();
				}
				ApplyTheme (false, true);
			}
			RefreshCategorySettings();
		}

		private AlertDialog CreateEditTextInputDialog()
		{
			if (_txtEditTextDialogTitle == null) {
				_txtEditTextDialogTitle = new TextView (this){ TextSize = FiglutDataBoxApplication.TEXT_SIZE_DIALOG_TITLE };
			}
			if (_inputEditTextDialogMapped == null) {
				_inputEditTextDialogMapped = new EditText (this);
				_inputEditTextDialogMapped.InputType = Android.Text.InputTypes.ClassText;
			}
			return UIHelper.CreateInputDialog (
				this,
				_txtEditTextDialogTitle,
				_inputEditTextDialogMapped,
				Resources.GetString(Resource.String.set),
				Resources.GetString(Resource.String.cancel),
				delegate(object sender, DialogClickEventArgs e) {
					ConfigurationManagerData.Instance.SetSettingValue (ConfigurationManagerData.Instance.CurrentSettingName, _inputEditTextDialogMapped.Text, true);
					RefreshCategorySettings();
					if(_txtEditTextDialogTitle.InputType != Android.Text.InputTypes.ClassText)
					{
						_inputEditTextDialogMapped.InputType = Android.Text.InputTypes.ClassText;
						_inputEditTextDialogMapped.TransformationMethod = SingleLineTransformationMethod.Instance;
					}
				},
				delegate(object sender, DialogClickEventArgs e) {
					ConfigurationManagerData.Instance.CurrentSettingName = null;
				});
		}

		private AlertDialog CreateWholeNumberInputDialog()
		{
			if (_txtWholeNumberDialogTitle == null) {
				_txtWholeNumberDialogTitle = new TextView (this){ TextSize = FiglutDataBoxApplication.TEXT_SIZE_DIALOG_TITLE };
			}
			if (_inputWholeNumberDialogMapped == null) {
				_inputWholeNumberDialogMapped = new EditText (this);
				_inputWholeNumberDialogMapped.InputType = Android.Text.InputTypes.ClassNumber;
			}
			return UIHelper.CreateInputDialog (
				this,
				_txtWholeNumberDialogTitle,
				_inputWholeNumberDialogMapped,
				Resources.GetString(Resource.String.set),
				Resources.GetString(Resource.String.cancel),
				delegate(object sender, DialogClickEventArgs e) {
					ConfigurationManagerData.Instance.SetSettingValue (ConfigurationManagerData.Instance.CurrentSettingName, _inputWholeNumberDialogMapped.Text, true);
					RefreshCategorySettings();
				},
				delegate(object sender, DialogClickEventArgs e) {
					ConfigurationManagerData.Instance.CurrentSettingName = null;
				});
		}

		private AlertDialog CreateCheckBoxInputDialog()
		{
			if (_txtCheckBoxDialogTitle == null) {
				_txtCheckBoxDialogTitle = new TextView (this) { TextSize = FiglutDataBoxApplication.TEXT_SIZE_DIALOG_TITLE };
			}
			if (_inputCheckBoxDialogMappped == null) {
				_inputCheckBoxDialogMappped = new CheckBox (this);
			}
			return UIHelper.CreateInputDialog (
				this,
				_txtCheckBoxDialogTitle,
				_inputCheckBoxDialogMappped,
				Resources.GetString(Resource.String.set),
				Resources.GetString(Resource.String.cancel),
				delegate(object sender, DialogClickEventArgs e) {
					ConfigurationManagerData.Instance.SetSettingValue (ConfigurationManagerData.Instance.CurrentSettingName, _inputCheckBoxDialogMappped.Checked, true);
					RefreshCategorySettings();
				},
				delegate(object sender, DialogClickEventArgs e) {
					ConfigurationManagerData.Instance.CurrentSettingName = null;
				});
		}

		private AlertDialog CreateSpinnerInputDialog()
		{
			if (_txtSpinnerDialogTitle == null) {
				_txtSpinnerDialogTitle = new TextView (this) { TextSize = FiglutDataBoxApplication.TEXT_SIZE_DIALOG_TITLE };
			}
			if (_inputSpinnerDialogMapped == null) {
				_inputSpinnerDialogMapped = new Spinner (this);
			}
			return UIHelper.CreateInputDialog (
				this,
				_txtSpinnerDialogTitle,
				_inputSpinnerDialogMapped,
				Resources.GetString(Resource.String.set),
				Resources.GetString(Resource.String.cancel),
				delegate(object sender, DialogClickEventArgs e) {
					ConfigurationManagerData.Instance.SetSettingValue (ConfigurationManagerData.Instance.CurrentSettingName, _inputSpinnerDialogMapped.SelectedItem, true);
					if(ConfigurationManagerData.Instance.CurrentSettingName == 
						EntityReader<FiglutMonoDroidDataBoxSettings>.GetPropertyName(p => p.ThemeColor, false))
					{
						FiglutDataBoxApplication.Instance.RefreshThemeColorFromSettings();
						ApplyTheme(true, false);
					}
					RefreshCategorySettings();
				},
				delegate(object sender, DialogClickEventArgs e) {
					ConfigurationManagerData.Instance.CurrentSettingName = null;
				});
		}

		private void PopulateSpinnerInputControl(SettingItem settingItem, Spinner spinner)
		{
			Array enumValues = EnumHelper.GetEnumValues (settingItem.SettingType);
			int selectedIndex = -1;
			ArrayAdapter<Enum> adapter = new ArrayAdapter<Enum> (this, Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
			for (int i = 0; i < enumValues.Length; i++) {
				Enum en = (Enum)enumValues.GetValue (i);
				adapter.Add (en);
				if (en.ToString () == settingItem.SettingValue.ToString ()) {
					selectedIndex = i;
				}
			}
			if (selectedIndex > -1) {
				spinner.SetSelection (selectedIndex, true);
			}
		}

		#endregion //Methods
	}
}

