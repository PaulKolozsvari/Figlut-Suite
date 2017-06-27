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
using Figlut.MonoDroid.Toolkit.Data;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	public class LoadDataBoxListAdapter : BaseAdapter
	{
		#region Constructors

		public LoadDataBoxListAdapter(Context context, List<string> dataBoxNames)
		{
			_inflator = LayoutInflater.FromContext (context);
			_dataBoxNames = dataBoxNames;
		}

		#endregion //Constructors

		#region Fields

		private List<string> _dataBoxNames;
		private LayoutInflater _inflator;

		#endregion ///Fields

		#region Methods

		public override Java.Lang.Object GetItem (int position)
		{
			return _dataBoxNames [position];
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ViewHolder holder = null;
			if (convertView == null) {
				convertView = _inflator.Inflate (Resource.Layout.data_box_row_layout, null);
				holder = new ViewHolder ();
				holder.TextPrimaryColumn = convertView.FindViewById<TextView> (Resource.Id.text_primary_value);
				convertView.Tag = holder;
			} 
			else
			{
				holder = (ViewHolder)convertView.Tag;
			}
			holder.TextPrimaryColumn.Text = DataShaper.ShapeCamelCaseString (_dataBoxNames [position]);
			return convertView;
		}

		public override int Count {
			get 
			{
				return _dataBoxNames.Count;
			}
		}

		#endregion //Methods

		#region Inner Types

		public class ViewHolder : Java.Lang.Object
		{
			public TextView TextPrimaryColumn{get;set;}
		}

		#endregion //Inner Types
	}
}