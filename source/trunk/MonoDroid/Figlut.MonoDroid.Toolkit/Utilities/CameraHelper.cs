#region Using Directives

using System;
using Android.Content;
using Android.Provider;
using System.Collections;
using Android.Content.PM;
using System.Collections.Generic;
using System.IO;
using Figlut.MonoDroid.Toolkit.Utilities;

#endregion //Using Directives

namespace Figlut.MonoDroid.Toolkit
{
	public class CameraHelper
	{
		#region Constructors

		public CameraHelper ()
		{
		}

		#endregion //Constructors

		#region Methods

		public static bool IsThereAnAppToTakePicture(PackageManager packageManager)
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities = packageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		public static string CreateDirectoryForPictures(string directoryName)
		{
			string result = Path.Combine (
				                Android.OS.Environment.GetExternalStoragePublicDirectory (Android.OS.Environment.DirectoryPictures).AbsolutePath,
				                directoryName);
			if (!Directory.Exists(result))
			{
				Directory.CreateDirectory (result);
			}
			return result;
		}

		#endregion //Methods
	}
}