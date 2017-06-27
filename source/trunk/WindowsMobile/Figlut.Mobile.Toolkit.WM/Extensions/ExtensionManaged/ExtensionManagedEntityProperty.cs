namespace Figlut.Mobile.Toolkit.Extensions.ExtensionManaged
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Mobile.Toolkit.Data;

    #endregion //Using Directives

    public class ExtensionManagedEntityProperty
    {
        #region Constructors

        public ExtensionManagedEntityProperty(
            string parentEntityFullTypeName,
            string propertyName)
        {
            _propertyName = propertyName;
        }

        #endregion //Constructors

        #region Fields

        protected string _propertyName;

        #endregion //Fields

        #region Properties

        public string PropertyName
        {
            get { return _propertyName; }
        }

        #endregion //Properties
    }
}