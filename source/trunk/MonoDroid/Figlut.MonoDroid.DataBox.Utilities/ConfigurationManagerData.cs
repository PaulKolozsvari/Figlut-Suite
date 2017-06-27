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
using Figlut.MonoDroid.Toolkit.Data;
using Figlut.MonoDroid.Toolkit.Utilities.SettingsFile;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox.Utilities
{
	public class ConfigurationManagerData
	{
		#region Singleton Setup

		private static ConfigurationManagerData _instance;

		public static ConfigurationManagerData Instance
		{
			get 
			{
				if (_instance == null) 
				{
					_instance = new ConfigurationManagerData ();
				}
				return _instance;
			}
		}

		#endregion //Singleton Setup

		#region Constructors

		private ConfigurationManagerData()
		{
		}

		#endregion //Constructors

		#region Fields

		private EntityCache<string, SettingItem> _currentCategorySettings;
		private string _currentSettingName;
		private View _currentInputControl;

		#endregion //Fields

		#region Properties

		public EntityCache<string, SettingItem> CurrentCategorySettings
		{
			get{ return _currentCategorySettings; }
			set{ _currentCategorySettings = value; }
		}

		public string CurrentSettingName
		{
			get{ return _currentSettingName; }
			set{ _currentSettingName = value; }
		}

		public View CurrentInputControl
		{
			get{ return _currentInputControl; }
			set{ _currentInputControl = value; }
		}

		#endregion //Properties

		#region Methods

		public void ClearData()
		{
			if (_currentCategorySettings != null)
			{
				_currentCategorySettings.Clear ();
				_currentCategorySettings = null;
			}
		}

		public void SetSettingValue(string settingName, object value, bool saveSettings)
		{
			SettingItem s = _currentCategorySettings [settingName];
			Type t = s.SettingType;
			if (t.Equals (typeof(string))) {
				s.SettingValue = value.ToString ();
			} else if (t.Equals (typeof(Int64))) {
				s.SettingValue = Convert.ToInt64 (value);
			} else if (t.Equals (typeof(Int32))) {
				s.SettingValue = Convert.ToInt32 (value);
			} else if (t.Equals (typeof(Int64))) {
				s.SettingValue = Convert.ToInt16 (value);
			} else if (t.Equals (typeof(Boolean))) {
				s.SettingValue = Convert.ToBoolean (value);
			} else if (t.IsEnum) {
				s.SettingValue = Enum.Parse (t, value.ToString ());
			} else {
				throw new NotImplementedException (string.Format ("Setting type {0} not supported.", t.FullName));
			}
			EntityReader.SetPropertyValue (s.SettingName, s.SettingsCategoryInfo.Settings, value, true);
			if (saveSettings) {
				s.SettingsCategoryInfo.Settings.SaveToFile ();
			}
		}

		#endregion //Methods
	}
}