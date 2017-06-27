namespace Figlut.Mobile.Toolkit.Utilities.SettingsFile
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class SettingsCategoryAttribute : Attribute
    {
        #region Constructors

        public SettingsCategoryAttribute(string category)
        {
            _category = category;
        }

        #endregion //Constructors

        #region Fields

        protected string _category;

        #endregion //Fields

        #region Properties

        public string Category
        {
            get { return _category; }
        }

        #endregion //Properties
    }
}