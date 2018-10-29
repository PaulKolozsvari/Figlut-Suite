namespace Figlut.Server.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class DataValidator
    {
        #region Methods

        public static void ValidateListNotEmptyOrNull<T>(List<T> list, string fieldName, string parentName)
        {
            ValidateObjectNotNull(list, fieldName, parentName);
            if (list.Count <= 0)
            {
                string message = string.IsNullOrEmpty(parentName) ?
                    string.Format("{0} may not be an empty list.", fieldName) :
                    string.Format("{0} may not be an empty list on a {1}.", fieldName, parentName);
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void ValidateObjectNotNull(object fieldValue, string fieldName, string parentName)
        {
            if (fieldValue == null)
            {
                string message = string.IsNullOrEmpty(parentName) ? 
                    string.Format("{0} may not be null.", fieldName) : 
                    string.Format("{0} may not be null on a {1}.", fieldName, parentName);
                throw new NullReferenceException(message);
            }
        }

        public static void ValidateStringNotEmptyOrNull(string fieldValue, string fieldName, string parentName)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                string message = string.IsNullOrEmpty(parentName) ?
                    string.Format("{0} may not be null or empty.", fieldName) :
                    string.Format("{0} may not be null or empty on a {1}.", fieldName, parentName);
                throw new NullReferenceException(message);
            }
        }

        public static void ValidateShortNotNegative(short fieldValue, string fieldName, string parentName)
        {
            if (fieldValue < 0)
            {
                string message = string.IsNullOrEmpty(parentName) ?
                    string.Format("{0} may not negative.", fieldName) :
                    string.Format("{0} may not negative on a {1}.", fieldName, parentName);
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void ValidateIntegerNotNegative(int fieldValue, string fieldName, string parentName)
        {
            if (fieldValue < 0)
            {
                string message = string.IsNullOrEmpty(parentName) ?
                    string.Format("{0} may not negative.", fieldName) :
                    string.Format("{0} may not negative on a {1}.", fieldName, parentName);
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void ValidateLongNotNegative(long fieldValue, string fieldName, string parentName)
        {
            if (fieldValue < 0)
            {
                string message = string.IsNullOrEmpty(parentName) ?
                    string.Format("{0} may not negative.", fieldName) :
                    string.Format("{0} may not negative on a {1}.", fieldName, parentName);
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void ValidateDecimalNotNegative(decimal fieldValue, string fieldName, string parentName)
        {
            if (fieldValue < 0)
            {
                string message = string.IsNullOrEmpty(parentName) ?
                    string.Format("{0} may not negative.", fieldName) :
                    string.Format("{0} may not negative on a {1}.", fieldName, parentName);
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void ValidateDoubleNotNegative(double fieldValue, string fieldName, string parentName)
        {
            if (fieldValue < 0)
            {
                string message = string.IsNullOrEmpty(parentName) ?
                    string.Format("{0} may not negative.", fieldName) :
                    string.Format("{0} may not negative on a {1}.", fieldName, parentName);
                throw new ArgumentOutOfRangeException(message);
            }
        }

        #endregion //Methods
    }
}
