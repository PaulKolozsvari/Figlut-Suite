namespace Figlut.Server.Toolkit.Utilities.SettingsFile
{
    #region Using Directives

    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Figlut.Server.Toolkit.Data;
using Figlut.Server.Toolkit.Mmc;
using Figlut.Server.Toolkit.Mmc.Forms;

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
            SettingsControl settingControl,
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
            _listViewItem = new ListViewItem(_settingDisplayName);
            if (_passwordChar != '\0')
            {
                _listViewItem.SubItems.Add(DataShaper.MaskPasswordString(_settingValue.ToString(), _passwordChar));
            }
            else
            {
                _listViewItem.SubItems.Add(_settingValue != null ? _settingValue.ToString() : null);
            }
            _listViewItem.SubItems.Add(_settingDescription);
            _listViewItem.Tag = this;
            _settingControl = settingControl;
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
        protected ListViewItem _listViewItem;
        protected SettingsControl _settingControl;
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

        public ListViewItem ListViewItem
        {
            get { return _listViewItem; }
        }

        public SettingsCategoryInfo SettingsCategoryInfo
        {
            get { return _settingsCategoryInfo; }
        }

        #endregion //Properties

        #region Methods

        public void RefreshSettingsByCategory(string settingValue)
        {
            _listViewItem.SubItems[1].Text = settingValue;
            _settingControl.RefreshData();
        }

        #endregion //Methods
    }
}