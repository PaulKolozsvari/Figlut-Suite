#region Using Directives

using System;
using Android.Graphics;
using System.IO;
using Android.Media;

#endregion //Using Directives

namespace Figlut.MonoDroid.Toolkit.Utilities
{
	public class BitmapHelper
	{
		#region Methods

		public static Bitmap GetBitmapFromFile(string fileName)
		{
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap scaledBitmap = BitmapFactory.DecodeFile(fileName, options);

			// Hack to determine whether the image is rotated
			bool rotated = scaledBitmap.Width > scaledBitmap.Height; //Landscape

			Bitmap result = null;
			// If not rotated, just scale it
			if (!rotated) {
				result = scaledBitmap;
				// If rotated, scale it by switching width and height and then rotated it
			} else {

				Matrix mat = new Matrix();
				mat.PostRotate (90);
				result = Bitmap.CreateBitmap(scaledBitmap, 0, 0, scaledBitmap.Width, scaledBitmap.Height, mat, true);
			}
			// Release image resources
			scaledBitmap.Recycle ();
			scaledBitmap = null;
			return result;
		}

		public static Bitmap LoadAndResizeBitmap(string fileName, int reqWidth, int reqHeight)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			double inSampleSize = 1D;
			if (outHeight > reqHeight || outWidth > reqWidth)
			{
				inSampleSize = 
					outWidth > outHeight
					? outHeight / reqHeight //Lanscape
					: outWidth / reqWidth; //Portrait
			}
//			if(outHeight > reqHeight || outWidth > reqWidth)
//			{
//				int halfHeight = (int)(outHeight / 2);
//				int halfWidth = (int)(outWidth / 2);
//				while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth) 
//				{
//					inSampleSize *= 2;
//				}
////				inSampleSize = 4;
//			}
			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = Convert.ToInt32(inSampleSize);;
			options.InJustDecodeBounds = false;
			options.InMutable = true;
			Bitmap scaledBitmap = BitmapFactory.DecodeFile(fileName, options);

			// Hack to determine whether the image is rotated
			bool rotated = scaledBitmap.Width > scaledBitmap.Height; //Landscape

			Bitmap result = null;
			// If not rotated, just scale it
			if (!rotated) {
				result = scaledBitmap;
				// If rotated, scale it by switching width and height and then rotated it
			} else {

				Matrix mat = new Matrix();
				mat.PostRotate (90);
				result = Bitmap.CreateBitmap(scaledBitmap, 0, 0, scaledBitmap.Width, scaledBitmap.Height, mat, true);
			}
			// Release image resources
			scaledBitmap.Recycle ();
			scaledBitmap = null;

			return result;
		}

		public static void SaveBitmapToFile(Bitmap bitmap, string filePath)
		{
			if (File.Exists (filePath)) 
			{
				File.Delete (filePath);
			}
			Bitmap mutableBitmap = bitmap.Copy (bitmap.GetConfig (), true);
			using (FileStream fs = new FileStream (filePath, FileMode.Create)) 
			{
				bitmap.Compress (Bitmap.CompressFormat.Jpeg, 100, fs);
				fs.Close ();
			}
		}

		/**
 * Converts a immutable bitmap to a mutable bitmap. This operation doesn't allocates
 * more memory that there is already allocated.
 * 
 * @param imgIn - Source image. It will be released, and should not be used more
 * @return a copy of imgIn, but muttable.
 */
//		public static Bitmap ConvertToMutable(Bitmap imgIn) {
//			//this is the file going to use temporally to save the bytes. 
//			// This file will not be a image, it will store the raw image data.
//			Java.IO.File file = new Java.IO.File (System.IO.Path.Combine (
//				                    Android.OS.Environment.GetExternalStoragePublicDirectory (Android.OS.Environment.DirectoryPictures).AbsolutePath, "temp.tmp"));
//
//			//Open an RandomAccessFile
//			//Make sure you have added uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE"
//			//into AndroidManifest.xml file
//			RandomAccessFile randomAccessFile = new RandomAccessFile(file, "rw");
//
//			// get the width and height of the source bitmap.
//			int width = imgIn.Width;
//			int height = imgIn.Height;
//			Config type = imgIn.GetConfig();
//
//			//Copy the byte to the file
//			//Assume source bitmap loaded using options.inPreferredConfig = Config.ARGB_8888;
//			FileChannel channel = randomAccessFile.Channel;
//			MappedByteBuffer map = channel.Map(FileChannel.MapMode.ReadWrite, 0, imgIn.RowBytes * height);
//			imgIn.CopyPixelsToBuffer(map);
//			//recycle the source bitmap, this will be no longer used.
//			imgIn.Recycle();
//			GC.Collect();// try to force the bytes from the imgIn to be released
//
//			//Create a new bitmap to load the bitmap again. Probably the memory will be available. 
//			imgIn = Bitmap.createBitmap(width, height, type);
//			map.Position = 0;
//			//load it back from temporary 
//			imgIn.CopyPixelsFromBuffer(map);
//			//close the temporary file and channel , then delete that also
//			channel.Close();
//			randomAccessFile.Close();
//
//			// delete the temp file
//			file.Delete();
//
//			return imgIn;
//		}

		public static byte[] BitmapToByteArray(Bitmap bmp)
		{
			using (MemoryStream ms = new MemoryStream ()) 
			{
				bmp.Compress (Bitmap.CompressFormat.Jpeg, 0, ms);
				byte[] result = ms.ToArray ();
				return result;
			}
		}

		#endregion //Methods
	}
}

