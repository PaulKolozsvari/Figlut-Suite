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
using Figlut.MonoDroid.Toolkit.Utilities.SettingsFile;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	public class SettingsCategoryItemListAdapter : BaseAdapter
	{
		#region Constructors

		public SettingsCategoryItemListAdapter(Context context, List<SettingsCategoryItem> items)
		{
			_inflator = LayoutInflater.FromContext (context);
			_items = items;
		}

		#endregion Constructors


		#region Fields

		private LayoutInflater _inflator;
		private List<SettingsCategoryItem> _items;

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
			if (convertView == null) 
			{
				convertView = _inflator.Inflate (Resource.Layout.edit_settings_row_layout, null);
				holder = new ViewHolder ();
				holder.SettingDisplayNameTextView = convertView.FindViewById<TextView> (Resource.Id.text_setting_display_name);
				holder.SettingValueTextView = convertView.FindViewById<TextView> (Resource.Id.text_setting_value);
				convertView.Tag = holder;
			} 
			else 
			{
				holder = (ViewHolder)convertView.Tag;
			}
			holder.SettingDisplayNameTextView.Text = _items [position].SettingDisplayName;
			holder.SettingValueTextView.Text = _items [position].SettingValue == null ? null : _items [position].SettingValue.ToString ();
			return convertView;
		}

		#endregion //Methods

		#region Inner Types

		public class ViewHolder : Java.Lang.Object
		{
			public TextView SettingDisplayNameTextView{get;set;}

			public TextView SettingValueTextView{ get; set;}
		}

		#endregion //Inner Types
	}
}