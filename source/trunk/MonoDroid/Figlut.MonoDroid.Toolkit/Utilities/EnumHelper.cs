namespace Figlut.MonoDroid.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using System.Drawing;

    #endregion //Using Directives

    public class EnumHelper
    {
        #region Methods

        /// <summary>
        /// Gets an array of enums from a given enum.
        /// </summary>
        /// <param name="enumType">The enum type to check.</param>
        /// <returns>Returns an array of enums from a given enum.</returns>
        public static Array GetEnumValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(string.Format("{0} is not an Enum type.", enumType.FullName));
            }
            object valAux = Activator.CreateInstance(enumType);
            FieldInfo[] fieldInfoArray = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

            Array res = Array.CreateInstance(enumType, fieldInfoArray.Length);
            for (int i = 0; i < res.Length; i++)
            {
                res.SetValue(fieldInfoArray[i].GetValue(valAux), i);
            }
            return res;
        }

        public static string GetEnumValuesAsCsv(Type enumType)
        {
            Array enumValues = EnumHelper.GetEnumValues(enumType);
            StringBuilder result = new StringBuilder();
            int enumValuesLength = enumValues.GetLength(0);
            for (int i = 0; i < enumValuesLength; i++)
            {
                string enumValue = enumValues.GetValue(i).ToString();
                if (i < (enumValuesLength - 1))
                {
                    result.AppendFormat("{0},", enumValue);
                }
                else
                {
                    result.AppendFormat("{0}", enumValue);
                }
            }
            return result.ToString();
        }

        #endregion //Methods
    }
}
