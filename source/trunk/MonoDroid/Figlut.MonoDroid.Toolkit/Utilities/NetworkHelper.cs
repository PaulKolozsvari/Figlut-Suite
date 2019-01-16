namespace Figlut.MonoDroid.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using Android.Content;
    using Android.Net;

    #endregion //Using Directives

    public class NetworkHelper
    {
        #region Methods

        private bool IsNetworkAvailable(ContextWrapper contextWrapper)
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)contextWrapper.GetSystemService(Context.ConnectivityService);
            NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;
            return networkInfo != null && networkInfo.IsConnected;
        }

        #endregion //Methods
    }
}
