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
using System.Data;
using Figlut.MonoDroid.DataBox.Configuration;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	public class DataTableListAdapter : BaseAdapter
	{
		#region Constructors

		public DataTableListAdapter(
			Context context, 
			DataTable table,
			FiglutMonoDroidDataBoxSettings settings)
		{
			_inflator = LayoutInflater.FromContext (context);
			_table = table;
			_settings = settings;
		}

		#endregion //Constructors

		#region Fields

		private LayoutInflater _inflator;
		private DataTable _table;
		private FiglutMonoDroidDataBoxSettings _settings;

		#endregion //Fields

		#region Properties

		public override int Count 
		{
			get 
			{
				return _table.Rows.Count;
			}
		}

		#endregion //Properties

		#region Methods

		public override Java.Lang.Object GetItem (int position)
		{
			string result = _table.Rows [position][_settings.EntityIdColumnName].ToString();
			return result;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ViewHolder holder;
			if (convertView == null) 
			{
				convertView = _inflator.Inflate (Resource.Layout.data_box_row_layout, null);
				holder = new ViewHolder ();
				holder.TextPrimaryColumn = convertView.FindViewById<TextView> (Resource.Id.text_primary_value);
				holder.TextSecondaryColumn = convertView.FindViewById<TextView> (Resource.Id.text_secondary_value);
				convertView.Tag = holder;
			} 
			else 
			{
				holder = (ViewHolder)convertView.Tag;
			}
			DataRow row = _table.Rows [position];
			holder.TextPrimaryColumn.Text = row[_settings.PrimaryColumnIndex].ToString ();
			holder.TextSecondaryColumn.Text = row [_settings.SecondaryColumnIndex].ToString ();
			return convertView;
		}

		#endregion //Methods

		#region Inner Types

		private class ViewHolder : Java.Lang.Object
		{
			public TextView TextPrimaryColumn{ get; set; }

			public TextView TextSecondaryColumn{get;set;}
		}

		#endregion //Inner Types
	}
}

