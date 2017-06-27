namespace Figlut.Server.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class DataShaper
    {
        #region Methods

        public static string ShapeCamelCaseString(string inputString)
        {
            List<int> spaceIndexes = new List<int>();
            for (int i = 0; i < inputString.Length; i++)
            {
                if (char.IsUpper(inputString[i]))
                {
                    spaceIndexes.Add(i);
                }
            }
            for (int i = 1; i < spaceIndexes.Count; i++)
            {
                inputString = inputString.Insert(spaceIndexes[i], " ");
                for (int j = i; j < spaceIndexes.Count; j++)
                {
                    spaceIndexes[j] += 1;
                }
            }
            return inputString;
        }

        public static string RestoreStringToCamelCase(string inputString)
        {
            return inputString.Replace(" ", string.Empty);
        }

        public static string MaskPasswordString(string passwordString, char passwordChar)
        {
            string result = string.Empty;
            return result.PadRight(passwordString.Length, passwordChar);
        }

        public static string GetUniqueIdentifier()
        {
            return string.Format("ID{0}", Guid.NewGuid().ToString().Replace("-", string.Empty));
        }

        #endregion //Methods
    }
}