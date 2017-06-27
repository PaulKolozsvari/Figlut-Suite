#region Using Directives

using System;
using System.ComponentModel;
using System.Security.Permissions;
using Microsoft.ManagementConsole;
using Figlut.Server.Toolkit.Utilities;
using Figlut.Server.Toolkit.Utilities.Logging;
using Figlut.Server.Toolkit.Mmc.Forms;
using System.Drawing;
using System.IO;
using Figlut.Server;
using Figlut.Server.Toolkit.Mmc;
using Microsoft.Aspnet.Snapin;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Figlut.Web.Service.Configuration;
using Figlut.Configuration.Manager.Utilities;
using Figlut.Server.Toolkit.Utilities.SettingsFile;

#endregion //Using Directives

[assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Unrestricted = true)]

namespace Figlut.Configuration.Manager
{
    /// <summary>
    /// RunInstaller attribute - Allows the .Net framework InstallUtil.exe to install the assembly.
    /// SnapInInstaller class - Installs snap-in for MMC.
    /// </summary>
    [RunInstaller(true)]
    public class InstallUtilSupport : SnapInInstaller
    {
    }
    
    /// <summary>
    /// SnapInSettings attribute - Used to set the registration information for the snap-in.
    /// SnapIn class - Provides the main entry point for the creation of a snap-in. 
    /// SelectionFormViewSnapIn class - SnapIn that uses a Form in the result pane
    /// </summary>
    [SnapInSettings("{D5A3B189-B242-4CFB-B459-61B3661803AB}",
       DisplayName = "Figlut Configuration Manager",
       Description = "An MMC snap-in for managing the Figlut Suite.",
       UseCustomHelp=true)]
    [SnapInAbout("Figlut.Configuration.Manager.About.x86.dll",
        ApplicationBaseRelative=true,
        DescriptionId=106,
        DisplayNameId=104,
        IconId=101,
        LargeFolderBitmapId=103,
        SmallFolderBitmapId=103,
        SmallFolderSelectedBitmapId=103,
        VendorId=105,
        VersionId=107)]
    public class FiglutConfigurationManagerSnapIn : SnapIn
	{
        #region Constants
		
        private const string HELP_FILE_NAME = "FiglutSuiteHelp.chm";
		
        #endregion //Constants

        #region Constructors

