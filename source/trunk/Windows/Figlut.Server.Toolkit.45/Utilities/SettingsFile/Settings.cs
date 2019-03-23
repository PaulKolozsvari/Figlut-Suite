namespace Figlut.Server.Toolkit.Utilities.SettingsFile
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
    using Figlut.Server.Toolkit.Mmc.Forms;
    using Figlut.Server.Toolkit.Mmc;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Email;

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

        #region GOC Settings

        /// <summary>
        /// The name of the application.
        /// </summary>
        [SettingInfo("Application", AutoFormatDisplayName = true, Description = "The name of the application. This setting should be loaded in the GOC for it to be displayed relevant places.", CategorySequenceId = 0)]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Whether or not a message box should be shown when an exception occurs. This should only be enabled for Windows Forms application.
        /// </summary>
        [SettingInfo("Application", AutoFormatDisplayName = true, Description = "Whether or not a message box should be shown when an exception occurs. This setting should be loaded into GOC for the Exception Handler to determine behavior. This should only be enabled for Windows Forms application.", CategorySequenceId = 1)]
        public bool ShowMessageBoxOnException { get; set; }

        #endregion //GOC Settings

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Queries all the settings in this class grouped by their categorires and writes the categories with each setting name and value to a string which can be logged or displayed.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            EntityCache<string, List<SettingItem>> settings = GetAllSettingItemsGroupedByCategories();
            List<string> categories = settings.GetEntitiesKeys();
            for (int i = 0; i < categories.Count; i++)
            {
                string category = categories[i];
                result.AppendLine($"*** {category} Settings ***");
                result.AppendLine();
                foreach (SettingItem s in settings[category])
                {
                    result.AppendLine($" * {s.SettingDisplayName} : {s.SettingValue}");
                }
                if (i < (categories.Count - 1))
                {
                    result.AppendLine(); //There are more categories to go.
                }
            }
            return result.ToString();
        }

        public virtual void RefreshFromFile(bool saveIfFileDoesNotExist, bool validateAllSettingValuesSet)
        {
            Type thisType = this.GetType();
            if (!File.Exists(_filePath))
            {
                if (GOC.Instance.Logger != null)
                {
                    GOC.Instance.Logger.LogMessage(new LogMessage(string.Format("Could not find {0} at {1} ...", thisType.FullName, _filePath), LogMessageType.Warning, LoggingLevel.Normal));
                }
                if (saveIfFileDoesNotExist)
                {
                    SaveToFile();
                }
                else
                {
                    throw new FileNotFoundException(string.Format("Could not find {0}.", Path.GetFileName(_filePath)));
                }
            }
            if (GOC.Instance.Logger != null)
            {
                GOC.Instance.Logger.LogMessage(new LogMessage(string.Format("Loading {0} from {1} ...", thisType.FullName, _filePath), LogMessageType.Information, LoggingLevel.Normal));
            }
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
                if (valueFromFile is string)
                {
                    valueFromFile = RemoveEscapeSequencesFromNewLineSettingValue((string)valueFromFile);
                }
                p.SetValue(this, valueFromFile, null);
            }
            if (requireSave)
            {
                SaveToFile();
            }
        }

        /// <summary>
        /// When deserializing \r and \n characters in the XML will be deserialized to \\r and \\n, but the real values we want is \r and \n.
        /// Therefore we need to strip the escape characters on carriage returns and new line characters to get the intended value of the setting.
        /// This method should be after Refreshing (deserializing) from the settings file.
        /// </summary>
        private string RemoveEscapeSequencesFromNewLineSettingValue(string settingValue)
        {
            return settingValue.Replace("\\r", "\r").Replace("\\n", "\n");
        }

        /// <summary>
        /// When serializing \r and \n characters to XML, these values will be saved as an actual new line in the XML text file when we in fact want to save
        /// the setting value as a \r and/or \r character. In order to achieve this, we need to add escape sequences to the carriage return and new line characters in order
        /// to save the intended setting values. 
        /// This method should be called before Saving to file (serializing).
        /// </summary>
        private string AddEscapeSequencesToNewLineSettingValue(string settingValue)
        {
            return settingValue.Replace("\r", "\\r").Replace("\n", "\\n");
        }

        public virtual void SaveToFile()
        {
            if (GOC.Instance.Logger != null)
            {
                GOC.Instance.Logger.LogMessage(new LogMessage(string.Format("Saving {0} to {1} ...", this.GetType().FullName, _filePath), LogMessageType.Information, LoggingLevel.Normal));
            }
            Type thisType = this.GetType();
            foreach (PropertyInfo p in thisType.GetProperties())
            {
                object settingValue = p.GetValue(this, null);
                if (settingValue is string)
                {
                    settingValue = AddEscapeSequencesToNewLineSettingValue((string)settingValue);
                }
                p.SetValue(this, settingValue, null);
            }
            GOC.Instance.GetSerializer(SerializerType.XML).SerializeToFile(this, _filePath);
        }

        public EntityCache<string, SettingItem> GetAllSettingItems()
        {
            return GetAllSettingItems(null);
        }

        public EntityCache<string, SettingItem> GetAllSettingItems(string name)
        {
            Type settingsType = this.GetType();
            List<SettingItem> settingItems = new List<SettingItem>();
            foreach (PropertyInfo p in settingsType.GetProperties())
            {
                object[] categoryAttributes = p.GetCustomAttributes(typeof(SettingInfoAttribute), true);
                if (categoryAttributes == null)
                {
                    continue;
                }
                foreach (SettingInfoAttribute c in categoryAttributes)
                {
                    SettingItem settingItem = new SettingItem(
                        c.Category,
                        p.Name,
                        EntityReader.GetPropertyValue(p.Name, this, false),
                        p.PropertyType,
                        c.AutoFormatDisplayName,
                        c.DisplayName,
                        c.Description,
                        c.CategorySequenceId,
                        c.PasswordChar,
                        null,
                        new SettingsCategoryInfo(this, c.Category));
                    settingItems.Add(settingItem);
                }
            }
            string entityCacheName = string.IsNullOrEmpty(name) ? DataShaper.ShapeCamelCaseString(this.GetType().Name) : name;
            EntityCache<string, SettingItem> result = new EntityCache<string, SettingItem>(entityCacheName);
            settingItems.OrderBy(p => p.Category).ToList().ForEach(p => result.Add(p.SettingName, p));
            return result;
        }

        public EntityCache<string, List<SettingItem>> GetAllSettingItemsGroupedByCategories()
        {
            EntityCache<string, List<SettingItem>> result = new EntityCache<string, List<SettingItem>>();
            foreach (var group in GetAllSettingItems().GroupBy(p => p.Category))
            {
                result.Add(group.Key, group.OrderBy(p => p.CategorySequenceId).ToList());
            }
            return result;
        }

        public EntityCache<string, SettingItem> GetSettingsByCategory(SettingsCategoryInfo settingsCategoryInfo)
        {
            return GetSettingsByCategory(settingsCategoryInfo, null);
        }

        public EntityCache<string, SettingItem> GetSettingsByCategory(SettingsCategoryInfo settingsCategoryInfo, SettingsControl settingsControl)
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
                            c.Category,
                            p.Name,
                            EntityReader.GetPropertyValue(p.Name, this, false),
                            p.PropertyType,
                            c.AutoFormatDisplayName,
                            c.DisplayName,
                            c.Description,
                            c.CategorySequenceId,
                            c.PasswordChar,
                            settingsControl,
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