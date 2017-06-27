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
	public class MainMenuItemListAdapter : BaseAdapter
	{
		#region Constructors

		public MainMenuItemListAdapter(Context context, List<MainMenuItem> items)
		{
			_inflator = LayoutInflater.From (context);
			_items = items;
			_imageIds = new int[] {

				Resource.Drawable.figlut_mobile_databox,
				Resource.Drawable.figlut_configuration_manager,
				Resource.Drawable.figlut_logging,
				Resource.Drawable.figlut_database,
				Resource.Drawable.figlut_exit
			};                   
		}

		#endregion //Constructors

		#region Fields

		private LayoutInflater _inflator;
		private List<MainMenuItem> _items;
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
			return _items[position];
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ViewHolder holder;
			if (convertView == null) {
				convertView = _inflator.Inflate (Resource.Layout.main_menu_row_layout, null); //This is a view containing multiple views for this specific row.
				holder = new ViewHolder ();
				holder.NameTextView = convertView.FindViewById<TextView> (Resource.Id.text_item_name);
				holder.ItemImageView = convertView.FindViewById<ImageView> (Resource.Id.image_item);
				convertView.Tag = holder;
			} 
			else {
				holder = (ViewHolder)convertView.Tag;
			}
			holder.NameTextView.Text = _items [position].Name;
			int imageIndex = _items [position].ImageNumber;
			holder.ItemImageView.SetImageResource (_imageIds [imageIndex]);
			return convertView; //Returns the view representing the row.
		}

		#endregion //Methods

		#region Inner Types

		//Holds all the views for a specific row.
		private class ViewHolder : Java.Lang.Object
		{
			public ImageView ItemImageView{ get; set; }

			public TextView NameTextView{ get; set; }
		}

		#endregion //Inner Types
	}
}