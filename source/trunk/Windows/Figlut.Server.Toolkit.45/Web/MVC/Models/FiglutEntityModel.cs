namespace Figlut.Server.Toolkit.Web.MVC.Models
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Figlut.Server.Toolkit.Data;

    #endregion //Using Directives

    public abstract class FiglutEntityModel
    {
        #region Methods

        public abstract bool IsValid(out string errorMessage);

        #region String Validation

        public bool IsStringFieldValid(string fieldName, string fieldValue, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(fieldName))
            {
                errorMessage = $"{fieldName} not entered.";
                return false;
            }
            return true;
        }

        #endregion //String Validation

        #region Int Validation

        public bool IsGuidFieldNotEmpty(string fieldName, Guid fieldValue, out string errorMessage)
        {
            errorMessage = null;
            if (fieldValue == Guid.Empty)
            {
                errorMessage = $"{fieldName} may not be empty.";
                return false;
            }
            return true;
        }

        public bool IsIntFieldNotNegative(string fieldName, int fieldValue, out string errorMessage)
        {
            errorMessage = null;
            if (fieldValue < 0)
            {
                errorMessage = $"{fieldName} may not be negative.";
                return false;
            }
            return true;
        }

        public bool IsIntFieldWithinRange(string fieldName, int fieldValue, int minimumValue, int maximumValue, out string errorMessage)
        {
            errorMessage = null;
            if ((fieldValue < minimumValue) || (fieldValue > maximumValue))
            {
                errorMessage = $"{fieldName} may not be less than {minimumValue} and not greater than {maximumValue}.";
                return false;
            }
            return true;
        }

        #endregion //Int Validation

        #region Long Validation

        public bool IsLongFieldNotNegative(string fieldName, long fieldValue, out string errorMessage)
        {
            errorMessage = null;
            if (fieldValue < 0)
            {
                errorMessage = $"{fieldName} may not be negative.";
                return false;
            }
            return true;
        }

        public bool IsLongFieldWithinRange(string fieldName, long fieldValue, long minimumValue, long maximumValue, out string errorMessage)
        {
            errorMessage = null;
            if ((fieldValue < minimumValue) || (fieldValue > maximumValue))
            {
                errorMessage = $"{fieldName} may not be less than {minimumValue} and not greater than {maximumValue}.";
                return false;
            }
            return true;
        }

        #endregion //Long Validation

        #region Double Validation

        public bool IsDoubleFieldNotNegative(string fieldName, double fieldValue, out string errorMessage)
        {
            errorMessage = null;
            if (fieldValue < 0.0)
            {
                errorMessage = $"{fieldName} may not be negative.";
                return false;
            }
            return true;
        }

        public bool IsDoubleFieldWithinRange(string fieldName, double fieldValue, double minimumValue, double maximumValue, out string errorMessage)
        {
            errorMessage = null;
            if ((fieldValue < minimumValue) || (fieldValue > maximumValue))
            {
                errorMessage = $"{fieldName} may not be less than {minimumValue} and not greater than {maximumValue}.";
                return false;
            }
            return true;
        }

        #endregion //Double Validation

        #region Decimal Validation

        public bool IsDecimalFieldNotNegative(string fieldName, decimal fieldValue, out string errorMessage)
        {
            errorMessage = null;
            if (fieldValue < 0)
            {
                errorMessage = $"{fieldName} may not be negative.";
                return false;
            }
            return true;
        }

        public bool IsDecimalFieldWithinRange(string fieldName, decimal fieldValue, decimal minimumValue, decimal maximumValue, out string errorMessage)
        {
            errorMessage = null;
            if ((fieldValue < minimumValue) || (fieldValue > maximumValue))
            {
                errorMessage = $"{fieldName} may not be less than {minimumValue} and not greater than {maximumValue}.";
                return false;
            }
            return true;
        }

        #endregion //Decimal Validation

        #region Date Validation

        public bool IsDateFieldSet(string fieldName, DateTime fieldValue, out string errorMessage)
        {
            errorMessage = null;
            if (fieldValue == new DateTime())
            {
                errorMessage = $"{ fieldName} not set.";
                return false;
            }
            return true;
        }

        #endregion //Date Validation

        #endregion //Methods
    }
}