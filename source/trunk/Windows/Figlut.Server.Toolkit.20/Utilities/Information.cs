namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Reflection;
    using System.Drawing;
    using Figlut.Server.Toolkit.Data;

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
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6);
        }

        /// <summary>
        /// Returns a dictionary of all the system colors with their names as the keys
        /// to the dictionary.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Color> GetSystemColors()
        {
            Type colorType = typeof(Color);
            Dictionary<string, Color> result = new Dictionary<string, Color>();
            foreach (PropertyInfo p in colorType.GetProperties())
            {
                if (p.PropertyType == colorType)
                {
                    Color c = (Color)p.GetValue(null, null);
                    result.Add(p.Name, c);
                }
            }
            return result;
        }

        #endregion //Methods
    }
}