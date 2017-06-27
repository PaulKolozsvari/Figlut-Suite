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
	public class ConfigurationManagerItemListAdapter : BaseAdapter
	{
		#region Constructors

		public ConfigurationManagerItemListAdapter(Context context, List<ConfigurationManagerItem> items)
		{
			_inflator = LayoutInflater.FromContext (context);
			_items = items;
			_imageIds = new int[] {
				Resource.Drawable.figlut_configuration_manager, //0
				Resource.Drawable.figlut_web_service, //1
				Resource.Drawable.figlut_database, //2
				Resource.Drawable.figlut_service, //3
				Resource.Drawable.figlut_logging, //4
				Resource.Drawable.figlut_mobile_databox, //5
				Resource.Drawable.figlut_web_site, //6
				Resource.Drawable.figlut_help, //7
				Resource.Drawable.figlut_exit //8
			};
		}

		#endregion //Constructors

		#region Fields

		private LayoutInflater _inflator;
		private List<ConfigurationManagerItem> _items;
		private int[] _imageIds;

		#endregion //Fields

		#region Properties

		public override int Count 
		{
			get{ return _items.Count; }
		}

		#endregion //Properties

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
			ViewHolder holder = null;
			if (convertView == null) {
				convertView = _inflator.Inflate (Resource.Layout.config_manager_row_layout, null);
				holder = new ViewHolder ();
				holder.CategoryNameTextView = convertView.FindViewById<TextView> (Resource.Id.text_category);
				holder.CategoryDescriptionTextView = convertView.FindViewById<TextView> (Resource.Id.text_category_description);
				holder.CategoryImageView = convertView.FindViewById<ImageView> (Resource.Id.image_category);
				convertView.Tag = holder;
			} else {
				holder = (ViewHolder)convertView.Tag;
			}
			holder.CategoryNameTextView.Text = _items [position].CategoryName;
			holder.CategoryDescriptionTextView.Text = _items [position].CategoryDescription;
			int imageIndex = _items [position].ImageNumber;
			holder.CategoryImageView.SetImageResource (_imageIds[imageIndex]);
			return convertView;
		}

		#endregion //Methods

		#region Inner Types

		private class ViewHolder : Java.Lang.Object
		{
			public TextView CategoryNameTextView{ get; set;}

			public TextView CategoryDescriptionTextView{ get; set;}

			public ImageView CategoryImageView{get;set;}
		}

		#endregion //Inner Types
	}
}