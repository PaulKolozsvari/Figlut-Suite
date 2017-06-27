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
using Figlut.MonoDroid.Toolkit.Utilities.SettingsFile;
using Figlut.MonoDroid.Toolkit.Data;
using Figlut.MonoDroid.DataBox.Utilities;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "Configuration Manager")]			
	public class ConfigurationManagerActivity : BaseActivity
	{
		#region Fields

		private View _contentView;
		private ListView _listConfigManager;
		private FiglutMonoDroidDataBoxSettings _settings;

		#endregion //Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			try 
			{
				base.OnCreate (bundle);
				_contentView = UIHelper.AddContentLayoutToViewStub (this, Resource.Layout.config_manager_activity_content_layout, Resource.Id.content_stub);
				ApplyTheme (true, true);
				_listConfigManager = _contentView.FindViewById<ListView> (Resource.Id.list_config_manager);	
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnResume ()
		{
			try
			{
				base.OnResume ();
				_listConfigManager.ItemClick += OnConfigManagerItemClick;
				_listConfigManager.Adapter = new ConfigurationManagerItemListAdapter (this, GetConfigManagerItems ());
				RefreshSettings ();
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnPause ()
		{
			try
			{
				base.OnPause ();
				_listConfigManager.ItemClick -= OnConfigManagerItemClick;
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		private void OnConfigManagerItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			try
			{
				Java.Lang.Object o = _listConfigManager.GetItemAtPosition (e.Position);
				ConfigurationManagerItem selectedCategory = (ConfigurationManagerItem)o;
				ConfigurationManagerData.Instance.CurrentCategorySettings = _settings.GetSettingsByCategory (new SettingsCategoryInfo (_settings, selectedCategory.CategoryName));
				Intent intentEditSettings = new Intent (this, typeof(EditSettingsActivity));
				intentEditSettings.PutExtra (FiglutDataBoxApplication.EXTRA_SETTINGS_CATEGORY_NAME, selectedCategory.CategoryName);
				StartActivityForResult (intentEditSettings, FiglutDataBoxApplication.REQUEST_CODE_EDIT_SETTINGS);
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
				if (requestCode != FiglutDataBoxApplication.REQUEST_CODE_EDIT_SETTINGS) {
					return;
				}
				ApplyTheme (true, true);
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
				switch (item.ItemId) {
				case Resource.Id.menu_item_reset_all_settings:
					FiglutMonoDroidDataBoxSettings settings = GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings>();
					settings.ResetAllSettings();
					settings.SaveToFile();
					FiglutDataBoxApplication.Instance.RefreshImageBannerFromSettings();
					FiglutDataBoxApplication.Instance.RefreshThemeColorFromSettings();
					ApplyTheme(true, true);
					RefreshSettings();
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
			using (TextView textConfigurationManagerTitle = _contentView.FindViewById<TextView> (Resource.Id.text_configuration_manager_title)) {
				textConfigurationManagerTitle.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
		}

		private void RefreshSettings()
		{
			ConfigurationManagerData.Instance.ClearData ();
			_settings = GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings> ();
		}

		private List<ConfigurationManagerItem> GetConfigManagerItems()
		{
			List<ConfigurationManagerItem> result = new List<ConfigurationManagerItem> ();
			result.Add (new ConfigurationManagerItem () {
				CategoryName = Resources.GetString (Resource.String.web_service),
				CategoryDescription = Resources.GetString(Resource.String.web_service_settings_description),
				ImageNumber = 1
			});
			result.Add (new ConfigurationManagerItem () {
				CategoryName = Resources.GetString (Resource.String.data_box),
				CategoryDescription = Resources.GetString(Resource.String.data_box_settings_description),
				ImageNumber = 5
			});
			result.Add (new ConfigurationManagerItem () {
				CategoryName = Resources.GetString (Resource.String.logging),
				CategoryDescription = Resources.GetString(Resource.String.logging_settings_description),
				ImageNumber = 4
			});
			return result;
		}

		#endregion //Methods
	}
}

