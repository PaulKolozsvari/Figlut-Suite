namespace Figlut.Mobile.Configuration.Manager.Utilities
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Data;
    using Figlut.Mobile.Toolkit.Utilities.SettingsFile;
    using System.IO;
    using Figlut.Mobile.DataBox.Configuration;
    using Microsoft.Win32;
    using Figlut.Mobile.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public class ComponentsSettings : EntityCache<ComponentId, Settings>
    {
        #region Singleton Setup

        private static ComponentsSettings _instance;

        public static ComponentsSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ComponentsSettings();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private ComponentsSettings()
        {
            RefreshComponentsSettings();
        }

        #endregion //Constructors

        #region Constants

        internal const string CONFIGURATION_MANAGER_INSTALLATION_DIRECTORY_REG_NAME = "FiglutConfigurationManagerInstallationDirectory";
        internal const string MOBILE_DATABOX_INSTALLATION_DIRECTORY_REG_NAME = "FiglutMobileDataBoxInstallationDirectory";

        #endregion //Constants

        #region Methods

        internal void RefreshComponentsSettings()
        {
            try
            {
                Dictionary<ComponentId, string> installationDirectories = GetComponentsInstallationDirectories();
                foreach (ComponentId componentId in installationDirectories.Keys)
                {
                    string installationDirectory = installationDirectories[componentId];
                    if (string.IsNullOrEmpty(installationDirectory))
                    {
                        continue;
                    }
                    switch (componentId)
                    {
                        case ComponentId.FiglutConfigurationManager:
                            FileSystemHelper.ValidateDirectoryExists(installationDirectory);
                            string configurationManagerSettingsFilePath = Path.Combine(installationDirectory, string.Format("{0}.xml", typeof(FiglutConfigurationManagerSettings).Name));
                            FileSystemHelper.ValidateFileExists(configurationManagerSettingsFilePath);
                            FiglutConfigurationManagerSettings configurationManagerSettings = new FiglutConfigurationManagerSettings(configurationManagerSettingsFilePath);
                            configurationManagerSettings.RefreshFromFile(true, false);
                            this.Add(ComponentId.FiglutConfigurationManager, configurationManagerSettings);
                            break;
                        case ComponentId.FiglutMobileDataBox:
                            FileSystemHelper.ValidateDirectoryExists(installationDirectory);
                            string desktopDataBoxSettingsFilePath = Path.Combine(installationDirectory, string.Format("{0}.xml", typeof(FiglutMobileDataBoxSettings).Name));
                            FileSystemHelper.ValidateFileExists(desktopDataBoxSettingsFilePath);
                            FiglutMobileDataBoxSettings desktopDataBoxSettings = new FiglutMobileDataBoxSettings(desktopDataBoxSettingsFilePath);
                            desktopDataBoxSettings.RefreshFromFile(true, false);
                            this.Add(ComponentId.FiglutMobileDataBox, desktopDataBoxSettings);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserThrownException(ex.Message, LoggingLevel.Minimum, true);
            }
        }

        internal Dictionary<ComponentId, string> GetComponentsInstallationDirectories()
        {
            Dictionary<ComponentId, string> result = new Dictionary<ComponentId, string>();
            using (RegistryKey figlutKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Figlut", false))
            {
                if (figlutKey == null)
                {
                    throw new UserThrownException("No Figlut components installed.", LoggingLevel.Minimum, true);
                }
                object value = null;

                value = figlutKey.GetValue(CONFIGURATION_MANAGER_INSTALLATION_DIRECTORY_REG_NAME);
                string configurationManagerDirectory = value == null ? null : value.ToString();
                result.Add(ComponentId.FiglutConfigurationManager, configurationManagerDirectory);

                value = figlutKey.GetValue(MOBILE_DATABOX_INSTALLATION_DIRECTORY_REG_NAME);
                string mobileDataBoxDirectory = value == null ? null : value.ToString();
                result.Add(ComponentId.FiglutMobileDataBox, mobileDataBoxDirectory);
            }
            return result;
        }

        public void SaveAllSettings()
        {
            _entities.Values.ToList().ForEach(p => p.SaveToFile());
        }

        #endregion //Methods
    }
}