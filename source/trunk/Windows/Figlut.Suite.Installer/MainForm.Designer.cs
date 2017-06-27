namespace Figlut.Suite.Installer
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
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.progressMain = new Figlut.Server.Toolkit.Winforms.CustomProgressBar();
            this.lblApplicationTitle = new System.Windows.Forms.Label();
            this.gpbInstallComponents = new System.Windows.Forms.GroupBox();
            this.chkFiglutMobileToolkitWM = new System.Windows.Forms.CheckBox();
            this.chkFiglutServerToolkit = new System.Windows.Forms.CheckBox();
            this.chkFiglutDesktopDataBox = new System.Windows.Forms.CheckBox();
            this.chkFiglutWebService = new System.Windows.Forms.CheckBox();
            this.chkFiglutConfigurationManager = new System.Windows.Forms.CheckBox();
            this.btnInstall = new Figlut.Server.Toolkit.Winforms.GradientButton();
            this.btnCancel = new Figlut.Server.Toolkit.Winforms.GradientButton();
            this.pnlFormContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.gpbInstallComponents.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.btnCancel);
            this.pnlFormContent.Controls.Add(this.btnInstall);
            this.pnlFormContent.Controls.Add(this.gpbInstallComponents);
            this.pnlFormContent.Controls.Add(this.progressMain);
            this.pnlFormContent.Controls.Add(this.lblApplicationTitle);
            this.pnlFormContent.Controls.Add(this.picLogo);
            this.pnlFormContent.Size = new System.Drawing.Size(528, 235);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormRight.BackgroundImage")));
            this.pnlFormRight.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.pnlFormRight.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlFormRight.Location = new System.Drawing.Point(543, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 235);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormLeft.BackgroundImage")));
            this.pnlFormLeft.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.pnlFormLeft.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 235);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.GradientEndColor = System.Drawing.Color.SteelBlue;
            this.lblFormTitle.Size = new System.Drawing.Size(558, 21);
            this.lblFormTitle.Text = "Figlut Suite Installer ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.statusMain.GradientStartColor = System.Drawing.Color.Gainsboro;
            this.statusMain.Location = new System.Drawing.Point(0, 256);
            this.statusMain.Size = new System.Drawing.Size(558, 21);
            this.statusMain.Text = "Select components to install.";
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.White;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(178, 235);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLogo.TabIndex = 11;
            this.picLogo.TabStop = false;
            // 
            // progressMain
            // 
            this.progressMain.BackColor = System.Drawing.Color.DarkGray;
            this.progressMain.ForeColor = System.Drawing.Color.Silver;
            this.progressMain.Location = new System.Drawing.Point(187, 36);
            this.progressMain.Name = "progressMain";
            this.progressMain.ProgressEndColor = System.Drawing.Color.SteelBlue;
            this.progressMain.ProgressStartColor = System.Drawing.Color.WhiteSmoke;
            this.progressMain.Size = new System.Drawing.Size(325, 15);
            this.progressMain.Step = 1;
            this.progressMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressMain.TabIndex = 16;
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationTitle.Location = new System.Drawing.Point(184, 12);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(140, 21);
            this.lblApplicationTitle.TabIndex = 14;
            this.lblApplicationTitle.Text = "Figlut Suite Installer";
            // 
            // gpbInstallComponents
            // 
            this.gpbInstallComponents.Controls.Add(this.chkFiglutMobileToolkitWM);
            this.gpbInstallComponents.Controls.Add(this.chkFiglutServerToolkit);
            this.gpbInstallComponents.Controls.Add(this.chkFiglutDesktopDataBox);
            this.gpbInstallComponents.Controls.Add(this.chkFiglutWebService);
            this.gpbInstallComponents.Controls.Add(this.chkFiglutConfigurationManager);
            this.gpbInstallComponents.Location = new System.Drawing.Point(187, 57);
            this.gpbInstallComponents.Name = "gpbInstallComponents";
            this.gpbInstallComponents.Size = new System.Drawing.Size(325, 139);
            this.gpbInstallComponents.TabIndex = 17;
            this.gpbInstallComponents.TabStop = false;
            this.gpbInstallComponents.Text = "Select components to install:";
            // 
            // chkFiglutMobileToolkitWM
            // 
            this.chkFiglutMobileToolkitWM.AutoSize = true;
            this.chkFiglutMobileToolkitWM.Location = new System.Drawing.Point(6, 111);
            this.chkFiglutMobileToolkitWM.Name = "chkFiglutMobileToolkitWM";
            this.chkFiglutMobileToolkitWM.Size = new System.Drawing.Size(225, 17);
            this.chkFiglutMobileToolkitWM.TabIndex = 4;
            this.chkFiglutMobileToolkitWM.Text = "Figlut Mobile Toolkit for Windows Mobile 6";
            this.chkFiglutMobileToolkitWM.UseVisualStyleBackColor = true;
            this.chkFiglutMobileToolkitWM.CheckStateChanged += new System.EventHandler(this.chkComponentCheckChanged_CheckStateChanged);
            // 
            // chkFiglutServerToolkit
            // 
            this.chkFiglutServerToolkit.AutoSize = true;
            this.chkFiglutServerToolkit.Location = new System.Drawing.Point(6, 88);
            this.chkFiglutServerToolkit.Name = "chkFiglutServerToolkit";
            this.chkFiglutServerToolkit.Size = new System.Drawing.Size(120, 17);
            this.chkFiglutServerToolkit.TabIndex = 3;
            this.chkFiglutServerToolkit.Text = "Figlut Server Toolkit";
            this.chkFiglutServerToolkit.UseVisualStyleBackColor = true;
            this.chkFiglutServerToolkit.CheckStateChanged += new System.EventHandler(this.chkComponentCheckChanged_CheckStateChanged);
            // 
            // chkFiglutDesktopDataBox
            // 
            this.chkFiglutDesktopDataBox.AutoSize = true;
            this.chkFiglutDesktopDataBox.Location = new System.Drawing.Point(6, 65);
            this.chkFiglutDesktopDataBox.Name = "chkFiglutDesktopDataBox";
            this.chkFiglutDesktopDataBox.Size = new System.Drawing.Size(138, 17);
            this.chkFiglutDesktopDataBox.TabIndex = 2;
            this.chkFiglutDesktopDataBox.Text = "Figlut Desktop DataBox";
            this.chkFiglutDesktopDataBox.UseVisualStyleBackColor = true;
            this.chkFiglutDesktopDataBox.CheckStateChanged += new System.EventHandler(this.chkComponentCheckChanged_CheckStateChanged);
            // 
            // chkFiglutWebService
            // 
            this.chkFiglutWebService.AutoSize = true;
            this.chkFiglutWebService.Location = new System.Drawing.Point(6, 42);
            this.chkFiglutWebService.Name = "chkFiglutWebService";
            this.chkFiglutWebService.Size = new System.Drawing.Size(116, 17);
            this.chkFiglutWebService.TabIndex = 1;
            this.chkFiglutWebService.Text = "Figlut Web Service";
            this.chkFiglutWebService.UseVisualStyleBackColor = true;
            this.chkFiglutWebService.CheckStateChanged += new System.EventHandler(this.chkComponentCheckChanged_CheckStateChanged);
            // 
            // chkFiglutConfigurationManager
            // 
            this.chkFiglutConfigurationManager.AutoSize = true;
            this.chkFiglutConfigurationManager.Location = new System.Drawing.Point(6, 19);
            this.chkFiglutConfigurationManager.Name = "chkFiglutConfigurationManager";
            this.chkFiglutConfigurationManager.Size = new System.Drawing.Size(161, 17);
            this.chkFiglutConfigurationManager.TabIndex = 0;
            this.chkFiglutConfigurationManager.Text = "Figlut Configuration Manager";
            this.chkFiglutConfigurationManager.UseVisualStyleBackColor = true;
            this.chkFiglutConfigurationManager.CheckStateChanged += new System.EventHandler(this.chkComponentCheckChanged_CheckStateChanged);
            // 
            // btnInstall
            // 
            this.btnInstall.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnInstall.BackgroundImage")));
            this.btnInstall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInstall.ForeColor = System.Drawing.Color.White;
            this.btnInstall.GradientEndColor = System.Drawing.Color.SteelBlue;
            this.btnInstall.GradientStartColor = System.Drawing.Color.White;
            this.btnInstall.Location = new System.Drawing.Point(187, 202);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(156, 23);
            this.btnInstall.TabIndex = 18;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.GradientEndColor = System.Drawing.Color.SteelBlue;
            this.btnCancel.GradientStartColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(356, 202);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(156, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnInstall;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(558, 277);
            this.FormTitle = "Figlut Suite Installer";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Status = "Select components to install.";
            this.Text = "Figlut Suite Installer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.pnlFormContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.gpbInstallComponents.ResumeLayout(false);
            this.gpbInstallComponents.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private Server.Toolkit.Winforms.CustomProgressBar progressMain;
        private System.Windows.Forms.Label lblApplicationTitle;
        private System.Windows.Forms.GroupBox gpbInstallComponents;
        private System.Windows.Forms.CheckBox chkFiglutMobileToolkitWM;
        private System.Windows.Forms.CheckBox chkFiglutServerToolkit;
        private System.Windows.Forms.CheckBox chkFiglutDesktopDataBox;
        private System.Windows.Forms.CheckBox chkFiglutWebService;
        private System.Windows.Forms.CheckBox chkFiglutConfigurationManager;
        private Server.Toolkit.Winforms.GradientButton btnInstall;
        private Server.Toolkit.Winforms.GradientButton btnCancel;
    }
}

