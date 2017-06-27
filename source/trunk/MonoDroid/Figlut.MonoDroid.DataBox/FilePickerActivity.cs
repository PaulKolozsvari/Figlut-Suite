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
using System.IO;
using Figlut.MonoDroid.Toolkit.Utilities.Logging;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "FilePickerActivity")]			
	public class FilePickerActivity : BaseActivity
	{
		#region Fields

		private ListView _listFilePicker;

		#endregion //Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			try 
			{
				base.OnCreate (bundle);
				View contentView = UIHelper.AddContentLayoutToViewStub (this, Resource.Layout.file_picker_content_layout, Resource.Id.content_stub);
				ApplyTheme (true, true);
				_listFilePicker = contentView.FindViewById<ListView> (Resource.Id.list_file_picker);
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
				_listFilePicker.ItemClick += OnFileListItemClick;
				RefreshFilesList (Directory.GetCurrentDirectory ());
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnPause ()
		{
			try 
			{
				base.OnPause ();
				_listFilePicker.ItemClick -= OnFileListItemClick;	
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		private void OnFileListItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			try 
			{
				FileItem selectedFileItem = (FileItem)_listFilePicker.GetItemAtPosition (e.Position);
				if (selectedFileItem.IsDirectory) {
					RefreshFilesList (selectedFileItem.FilePath); //Because it's a directory the FilePath will be the path to the directory not a file.
				} else {
					Intent i = new Intent (this, typeof(EditSettingsActivity));
					i.PutExtra (FiglutDataBoxApplication.EXTRA_SELECTED_FILE_PATH, selectedFileItem.FilePath);
					i.PutExtra (FiglutDataBoxApplication.EXTRA_SELECTED_FILE_NAME, selectedFileItem.FileName);
					SetResult (Result.Ok, i);
					Finish ();
				}	
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		#endregion //Event Handlers

		#region Methods

		private void RefreshFilesList(string directoryPath)
		{
			List<FileItem> fileItems = new List<FileItem> ();
			DirectoryInfo directoryInfo = new DirectoryInfo (directoryPath);
			try
			{
				FileListAdapter adapter = new FileListAdapter(this, fileItems);
				foreach(FileSystemInfo f in directoryInfo.GetFileSystemInfos())
				{
					FileItem fileItem = new FileItem(){FilePath =  f.FullName, FileName = f.Name, FileSystemInfo = f};
					FileAttributes attributes = File.GetAttributes(f.FullName);
					if((attributes & FileAttributes.Directory) == FileAttributes.Directory)
					{
						fileItem.IsDirectory = true;
					}
					fileItems.Add(fileItem);
				}
				_listFilePicker.Adapter = adapter;
//				_listFilePicker.RefreshDrawableState();
			}
			catch(System.UnauthorizedAccessException ex) {
				Toast.MakeText (this, ex.Message, ToastLength.Short).Show ();
			}
		}

		#endregion //Methods
	}
}