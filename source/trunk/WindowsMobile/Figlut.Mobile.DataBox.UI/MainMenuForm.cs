namespace Figlut.Mobile.DataBox.UI
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using Figlut.Mobile.DataBox.UI.Base;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.DataBox.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.Logging;
    using System.Net;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;
    using System.IO;
    using Figlut.Mobile.Toolkit.Data;
    using Microsoft.Win32;
    using Figlut.Mobile.Configuration.Manager.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.SettingsFile;
    using System.Diagnostics;
    using Figlut.Mobile.DataBox.UI.Utilities;

    #endregion //Using Directives

    public partial class MainMenuForm : BaseForm
    {
        #region Constructors

        public MainMenuForm(FiglutMobileDataBoxSettings settings)
        {
            GOC.Instance.ShowMessageBoxOnException = true;
            InitializeComponent();
            SetUserAgent();
            _settings = settings;
            picLogo.Image = FiglutDataBoxApplication.Instance.ApplicationBannerImage;
            using (SplashForm f = new SplashForm())
            {
                if (f.ShowDialog() == DialogResult.Cancel)
                {
                    Close();
                }
            }
        }

        #endregion //Constructors

        #region Constants

        private const string CONFIGURATION_MANAGER_FILE_NAME = "Figlut.Mobile.Configuration.Manager.exe";
        private Guid APPLICATION_ID = new Guid("F92655FA-A532-4a51-8EE7-18C84D05E8B6");

        #endregion //Constants

        #region Fields

        private FiglutMobileDataBoxSettings _settings;

        private Image _figlutSuiteImage;
        private Image _configurationManagerImage;
        private Image _webServiceImage;
        private Image _databaseImage;
        private Image _serviceImage;
        private Image _loggingImage;
        private Image _desktokDataBoxImage;
        private Image _webSiteImage;
        private Image _helpImage;
        private Image _exitImage;
        private ImageList _lstImageList;

        #endregion //Fields

        #region Methods

        private void SetUserAgent()
        {
            GOC.Instance.UserAgent = new FiglutClientInfo(
                APPLICATION_ID,
                GOC.Instance.ExecutableName,
                GOC.Instance.Version,
                Environment.OSVersion.ToString(),
                Information.GetDeviceIdBase64String(GOC.Instance.ExecutableName)).GetHash();
        }

        private void InitializeImages()
        {
            string imagesFolder = Path.Combine(Information.GetExecutingDirectory(), "Images");
            FileSystemHelper.ValidateDirectoryExists(imagesFolder);

            string figlutSuiteImageFilePath = Path.Combine(imagesFolder, "FiglutManager.png"); //0
            FileSystemHelper.ValidateFileExists(figlutSuiteImageFilePath);

            string configurationManagerImageFilePath = Path.Combine(imagesFolder, "FiglutConfigurationManager.png"); //1
            FileSystemHelper.ValidateFileExists(configurationManagerImageFilePath);
            string webServiceImageFilePath = Path.Combine(imagesFolder, "FiglutWebService.png"); //2
            FileSystemHelper.ValidateFileExists(webServiceImageFilePath);
            string databaseImageFilePath = Path.Combine(imagesFolder, "FiglutDatabase.png"); //3
            FileSystemHelper.ValidateFileExists(databaseImageFilePath);
            string serviceImageFilePath = Path.Combine(imagesFolder, "FiglutService.png"); //4
            FileSystemHelper.ValidateFileExists(serviceImageFilePath);
            string loggingImageFilePath = Path.Combine(imagesFolder, "FiglutLogging.png"); //5
            FileSystemHelper.ValidateFileExists(loggingImageFilePath);
            string desktopDataBoxImageFilePath = Path.Combine(imagesFolder, "FiglutMobileDataBox.png"); //6
            FileSystemHelper.ValidateFileExists(desktopDataBoxImageFilePath);
            string webSiteImageFilePath = Path.Combine(imagesFolder, "FiglutWebSite.png"); //7
            FileSystemHelper.ValidateFileExists(webSiteImageFilePath);
            string helpImageFilePath = Path.Combine(imagesFolder, "FiglutHelp.png"); //8
            FileSystemHelper.ValidateFileExists(helpImageFilePath);
            string exitImageFilePath = Path.Combine(imagesFolder, "FiglutExit.png"); //9
            FileSystemHelper.ValidateFileExists(exitImageFilePath);

            _figlutSuiteImage = new Bitmap(figlutSuiteImageFilePath);
            _configurationManagerImage = new Bitmap(configurationManagerImageFilePath);
            _webServiceImage = new Bitmap(webServiceImageFilePath);
            _databaseImage = new Bitmap(databaseImageFilePath);
            _serviceImage = new Bitmap(serviceImageFilePath);
            _loggingImage = new Bitmap(loggingImageFilePath);
            _desktokDataBoxImage = new Bitmap(desktopDataBoxImageFilePath);
            _webSiteImage = new Bitmap(webSiteImageFilePath);
            _helpImage = new Bitmap(helpImageFilePath);
            _exitImage = new Bitmap(exitImageFilePath);

            _lstImageList = new ImageList();
            _lstImageList.ImageSize = new Size(32, 32);
            _lstImageList.Images.Add(_figlutSuiteImage);
            _lstImageList.Images.Add(_configurationManagerImage);
            _lstImageList.Images.Add(_webServiceImage);
            _lstImageList.Images.Add(_databaseImage);
            _lstImageList.Images.Add(_serviceImage);
            _lstImageList.Images.Add(_loggingImage);
            _lstImageList.Images.Add(_desktokDataBoxImage);
            _lstImageList.Images.Add(_webServiceImage);
            _lstImageList.Images.Add(_helpImage);
            _lstImageList.Images.Add(_exitImage);

            lstMenu.LargeImageList = _lstImageList;
            lstMenu.SmallImageList = _lstImageList;
        }

        private void InitializeMenuListView()
        {
            lstMenu.Items.Clear();
            InitializeImages();
            lstMenu.Columns.Add("Menu", lstMenu.Width - 5, HorizontalAlignment.Right);
            lstMenu.Items.Add(new ListViewItem(MainMenuOption.DataBox.ToString()) { Tag = MainMenuOption.DataBox, ImageIndex = 6 });
            lstMenu.Items.Add(new ListViewItem(DataShaper.ShapeCamelCaseString(MainMenuOption.FiglutConfigurationManager.ToString())) { Tag = MainMenuOption.FiglutConfigurationManager, ImageIndex = 1 });
            //lstMenu.Items.Add(new ListViewItem(MainMenuOption.Help.ToString()) { Tag = MainMenuOption.Help, ImageIndex = 8 });
            lstMenu.Items.Add(new ListViewItem(MainMenuOption.Exit.ToString()) { Tag = MainMenuOption.Exit, ImageIndex = 9 });
            lstMenu.LargeImageList = _lstImageList;
            lstMenu.LargeImageList.ImageSize = new Size(64, 64);
            lstMenu.SmallImageList = _lstImageList;
            lstMenu.SmallImageList.ImageSize = new Size(64, 64);
        }

        private void StartMobileConfigurationManager()
        {
            using (WaitProcess w = new WaitProcess(mnuMain, statusMain))
            {
                string configManagerFriendlyName = "Configuration Manager";
                w.ChangeStatus(string.Format("Searching for {0} ...", configManagerFriendlyName));
                Settings configManagerSettings = ComponentsSettings.Instance[ComponentId.FiglutConfigurationManager];
                if (configManagerSettings == null)
                {
                    throw new UserThrownException(
                        string.Format("{0} not installed.", configManagerFriendlyName),
                        LoggingLevel.Maximum);
                }
                string installDirectory = Path.GetDirectoryName(configManagerSettings.FilePath);
                string configManagerFilePath = Path.Combine(installDirectory, CONFIGURATION_MANAGER_FILE_NAME);
                if (!File.Exists(configManagerFilePath))
                {
                    throw new FileNotFoundException(string.Format("Could not find {0}.", configManagerFilePath));
                }
                w.ChangeStatus(string.Format("Starting {0} ...", configManagerFriendlyName));
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = configManagerFilePath,
                    //WorkingDirectory = installDirectory,
                    UseShellExecute = true
                };
                using (Process p = new Process() { StartInfo = startInfo })
                {
                    p.Start();
                }
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Select menu option.";
                if (!GOC.Instance.GetSettings<FiglutMobileDataBoxSettings>().MainMenuVisible)
                {
                    this.Menu = null;
                    lstMenu.Height += 50;
                }
                InitializeMenuListView();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (UIHelper.AskQuestion("Are you sure you want to exit?") == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, MainMenuForm_KeyDown);
            }
        }

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstMenu.SelectedIndices.Count < 1)
                {
                    lstMenu.Focus();
                    UIHelper.DisplayError("No menu option selected.");
                    return;
                }
                MainMenuOption selectedOption = (MainMenuOption)lstMenu.Items[lstMenu.SelectedIndices[0]].Tag;
                switch (selectedOption)
                {
                    case MainMenuOption.DataBox:
                        using (DataBoxForm f = new DataBoxForm())
                        {
                            f.ShowDialog();
                        }
                        break;
                    case MainMenuOption.FiglutConfigurationManager:
                        StartMobileConfigurationManager();
                        break;
                    case MainMenuOption.Help:
                        throw new NotImplementedException();
                    case MainMenuOption.Exit:
                        mnuExit_Click(sender, e);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, MainMenuForm_KeyDown);
            }
        }

        private void MainMenuForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuExit_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    mnuSelect_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, MainMenuForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}