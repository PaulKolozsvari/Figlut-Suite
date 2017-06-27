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
using Figlut.MonoDroid.DataBox.Configuration;
using Figlut.MonoDroid.Toolkit.Utilities;
using Android.Graphics;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	[Activity (Label = "BaseActivity")]			
	public class BaseActivity : Activity
	{
		#region Methods

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.base_activity_layout);
		}

		protected virtual void ApplyTheme(bool colors, bool bannerImage)
		{
			if (colors) {
				using (View borderBottom = this.FindViewById<View> (Resource.Id.border_bottom)) {
					borderBottom.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
				}
				using (View borderLeft = this.FindViewById<View> (Resource.Id.border_left)) {
					borderLeft.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
				}
				using (View borderRight = this.FindViewById<View> (Resource.Id.border_right)) {
					borderRight.SetBackgroundColor (FiglutDataBoxApplication.Instance.ThemeColor);
				}
			}
			if (bannerImage) {
				using (ImageView bannerImageView = this.FindViewById<ImageView> (Resource.Id.image_logo)) {
					bannerImageView.SetImageBitmap (FiglutDataBoxApplication.Instance.ImageBanner);
					bannerImageView.RefreshDrawableState ();
				}
			}
		}

		#endregion //Methods
	}
}

