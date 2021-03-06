﻿namespace Figlut.Server.Toolkit.Utilities.SettingsFile
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using Figlut.Server.Toolkit.Utilities;
    using System.Reflection;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities.Serialization;

    #endregion //Using Directives

    public abstract class Settings
    {
        #region Constructors

        public Settings()
        {
            Type type = this.GetType();
            _name = type.Name;
            _filePath = Path.Combine(Information.GetExecutingDirectory(), string.Format("{0}.xml", type.Name));
        }

        public Settings(string filePath)
        {
            Type type = this.GetType();
            _name = type.Name;
            _filePath = filePath;
        }

        public Settings(string name, string filePath)
        {
            _name = name;
            _filePath = filePath;
        }

        #endregion //Constructors

        #region Fields

        protected string _name;
        protected string _filePath;
        protected bool _showMessageBoxOnException;

        #endregion //Fields

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public bool ShowMessageBoxOnException
        {
            get { return _showMessageBoxOnException; }
            set { _showMessageBoxOnException = value; }
        }

        #endregion //Properties

        #region Methods

        public virtual void RefreshFromFile(bool saveIfFileDoesNotExist, bool validateAllSettingValuesSet)
        {
            if (!File.Exists(_filePath))
            {
                if (saveIfFileDoesNotExist)
                {
                    SaveToFile();
                }
                else
                {
                    throw new FileNotFoundException(string.Format("Could not find {0}.", Path.GetFileName(_filePath)));
                }
            }
            Type thisType = this.GetType();
            object fileSettings = GOC.Instance.GetSerializer(SerializerType.XML).DeserializeFromFile(this.GetType(), _filePath);
            bool requireSave = false;
            foreach (PropertyInfo p in thisType.GetProperties())
            {
                object valueFromFile = p.GetValue(fileSettings, null);
                if (p.Name == EntityReader<Settings>.GetPropertyName(sp => sp.FilePath, false))
                {
                    if (valueFromFile.ToString() != _filePath)
                    {
                        /*The location of the settings file has changed and we need
                         * to keep the latest file path (not the one from the file)
                         * and save the file with this new file path in place.*/
                        requireSave = true;
                    }
                    continue;
                }
                if (validateAllSettingValuesSet && valueFromFile == null)
                {
                    throw new NullReferenceException(string.Format(
                        "{0} not set in {1}.",
                        p.Name,
                        _filePath));
                }
                p.SetValue(this, valueFromFile, null);
            }
            if (requireSave)
            {
                SaveToFile();
            }
        }

        public void SaveToFile()
        {
            GOC.Instance.GetSerializer(SerializerType.XML).SerializeToFile(this, _filePath);
        }

        public EntityCache<string, SettingItem> GetSettingsByCategory(SettingsCategoryInfo settingsCategoryInfo)
        {
            string categoryLower = settingsCategoryInfo.Category.Trim().ToLower();
            Type settingsType = this.GetType();
            List<SettingItem> settingItems = new List<SettingItem>();
            foreach (PropertyInfo p in settingsType.GetProperties())
            {
                object[] categoryAttributes = p.GetCustomAttributes(typeof(SettingInfoAttribute), true);
                if(categoryAttributes == null)
                {
                    continue;
                }
                foreach (SettingInfoAttribute c in categoryAttributes)
                {
                    if (c.Category.Trim().ToLower() == categoryLower)
                    {
                        SettingItem settingItem = new SettingItem(
                            p.Name,
                            EntityReader.GetPropertyValue(p.Name, this, false),
                            p.PropertyType,
                            c.AutoFormatDisplayName,
                            c.DisplayName,
                            c.Description,
                            c.CategorySequenceId,
                            c.PasswordChar,
                            settingsCategoryInfo);
                        settingItems.Add(settingItem);
                    }
                }
            }
            string entityCacheName = string.Format("{0} {1} Settings", DataShaper.ShapeCamelCaseString(settingsType.Name).Replace("Settings", "").Trim(), settingsCategoryInfo.Category);
            EntityCache<string, SettingItem> result = new EntityCache<string, SettingItem>(entityCacheName);
            settingItems.OrderBy(p => p.CategorySequenceId).ToList().ForEach(p => result.Add(p.SettingName, p));
            return result;
        }

        #endregion //Methods
    }
}