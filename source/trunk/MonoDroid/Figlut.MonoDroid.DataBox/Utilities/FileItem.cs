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
using System.IO;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	public class FileItem : Java.Lang.Object
	{
		#region Properties

		public string FileName{get;set;}

		public string FilePath{get;set;}

		public bool IsDirectory{get;set;}

		public FileSystemInfo FileSystemInfo{ get; set; }

		#endregion //Properties
	}
}