        public FiglutConfigurationManagerSnapIn()
        {
            try
            {
                FiglutConfigurationManagerSettings settings = GOC.Instance.GetSettings<FiglutConfigurationManagerSettings>(true, true);
                GOC.Instance.Logger = new Logger(
                    settings.LogToFile,
                    settings.LogToWindowsEventLog,
                    settings.LogToConsole,
                    settings.LoggingLevel,
                    settings.LogFileName,
                    settings.EventSourceName,
                    settings.EventLogName);
                GOC.Instance.ShowMessageBoxOnException = true;

                InitializeImages();

                ScopeNode figlutSuiteNode = MmcScopeNodeFactory.BuildRootNode("Figlut Suite", 0, 0, null);
                this.RootNode = figlutSuiteNode;
                this.RootNode.HelpTopic = "FiglutServerManager";

                this.RootNode.HelpTopic = @"FiglutSuiteHelp.chm::/FiglutServerManager.htm";

                if (ComponentsSettings.Instance[ComponentId.FiglutConfigurationManager] != null)
                {
                    Settings configurationManagerSettings = ComponentsSettings.Instance[ComponentId.FiglutConfigurationManager];
                    ScopeNode configurationManagerNode = MmcScopeNodeFactory.BuildRootNode("Figlut Configuration Manager", 1, 1, configurationManagerSettings);
                    figlutSuiteNode.Children.Add(configurationManagerNode);
                    configurationManagerNode.Children.Add(MmcScopeNodeFactory.BuildSettingsCategoryNode("Logging", "Logging Settings", new SettingsCategoryInfo(configurationManagerSettings, "Logging"), 5, 5, true));
                }
                if (ComponentsSettings.Instance[ComponentId.FiglutWebService] != null)
                {
                    Settings webServiceSettings = ComponentsSettings.Instance[ComponentId.FiglutWebService];
                    ScopeNode webServiceNode = MmcScopeNodeFactory.BuildRootNode("Figlut Web Service", 2, 2, webServiceSettings);
                    figlutSuiteNode.Children.Add(webServiceNode);
                    webServiceNode.Children.Add(MmcScopeNodeFactory.BuildSettingsCategoryNode("Database", "Database Settings", new SettingsCategoryInfo(webServiceSettings, "Database"), 3, 3, true));
                    webServiceNode.Children.Add(MmcScopeNodeFactory.BuildSettingsCategoryNode("Service", "Service Settings", new SettingsCategoryInfo(webServiceSettings, "Service"), 4, 4, true));
                    webServiceNode.Children.Add(MmcScopeNodeFactory.BuildSettingsCategoryNode("Logging", "Logging Settings", new SettingsCategoryInfo(webServiceSettings, "Logging"), 5, 5, true));
                }
                if (ComponentsSettings.Instance[ComponentId.FiglutDesktopDataBox] != null)
                {
                    Settings desktopDataBoxSettings = ComponentsSettings.Instance[ComponentId.FiglutDesktopDataBox];
                    ScopeNode desktopDataBoxNode = MmcScopeNodeFactory.BuildRootNode("Figlut Desktop DataBox", 6, 6, desktopDataBoxSettings);
                    figlutSuiteNode.Children.Add(desktopDataBoxNode);
                    desktopDataBoxNode.Children.Add(MmcScopeNodeFactory.BuildSettingsCategoryNode("Web Service", "Web Service Settings", new SettingsCategoryInfo(desktopDataBoxSettings, "Web Service"), 4, 4, true));
                    desktopDataBoxNode.Children.Add(MmcScopeNodeFactory.BuildSettingsCategoryNode("DataBox", "DataBox Settings", new SettingsCategoryInfo(desktopDataBoxSettings, "DataBox"), 6, 6, true));
                    desktopDataBoxNode.Children.Add(MmcScopeNodeFactory.BuildSettingsCategoryNode("Logging", "Logging Settings", new SettingsCategoryInfo(desktopDataBoxSettings, "Logging"), 5, 5, true));
                }

                //ScopeNode webSiteNode = MmcScopeNodeFactory.BuildRootNode("Figlut Web Site", 11, 11);
                //serverNode.Children.Add(webSiteNode);

                SnapInCallbackService.RegisterSnapInHelpTopicCallback(this, DisplayHelp);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion //Constructors

        #region Methods

        private void InitializeImages()
        {
            string imagesFolder = Path.Combine(Information.GetExecutingDirectory(), "Images");
            FileSystemHelper.ValidataDirectoryExists(imagesFolder);

            string figlutSuiteImageFilePath = Path.Combine(imagesFolder, "FiglutManager.png");
            FileSystemHelper.ValidateFileExists(figlutSuiteImageFilePath);

            string configurationManagerImageFilePath = Path.Combine(imagesFolder, "FiglutConfigurationManager.png");
            FileSystemHelper.ValidateFileExists(configurationManagerImageFilePath);
            string webServiceImageFilePath = Path.Combine(imagesFolder, "FiglutWebService.png");
            FileSystemHelper.ValidateFileExists(webServiceImageFilePath);
            string databaseImageFilePath = Path.Combine(imagesFolder, "FiglutDatabase.png");
            FileSystemHelper.ValidateFileExists(databaseImageFilePath);
            string serviceImageFilePath = Path.Combine(imagesFolder, "FiglutService.png");
            FileSystemHelper.ValidateFileExists(serviceImageFilePath);
            string loggingImageFilePath = Path.Combine(imagesFolder, "FiglutLogging.png");
            FileSystemHelper.ValidateFileExists(loggingImageFilePath);
            string desktopDataBoxImageFilePath = Path.Combine(imagesFolder, "FiglutDesktopDataBox.png");
            FileSystemHelper.ValidateFileExists(desktopDataBoxImageFilePath);
            string webSiteImageFilePath = Path.Combine(imagesFolder, "FiglutWebSite.png");
            FileSystemHelper.ValidateFileExists(webSiteImageFilePath);

            //FiglutDesktopDataBox


            Image figlutSuiteImage = Image.FromFile(figlutSuiteImageFilePath); //0
            Image configurationManagerImage = Image.FromFile(configurationManagerImageFilePath); //1
            Image webServiceImage = Image.FromFile(webServiceImageFilePath); //2
            Image databaseImage = Image.FromFile(databaseImageFilePath); //3
            Image serviceImage = Image.FromFile(serviceImageFilePath); //4
            Image loggingImage = Image.FromFile(loggingImageFilePath); //5
            Image desktokDataBoxImage = Image.FromFile(desktopDataBoxImageFilePath); //6
            Image webSiteImage = Image.FromFile(webSiteImageFilePath); //7

            AddImageTo(figlutSuiteImage, true, true);
            AddImageTo(configurationManagerImage, true, true);
            AddImageTo(webServiceImage, true, true);
            AddImageTo(databaseImage, true, true);
            AddImageTo(serviceImage, true, true);
            AddImageTo(loggingImage, true, true);
            AddImageTo(desktokDataBoxImage, true, true);
            AddImageTo(webSiteImage, true, true);
        }

        private void AddImageTo(Image image, bool smallImages, bool largeImages)
        {
            if (smallImages)
            {
                this.SmallImages.Add(image);
            }
            if (largeImages)
            {
                this.LargeImages.Add(image);
            }
        }

        private void DisplayHelp(object obj)
        {
            try
            {
                using (Form f = new Form())
                {
                    string helpFilePath = Path.Combine(Information.GetExecutingDirectory(), HELP_FILE_NAME);
                    if (!File.Exists(helpFilePath))
                    {
                        throw new FileNotFoundException(string.Format("Could not find {0}.", helpFilePath));
                    }
                    Help.ShowHelp(
                        f,
                        helpFilePath,
                        HelpNavigator.Find,
                        "");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion //Methods
    }

}