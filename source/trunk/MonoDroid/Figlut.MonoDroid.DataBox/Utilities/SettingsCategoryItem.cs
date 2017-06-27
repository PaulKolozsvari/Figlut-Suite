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

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	public class SettingsCategoryItem : Java.Lang.Object
	{
		#region Properties

		public string SettingDisplayName{ get; set;}

		public string SettingName{get;set;}

		public object SettingValue{get;set;}

		#endregion //Properties
	}
}