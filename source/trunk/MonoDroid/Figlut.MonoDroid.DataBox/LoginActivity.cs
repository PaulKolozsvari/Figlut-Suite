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
using Android.Text.Method;
using Figlut.MonoDroid.Toolkit.Utilities.Logging;
using Figlut.MonoDroid.Toolkit.Data;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "Login")]			
	public class LoginActivity : BaseActivity
	{
		#region Fields

		private View _contentView;
		private TextView _txtEditTextDialogTitle;
		private EditText _edtInputEditTextDialogMapped;

		private EditText _edtUserName;
		private EditText _edtPassword;

		private string _userName;
		private string _password;

		private enum LoginInputControl
		{
			UserName,
			Password
		}

		#endregion //Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			try 
			{
				base.OnCreate (bundle);
				_contentView = UIHelper.AddContentLayoutToViewStub(this, Resource.Layout.login_activity_content_layout, Resource.Id.content_stub);
				ApplyTheme(true, true);
				InitializeInputControls();
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
				MenuInflater inflator = new MenuInflater(this);
				inflator.Inflate(Resource.Layout.login_activity_main_menu_layout, menu);
				return true;
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return false;	
			}
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			try 
			{
				base.OnOptionsItemSelected (item);
				switch (item.ItemId) {
				case Resource.Id.menu_item_login:
					Login();
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

		protected override Dialog OnCreateDialog (int id, Bundle args)
		{
			try 
			{
				if(id != FiglutDataBoxApplication.DIALOG_INPUT_TEXT) {
					throw new UserThrownException(string.Format("Unexpected dialog ID {0}.", id.ToString()), LoggingLevel.Minimum);
				}
				if(_txtEditTextDialogTitle == null){
					_txtEditTextDialogTitle = new TextView(this) {
						TextSize = FiglutDataBoxApplication.TEXT_SIZE_DIALOG_TITLE,
					};
				}
				if(_edtInputEditTextDialogMapped == null) {
					_edtInputEditTextDialogMapped = new EditText(this){
						InputType = Android.Text.InputTypes.ClassText
					};
				}
				return UIHelper.CreateInputDialog(
					this,
					_txtEditTextDialogTitle,
					_edtInputEditTextDialogMapped,
					Resources.GetString(Resource.String.set),
					Resources.GetString(Resource.String.cancel),
					delegate(object sender, DialogClickEventArgs e) {
						UpdateUIControlFromMappedControl();
					},
					delegate(object sender, DialogClickEventArgs e) {
						_edtInputEditTextDialogMapped.Tag = null;
						_edtInputEditTextDialogMapped.Text = null;
					});
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
				base.OnPrepareDialog (id, dialog, args);
				LoginInputControl inputControl = (LoginInputControl)args.GetInt(FiglutDataBoxApplication.EXTRA_LOGIN_INPUT_CONTROL);
				_txtEditTextDialogTitle.Text = DataShaper.ShapeCamelCaseString(inputControl.ToString());
				_edtInputEditTextDialogMapped.Tag = (int)inputControl;
				switch (inputControl) {
				case LoginInputControl.UserName:
					_edtInputEditTextDialogMapped.InputType = Android.Text.InputTypes.ClassText;
					_edtInputEditTextDialogMapped.TransformationMethod = SingleLineTransformationMethod.Instance;
					_edtInputEditTextDialogMapped.Text = _userName;
					break;
				case LoginInputControl.Password:
					_edtInputEditTextDialogMapped.InputType = Android.Text.InputTypes.TextVariationPassword;
					_edtInputEditTextDialogMapped.TransformationMethod = PasswordTransformationMethod.Instance;
					_edtInputEditTextDialogMapped.Text = _password;
				default:
				break;
				}
				_edtInputEditTextDialogMapped.RequestFocus();
				_edtInputEditTextDialogMapped.SelectAll();
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		#endregion //Event Handlers

		#region Methods

		protected override void ApplyTheme (bool colors, bool bannerImage)
		{
			base.ApplyTheme (colors, bannerImage);
			using (TextView textLoginTitle = _contentView.FindViewById<TextView> (Resource.Id.text_login_title)) {
				textLoginTitle.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
		}

		private void InitializeInputControls()
		{
			_edtUserName = _contentView.FindViewById<EditText>(Resource.Id.edit_text_user_name);
			_edtPassword = _contentView.FindViewById<EditText>(Resource.Id.edit_text_password);
			_edtUserName.Focusable = false;
			_edtPassword.Focusable = false;

			_edtUserName.InputType = Android.Text.InputTypes.Null;
			_edtPassword.InputType = Android.Text.InputTypes.Null;
			_edtPassword.TransformationMethod = PasswordTransformationMethod.Instance;
			_edtUserName.Click += delegate(object sender, EventArgs e) {
				InputPrompt(LoginInputControl.UserName);
			};
			_edtPassword.Click += delegate(object sender, EventArgs e) {
				InputPrompt(LoginInputControl.Password);
			};
		}

		private void InputPrompt(LoginInputControl inputControl)
		{
			Bundle args = new Bundle ();
			args.PutInt (FiglutDataBoxApplication.EXTRA_LOGIN_INPUT_CONTROL, (int)inputControl);
			ShowDialog (FiglutDataBoxApplication.DIALOG_INPUT_TEXT, args);
		}

		private void UpdateUIControlFromMappedControl()
		{
			int inputControlInt = (int)_edtInputEditTextDialogMapped.Tag;
			LoginInputControl inputControl = (LoginInputControl)inputControlInt;
			switch (inputControl) {
			case LoginInputControl.UserName:
				_edtUserName.Text = _edtInputEditTextDialogMapped.Text;
				_userName = _edtInputEditTextDialogMapped.Text;
				break;
			case LoginInputControl.Password:
				_edtPassword.Text = DataShaper.MaskPasswordString (_edtInputEditTextDialogMapped.Text, '*');
				_password = _edtInputEditTextDialogMapped.Text;
			default:
				break;
			}
			_edtInputEditTextDialogMapped.Tag = null;
			_edtInputEditTextDialogMapped.Text = null;
		}

		private void Login()
		{
			if (string.IsNullOrEmpty (_edtUserName.Text)) {
				Toast.MakeText (this, "User Name not entered.", ToastLength.Long).Show ();
				return;
			}
			Intent intentMainActivity = new Intent (this, typeof(MainActivity));
			intentMainActivity.PutExtra (FiglutDataBoxApplication.EXTRA_LOGIN_USER_NAME, _userName);
			intentMainActivity.PutExtra (FiglutDataBoxApplication.EXTRA_LOGIN_PASSWORD, _password);
			SetResult (Result.Ok, intentMainActivity);
			Finish ();
		}

		#endregion //Methods
	}
}

