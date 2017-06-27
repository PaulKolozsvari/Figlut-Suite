namespace Figlut.Desktop.Barcode
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.barcodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBarcodeGenerate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBarcodeDecode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBarcodeCopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlBarcodeProperties = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.tabData = new Figlut.Server.Toolkit.Winforms.CustomTab();
            this.tabPageDataInput = new System.Windows.Forms.TabPage();
            this.pnlDataInput = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.lblHeight = new System.Windows.Forms.Label();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.lblWidth = new System.Windows.Forms.Label();
            this.cboImageFormat = new System.Windows.Forms.ComboBox();
            this.lblImageFormat = new System.Windows.Forms.Label();
            this.cboBarcodeFormat = new System.Windows.Forms.ComboBox();
            this.lblBarcodeFormat = new System.Windows.Forms.Label();
            this.txtBarcodeContent = new System.Windows.Forms.TextBox();
            this.lblBarcodeContent = new System.Windows.Forms.Label();
            this.pnlResetInputControls = new System.Windows.Forms.Panel();
            this.lnkResetInputControls = new System.Windows.Forms.LinkLabel();
            this.picBarcode = new System.Windows.Forms.PictureBox();
            this.picResizeWindow = new System.Windows.Forms.PictureBox();
            this.svdExport = new System.Windows.Forms.SaveFileDialog();
            this.opdImport = new System.Windows.Forms.OpenFileDialog();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.pnlBarcodeProperties.SuspendLayout();
            this.tabData.SuspendLayout();
            this.tabPageDataInput.SuspendLayout();
            this.pnlDataInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.pnlResetInputControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBarcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picResizeWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.picBarcode);
            this.pnlFormContent.Controls.Add(this.pnlBarcodeProperties);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Size = new System.Drawing.Size(470, 308);
            this.pnlFormContent.TabIndex = 0;
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(485, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 308);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 308);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(500, 21);
            this.lblFormTitle.Text = "Figlut Barcode ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 329);
            this.statusMain.Size = new System.Drawing.Size(500, 21);
            this.statusMain.Text = "Generate or decode barcodes.";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.barcodeToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(470, 24);
            this.mnuMain.TabIndex = 1;
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExport,
            this.mnuImport,
            this.toolStripSeparator1,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuExport
            // 
            this.mnuExport.Name = "mnuExport";
            this.mnuExport.Size = new System.Drawing.Size(211, 22);
            this.mnuExport.Text = "&Export (CTRL + SHIFT + E)";
            this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click);
            // 
            // mnuImport
            // 
            this.mnuImport.Name = "mnuImport";
            this.mnuImport.Size = new System.Drawing.Size(211, 22);
            this.mnuImport.Text = "&Import (CTRL + SHIFT + I)";
            this.mnuImport.Visible = false;
            this.mnuImport.Click += new System.EventHandler(this.mnuImport_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(208, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(211, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // barcodeToolStripMenuItem
            // 
            this.barcodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBarcodeGenerate,
            this.mnuBarcodeDecode,
            this.mnuBarcodeCopyToClipboard});
            this.barcodeToolStripMenuItem.Name = "barcodeToolStripMenuItem";
            this.barcodeToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.barcodeToolStripMenuItem.Text = "&Barcode";
            // 
            // mnuBarcodeGenerate
            // 
            this.mnuBarcodeGenerate.Name = "mnuBarcodeGenerate";
            this.mnuBarcodeGenerate.Size = new System.Drawing.Size(290, 22);
            this.mnuBarcodeGenerate.Text = "&Generate (CTRL + SHIFT + Enter)";
            this.mnuBarcodeGenerate.Click += new System.EventHandler(this.mnuBarcodeGenerate_Click);
            // 
            // mnuBarcodeDecode
            // 
            this.mnuBarcodeDecode.Name = "mnuBarcodeDecode";
            this.mnuBarcodeDecode.Size = new System.Drawing.Size(290, 22);
            this.mnuBarcodeDecode.Text = "&Decode (CTRL + SHIFT + D)";
            this.mnuBarcodeDecode.Visible = false;
            this.mnuBarcodeDecode.Click += new System.EventHandler(this.mnuBarcodeDecode_Click);
            // 
            // mnuBarcodeCopyToClipboard
            // 
            this.mnuBarcodeCopyToClipboard.Name = "mnuBarcodeCopyToClipboard";
            this.mnuBarcodeCopyToClipboard.Size = new System.Drawing.Size(290, 22);
            this.mnuBarcodeCopyToClipboard.Text = "&Copy Barcode Image (CTRL + SHIFT + C)";
            this.mnuBarcodeCopyToClipboard.Click += new System.EventHandler(this.mnuBarcodeCopyToClipboard_Click);
            // 
            // pnlBarcodeProperties
            // 
            this.pnlBarcodeProperties.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlBarcodeProperties.BackgroundImage")));
            this.pnlBarcodeProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlBarcodeProperties.Controls.Add(this.tabData);
            this.pnlBarcodeProperties.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBarcodeProperties.GradientEndColor = System.Drawing.Color.White;
            this.pnlBarcodeProperties.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlBarcodeProperties.GradientStartColor = System.Drawing.Color.White;
            this.pnlBarcodeProperties.Location = new System.Drawing.Point(0, 24);
            this.pnlBarcodeProperties.Name = "pnlBarcodeProperties";
            this.pnlBarcodeProperties.Size = new System.Drawing.Size(200, 284);
            this.pnlBarcodeProperties.TabIndex = 2;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.tabPageDataInput);
            this.tabData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabData.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabData.HotTrack = true;
            this.tabData.Location = new System.Drawing.Point(0, 0);
            this.tabData.Name = "tabData";
            this.tabData.SelectedBackEndColor = System.Drawing.Color.Gainsboro;
            this.tabData.SelectedBackStartColor = System.Drawing.Color.WhiteSmoke;
            this.tabData.SelectedForeBrushColor = System.Drawing.Color.DimGray;
            this.tabData.SelectedIndex = 0;
            this.tabData.Size = new System.Drawing.Size(200, 284);
            this.tabData.TabIndex = 379;
            this.tabData.UnselectedBackEndColor = System.Drawing.Color.WhiteSmoke;
            this.tabData.UnselectedBackStartColor = System.Drawing.Color.Gainsboro;
            this.tabData.UnselectedForeBrushColor = System.Drawing.Color.DimGray;
            // 
            // tabPageDataInput
            // 
            this.tabPageDataInput.BackColor = System.Drawing.Color.LightGray;
            this.tabPageDataInput.Controls.Add(this.pnlDataInput);
            this.tabPageDataInput.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataInput.Name = "tabPageDataInput";
            this.tabPageDataInput.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataInput.Size = new System.Drawing.Size(192, 258);
            this.tabPageDataInput.TabIndex = 2;
            this.tabPageDataInput.Text = "Input";
            this.tabPageDataInput.UseVisualStyleBackColor = true;
            // 
            // pnlDataInput
            // 
            this.pnlDataInput.AutoScroll = true;
            this.pnlDataInput.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlDataInput.BackgroundImage")));
            this.pnlDataInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlDataInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDataInput.Controls.Add(this.nudHeight);
            this.pnlDataInput.Controls.Add(this.lblHeight);
            this.pnlDataInput.Controls.Add(this.nudWidth);
            this.pnlDataInput.Controls.Add(this.lblWidth);
            this.pnlDataInput.Controls.Add(this.cboImageFormat);
            this.pnlDataInput.Controls.Add(this.lblImageFormat);
            this.pnlDataInput.Controls.Add(this.cboBarcodeFormat);
            this.pnlDataInput.Controls.Add(this.lblBarcodeFormat);
            this.pnlDataInput.Controls.Add(this.txtBarcodeContent);
            this.pnlDataInput.Controls.Add(this.lblBarcodeContent);
            this.pnlDataInput.Controls.Add(this.pnlResetInputControls);
            this.pnlDataInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataInput.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.pnlDataInput.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlDataInput.GradientStartColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDataInput.Location = new System.Drawing.Point(3, 3);
            this.pnlDataInput.Name = "pnlDataInput";
            this.pnlDataInput.Size = new System.Drawing.Size(186, 252);
            this.pnlDataInput.TabIndex = 1;
            // 
            // nudHeight
            // 
            this.nudHeight.Dock = System.Windows.Forms.DockStyle.Top;
            this.nudHeight.Location = new System.Drawing.Point(0, 164);
            this.nudHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(182, 20);
            this.nudHeight.TabIndex = 36;
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.BackColor = System.Drawing.Color.Transparent;
            this.lblHeight.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeight.Location = new System.Drawing.Point(0, 151);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 41;
            this.lblHeight.Text = "Height:";
            // 
            // nudWidth
            // 
            this.nudWidth.Dock = System.Windows.Forms.DockStyle.Top;
            this.nudWidth.Location = new System.Drawing.Point(0, 131);
            this.nudWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(182, 20);
            this.nudWidth.TabIndex = 35;
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.BackColor = System.Drawing.Color.Transparent;
            this.lblWidth.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWidth.Location = new System.Drawing.Point(0, 118);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 40;
            this.lblWidth.Text = "Width:";
            // 
            // cboImageFormat
            // 
            this.cboImageFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImageFormat.FormattingEnabled = true;
            this.cboImageFormat.Location = new System.Drawing.Point(0, 97);
            this.cboImageFormat.Name = "cboImageFormat";
            this.cboImageFormat.Size = new System.Drawing.Size(182, 21);
            this.cboImageFormat.TabIndex = 34;
            // 
            // lblImageFormat
            // 
            this.lblImageFormat.AutoSize = true;
            this.lblImageFormat.BackColor = System.Drawing.Color.Transparent;
            this.lblImageFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblImageFormat.Location = new System.Drawing.Point(0, 84);
            this.lblImageFormat.Name = "lblImageFormat";
            this.lblImageFormat.Size = new System.Drawing.Size(74, 13);
            this.lblImageFormat.TabIndex = 39;
            this.lblImageFormat.Text = "Image Format:";
            // 
            // cboBarcodeFormat
            // 
            this.cboBarcodeFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboBarcodeFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBarcodeFormat.FormattingEnabled = true;
            this.cboBarcodeFormat.Location = new System.Drawing.Point(0, 63);
            this.cboBarcodeFormat.Name = "cboBarcodeFormat";
            this.cboBarcodeFormat.Size = new System.Drawing.Size(182, 21);
            this.cboBarcodeFormat.TabIndex = 33;
            // 
            // lblBarcodeFormat
            // 
            this.lblBarcodeFormat.AutoSize = true;
            this.lblBarcodeFormat.BackColor = System.Drawing.Color.Transparent;
            this.lblBarcodeFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBarcodeFormat.Location = new System.Drawing.Point(0, 50);
            this.lblBarcodeFormat.Name = "lblBarcodeFormat";
            this.lblBarcodeFormat.Size = new System.Drawing.Size(85, 13);
            this.lblBarcodeFormat.TabIndex = 38;
            this.lblBarcodeFormat.Text = "Barcode Format:";
            // 
            // txtBarcodeContent
            // 
            this.txtBarcodeContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBarcodeContent.Location = new System.Drawing.Point(0, 30);
            this.txtBarcodeContent.Name = "txtBarcodeContent";
            this.txtBarcodeContent.Size = new System.Drawing.Size(182, 20);
            this.txtBarcodeContent.TabIndex = 32;
            // 
            // lblBarcodeContent
            // 
            this.lblBarcodeContent.AutoSize = true;
            this.lblBarcodeContent.BackColor = System.Drawing.Color.Transparent;
            this.lblBarcodeContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBarcodeContent.Location = new System.Drawing.Point(0, 17);
            this.lblBarcodeContent.Name = "lblBarcodeContent";
            this.lblBarcodeContent.Size = new System.Drawing.Size(90, 13);
            this.lblBarcodeContent.TabIndex = 37;
            this.lblBarcodeContent.Text = "Barcode Content:";
            // 
            // pnlResetInputControls
            // 
            this.pnlResetInputControls.BackColor = System.Drawing.Color.Transparent;
            this.pnlResetInputControls.Controls.Add(this.lnkResetInputControls);
            this.pnlResetInputControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlResetInputControls.Location = new System.Drawing.Point(0, 0);
            this.pnlResetInputControls.Name = "pnlResetInputControls";
            this.pnlResetInputControls.Size = new System.Drawing.Size(182, 17);
            this.pnlResetInputControls.TabIndex = 31;
            // 
            // lnkResetInputControls
            // 
            this.lnkResetInputControls.AutoSize = true;
            this.lnkResetInputControls.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkResetInputControls.Location = new System.Drawing.Point(147, 0);
            this.lnkResetInputControls.Name = "lnkResetInputControls";
            this.lnkResetInputControls.Size = new System.Drawing.Size(35, 13);
            this.lnkResetInputControls.TabIndex = 0;
            this.lnkResetInputControls.TabStop = true;
            this.lnkResetInputControls.Text = "Reset";
            this.lnkResetInputControls.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkResetInputControls_LinkClicked);
            // 
            // picBarcode
            // 
            this.picBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBarcode.Location = new System.Drawing.Point(200, 24);
            this.picBarcode.Name = "picBarcode";
            this.picBarcode.Size = new System.Drawing.Size(270, 284);
            this.picBarcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBarcode.TabIndex = 3;
            this.picBarcode.TabStop = false;
            // 
            // picResizeWindow
            // 
            this.picResizeWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picResizeWindow.BackColor = System.Drawing.Color.LightGray;
            this.picResizeWindow.ErrorImage = null;
            this.picResizeWindow.Image = ((System.Drawing.Image)(resources.GetObject("picResizeWindow.Image")));
            this.picResizeWindow.Location = new System.Drawing.Point(484, 335);
            this.picResizeWindow.Name = "picResizeWindow";
            this.picResizeWindow.Size = new System.Drawing.Size(16, 15);
            this.picResizeWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picResizeWindow.TabIndex = 37;
            this.picResizeWindow.TabStop = false;
            this.picResizeWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picResizeWindow_MouseMove);
            // 
            // svdExport
            // 
            this.svdExport.Title = "Export Barcode";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.picResizeWindow);
            this.FormTitle = "Figlut Barcode";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Status = "Generate or decode barcodes.";
            this.Text = "Figlut Barcode";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.Controls.SetChildIndex(this.statusMain, 0);
            this.Controls.SetChildIndex(this.lblFormTitle, 0);
            this.Controls.SetChildIndex(this.pnlFormLeft, 0);
            this.Controls.SetChildIndex(this.pnlFormRight, 0);
            this.Controls.SetChildIndex(this.pnlFormContent, 0);
            this.Controls.SetChildIndex(this.picResizeWindow, 0);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.pnlBarcodeProperties.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.tabPageDataInput.ResumeLayout(false);
            this.pnlDataInput.ResumeLayout(false);
            this.pnlDataInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.pnlResetInputControls.ResumeLayout(false);
            this.pnlResetInputControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBarcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picResizeWindow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem barcodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuBarcodeGenerate;
        private System.Windows.Forms.ToolStripMenuItem mnuBarcodeDecode;
        private System.Windows.Forms.ToolStripMenuItem mnuExport;
        private System.Windows.Forms.ToolStripMenuItem mnuImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private Server.Toolkit.Winforms.GradientPanel pnlBarcodeProperties;
        private System.Windows.Forms.PictureBox picBarcode;
        private System.Windows.Forms.PictureBox picResizeWindow;
        private Server.Toolkit.Winforms.CustomTab tabData;
        private System.Windows.Forms.TabPage tabPageDataInput;
        private Server.Toolkit.Winforms.GradientPanel pnlDataInput;
        private System.Windows.Forms.Panel pnlResetInputControls;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.ComboBox cboImageFormat;
        private System.Windows.Forms.Label lblImageFormat;
        private System.Windows.Forms.ComboBox cboBarcodeFormat;
        private System.Windows.Forms.Label lblBarcodeFormat;
        private System.Windows.Forms.TextBox txtBarcodeContent;
        private System.Windows.Forms.Label lblBarcodeContent;
        private System.Windows.Forms.LinkLabel lnkResetInputControls;
        private System.Windows.Forms.ToolStripMenuItem mnuBarcodeCopyToClipboard;
        private System.Windows.Forms.SaveFileDialog svdExport;
        private System.Windows.Forms.OpenFileDialog opdImport;

    }
}