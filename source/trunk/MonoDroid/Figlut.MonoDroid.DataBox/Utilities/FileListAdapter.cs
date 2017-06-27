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
	public class FileListAdapter : BaseAdapter
	{
		#region Constructors

		public FileListAdapter(Context context, List<FileItem> items)
		{
			_inflator = LayoutInflater.FromContext (context);
			_items = items;
		}

		#endregion //Constructors

		#region Fields

		private LayoutInflater _inflator;
		private List<FileItem> _items;

		#region Properties

		public override int Count {
			get{ return _items.Count; }
		}

		#endregion //Properties

		#endregion //Fields

		#region Methods

		public override Java.Lang.Object GetItem (int position)
		{
			return _items [position];
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			FileListRowHolder holder = null;
			if (convertView == null) {
				convertView = _inflator.Inflate (Resource.Layout.file_picker_row_layout, null);
				holder = new FileListRowHolder (
					convertView.FindViewById<ImageView> (Resource.Id.image_file_list_item),
					convertView.FindViewById<TextView> (Resource.Id.text_file_list_item));
				convertView.Tag = holder;
			} else {
				holder = (FileListRowHolder)convertView.Tag;
			}
			FileItem fileItem = _items [position];
			int imageResourceId = fileItem.IsDirectory ? Resource.Drawable.folder : Resource.Drawable.file;
			holder.Update (imageResourceId, fileItem.FileName);
			return convertView;
		}

		#endregion //Methods

		#region Inner Types

		public class FileListRowHolder : Java.Lang.Object
		{
			#region Constructors

			public FileListRowHolder(ImageView iconImageView, TextView fileNameTextView)
			{
				_iconImageView = iconImageView;
				_fileNameTextView = fileNameTextView;
			}

			#endregion //Constructors

			#region Fields

			private ImageView _iconImageView;
			private TextView _fileNameTextView;

			#endregion //Fields

			public ImageView IconImageView
			{
				get{ return _iconImageView; }
			}

			public TextView FileNameTextView
			{
				get{ return _fileNameTextView; }
			}

			public void Update(int imageResourceId, string fileName)
			{
				_iconImageView.SetImageResource (imageResourceId);
				_fileNameTextView.Text = fileName;
			}
		}

		#endregion //Inner Types
	}
}