namespace Figlut.Configuration.Manager
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Configuration.Manager.AuxilaryUI;
    using Figlut.Configuration.Manager.Utilities;
    using Figlut.Desktop.BaseUI;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Mmc;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public partial class MainForm : FiglutBaseForm
    {
        #region Constructors

        public MainForm()
        {
            InitializeComponent();
            InitHiddenConfigurationColumns();
            InitHiddenClientsColumns();
            InitializeImages();
        }

        #endregion //Constructors

        #region Fields

        private bool _forceClose;

        private Image _figlutSuiteImage;
        private Image _configurationManagerImage;
        private Image _webServiceImage;
        private Image _databaseImage;
        private Image _serviceImage;
        private Image _loggingImage;
        private Image _desktokDataBoxImage;
        private Image _webSiteImage;
        private Image _reModemImage;
        private Image _barcodeImage;
        private ImageList _treeImageList;

        private List<string> _hiddenConfigurationProperties;
        private EntityCache<string, SettingItem> _currentCategorySettings;
        private TreeNode _selectedConfigurationNode;
        private Settings _currentSettings;

        private List<string> _hiddenClientsProperties;
        private FiglutWebServiceClientManager _figlutWebServiceClientManager;
        private ClientView _currentClientTypeGroup;
        private TreeNode _selectedClientTypeNode;

        #endregion //Fields

        #region Properties

        public bool ForceClose
        {
            get { return _forceClose; }
            set { _forceClose = true; }
        }

        #endregion //Properties

        #region Methods

        private void InitHiddenConfigurationColumns()
        {
            _hiddenConfigurationProperties = new List<string>();
            _hiddenConfigurationProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.SettingName, true));
            _hiddenConfigurationProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.SettingType, true));
            _hiddenConfigurationProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.CategorySequenceId, true));
            _hiddenConfigurationProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.PasswordChar, true));
            _hiddenConfigurationProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.ListViewItem, true));
            _hiddenConfigurationProperties.Add(EntityReader<SettingItem>.GetPropertyName(p => p.SettingsCategoryInfo, true));
            UIHelper.GetDataGridTableStyle<SettingItem>(400, _hiddenConfigurationProperties, true); //TODO The width here makes no difference. The width is set in the refresh grid methods.
        }

        private void InitHiddenClientsColumns()
        {
            _hiddenClientsProperties = new List<string>();
            _hiddenClientsProperties.Add(EntityReader<FiglutClientInfo>.GetPropertyName(p => p.ApplicationId, true));
            UIHelper.GetDataGridTableStyle<FiglutClientInfo>(100, _hiddenClientsProperties, true);
        }

        private void InitializeImages()
        {
            string imagesFolder = Path.Combine(Information.GetExecutingDirectory(), "Images");
            FileSystemHelper.ValidateDirectoryExists(imagesFolder);

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
            string reModemImageFilePath = Path.Combine(imagesFolder, "FiglutReModem.jpg");
            FileSystemHelper.ValidateFileExists(reModemImageFilePath);
            string barcodeImageFilePath = Path.Combine(imagesFolder, "FiglutBarcode.png");
            FileSystemHelper.ValidateFileExists(barcodeImageFilePath);

            _figlutSuiteImage = Image.FromFile(figlutSuiteImageFilePath);
            _configurationManagerImage = Image.FromFile(configurationManagerImageFilePath);
            _webServiceImage = Image.FromFile(webServiceImageFilePath);
            _databaseImage = Image.FromFile(databaseImageFilePath);
            _serviceImage = Image.FromFile(serviceImageFilePath);
            _loggingImage = Image.FromFile(loggingImageFilePath);
            _desktokDataBoxImage = Image.FromFile(desktopDataBoxImageFilePath);
            _webSiteImage = Image.FromFile(webSiteImageFilePath);
            _reModemImage = Image.FromFile(reModemImageFilePath);
            _barcodeImage = Image.FromFile(barcodeImageFilePath);

            _treeImageList = new ImageList();
            _treeImageList.Images.Add(_figlutSuiteImage); //0
            _treeImageList.Images.Add(_configurationManagerImage); //1
            _treeImageList.Images.Add(_webServiceImage); //2
            _treeImageList.Images.Add(_databaseImage); //3
            _treeImageList.Images.Add(_serviceImage); //4
            _treeImageList.Images.Add(_loggingImage); //5
            _treeImageList.Images.Add(_desktokDataBoxImage); //6
            _treeImageList.Images.Add(_webSiteImage); //7
            _treeImageList.Images.Add(_reModemImage); //8
            _treeImageList.Images.Add(_barcodeImage); //9
            trvComponentSettings.ImageList = _treeImageList; //10
        }

        private void InitializeComponents()
        {
            trvComponentSettings.Nodes.Clear();
            TreeNode figlutSuiteNode = new TreeNode("Figlut Suite", 0, 0);

            if (ComponentsSettings.Instance[ComponentId.FiglutConfigurationManager] != null)
            {
                Settings configurationManagerSettings = ComponentsSettings.Instance[ComponentId.FiglutConfigurationManager];
                TreeNode configurationManagerNode = new TreeNode("Figlut Configuration Manager", 1, 1);
                configurationManagerNode.Tag = configurationManagerSettings;
                configurationManagerNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(configurationManagerSettings, "Logging"), 5));
                figlutSuiteNode.Nodes.Add(configurationManagerNode);
            }
            if (ComponentsSettings.Instance[ComponentId.FiglutWebService] != null)
            {
                Settings webServiceSettings = ComponentsSettings.Instance[ComponentId.FiglutWebService];
                TreeNode webServiceNode = new TreeNode("Figlut Web Service", 2, 2);
                webServiceNode.Tag = webServiceSettings;
                webServiceNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(webServiceSettings, "Database"), 3));
                webServiceNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(webServiceSettings, "Service"), 4));
                webServiceNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(webServiceSettings, "Logging"), 5));
                figlutSuiteNode.Nodes.Add(webServiceNode);
            }
            if (ComponentsSettings.Instance[ComponentId.FiglutDesktopDataBox] != null)
            {
                Settings desktopDataBoxSettings = ComponentsSettings.Instance[ComponentId.FiglutDesktopDataBox];
                TreeNode desktopDataBoxNode = new TreeNode("Figlut Desktop DataBox", 6, 6);
                desktopDataBoxNode.Tag = desktopDataBoxSettings;
                desktopDataBoxNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(desktopDataBoxSettings, "Web Service"), 2));
                desktopDataBoxNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(desktopDataBoxSettings, "DataBox"), 6));
                desktopDataBoxNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(desktopDataBoxSettings, "Logging"), 5));
                figlutSuiteNode.Nodes.Add(desktopDataBoxNode);
            }
            if (ComponentsSettings.Instance[ComponentId.FiglutReModem] != null)
            {
                Settings reModemSettings = ComponentsSettings.Instance[ComponentId.FiglutReModem];
                TreeNode reModemNode = new TreeNode("Figlut ReModem", 8, 8);
                reModemNode.Tag = reModemSettings;
                reModemNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(reModemSettings, "ReModem"), 8));
                reModemNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(reModemSettings, "Logging"), 5));
                figlutSuiteNode.Nodes.Add(reModemNode);
            }
            if (ComponentsSettings.Instance[ComponentId.FiglutDesktopBarcode] != null)
            {
                Settings desktopBarcodeSettings = ComponentsSettings.Instance[ComponentId.FiglutDesktopBarcode];
                TreeNode desktopBarcodeNode = new TreeNode("Figut Desktop Barcode", 9, 9);
                desktopBarcodeNode.Tag = desktopBarcodeSettings;
                desktopBarcodeNode.Nodes.Add(BuildSettingsCategoryNode(new SettingsCategoryInfo(desktopBarcodeSettings, "Logging"), 9));
                figlutSuiteNode.Nodes.Add(desktopBarcodeNode);
            }
            trvComponentSettings.Nodes.Add(figlutSuiteNode);
        }

        private void InitializeFiglutWebServiceClientManager()
        {
            if (ComponentsSettings.Instance[ComponentId.FiglutWebService] != null)
            {
                _figlutWebServiceClientManager = new FiglutWebServiceClientManager();
                _figlutWebServiceClientManager.OnFiglutWebServiceClientsInfoChanged += _figlutWebServiceClientManager_OnFiglutWebServiceClientsInfoChanged;
                _figlutWebServiceClientManager.RefreshAllClientsInfoFiles();
                TreeNode allClientsNode = new TreeNode(DataShaper.ShapeCamelCaseString(ClientViewType.AllClients.ToString())) { Tag = _figlutWebServiceClientManager.AllClients };
                allClientsNode.Nodes.Add(new TreeNode(DataShaper.ShapeCamelCaseString(ClientViewType.FiglutDesktopDataBox.ToString())) { Tag = _figlutWebServiceClientManager.FiglutDesktopDataBoxClients });
                allClientsNode.Nodes.Add(new TreeNode(DataShaper.ShapeCamelCaseString(ClientViewType.FiglutMobileDataBox.ToString())) { Tag = _figlutWebServiceClientManager.FiglutMobileDataBoxClients });
                allClientsNode.Nodes.Add(new TreeNode(DataShaper.ShapeCamelCaseString(ClientViewType.NonFiglutClients.ToString())) { Tag = _figlutWebServiceClientManager.NonFiglutClients });
                trvClients.Nodes.Add(allClientsNode);

                trvClients.Nodes.Add(new TreeNode(DataShaper.ShapeCamelCaseString(ClientViewType.Trace.ToString())) { Tag = _figlutWebServiceClientManager.ClientTrace });
            }
        }

        private TreeNode BuildSettingsCategoryNode(SettingsCategoryInfo settingsCategoryInfo, int imageIndex)
        {
            TreeNode result = new TreeNode(settingsCategoryInfo.Category, imageIndex, imageIndex);
            result.Tag = settingsCategoryInfo.Settings.GetSettingsByCategory(settingsCategoryInfo);
            return result;
        }

        private void RefreshConfigurationGrid(TreeNode selectedNode)
        {
            try
            {
                if (selectedNode == null)
                {
                    grdSettingValues.DataSource = null;
                    grdSettingValues.Refresh();
                    lblCurrentCategoryName.Text = string.Empty;
                    return;
                }
                int selectedRowIndex = -1;
                if (grdSettingValues.SelectedRows.Count > 0)
                {
                    selectedRowIndex = grdSettingValues.SelectedRows[0].Index;
                }
                using (WaitProcess w = new WaitProcess(this))
                {
                    w.ChangeStatus("Refreshing settings ...");
                    _currentCategorySettings = null;
                    _currentCategorySettings = selectedNode.Tag as EntityCache<string, SettingItem>;
                    if (_currentCategorySettings == null)
                    {
                        throw new InvalidCastException(string.Format("Expected a {0} in the selected node and found a {1}.",
                            _currentCategorySettings.GetType().FullName,
                            selectedNode.Tag.GetType().FullName));
                    }
                    _currentSettings = _currentCategorySettings.Count > 0 ? _currentCategorySettings.Entities[0].SettingsCategoryInfo.Settings : null;
                    DataTable table = _currentCategorySettings.GetDataTable(null, false, true);
                    foreach (SettingItem s in _currentCategorySettings)
                    {
                        if (s.PasswordChar != '\0')
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                if (row[EntityReader<SettingItem>.GetPropertyName(p => p.SettingName, true)].ToString() == 
                                    s.SettingName)
                                {
                                    row[EntityReader<SettingItem>.GetPropertyName(p => p.SettingValue, true)] = DataShaper.MaskPasswordString(s.SettingValue.ToString(), s.PasswordChar);
                                }
                            }
                        }
                    }
                    grdSettingValues.DataSource = table;
                    _hiddenConfigurationProperties.ForEach(c => grdSettingValues.Columns[c].Visible = false);
                    for (int i = 0; i < grdSettingValues.Columns.Count; i++)
                    {
                        grdSettingValues.Columns[i].Width = 250;
                    }
                    grdSettingValues.Refresh();
                    lblCurrentCategoryName.Text = string.Format("{0}:", _currentCategorySettings.Name);
                    if (selectedRowIndex < grdSettingValues.Rows.Count && selectedRowIndex > -1)
                    {
                        grdSettingValues.Rows[selectedRowIndex].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                grdSettingValues.DataSource = null;
                grdSettingValues.Refresh();
                throw ex;
            }
        }

        private void RefreshClientsGrid(TreeNode selectedNode)
        {
            try
            {
                if (selectedNode == null)
                {
                    grdClients.DataSource = null;
                    grdClients.Refresh();
                    lblCurrentClientsType.Text = string.Empty;
                    return;
                }
                int selectedRowIndex = -1;
                if (grdClients.SelectedRows.Count > 0)
                {
                    selectedRowIndex = grdClients.SelectedRows[0].Index;
                }
                using (WaitProcess w = new WaitProcess(this))
                {
                    DataTable table = null;
                    if (selectedNode.Tag is EntityCache<Guid, FiglutClientInfo>)
                    {
                        EntityCache<Guid, FiglutClientInfo> clientTrace = (EntityCache<Guid, FiglutClientInfo>)selectedNode.Tag;
                        table = clientTrace.GetDataTable(null, false, true);
                    }
                    else if (selectedNode.Tag is ClientView)
                    {
                        w.ChangeStatus("Refreshing clients ...");
                        _currentClientTypeGroup = (ClientView)selectedNode.Tag;
                        table = _currentClientTypeGroup.Clients.GetDataTable(null, false, true);
                    }
                    grdClients.DataSource = table;
                    _hiddenClientsProperties.ForEach(c => grdClients.Columns[c].Visible = false);
                    for (int i = 0; i < grdClients.Columns.Count; i++)
                    {
                        grdClients.Columns[i].Width = 350;
                    }
                    grdClients.Refresh();
                    lblCurrentClientsType.Text = string.Format("{0}:", DataShaper.ShapeCamelCaseString(_currentClientTypeGroup.ClientType.ToString()));
                    if (selectedRowIndex < grdClients.Rows.Count && selectedRowIndex > -1)
                    {
                        grdClients.Rows[selectedRowIndex].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                grdClients.DataSource = null;
                grdClients.Refresh();
                throw ex;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (_figlutWebServiceClientManager != null)
            {
                _figlutWebServiceClientManager.OnFiglutWebServiceClientsInfoChanged -= _figlutWebServiceClientManager_OnFiglutWebServiceClientsInfoChanged;
                _figlutWebServiceClientManager.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion //Methods

        #region Event Handlers

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void picResizeWindow_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderLessFormResize(sender, e);
        }

        private void picMinimize_Click(object sender, EventArgs e)
        {
            base.BorderlessForm_Minimize(sender, e);
        }

        private void picMaximize_Click(object sender, EventArgs e)
        {
            base.BorderlessForm_Maximize(sender, e);
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_forceClose && UIHelper.AskQuestion("Are you sure you want to exit?") != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                AnimateHideForm();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GOC.Instance.ShowMessageBoxOnException = true;
            this.Refresh();
            FiglutConfigurationManagerSettings settings = GOC.Instance.GetSettings<FiglutConfigurationManagerSettings>(true, true);
            GOC.Instance.Logger = new Logger(
                settings.LogToFile,
                settings.LogToWindowsEventLog,
                settings.LogToConsole,
                settings.LoggingLevel,
                settings.LogFileName,
                settings.EventSourceName,
                settings.EventLogName);
            InitializeComponents();
            InitializeFiglutWebServiceClientManager();
        }

        private void trvComponentSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.IsSelected &&
                e.Node.Tag != null &&
                e.Node.Tag.GetType().Equals(typeof(EntityCache<string, SettingItem>)))
            {
                RefreshConfigurationGrid(e.Node);
                _selectedConfigurationNode = e.Node;
            }
        }

        private void tsUpdate_Click(object sender, EventArgs e)
        {
            string selectedSettingName = UIHelper.GetSelectedDataGridViewRowCellValue<string>(
                grdSettingValues,
                EntityReader<SettingItem>.GetPropertyName(p => p.SettingName, true));
            if (string.IsNullOrEmpty(selectedSettingName))
            {
                throw new UserThrownException("No setting selected to be updated.", LoggingLevel.None);
            }
            SettingItem selectedSetting = _currentCategorySettings[selectedSettingName];
            if (selectedSetting == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find setting with Setting Name {0}.",
                    selectedSettingName));
            }
            Form editSettingForm = null;
            if (selectedSetting.SettingType.Equals(typeof(string)))
            {
                editSettingForm = new EditTextSettingForm(selectedSetting);
            }
            if (selectedSetting.SettingType.IsEnum)
            {
                editSettingForm = new EditEnumSettingForm(selectedSetting);
            }
            if(selectedSetting.SettingType.Equals(typeof(Boolean)))
            {
                editSettingForm = new EditBoolSettingForm(selectedSetting);
            }
            if(selectedSetting.SettingType.Equals(typeof(Int64)) ||
                selectedSetting.SettingType.Equals(typeof(Int32)) ||
                selectedSetting.SettingType.Equals(typeof(Int16)))
            {
                editSettingForm = new EditLongSettingForm(selectedSetting);
            }
            using (editSettingForm)
            {
                if (editSettingForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }
            EntityReader.SetPropertyValue(selectedSetting.SettingName, selectedSetting.SettingsCategoryInfo.Settings, selectedSetting.SettingValue);
            RefreshConfigurationGrid(_selectedConfigurationNode);
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            using (WaitProcess w = new WaitProcess(this))
            {
                w.ChangeStatus("Saving settings ...");
                ComponentsSettings.Instance.SaveAllSettings();
            }
            UIHelper.DisplayInformation("Settings saved.");
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            using (AboutForm f = new AboutForm())
            {
                f.ShowDialog();
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuExportComponentSettings_Click(object sender, EventArgs e)
        {
            if (trvComponentSettings.SelectedNode == null ||
                trvComponentSettings.SelectedNode.Tag == null ||
                !(trvComponentSettings.SelectedNode.Tag is Settings))
            {
                throw new UserThrownException("No component's settings selected to be exported.");
            }
            Settings selectedSettings = (Settings)trvComponentSettings.SelectedNode.Tag;
            string originalFilePath = selectedSettings.FilePath;
            try
            {
                svdExportSettings.FileName = Path.GetFileName(selectedSettings.FilePath);
                if (svdExportSettings.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedSettings.FilePath = svdExportSettings.FileName;
                    using (WaitProcess w = new WaitProcess(this))
                    {
                        w.ChangeStatus("Exporting settings ...");
                        selectedSettings.SaveToFile();
                    }
                }
            }
            finally
            {
                selectedSettings.FilePath = originalFilePath;
            }
            selectedSettings.FilePath = originalFilePath;
        }

        private void mnuImportComponentSettings_Click(object sender, EventArgs e)
        {
            if (trvComponentSettings.SelectedNode == null ||
                trvComponentSettings.SelectedNode.Tag == null ||
                !(trvComponentSettings.SelectedNode.Tag is Settings))
            {
                throw new UserThrownException("No component's settings selected to be imported.");
            }
            Settings selectedSettings = (Settings)trvComponentSettings.SelectedNode.Tag;
            string originalFilePath = selectedSettings.FilePath;
            try
            {
                opdImportSettings.FileName = Path.GetFileName(selectedSettings.FilePath);
                if (opdImportSettings.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedSettings.FilePath = opdImportSettings.FileName;
                    selectedSettings.RefreshFromFile(false, false);
                }
            }
            finally
            {
                selectedSettings.FilePath = originalFilePath;
            }
            InitializeComponents();
            RefreshConfigurationGrid(null);
        }

        private void trvClientType_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.IsSelected &&
                e.Node.Tag != null &&
                e.Node.Tag.GetType().Equals(typeof(ClientView)))
            {
                RefreshClientsGrid(e.Node);
                _selectedClientTypeNode = e.Node;
            }
        }

        private void _figlutWebServiceClientManager_OnFiglutWebServiceClientsInfoChanged(EventArgs e)
        {
            if (_selectedClientTypeNode != null)
            {
                this.Invoke(new Action<TreeNode>(RefreshClientsGrid), _selectedClientTypeNode);
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if ((e.KeyCode == Keys.F) & e.Control & e.Shift)
            {
                base.BorderlessForm_Maximize(sender, e);
            }
            else if ((e.KeyCode == Keys.U) & e.Control & e.Shift)
            {
                tsUpdate_Click(sender, e);
            }
            else if ((e.KeyCode == Keys.R) & e.Control & e.Shift)
            {
                RefreshConfigurationGrid(_selectedConfigurationNode);
            }
            else if ((e.KeyCode == Keys.S) & e.Control & e.Shift)
            {
                tsSave_Click(sender, e);
            }
        }

        #endregion //Event Handlers
    }
}