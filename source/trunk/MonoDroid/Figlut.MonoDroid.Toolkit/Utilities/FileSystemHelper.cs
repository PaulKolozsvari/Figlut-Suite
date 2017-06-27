using Android.Content;
using Android.App;

namespace Figlut.MonoDroid.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Runtime.InteropServices;

    #endregion //Using Directives

    public class FileSystemHelper
    {
        #region Methods

        private const int ERROR_SHARING_VIOLATION = 32;
        private const int ERROR_LOCK_VIOLATION = 33;

        public static bool IsFileLocked(Exception exception)
        {
            int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        }

        public static void ValidateFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("Could not find {0}.", filePath));
            }
        }

        public static void ValidateFileExists(string directoryPath, string fileName)
        {
            ValidateFileExists(Path.Combine(directoryPath, fileName));
        }

        public static void ValidateDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException(string.Format("Could not find {0}.", directoryPath));
            }
        }

        public static byte[] GetFileBytes(string filePath)
        {
            ValidateFileExists(filePath);
            using (FileStream fs = File.OpenRead(filePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.SetLength(fs.Length);
                    fs.Read(ms.GetBuffer(), 0, (int)fs.Length);
                    return ms.ToArray();
                }
            }
        }

		public static string GetRealFilePathFromUri(Android.Net.Uri contentUri, Activity activity)
		{
			String[] projection = new String[] { Android.Provider.MediaStore.MediaColumnsConsts.Data }; 
			ContentResolver cr = activity.ContentResolver; 
			Android.Database.ICursor cursor = cr.Query(contentUri, projection, null, null, null); 
			if (cursor != null && cursor.Count > 0) 
			{ 
				cursor.MoveToFirst(); 
				int index = 
					cursor.GetColumnIndex(Android.Provider.MediaStore.MediaColumnsConsts.Data); 
				return cursor.GetString(index); 
			} 
			return null; 
		}

        #endregion //Methods
    }
}