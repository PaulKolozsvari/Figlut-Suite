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

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	public class DataTableGridAdapter : BaseAdapter<string>
	{
		#region Constructors

		public DataTableGridAdapter(Activity context, DataTable table) : base()
		{
			_context = context;
			_table = table;
		}

		#endregion //Constructors

		#region Fields

		private Activity _context;
		private DataTable _table;

		#endregion //Fields

		#region Methods

		public int GetColumnFromCellPosition(int position)
		{
			int result = position % _table.Columns.Count;
			return result;
		}

		public int GetRowFromCellPosition(int position)
		{
			int result = position / _table.Columns.Count;
			return result;
		}

		public string GetStringFromCellPosition(int position)
		{
			int row = GetRowFromCellPosition (position);
			int column = GetColumnFromCellPosition(position);
//			string result = _table.Rows [row] [column].ToString ();
//			return result;

			string result = string.Empty;
			if (row == 0) 
			{
				result = _table.Columns[column].ColumnName;
			}
			else if (_table.Rows [row - 1][column] != DBNull.Value) 
			{
				result = _table.Rows [row - 1][column].ToString();
			}
			return result;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		int getViewCount = 0;
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			getViewCount++;
			int currentRow = GetRowFromCellPosition (position);
			string currentCellValue = GetStringFromCellPosition (position);
			EditText lblColumnName = convertView as EditText ?? new EditText (_context); //Find if we have a control for the cell, otherwise create a new one.
			if (currentRow == 0) //This is the header row 
			{ 
				lblColumnName.SetBackgroundColor( new Android.Graphics.Color(0xff,0x43,0x7e,0x9f));
				lblColumnName.SetTextColor (new Android.Graphics.Color(0x0,0x0,0x0));
			}
			lblColumnName.Text = currentCellValue;
			lblColumnName.SetText (currentCellValue, TextView.BufferType.Normal);
			return lblColumnName;		
		}

		int countCount = 0;
		public override int Count 
		{
			get 
			{
				countCount++;
				int result = (_table.Rows.Count + 1) * _table.Columns.Count;
				return result;
			}
		}

		public override string this [int position] 
		{
			get 
			{
				return GetStringFromCellPosition (position);
			}
		}

		#endregion //Methdos
	}
}

