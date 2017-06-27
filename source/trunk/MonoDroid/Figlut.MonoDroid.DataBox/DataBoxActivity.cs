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
using Figlut.MonoDroid.Toolkit.Web.Client.FiglutWebService;
using Figlut.MonoDroid.Toolkit.Data;
using System.Collections;
using Figlut.MonoDroid.Toolkit.Utilities;
using Figlut.MonoDroid.DataBox.Configuration;
using System.Net;
using Figlut.MonoDroid.Toolkit.Data.DB.SQLServer;
using Figlut.MonoDroid.DataBox.Utilities;
using Figlut.MonoDroid.Toolkit.Utilities.Logging;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "DataBox")]			
	public class DataBoxActivity : BaseActivity
	{
		#region Fields

		private FiglutMonoDroidDataBoxSettings _settings;

		private View _contentView;
		private TextView _txtDataBoxTitle;
		private TextView _txtPrimaryColumnHeader;
		private TextView _txtSecondaryColumnHeader;
		private ListView _listDataBox;
		private string _selectedDataBox;

		#endregion Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_contentView = UIHelper.AddContentLayoutToViewStub (this, Resource.Layout.data_box_activity_content_layout, Resource.Id.content_stub);
			ApplyTheme ();

			_txtDataBoxTitle = _contentView.FindViewById<TextView> (Resource.Id.text_data_box_title);
			_txtPrimaryColumnHeader = _contentView.FindViewById<TextView> (Resource.Id.text_primary_value_header);
			_txtSecondaryColumnHeader = _contentView.FindViewById<TextView> (Resource.Id.text_secondary_value_header);
			_listDataBox = _contentView.FindViewById<ListView> (Resource.Id.list_data_box);
			RegisterForContextMenu (_listDataBox);
			_selectedDataBox = Intent.GetStringExtra (FiglutDataBoxApplication.EXTRA_SELECTED_DATA_BOX);
			DataBoxData.Instance.CurrentTable = (SqlDatabaseTable)GOC.Instance.GetDatabase<SqlDatabase> ().GetDatabaseTable (_selectedDataBox);
			_settings = GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings> ();
		}

		protected override void OnDestroy ()
		{
			try
			{
				base.OnDestroy ();
				DataBoxData.Instance.ClearData ();
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnResume ()
		{
			try
			{
				base.OnResume ();
				_listDataBox.ItemClick += OnListDataBoxItemClick;
				RefreshDataBox ();
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnPause ()
		{
			try
			{
				base.OnPause ();
				_listDataBox.ItemClick -= OnListDataBoxItemClick;
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		private void OnListDataBoxItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			try
			{
				Java.Lang.Object selectedEntityId = _listDataBox.GetItemAtPosition (e.Position);
				if(selectedEntityId == null)
				{
					throw new NullReferenceException (string.Format ("Entity in DataBox at position {0} is null.", e.Position));
				}
				DataBoxData.Instance.EntityIdUnderUpdate = new Guid (selectedEntityId.ToString ());
				DataBoxData.Instance.EntityUnderUpdate = DataBoxData.Instance.CurrentEntityCache [DataBoxData.Instance.EntityIdUnderUpdate.Value];
				DataBoxData.Instance.InUpdateMode = true;

				Intent manageEntityIntent = new Intent (this, typeof(ManageEntityActivity));
				manageEntityIntent.PutExtra (FiglutDataBoxApplication.EXTRA_ENTITY_OPERATION, (int)EntityOperation.Update);
				StartActivityForResult (manageEntityIntent, FiglutDataBoxApplication.REQUEST_CODE_MANAGE_ENTITY);
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			try
			{
				base.OnActivityResult (requestCode, resultCode, data);
				if (requestCode == FiglutDataBoxApplication.REQUEST_CODE_MANAGE_ENTITY)
				{
					if (DataBoxData.Instance.ManageEntityChangesMade)
					{
						RefreshDataBox ();
						DataBoxData.Instance.UnsavedChanges = true;
					}
					DataBoxData.Instance.ClearEntityUpdateMode ();
				}
			}
			catch(Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			try 
			{
				base.OnCreateOptionsMenu (menu);
				MenuInflater menuInflator = new MenuInflater (this);
				menuInflator.Inflate (Resource.Layout.data_box_main_menu_layout, menu);
				return true;
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);	
				return false;
			}
		}

		public override bool OnMenuItemSelected (int featureId, IMenuItem item)
		{
			try 
			{
				base.OnMenuItemSelected (featureId, item);
				switch (item.ItemId) {
				case Resource.Id.menu_item_add:
					AddEntity ();
					break;
				case Resource.Id.menu_item_refresh:
					DataBoxData.Instance.ClearData ();
					RefreshDataBox (); //Because the data has been cleared, the DataBox refresh will be from the server.
					break;
				case Resource.Id.menu_item_save:
					Save ();
					break;
				default:
					break;
				}
				return true;
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return false;
			}
		}

		public override void OnCreateContextMenu (IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
		{
			try 
			{
				base.OnCreateContextMenu (menu, v, menuInfo);
				if (v != _listDataBox)
				{
					return;
				}
				MenuInflater menuInflator = new MenuInflater (this);
				menuInflator.Inflate (Resource.Layout.data_box_context_menu_layout, menu);
			} catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		public override bool OnContextItemSelected (IMenuItem item)
		{
			try 
			{
				base.OnContextItemSelected (item);
				switch (item.ItemId) {
				case Resource.Id.menu_item_delete:
					DeleteEntity (item);
					break;
				default:
					break;
				}
				return true;	
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return false;
			}
		}

		#endregion //Event Handlers

		#region Methods

		protected virtual void ApplyTheme ()
		{
			base.ApplyTheme (true, true);
			using (TextView textDataBoxTitle = _contentView.FindViewById<TextView> (Resource.Id.text_data_box_title)) {
				textDataBoxTitle.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
			using (LinearLayout headerLayout = _contentView.FindViewById<LinearLayout> (Resource.Id.header_layout)) {
				headerLayout.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
			using (TextView textPrimaryValueHeader = _contentView.FindViewById<TextView> (Resource.Id.text_primary_value_header)) {
				textPrimaryValueHeader.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
			using (TextView textSecondaryValueHeader = _contentView.FindViewById<TextView> (Resource.Id.text_secondary_value_header)) {
				textSecondaryValueHeader.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
		}

		private bool RefreshDataBox()
		{
			if (DataBoxData.Instance.CurrentEntityCache == null)
			{
				RefreshFromServer ();
			}
			System.Data.DataTable dataSourceTable = DataBoxData.Instance.CurrentEntityCache.GetDataTable (null, false, true, _settings.EntityIdColumnName);
			DataTableListAdapter adapter = new DataTableListAdapter (this, dataSourceTable, _settings);
			_listDataBox.Adapter = adapter;
			_txtDataBoxTitle.Text = dataSourceTable.TableName;
			_txtPrimaryColumnHeader.Text = dataSourceTable.Columns [_settings.PrimaryColumnIndex].ColumnName;
			_txtSecondaryColumnHeader.Text = dataSourceTable.Columns [_settings.SecondaryColumnIndex].ColumnName;
			if (DataBoxData.Instance.HiddenProperties == null)
			{
				DataBoxData.Instance.HiddenProperties = new List<string> ();
			}
			DataBoxData.Instance.HiddenProperties.Clear ();
			EntityReader.GetPropertyNamesByType (DataBoxData.Instance.CurrentTable.MappedType, _settings.GetHiddenTypes (), true)
				.ForEach (p => DataBoxData.Instance.HiddenProperties.Add (p));
			return true;
		}

		private void RefreshFromServer()
		{
			FiglutWebServiceFilterString filterString = new FiglutWebServiceFilterString (_selectedDataBox, null);
			string rawOutput = null;
			if (DataBoxData.Instance.CurrentTable == null) {
				DataBoxData.Instance.CurrentTable = (SqlDatabaseTable)GOC.Instance.GetDatabase<SqlDatabase> ().GetDatabaseTable (_selectedDataBox);
			}
			IList result = DataHelper.GetListOfType (DataBoxData.Instance.CurrentTable.MappedType);
			result = (IList)GOC.Instance.FiglutWebServiceClient.Query (result.GetType (), filterString, out rawOutput, true);
			DataBoxData.Instance.CurrentEntityCache = new FiglutEntityCacheUnique (DataBoxData.Instance.CurrentTable.MappedType);
			DataBoxData.Instance.CurrentEntityCache.OverrideFromList (result);
		}

		private void DeleteEntity(IMenuItem item)
		{
			AdapterView.AdapterContextMenuInfo menuInfo = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
			Java.Lang.Object id = _listDataBox.GetItemAtPosition (menuInfo.Position);
			if (id == null) 
			{
				throw new NullReferenceException (string.Format ("Entity in DataBox at position {0} is null.", menuInfo.Position));
			}
			Guid selectedEntityId = new Guid (id.ToString ());

			DataBoxData.Instance.CurrentEntityCache.Delete (selectedEntityId);
			RefreshDataBox ();
			DataBoxData.Instance.UnsavedChanges = true;
		}

		private void AddEntity()
		{
			DataBoxData.Instance.EntityUnderUpdate = Activator.CreateInstance (DataBoxData.Instance.CurrentTable.MappedType);
			DataBoxData.Instance.InUpdateMode = true;

			Intent manageEntityIntent = new Intent (this, typeof(ManageEntityActivity));
			manageEntityIntent.PutExtra (FiglutDataBoxApplication.EXTRA_ENTITY_OPERATION, (int)EntityOperation.Add);
			StartActivityForResult (manageEntityIntent, FiglutDataBoxApplication.REQUEST_CODE_MANAGE_ENTITY);
		}

		private void Save()
		{
			string messageResult = null;
			bool saveSuccessful = DataBoxData.Instance.CurrentEntityCache.SaveToServer (out messageResult, true);
			DataBoxData.Instance.UnsavedChanges = false;
			Toast.MakeText (this, messageResult, ToastLength.Long).Show ();
		}

		#endregion //Methods
	}
}