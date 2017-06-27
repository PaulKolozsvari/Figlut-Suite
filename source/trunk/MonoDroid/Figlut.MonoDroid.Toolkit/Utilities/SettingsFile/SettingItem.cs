using Figlut.MonoDroid.Toolkit.Data;

namespace Figlut.MonoDroid.Toolkit.Utilities.SettingsFile
{
    #region Using Directives

    using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    #endregion //Using Directives

    public class SettingItem
    {
        #region Constructors

        public SettingItem(
            string settingName,
            object settingValue,
            Type settingType,
            bool autoFormatDisplayName,
            string settingDisplayName,
            string settingDescription,
            int categorySequenceId,
            char passwordChar,
			bool selectFilePath,
			bool visible,
            SettingsCategoryInfo settingsCategoryInfo)
        {
            _settingName = settingName;
            _settingValue = settingValue;
            _settingType = settingType;
            _settingsCategoryInfo = settingsCategoryInfo;
            if (string.IsNullOrEmpty(settingDisplayName))
            {
                _settingDisplayName = autoFormatDisplayName ? DataShaper.ShapeCamelCaseString(settingName) : settingName;
            }
            else
            {
                _settingDisplayName = settingDisplayName;
            }
            _settingDescription = settingDescription;
            _categorySequenceId = categorySequenceId;
            _passwordChar = passwordChar;
			_selectFilePath = selectFilePath;
			_visible = visible;
        }

        #endregion //Constructors

        #region Fields

        protected string _settingName;
        protected object _settingValue;
        protected Type _settingType;
        protected string _settingDisplayName;
        protected string _settingDescription;
        protected int _categorySequenceId;
        protected char _passwordChar;
		protected bool _selectFilePath;
		protected bool _visible;
        protected SettingsCategoryInfo _settingsCategoryInfo;

        #endregion //Fields

        #region Properties

        public string SettingDisplayName
        {
            get { return _settingDisplayName; }
        }

        public string SettingName
        {
            get { return _settingName; }
        }

        public object SettingValue
        {
            get { return _settingValue; }
            set { _settingValue = value; }
        }

        public string SettingDescription
        {
            get { return _settingDescription; }
        }

        public Type SettingType
        {
            get { return _settingType; }
        }

        public int CategorySequenceId
        {
            get { return _categorySequenceId; }
            set { _categorySequenceId = value; }
        }

        public char PasswordChar
        {
            get { return _passwordChar; }
            set { _passwordChar = value; }
        }

		public bool SelectFilePath
		{
			get{ return _selectFilePath; }
			set{ _selectFilePath = value; }
		}

		public bool Visible
		{
			get{ return _visible; }
			set{ _visible = value; }
		}

        public SettingsCategoryInfo SettingsCategoryInfo
        {
            get { return _settingsCategoryInfo; }
        }

        #endregion //Properties
    }
}