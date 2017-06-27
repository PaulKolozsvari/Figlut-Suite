namespace Figlut.Mobile.Toolkit.Demo.WM
{
    partial class DemoImageMapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mnuMain;

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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuBack = new System.Windows.Forms.MenuItem();
            this.mnuMenu = new System.Windows.Forms.MenuItem();
            this.mnuSelectImage = new System.Windows.Forms.MenuItem();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.mnuSetHotspotColor = new System.Windows.Forms.MenuItem();
            this.mnuEnableHotspotHighlightOnMouseMove = new System.Windows.Forms.MenuItem();
            this.mnuEnableHotspotHighlight = new System.Windows.Forms.MenuItem();
            this.mnuShowMousePosition = new System.Windows.Forms.MenuItem();
            this.mnuSingleHotspotHighlight = new System.Windows.Forms.MenuItem();
            this.mnuShowResetCount = new System.Windows.Forms.MenuItem();
            this.mnuActions = new System.Windows.Forms.MenuItem();
            this.mnuResetHotspotHighlights = new System.Windows.Forms.MenuItem();
            this.mnuExportHotspots = new System.Windows.Forms.MenuItem();
            this.mnuImportHotspots = new System.Windows.Forms.MenuItem();
            this.mnuClearHotspots = new System.Windows.Forms.MenuItem();
            this.mnuCreateHotspot = new System.Windows.Forms.MenuItem();
            this.mnuBeginHotspot = new System.Windows.Forms.MenuItem();
            this.mnuEndHotspot = new System.Windows.Forms.MenuItem();
            this.mnuCancelHotspot = new System.Windows.Forms.MenuItem();
            this.statusMain = new System.Windows.Forms.StatusBar();
            this.opdImport = new System.Windows.Forms.OpenFileDialog();
            this.svdExport = new System.Windows.Forms.SaveFileDialog();
            this.opdSelectImage = new System.Windows.Forms.OpenFileDialog();
            this.imageMap = new Figlut.Mobile.Toolkit.Tools.IM.ImageMap();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuBack);
            this.mnuMain.MenuItems.Add(this.mnuMenu);
            // 
            // mnuBack
            // 
            this.mnuBack.Text = "Back";
            this.mnuBack.Click += new System.EventHandler(this.mnuBack_Click);
            // 
            // mnuMenu
            // 
            this.mnuMenu.MenuItems.Add(this.mnuSelectImage);
            this.mnuMenu.MenuItems.Add(this.mnuOptions);
            this.mnuMenu.MenuItems.Add(this.mnuActions);
            this.mnuMenu.MenuItems.Add(this.mnuCreateHotspot);
            this.mnuMenu.Text = "Menu";
            // 
            // mnuSelectImage
            // 
            this.mnuSelectImage.Text = "Select Image";
            this.mnuSelectImage.Click += new System.EventHandler(this.mnuSelectImage_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.MenuItems.Add(this.mnuSetHotspotColor);
            this.mnuOptions.MenuItems.Add(this.mnuEnableHotspotHighlightOnMouseMove);
            this.mnuOptions.MenuItems.Add(this.mnuEnableHotspotHighlight);
            this.mnuOptions.MenuItems.Add(this.mnuShowMousePosition);
            this.mnuOptions.MenuItems.Add(this.mnuSingleHotspotHighlight);
            this.mnuOptions.MenuItems.Add(this.mnuShowResetCount);
            this.mnuOptions.Text = "Options";
            // 
            // mnuSetHotspotColor
            // 
            this.mnuSetHotspotColor.Text = "Set Hotspot Color";
            this.mnuSetHotspotColor.Click += new System.EventHandler(this.mnuSetHotspotColor_Click);
            // 
            // mnuEnableHotspotHighlightOnMouseMove
            // 
            this.mnuEnableHotspotHighlightOnMouseMove.Text = "Enable Highlight On Move";
            this.mnuEnableHotspotHighlightOnMouseMove.Click += new System.EventHandler(this.mnuEnableHotspotHighlightOnMouseMove_Click);
            // 
            // mnuEnableHotspotHighlight
            // 
            this.mnuEnableHotspotHighlight.Text = "Enable Highlight";
            this.mnuEnableHotspotHighlight.Click += new System.EventHandler(this.mnuEnableHotspotHighlight_Click);
            // 
            // mnuShowMousePosition
            // 
            this.mnuShowMousePosition.Text = "Show Mouse Position";
            this.mnuShowMousePosition.Click += new System.EventHandler(this.mnuShowMousePosition_Click);
            // 
            // mnuSingleHotspotHighlight
            // 
            this.mnuSingleHotspotHighlight.Text = "Single Hotspot Highlight";
            this.mnuSingleHotspotHighlight.Click += new System.EventHandler(this.mnuSingleHotspotHighlight_Click);
            // 
            // mnuShowResetCount
            // 
            this.mnuShowResetCount.Text = "Show Reset Count";
            this.mnuShowResetCount.Click += new System.EventHandler(this.mnuShowResetCount_Click);
            // 
            // mnuActions
            // 
            this.mnuActions.MenuItems.Add(this.mnuResetHotspotHighlights);
            this.mnuActions.MenuItems.Add(this.mnuExportHotspots);
            this.mnuActions.MenuItems.Add(this.mnuImportHotspots);
            this.mnuActions.MenuItems.Add(this.mnuClearHotspots);
            this.mnuActions.Text = "Actions";
            // 
            // mnuResetHotspotHighlights
            // 
            this.mnuResetHotspotHighlights.Text = "Reset Highlights";
            this.mnuResetHotspotHighlights.Click += new System.EventHandler(this.mnuResetHotspotHighlights_Click);
            // 
            // mnuExportHotspots
            // 
            this.mnuExportHotspots.Text = "Export Hotspots";
            this.mnuExportHotspots.Click += new System.EventHandler(this.mnuExportHotspots_Click);
            // 
            // mnuImportHotspots
            // 
            this.mnuImportHotspots.Text = "Import Hotspots";
            this.mnuImportHotspots.Click += new System.EventHandler(this.mnuImportHotspots_Click);
            // 
            // mnuClearHotspots
            // 
            this.mnuClearHotspots.Text = "Clear Hotspots";
            this.mnuClearHotspots.Click += new System.EventHandler(this.mnuClearHotspots_Click);
            // 
            // mnuCreateHotspot
            // 
            this.mnuCreateHotspot.MenuItems.Add(this.mnuBeginHotspot);
            this.mnuCreateHotspot.MenuItems.Add(this.mnuEndHotspot);
            this.mnuCreateHotspot.MenuItems.Add(this.mnuCancelHotspot);
            this.mnuCreateHotspot.Text = "Create Hotspot";
            // 
            // mnuBeginHotspot
            // 
            this.mnuBeginHotspot.Text = "Begin";
            this.mnuBeginHotspot.Click += new System.EventHandler(this.mnuBeginHotspot_Click);
            // 
            // mnuEndHotspot
            // 
            this.mnuEndHotspot.Enabled = false;
            this.mnuEndHotspot.Text = "End";
            this.mnuEndHotspot.Click += new System.EventHandler(this.mnuEndHotspot_Click);
            // 
            // mnuCancelHotspot
            // 
            this.mnuCancelHotspot.Enabled = false;
            this.mnuCancelHotspot.Text = "Cancel";
            this.mnuCancelHotspot.Click += new System.EventHandler(this.mnuCancelHotspot_Click);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 246);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(240, 22);
            this.statusMain.Text = "Move mouse over hotspots.";
            // 
            // opdImport
            // 
            this.opdImport.FileName = "ImageMapHotspots.xml";
            this.opdImport.Filter = "XML Files|*.xml";
            // 
            // svdExport
            // 
            this.svdExport.FileName = "ImageMapHotspots.xml";
            this.svdExport.Filter = "XML Files|*.xml";
            // 
            // imageMap
            // 
            this.imageMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageMap.EnableHotspotHighlight = true;
            this.imageMap.EnableHotspotHighlightOnMouseMove = true;
            this.imageMap.HotspotColor = System.Drawing.Color.Red;
            this.imageMap.Location = new System.Drawing.Point(0, 0);
            this.imageMap.Name = "imageMap";
            this.imageMap.ShowImageResetCountInStatus = true;
            this.imageMap.ShowMousePositionInStatus = true;
            this.imageMap.SingleHotspotHighlight = true;
            this.imageMap.Size = new System.Drawing.Size(240, 246);
            this.imageMap.StatusBar = this.statusMain;
            this.imageMap.TabIndex = 1;
            this.imageMap.OnHotspotClick += new Figlut.Mobile.Toolkit.Tools.IM.ImageMap.HotspotClickedHandler(this.imageMap_OnHotspotClick);
            // 
            // DemoImageMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.imageMap);
            this.Controls.Add(this.statusMain);
            this.Menu = this.mnuMain;
            this.Name = "DemoImageMapForm";
            this.Text = "Image Map";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuBack;
        private System.Windows.Forms.StatusBar statusMain;
        private Figlut.Mobile.Toolkit.Tools.IM.ImageMap imageMap;
        private System.Windows.Forms.MenuItem mnuMenu;
        private System.Windows.Forms.MenuItem mnuOptions;
        private System.Windows.Forms.MenuItem mnuSetHotspotColor;
        private System.Windows.Forms.MenuItem mnuEnableHotspotHighlightOnMouseMove;
        private System.Windows.Forms.MenuItem mnuEnableHotspotHighlight;
        private System.Windows.Forms.MenuItem mnuShowMousePosition;
        private System.Windows.Forms.MenuItem mnuSingleHotspotHighlight;
        private System.Windows.Forms.MenuItem mnuShowResetCount;
        private System.Windows.Forms.MenuItem mnuActions;
        private System.Windows.Forms.MenuItem mnuResetHotspotHighlights;
        private System.Windows.Forms.MenuItem mnuExportHotspots;
        private System.Windows.Forms.OpenFileDialog opdImport;
        private System.Windows.Forms.SaveFileDialog svdExport;
        private System.Windows.Forms.MenuItem mnuImportHotspots;
        private System.Windows.Forms.MenuItem mnuCreateHotspot;
        private System.Windows.Forms.MenuItem mnuBeginHotspot;
        private System.Windows.Forms.MenuItem mnuEndHotspot;
        private System.Windows.Forms.MenuItem mnuClearHotspots;
        private System.Windows.Forms.MenuItem mnuCancelHotspot;
        private System.Windows.Forms.MenuItem mnuSelectImage;
        private System.Windows.Forms.OpenFileDialog opdSelectImage;
    }
}

