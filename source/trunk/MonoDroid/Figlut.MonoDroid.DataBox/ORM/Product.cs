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
	public class Product
	{
		public Guid ProductId{get;set;}

		public string SKU{get;set;}

		public string ProductName{get;set;}

		public string ProductDescription{get;set;}

		public string DateCreated{ get; set; }
	}
}