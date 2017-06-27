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
	public class InputControlInfoHolder : Java.Lang.Object
	{
		#region Properties

		public string InputControlName{get;set;}

		public Android.Text.InputTypes InputType{get;set;}

		#endregion //Properties
	}
}