namespace Figlut.Server.Toolkit.Mmc
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public class SettingsCategoryInfo
    {
        #region Constructors

        public SettingsCategoryInfo(Settings settings, string category)
        {
            _settings = settings;
            if (string.IsNullOrEmpty(category))
            {
                throw new NullReferenceException(string.Format(
                    "Category may not be set to null when constructing a {0}.",
                    this.GetType().FullName));
            }
            _category = category;
        }

        #endregion //Constructors

        #region Fields

        private Settings _settings;
        private string _category;

        #endregion //Fields

        #region Properties

        public Settings Settings
        {
            get { return _settings; }
        }

        public string Category
        {
            get { return _category; }
        }

        #endregion //Properties
    }
}