using System.Drawing;
using Figlut.Desktop.DataBox.Controls;
namespace Figlut.Desktop.DataBox
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
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.picExit = new System.Windows.Forms.PictureBox();
            this.picMinimize = new System.Windows.Forms.PictureBox();
            this.picMaximize = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileDataBox = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileTimesheetExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileTimesheetImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tabDataBox = new Figlut.Server.Toolkit.Winforms.CustomTab();
            this.tabPageDataBox = new System.Windows.Forms.TabPage();
            this.pnlDataBox = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.dataBoxControl = new Figlut.Desktop.DataBox.Controls.DataBoxControl();
            this.picResizeWindow = new System.Windows.Forms.PictureBox();
            this.pnlFormContent.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMaximize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.mnuMain.SuspendLayout();
            this.tabDataBox.SuspendLayout();
            this.tabPageDataBox.SuspendLayout();
            this.pnlDataBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResizeWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.tabDataBox);
            this.pnlFormContent.Controls.Add(this.pnlTitle);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlFormContent.Size = new System.Drawing.Size(1694, 1076);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormRight.BackgroundImage")));
            this.pnlFormRight.Location = new System.Drawing.Point(1724, 40);
            this.pnlFormRight.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlFormRight.Size = new System.Drawing.Size(30, 1076);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormLeft.BackgroundImage")));
            this.pnlFormLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlFormLeft.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlFormLeft.Size = new System.Drawing.Size(30, 1076);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Margin = new System.Windows.Forms.Padding(24, 0, 24, 0);
            this.lblFormTitle.Size = new System.Drawing.Size(1754, 40);
            this.lblFormTitle.Text = "Figlut DataBox ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 1116);
            this.statusMain.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.statusMain.Size = new System.Drawing.Size(1754, 40);
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.Color.White;
            this.pnlTitle.Controls.Add(this.picExit);
            this.pnlTitle.Controls.Add(this.picMinimize);
            this.pnlTitle.Controls.Add(this.picMaximize);
            this.pnlTitle.Controls.Add(this.picLogo);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 47);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(1694, 235);
            this.pnlTitle.TabIndex = 147;
            // 
            // picExit
            // 
            this.picExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picExit.Image = ((System.Drawing.Image)(resources.GetObject("picExit.Image")));
            this.picExit.Location = new System.Drawing.Point(1618, 6);
            this.picExit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.picExit.Name = "picExit";
            this.picExit.Size = new System.Drawing.Size(64, 62);
            this.picExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picExit.TabIndex = 33;
            this.picExit.TabStop = false;
            this.picExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // picMinimize
            // 
            this.picMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picMinimize.ErrorImage = null;
            this.picMinimize.Image = ((System.Drawing.Image)(resources.GetObject("picMinimize.Image")));
            this.picMinimize.Location = new System.Drawing.Point(1466, 6);
            this.picMinimize.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.picMinimize.Name = "picMinimize";
            this.picMinimize.Size = new System.Drawing.Size(64, 62);
            this.picMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMinimize.TabIndex = 32;
            this.picMinimize.TabStop = false;
            this.picMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // picMaximize
            // 
            this.picMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picMaximize.Image = ((System.Drawing.Image)(resources.GetObject("picMaximize.Image")));
            this.picMaximize.Location = new System.Drawing.Point(1542, 6);
            this.picMaximize.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.picMaximize.Name = "picMaximize";
            this.picMaximize.Size = new System.Drawing.Size(64, 62);
            this.picMaximize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMaximize.TabIndex = 31;
            this.picMaximize.TabStop = false;
            this.picMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.White;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(1694, 235);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLogo.TabIndex = 23;
            this.picLogo.TabStop = false;
            this.picLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.picLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.picLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuHelp});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.mnuMain.Size = new System.Drawing.Size(1694, 47);
            this.mnuMain.TabIndex = 0;
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileDataBox,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(64, 39);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileDataBox
            // 
            this.mnuFileDataBox.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileTimesheetExport,
            this.mnuFileTimesheetImport});
            this.mnuFileDataBox.Name = "mnuFileDataBox";
            this.mnuFileDataBox.Size = new System.Drawing.Size(182, 40);
            this.mnuFileDataBox.Text = "&DataBox";
            this.mnuFileDataBox.Visible = false;
            // 
            // mnuFileTimesheetExport
            // 
            this.mnuFileTimesheetExport.Name = "mnuFileTimesheetExport";
            this.mnuFileTimesheetExport.Size = new System.Drawing.Size(166, 40);
            this.mnuFileTimesheetExport.Text = "&Export";
            // 
            // mnuFileTimesheetImport
            // 
            this.mnuFileTimesheetImport.Name = "mnuFileTimesheetImport";
            this.mnuFileTimesheetImport.Size = new System.Drawing.Size(166, 40);
            this.mnuFileTimesheetImport.Text = "&Import";
            // 
            // mnuExit
            // 
            this.mnuExit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(182, 40);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(79, 39);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Image = ((System.Drawing.Image)(resources.GetObject("mnuAbout.Image")));
            this.mnuAbout.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(158, 40);
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // tabDataBox
            // 
            this.tabDataBox.Controls.Add(this.tabPageDataBox);
            this.tabDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDataBox.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabDataBox.HotTrack = true;
            this.tabDataBox.Location = new System.Drawing.Point(0, 282);
            this.tabDataBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabDataBox.Name = "tabDataBox";
            this.tabDataBox.SelectedBackEndColor = System.Drawing.Color.Gainsboro;
            this.tabDataBox.SelectedBackStartColor = System.Drawing.Color.WhiteSmoke;
            this.tabDataBox.SelectedForeBrushColor = System.Drawing.Color.DimGray;
            this.tabDataBox.SelectedIndex = 0;
            this.tabDataBox.Size = new System.Drawing.Size(1694, 794);
            this.tabDataBox.TabIndex = 379;
            this.tabDataBox.UnselectedBackEndColor = System.Drawing.Color.WhiteSmoke;
            this.tabDataBox.UnselectedBackStartColor = System.Drawing.Color.Gainsboro;
            this.tabDataBox.UnselectedForeBrushColor = System.Drawing.Color.DimGray;
            // 
            // tabPageDataBox
            // 
            this.tabPageDataBox.BackColor = System.Drawing.Color.LightGray;
            this.tabPageDataBox.Controls.Add(this.pnlDataBox);
            this.tabPageDataBox.Location = new System.Drawing.Point(4, 34);
            this.tabPageDataBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageDataBox.Name = "tabPageDataBox";
            this.tabPageDataBox.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageDataBox.Size = new System.Drawing.Size(1686, 756);
            this.tabPageDataBox.TabIndex = 0;
            this.tabPageDataBox.Text = "DataBox";
            // 
            // pnlDataBox
            // 
            this.pnlDataBox.AutoScroll = true;
            this.pnlDataBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlDataBox.BackgroundImage")));
            this.pnlDataBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlDataBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDataBox.Controls.Add(this.dataBoxControl);
            this.pnlDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataBox.GradientEndColor = System.Drawing.Color.LightGray;
            this.pnlDataBox.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlDataBox.GradientStartColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDataBox.Location = new System.Drawing.Point(6, 6);
            this.pnlDataBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlDataBox.Name = "pnlDataBox";
            this.pnlDataBox.Size = new System.Drawing.Size(1674, 744);
            this.pnlDataBox.TabIndex = 0;
            // 
            // dataBoxControl
            // 
            this.dataBoxControl.BackColor = System.Drawing.Color.White;
            this.dataBoxControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataBoxControl.Location = new System.Drawing.Point(0, 0);
            this.dataBoxControl.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.dataBoxControl.Name = "dataBoxControl";
            this.dataBoxControl.Size = new System.Drawing.Size(1670, 740);
            this.dataBoxControl.TabIndex = 0;
            // 
            // picResizeWindow
            // 
            this.picResizeWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picResizeWindow.BackColor = System.Drawing.Color.LightGray;
            this.picResizeWindow.ErrorImage = null;
            this.picResizeWindow.Image = ((System.Drawing.Image)(resources.GetObject("picResizeWindow.Image")));
            this.picResizeWindow.Location = new System.Drawing.Point(1722, 1127);
            this.picResizeWindow.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.picResizeWindow.Name = "picResizeWindow";
            this.picResizeWindow.Size = new System.Drawing.Size(32, 29);
            this.picResizeWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picResizeWindow.TabIndex = 36;
            this.picResizeWindow.TabStop = false;
            this.picResizeWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picResizeWindow_MouseMove);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1754, 1156);
            this.Controls.Add(this.picResizeWindow);
            this.FormTitle = "Figlut DataBox";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(24, 23, 24, 23);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Figlut DataBox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.Controls.SetChildIndex(this.statusMain, 0);
            this.Controls.SetChildIndex(this.picResizeWindow, 0);
            this.Controls.SetChildIndex(this.lblFormTitle, 0);
            this.Controls.SetChildIndex(this.pnlFormLeft, 0);
            this.Controls.SetChildIndex(this.pnlFormRight, 0);
            this.Controls.SetChildIndex(this.pnlFormContent, 0);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMaximize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.tabDataBox.ResumeLayout(false);
            this.tabPageDataBox.ResumeLayout(false);
            this.pnlDataBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picResizeWindow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.PictureBox picExit;
        private System.Windows.Forms.PictureBox picMinimize;
        private System.Windows.Forms.PictureBox picMaximize;
        private Figlut.Server.Toolkit.Winforms.CustomTab tabDataBox;
        private System.Windows.Forms.TabPage tabPageDataBox;
        private Figlut.Server.Toolkit.Winforms.GradientPanel pnlDataBox;
        private System.Windows.Forms.PictureBox picResizeWindow;
        private System.Windows.Forms.ToolStripMenuItem mnuFileDataBox;
        private System.Windows.Forms.ToolStripMenuItem mnuFileTimesheetExport;
        private System.Windows.Forms.ToolStripMenuItem mnuFileTimesheetImport;
        internal Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private DataBoxControl dataBoxControl;




    }
}

