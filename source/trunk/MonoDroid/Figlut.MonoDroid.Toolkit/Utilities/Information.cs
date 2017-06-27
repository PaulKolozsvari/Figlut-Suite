namespace Figlut.MonoDroid.Toolkit.Utilities
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.IO;
	using System.Reflection;
	using System.Drawing;
	using System.Linq;

	#endregion //Using Directives

	/// <summary>
	/// A helper class helps in retrieving system information.
	/// </summary>
	public class Information
	{
		#region Methods

		/// <summary>
		/// Gets the executing directory of the current application.
		/// </summary>
		public static string GetExecutingDirectory()
		{
//			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6);
//			return Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
			return Environment.GetFolderPath (Environment.SpecialFolder.Personal);
		}

		/// <summary>
		/// Returns a dictionary of all the system colors with their names as the keys
		/// to the dictionary.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, Android.Graphics.Color> GetSystemColors()
		{
			Type colorType = typeof(Android.Graphics.Color);
			Dictionary<string, Android.Graphics.Color> result = new Dictionary<string, Android.Graphics.Color>();
//			foreach (PropertyInfo p in colorType.GetProperties().Where(p => p.PropertyType == colorType))
//			{
//				Android.Graphics.Color c = (Android.Graphics.Color)p.GetValue(null, null);
//				result.Add(p.Name, c);
//			}
			PropertyInfo[] colorProperties = colorType.GetProperties ();
			if (colorProperties.Length < 1) {
				throw new UserThrownException ("Less than one system color exists.");
			}
			foreach (PropertyInfo p in colorProperties) {
				if (p.PropertyType != colorType) {
					continue;
				}
				Android.Graphics.Color color = (Android.Graphics.Color)p.GetValue(null, null);
				result.Add (p.Name, color);
			}
			return result;
		}

		public static string GetDomainAndMachineName()
		{
			return string.Format("{0}.{1}", Environment.UserDomainName, Environment.MachineName);
		}

		#endregion //Methods
	}
}