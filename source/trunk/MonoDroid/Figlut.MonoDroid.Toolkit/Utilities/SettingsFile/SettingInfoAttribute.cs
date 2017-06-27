namespace Figlut.MonoDroid.Toolkit.Utilities.SettingsFile
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class SettingInfoAttribute : Attribute
    {
        #region Constructors

        public SettingInfoAttribute(string category)
        {
            _category = category;
            _autoFormatDisplayName = false;
			_visible = true;
        }

        #endregion //Constructors

        #region Fields

        protected string _category;
        protected bool _autoFormatDisplayName;
        protected string _displayName;
        protected string _description;
        protected int _categorySequenceId;
        protected char _passwordChar;
		protected bool _selectFilePath;
		protected bool _visible;

        #endregion //Fields

        #region Properties

        public string Category
        {
            get { return _category; }
        }

        public bool AutoFormatDisplayName
        {
            get { return _autoFormatDisplayName; }
            set { _autoFormatDisplayName = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
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

        #endregion //Properties
    }
}