namespace Figlut.Mobile.Configuration.Manager.UI
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
    using Figlut.Mobile.Toolkit.Data;
    using Figlut.Mobile.Toolkit.Utilities.SettingsFile;
    using Figlut.Mobile.Toolkit.Utilities;
    using System.IO;
    using Figlut.Mobile.Configuration.Manager.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public partial class MainForm : BaseForm
    {
        #region Constructors

        public MainForm()
        {
            GOC.Instance.ShowMessageBoxOnException = true;
            InitializeComponent();
            using (SplashForm f = new SplashForm())
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    Close();
                }
            }
            InitHiddenColumns();
            InitializeImages();
        }

        #endregion //Constructors

        #region Fields

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
        private ImageList _treeImageList;

        private List<string> _hiddenProperties;
        private EntityCache<string, SettingItem> _currentCategorySettings;
        private TreeNode _selectedNode;
        private Settings _currentSettings;

        #endregion //Fields

        #region Methods

        private void InitHiddenColumns()
        {
            _hiddenProperties = new List<string>();
            _hiddenProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.SettingName, true));
            _hiddenProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.SettingType, true));
            _hiddenProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.CategorySequenceId, true));
            _hiddenProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.PasswordChar, true));
            _hiddenProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.ListViewItem, true));
            _hiddenProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.SettingsCategoryInfo, true));
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

            _treeImageList = new ImageList();
            _treeImageList.ImageSize = new Size(32, 32);
            _treeImageList.Images.Add(_figlutSuiteImage);
            _treeImageList.Images.Add(_configurationManagerImage);
            _treeImageList.Images.Add(_webServiceImage);
            _treeImageList.Images.Add(_databaseImage);
            _treeImageList.Images.Add(_serviceImage);
            _treeImageList.Images.Add(_loggingImage);
            _treeImageList.Images.Add(_desktokDataBoxImage);
            _treeImageList.Images.Add(_webServiceImage);
            _treeImageList.Images.Add(_helpImage);
            _treeImageList.Images.Add(_exitImage);

            trvComponentSettings.ImageList = _treeImageList;
        }

        private void InitializeComponents()
        {
            trvComponentSettings.Nodes.Clear();
            TreeNode figlutSuiteNode = new TreeNode("Figlut Suite") { ImageIndex = 0, SelectedImageIndex = 0 };

            if (ComponentsSettings.Instance[ComponentId.FiglutConfigurationManager] != null)
            {
                Settings configurationManagerSettings = ComponentsSettings.Instance[ComponentId.FiglutConfigurationManager];
                TreeNode configurationManagerNode = new TreeNode("Figlut Configuration Manager") { ImageIndex = 1, SelectedImageIndex = 1 };
                configurationManagerNode.Tag = configurationManagerSettings;
                configurationManagerNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(configurationManagerSettings, "User Interface"), 6));
                configurationManagerNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(configurationManagerSettings, "Logging"), 5));
                figlutSuiteNode.Nodes.Add(configurationManagerNode);
            }
            if (ComponentsSettings.Instance[ComponentId.FiglutMobileDataBox] != null)
            {
                Settings mobileDataBoxSettings = ComponentsSettings.Instance[ComponentId.FiglutMobileDataBox];
                TreeNode desktopDataBoxNode = new TreeNode("Figlut Mobile DataBox") { SelectedImageIndex = 6, ImageIndex = 6 };
                desktopDataBoxNode.Tag = mobileDataBoxSettings;
                desktopDataBoxNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(mobileDataBoxSettings, "Web Service"), 2));
                desktopDataBoxNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(mobileDataBoxSettings, "DataBox"), 6));
                desktopDataBoxNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(mobileDataBoxSettings, "Logging"), 5));
                figlutSuiteNode.Nodes.Add(desktopDataBoxNode);
            }
            trvComponentSettings.Nodes.Add(figlutSuiteNode);
        }

        private TreeNode BuildSettingsCategoryNode(SettingsCategoryInfo settingsCategoryInfo, int imageIndex)
        {
            TreeNode result = new TreeNode(settingsCategoryInfo.Category) { SelectedImageIndex = imageIndex, ImageIndex = imageIndex };
            result.Tag = settingsCategoryInfo.Settings.GetSettingsByCategory(settingsCategoryInfo);
            return result;
        }

        #endregion //Methods

        #region Event Handlers

        private void mnuExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (UIHelper.AskQuestion("Are you sure you want to exit?") == DialogResult.Yes)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, MainForm_KeyDown);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                statusMain.Text = "Configure component settings.";
                FiglutConfigurationManagerSettings settings = GOC.Instance.GetSettings<FiglutConfigurationManagerSettings>();
                if (!settings.MainMenuVisible)
                {
                    this.Menu = null;
                    trvComponentSettings.Height += 50;
                }
                GOC.Instance.Logger = new Logger(
                    settings.LogToFile,
                    settings.LogToWindowsEventLog,
                    settings.LogFileName,
                    settings.LoggingLevel,
                    settings.EventSourceName,
                    settings.EventLogName);
                InitializeComponents();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, MainForm_KeyDown);
            }
        }

        private void trvComponentSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.IsSelected &&
                    e.Node.Tag != null &&
                    e.Node.Tag.GetType().Equals(typeof(EntityCache<string, SettingItem>)))
                {
                    _selectedNode = e.Node;
                }
                else
                {
                    _selectedNode = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, MainForm_KeyDown);
            }
        }

        private void mnuUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedNode == null)
                {
                    UIHelper.DisplayError("No settings category selected to be updated.", this, MainForm_KeyDown);
                    return;
                }
                Settings settings = _selectedNode.Tag as Settings;
                _currentCategorySettings = null;
                _currentCategorySettings = _selectedNode.Tag as EntityCache<string, SettingItem>;
                if (_currentCategorySettings == null)
                {
                    throw new InvalidCastException(string.Format("Expected a {0} in the selected node and found a {1}.",
                        _currentCategorySettings.GetType().FullName,
                        _selectedNode.Tag.GetType().FullName));
                }
                using (EditSettingsForm f = new EditSettingsForm(_currentCategorySettings, _hiddenProperties))
                {
                    f.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, MainForm_KeyDown);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    mnuExit_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    mnuUpdate_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true, this, MainForm_KeyDown);
            }
        }

        #endregion //Event Handlers
    }
}