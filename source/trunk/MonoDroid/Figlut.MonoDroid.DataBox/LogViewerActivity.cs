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

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "Log Viewer")]			
	public class LogViewerActivity : BaseActivity
	{
		#region Fields

		private View _contentView;
		private TextView _textLogViewer;

		#endregion //Fields

		#region Event Handlers

		protected override void OnCreate (Bundle bundle)
		{
			try 
			{
				base.OnCreate (bundle);
				_contentView = UIHelper.AddContentLayoutToViewStub(this, Resource.Layout.log_viewer_activity_content_layout, Resource.Id.content_stub);
				_textLogViewer = _contentView.FindViewById<TextView>(Resource.Id.text_log_viewer);
				ApplyTheme(true, true);
			} catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		protected override void OnResume ()
		{
			try 
			{
				base.OnResume ();
				RefresehLog();
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			try 
			{
				base.OnCreateOptionsMenu (menu);
				MenuInflater inflator = new MenuInflater(this);
				inflator.Inflate(Resource.Layout.log_viewer_activity_main_menu_layout, menu);
				return true;
			} 
			catch (Exception ex) {
				ExceptionHandler.HandleException (ex, this);
				return false;
			}
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			try 
			{
				base.OnOptionsItemSelected (item);
				switch (item.ItemId) {
				case Resource.Id.menu_item_clear:
					GOC.Instance.Logger.DeleteLog();
					RefresehLog();
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

		protected override void ApplyTheme (bool colors, bool bannerImage)
		{
			base.ApplyTheme (colors, bannerImage);
			using (TextView textLogViewerTitle = _contentView.FindViewById<TextView> (Resource.Id.text_log_viewer_title)) {
				textLogViewerTitle.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
			}
		}

		private void RefresehLog()
		{
			_textLogViewer.Text = GOC.Instance.Logger.ReadLogContents ();
		}

		#endregion //Methods
	}
}

