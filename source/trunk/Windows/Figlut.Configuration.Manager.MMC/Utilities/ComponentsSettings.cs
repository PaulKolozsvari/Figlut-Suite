namespace Figlut.Configuration.Manager.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Figlut.Desktop.DataBox.Configuration;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;
    using Figlut.Web.Service.Configuration;
    using Microsoft.Win32;

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
        internal const string WEB_SERVICE_INSTALLATION_DIRECTORY_REG_NAME = "FiglutWebServiceInstallationDirectory";
        internal const string DESKTOP_DATABOX_INSTALLATION_DIRECTORY_REG_NAME = "FiglutDesktopDataBoxInstallationDirectory";
        internal const string SERVER_TOOLKIT_INSTALLATION_DIRECTORY_REG_NAME = "FiglutServerToolkitInstallationDirectory";
        internal const string MOBILE_TOOLKIT_INSTALLATION_DIRECTRORY_REG_NAME = "FiglutMobileToolkitWMInstallationDirectory";

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
                            FileSystemHelper.ValidataDirectoryExists(installationDirectory);
                            string configurationManagerSettingsFilePath = Path.Combine(installationDirectory, string.Format("{0}.xml", typeof(FiglutConfigurationManagerSettings).Name));
                            FileSystemHelper.ValidateFileExists(configurationManagerSettingsFilePath);
                            FiglutConfigurationManagerSettings configurationManagerSettings = new FiglutConfigurationManagerSettings(configurationManagerSettingsFilePath);
                            configurationManagerSettings.RefreshFromFile(true, false);
                            this.Add(ComponentId.FiglutConfigurationManager, configurationManagerSettings);
                            break;
                        case ComponentId.FiglutWebService:
                            FileSystemHelper.ValidataDirectoryExists(installationDirectory);
                            string webServiceSettingsFilePath = Path.Combine(installationDirectory, string.Format("{0}.xml", typeof(FiglutWebServiceSettings).Name));
                            FileSystemHelper.ValidateFileExists(webServiceSettingsFilePath);
                            FiglutWebServiceSettings webServiceSettings = new FiglutWebServiceSettings(webServiceSettingsFilePath);
                            webServiceSettings.RefreshFromFile(true, false);
                            this.Add(ComponentId.FiglutWebService, webServiceSettings);
                            break;
                        case ComponentId.FiglutDesktopDataBox:
                            FileSystemHelper.ValidataDirectoryExists(installationDirectory);
                            string desktopDataBoxSettingsFilePath = Path.Combine(installationDirectory, string.Format("{0}.xml", typeof(FiglutDesktopDataBoxSettings).Name));
                            FileSystemHelper.ValidateFileExists(desktopDataBoxSettingsFilePath);
                            FiglutDesktopDataBoxSettings desktopDataBoxSettings = new FiglutDesktopDataBoxSettings(desktopDataBoxSettingsFilePath);
                            desktopDataBoxSettings.RefreshFromFile(true, false);
                            this.Add(ComponentId.FiglutDesktopDataBox, desktopDataBoxSettings);
                            break;
                        case ComponentId.FiglutServerToolkit:
                            break;
                        case ComponentId.FiglutMobileToolkit:
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
            using (RegistryKey figlutKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Figlut", RegistryKeyPermissionCheck.ReadSubTree, System.Security.AccessControl.RegistryRights.ReadKey))
            {
                if (figlutKey == null)
                {
                    throw new UserThrownException("No Figlut components installed.", LoggingLevel.Minimum, true);
                }
                object value = null;
                
                value = figlutKey.GetValue(CONFIGURATION_MANAGER_INSTALLATION_DIRECTORY_REG_NAME);
                string configurationManagerDirectory = value == null ? null : value.ToString();
                result.Add(ComponentId.FiglutConfigurationManager, configurationManagerDirectory);

                value = figlutKey.GetValue(WEB_SERVICE_INSTALLATION_DIRECTORY_REG_NAME);
                string webServiceDirectory = value == null ? null : value.ToString();
                result.Add(ComponentId.FiglutWebService, webServiceDirectory);

                value = figlutKey.GetValue(DESKTOP_DATABOX_INSTALLATION_DIRECTORY_REG_NAME);
                string desktopDataBoxDirectory = value == null ? null : value.ToString();
                result.Add(ComponentId.FiglutDesktopDataBox, desktopDataBoxDirectory);

                value = figlutKey.GetValue(SERVER_TOOLKIT_INSTALLATION_DIRECTORY_REG_NAME);
                string serverToolkitDirectory = value == null ? null : value.ToString();
                result.Add(ComponentId.FiglutServerToolkit, desktopDataBoxDirectory);

                value = figlutKey.GetValue(MOBILE_TOOLKIT_INSTALLATION_DIRECTRORY_REG_NAME);
                string mobileToolkitDirectory = value == null ? null : value.ToString();
                result.Add(ComponentId.FiglutMobileToolkit, mobileToolkitDirectory);
            }
            return result;
        }

        public void SaveAllSettings()
        {
            foreach (Settings s in _entities.Values)
            {
                s.SaveToFile();
            }
        }

        #endregion //Methods
    }
}