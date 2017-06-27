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
using Figlut.MonoDroid.Toolkit.Data;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "Load DataBox")]			
	public class LoadDataBoxActivity : BaseActivity
	{
		#region Fields

		private View _contentView;
		private ListView _listSelectDataBox;
		private List<string> _dataBoxNames;

		#endregion //Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			try 
			{
				base.OnCreate (bundle);
				_contentView = UIHelper.AddContentLayoutToViewStub (this, Resource.Layout.load_data_box_activity_content_layout, Resource.Id.content_stub);
				ApplyTheme (true, true);

				_listSelectDataBox = _contentView.FindViewById<ListView> (Resource.Id.list_select_data_box);
				_dataBoxNames = Intent.GetStringArrayExtra (FiglutDataBoxApplication.EXTRA_DATABOX_NAMES).ToList ();	
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
				_listSelectDataBox.ItemClick += OnListDataBoxItemClick;
				_listSelectDataBox.Adapter = new LoadDataBoxListAdapter (this, _dataBoxNames);	
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
				_listSelectDataBox.ItemClick -= OnListDataBoxItemClick;
			} catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		private void OnListDataBoxItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			try 
			{
				string selectedDataBox = DataShaper.RestoreStringToCamelCase (_listSelectDataBox.GetItemAtPosition (e.Position).ToString ());
				Intent intentMainActivity = new Intent (this, typeof(MainActivity));
				intentMainActivity.PutExtra (FiglutDataBoxApplication.EXTRA_SELECTED_DATA_BOX, selectedDataBox);
				SetResult (Result.Ok, intentMainActivity);
				Finish ();	
			} catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		#endregion //Event Handlers

		#region Methods

		protected override void ApplyTheme (bool colors, bool bannerImage)
		{
			base.ApplyTheme (colors, bannerImage);
			using (TextView textDataBoxes = _contentView.FindViewById<TextView> (Resource.Id.text_data_boxes)) {
				textDataBoxes.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
		}

		#endregion //Methods
	}
}