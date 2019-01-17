namespace Figlut.MonoDroid.Toolkit.Utilities
{
    #region Using Directives

    using Android.Content;
    using Android.Net;

    #endregion //Using Directives

    public class NetworkHelper
    {
        #region Methods

        public static bool IsNetworkAvailable(ContextWrapper contextWrapper)
        {
            return IsNetworkAvailable((ConnectivityManager)contextWrapper.GetSystemService(Context.ConnectivityService));
        }

        public static bool IsNetworkAvailable(Context context)
        {
            return IsNetworkAvailable((ConnectivityManager)context.GetSystemService(Context.ConnectivityService));
        }

        public static bool IsNetworkAvailable(ConnectivityManager connectivityManager)
        {
            NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;
            return networkInfo != null && networkInfo.IsConnected;
        }

        #endregion //Methods
    }
}
