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

namespace Figlut.MonoDroid.DataBox.Utilities
{
	public class LoginCredentials
	{
		#region Constructors

		public LoginCredentials(string userName, string password)
		{
			_userName = userName;
			_password = password;
		}

		#endregion //Constructors

		#region Fields

		private string _userName;
		private string _password;

		#endregion //Fields

		#region Properies

		public string UserName
		{
			get{ return _userName; }
		}

		public string Password
		{
			get{ return _password; }
		}

		#endregion //Properties
	}
}