#region Using Directives

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Figlut.MonoDroid.Toolkit.Utilities;
using Figlut.MonoDroid.DataBox.Configuration;
using System.Net;
using Figlut.MonoDroid.Toolkit.Utilities.Logging;
using System.IO;
using System.Reflection;
using Figlut.MonoDroid.Toolkit.Utilities.Serialization;
using Figlut.MonoDroid.Toolkit.Data.DB.SQLServer;
using Figlut.MonoDroid.DataBox.Utilities;
using Figlut.MonoDroid.Toolkit.Data;
using Figlut.MonoDroid.Toolkit;
using Android.Graphics;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "Figlut Android DataBox", MainLauncher = true)]
	public class MainActivity : BaseActivity
	{
		#region Fields

		private View _contentView;
		private ListView _listMainMenu;

		#endregion //Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			try 
			{
 				base.OnCreate (bundle);
				_contentView = UIHelper.AddContentLayoutToViewStub (this, Resource.Layout.main_activity_content_layout, Resource.Id.content_stub);
				if(bundle == null) //This is the first time the application has started.
				{
					try
					{
						GOC.Instance.ShowMessageBoxOnException = true;
						InitializeApplication();
					}
					catch(Exception ex) {
						ExceptionHandler.HandleException (ex, this);
					}
				}
				_listMainMenu = _contentView.FindViewById<ListView> (Resource.Id.list_main_menu);
				FiglutDataBoxApplication.Instance.RefreshImageBannerFromSettings();
				FiglutDataBoxApplication.Instance.RefreshThemeColorFromSettings();
				ApplyTheme (true, true);
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (new UserThrownException (ex.Message, LoggingLevel.Minimum, false), this);
			}
		}

		protected override void OnResume ()
		{
			try
			{
				base.OnResume ();
				_listMainMenu.ItemClick += OnMainMenuItemClick;
				_listMainMenu.Adapter = new MainMenuItemListAdapter (this, GetMenuItems ());
				DataBoxData.Instance.ClearData (); //Clear all the DataBoxData whenever this main activity comes into the foreground again i.e. other activities have been closed.
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			_listMainMenu.ItemClick -= OnMainMenuItemClick;
		}

		private void OnMainMenuItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			try
			{
				Java.Lang.Object o = _listMainMenu.GetItemAtPosition(e.Position);
				MainMenuItem menuItem = (MainMenuItem)o;
				if (menuItem.Name == Resources.GetString (Resource.String.data_box))
				{
					if(GOC.Instance.DataBaseCount < 1)
					{
						Toast.MakeText(
							this,
							"Schema not initialized. Configure Web Service settings to point to the correct URL and then Reinitialize.",
							ToastLength.Long).Show();
						return;
					}
					Intent intentLoadDataBox = new Intent (this, typeof(LoadDataBoxActivity));
					intentLoadDataBox.PutExtra (FiglutDataBoxApplication.EXTRA_DATABOX_NAMES, GOC.Instance.GetDatabase<SqlDatabase> ().Tables.GetEntitiesKeys ().ToArray ());
					StartActivityForResult (intentLoadDataBox, FiglutDataBoxApplication.REQUEST_CODE_LOAD_DATA_BOX);
				}
				else if (menuItem.Name == Resources.GetString (Resource.String.configuration_manager)) 
				{
					StartActivityForResult (typeof(ConfigurationManagerActivity), FiglutDataBoxApplication.REQUEST_CODE_CONFIGURATION_MANAGER);
				}
				else if(menuItem.Name == Resources.GetString(Resource.String.log_viewer))
				{
					StartActivity(typeof(LogViewerActivity));
				}
				else if(menuItem.Name == Resources.GetString(Resource.String.reinitialize))
				{
					InitializeApplication();
					ApplyTheme(true, true);
				}
				else if(menuItem.Name == Resources.GetString(Resource.String.exit))
				{
					this.Finish ();
				}
				else
				{
					Toast.MakeText(this, menuItem.Name, ToastLength.Short).Show();
				}
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			try
			{
				base.OnActivityResult (requestCode, resultCode, data);
				if (requestCode == FiglutDataBoxApplication.REQUEST_CODE_CONFIGURATION_MANAGER) //Always apply the theme if coming back from the the config manager activity.
				{
					ApplyTheme (true, true);
					return;
				}
				if (resultCode != Result.Ok)
				{
					return;
				}
				if(requestCode == FiglutDataBoxApplication.REQUEST_CODE_LOGIN)
				{
					InitializeDatabaseSchema(
						data.GetStringExtra(FiglutDataBoxApplication.EXTRA_LOGIN_USER_NAME),
						data.GetStringExtra(FiglutDataBoxApplication.EXTRA_LOGIN_PASSWORD),
						GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings>());
				}
				else if (requestCode == FiglutDataBoxApplication.REQUEST_CODE_LOAD_DATA_BOX) 
				{
					string dataBoxSelected = data.GetStringExtra (FiglutDataBoxApplication.EXTRA_SELECTED_DATA_BOX);
					Intent intentDataBox = new Intent (this, typeof(DataBoxActivity));
					intentDataBox.PutExtra (FiglutDataBoxApplication.EXTRA_SELECTED_DATA_BOX, dataBoxSelected);
					StartActivity (intentDataBox);
				}
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		#endregion //Event Handlers

		#region Methods

		protected override void ApplyTheme (bool colors, bool bannerImage)
		{
			base.ApplyTheme (colors, bannerImage);
			FiglutMonoDroidDataBoxSettings settings = GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings> ();
			using (TextView textMainTitle = _contentView.FindViewById<TextView> (Resource.Id.text_main_title)) {
				textMainTitle.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
				this.Title = string.Format ("{0} {1}", settings.ApplicationTitle, settings.ApplicationVersion);
				textMainTitle.Text = settings.ApplicationTitle;
			}
		}

		private FiglutMonoDroidDataBoxSettings InitializeSettings()
		{
			FiglutMonoDroidDataBoxSettings settings = new FiglutMonoDroidDataBoxSettings ();
			if (File.Exists (settings.FilePath))
			{
//				File.Delete (settings.FilePath);
				return GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings> (true, false);
			}
			settings.ResetAllSettings ();
			settings.SaveToFile ();
			return GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings> (true, false);
		}

		public void InitializeApplication()
		{
			FiglutMonoDroidDataBoxSettings settings = InitializeSettings ();
			string userName = settings.AuthenticationUserName;
			string password = settings.AuthenticationPassword;

			//N.B. A different combination of data formats will not work.
			settings.FiglutWebServiceMessagingFormat = SerializerType.JSON;
			settings.FiglutWebServiceSchemaAcquisitionMessagingFormat = SerializerType.XML;
//			settings.FiglutWebServiceBaseUrl = "http://192.168.0.103:2983/Figlut/";

			if (settings.UseAuthentication && settings.PromptForCredentials) {
				StartActivityForResult (typeof(LoginActivity), FiglutDataBoxApplication.REQUEST_CODE_LOGIN);
				return;
			} else if (settings.UseAuthentication &&
			         !settings.PromptForCredentials &&
			         string.IsNullOrEmpty (settings.AuthenticationUserName)) {
				throw new UserThrownException ("Application is configured to use authentication and not prompt for credentials (i.e. use configured credentials), but no credentials were configured.");
			}
			InitializeDatabaseSchema (userName, password, settings);
		}

		private void InitializeDatabaseSchema(string userName, string password, FiglutMonoDroidDataBoxSettings settings)
		{
			while (true)
			{
				try
				{
					FiglutDataBoxApplication.Instance.Initialize(userName, password, settings, false);
					Toast.MakeText(this, "Initialization complete", ToastLength.Long).Show();
					break;
				}
				catch(UserThrownException uex) 
				{
					throw uex;
				}
				catch(WebException wex)
				{
					HttpWebResponse response = wex.Response == null ? null : (HttpWebResponse)wex.Response;
//					if (settings.PromptForCredentials &&
//					    response != null &&
//						(response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)) {
//						UIHelper.DisplayMessage (
//							this, 
//							string.Format ("{0} {1} : {2}", 
//								response.StatusCode.ToString (), 
//								(int)response.StatusCode, 
//								response.StatusDescription));
//					} 
//					else if (response != null) 
//					{
//						throw new UserThrownException (string.Format ("{0} {1} : {2}",
//							response.StatusCode.ToString (),
//							(int)response.StatusCode,
//							response.StatusDescription),
//							LoggingLevel.Minimum);
//					} 
//					else
//					{
//						throw new UserThrownException (wex.Message, LoggingLevel.Minimum, false);
//					}
					if (response != null) {
						throw new UserThrownException (string.Format ("{0} {1} : {2}",
							response.StatusCode.ToString (),
							(int)response.StatusCode,
							response.StatusDescription));
					} else {
						throw new UserThrownException (wex.Message, LoggingLevel.Minimum, false);
					}
				}
			}
		}

		private List<MainMenuItem> GetMenuItems()
		{
			List<MainMenuItem> result = new List<MainMenuItem> ();
			result.Add (new MainMenuItem () {
				Name = Resources.GetString(Resource.String.data_box),
				ImageNumber = 0
			});
			result.Add (new MainMenuItem () {
				Name = Resources.GetString(Resource.String.configuration_manager),
				ImageNumber = 1
			});
			result.Add (new MainMenuItem () {
				Name = Resources.GetString (Resource.String.log_viewer),
				ImageNumber = 2,
			});
			result.Add (new MainMenuItem () {
				Name = Resources.GetString (Resource.String.reinitialize),
				ImageNumber = 3
			});
			result.Add (new MainMenuItem () {
				Name = Resources.GetString(Resource.String.exit),
				ImageNumber = 4
			});
			return result;
		}

		#endregion //Methods
	}
}


