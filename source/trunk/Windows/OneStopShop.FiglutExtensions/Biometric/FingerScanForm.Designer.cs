namespace OneStopShop.FiglutExtensions.Biometric
{
    partial class FingerScanForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FingerScanForm));
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuApply = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLoadDataBox = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.cboFinger = new System.Windows.Forms.ComboBox();
            this.progressQuality = new Figlut.Server.Toolkit.Winforms.CustomProgressBar();
            this.txtQuality = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.cboDevices = new System.Windows.Forms.ComboBox();
            this.picFingerprint = new System.Windows.Forms.PictureBox();
            this.btnCancel = new Figlut.Server.Toolkit.Winforms.GradientButton();
            this.btnAcquire = new Figlut.Server.Toolkit.Winforms.GradientButton();
            this.btnDescriptor = new Figlut.Server.Toolkit.Winforms.GradientButton();
            this.btnRefreshDevices = new Figlut.Server.Toolkit.Winforms.GradientButton();
            this.mnuMain.SuspendLayout();
            this.pnlLoadDataBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFingerprint)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Size = new System.Drawing.Size(349, 371);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(364, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 371);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 371);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(379, 21);
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FingerScanForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FingerScanForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FingerScanForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 392);
            this.statusMain.Size = new System.Drawing.Size(379, 21);
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuApply,
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(15, 21);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(349, 24);
            this.mnuMain.TabIndex = 158;
            this.mnuMain.Text = "customMainMenu1";
            // 
            // mnuApply
            // 
            this.mnuApply.Enabled = false;
            this.mnuApply.Name = "mnuApply";
            this.mnuApply.Size = new System.Drawing.Size(50, 20);
            this.mnuApply.Text = "&Apply";
            this.mnuApply.Click += new System.EventHandler(this.mnuApply_Click);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Name = "mnuCancel";
            this.mnuCancel.Size = new System.Drawing.Size(55, 20);
            this.mnuCancel.Text = "&Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // pnlLoadDataBox
            // 
            this.pnlLoadDataBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLoadDataBox.BackgroundImage")));
            this.pnlLoadDataBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlLoadDataBox.Controls.Add(this.cboFinger);
            this.pnlLoadDataBox.Controls.Add(this.progressQuality);
            this.pnlLoadDataBox.Controls.Add(this.txtQuality);
            this.pnlLoadDataBox.Controls.Add(this.txtStatus);
            this.pnlLoadDataBox.Controls.Add(this.cboDevices);
            this.pnlLoadDataBox.Controls.Add(this.picFingerprint);
            this.pnlLoadDataBox.Controls.Add(this.btnCancel);
            this.pnlLoadDataBox.Controls.Add(this.btnAcquire);
            this.pnlLoadDataBox.Controls.Add(this.btnDescriptor);
            this.pnlLoadDataBox.Controls.Add(this.btnRefreshDevices);
            this.pnlLoadDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoadDataBox.GradientEndColor = System.Drawing.Color.WhiteSmoke;
            this.pnlLoadDataBox.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlLoadDataBox.GradientStartColor = System.Drawing.Color.White;
            this.pnlLoadDataBox.Location = new System.Drawing.Point(15, 45);
            this.pnlLoadDataBox.Name = "pnlLoadDataBox";
            this.pnlLoadDataBox.Size = new System.Drawing.Size(349, 347);
            this.pnlLoadDataBox.TabIndex = 159;
            // 
            // cboFinger
            // 
            this.cboFinger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFinger.FormattingEnabled = true;
            this.cboFinger.Location = new System.Drawing.Point(133, 32);
            this.cboFinger.Name = "cboFinger";
            this.cboFinger.Size = new System.Drawing.Size(208, 21);
            this.cboFinger.TabIndex = 15;
            // 
            // progressQuality
            // 
            this.progressQuality.BackColor = System.Drawing.Color.DarkGray;
            this.progressQuality.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressQuality.ForeColor = System.Drawing.Color.Silver;
            this.progressQuality.Location = new System.Drawing.Point(0, 332);
            this.progressQuality.Maximum = 120;
            this.progressQuality.Name = "progressQuality";
            this.progressQuality.ProgressEndColor = System.Drawing.Color.DarkRed;
            this.progressQuality.ProgressStartColor = System.Drawing.Color.WhiteSmoke;
            this.progressQuality.Size = new System.Drawing.Size(349, 15);
            this.progressQuality.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressQuality.TabIndex = 14;
            // 
            // txtQuality
            // 
            this.txtQuality.Location = new System.Drawing.Point(133, 299);
            this.txtQuality.Name = "txtQuality";
            this.txtQuality.Size = new System.Drawing.Size(208, 20);
            this.txtQuality.TabIndex = 7;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(133, 273);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(208, 20);
            this.txtStatus.TabIndex = 6;
            // 
            // cboDevices
            // 
            this.cboDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevices.FormattingEnabled = true;
            this.cboDevices.Location = new System.Drawing.Point(133, 5);
            this.cboDevices.Name = "cboDevices";
            this.cboDevices.Size = new System.Drawing.Size(208, 21);
            this.cboDevices.TabIndex = 5;
            // 
            // picFingerprint
            // 
            this.picFingerprint.BackColor = System.Drawing.Color.White;
            this.picFingerprint.Location = new System.Drawing.Point(133, 59);
            this.picFingerprint.Name = "picFingerprint";
            this.picFingerprint.Size = new System.Drawing.Size(208, 208);
            this.picFingerprint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFingerprint.TabIndex = 4;
            this.picFingerprint.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.GradientEndColor = System.Drawing.Color.DarkRed;
            this.btnCancel.GradientStartColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(6, 90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(121, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel Acquisition";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAcquire
            // 
            this.btnAcquire.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAcquire.BackgroundImage")));
            this.btnAcquire.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAcquire.GradientEndColor = System.Drawing.Color.DarkRed;
            this.btnAcquire.GradientStartColor = System.Drawing.Color.White;
            this.btnAcquire.Location = new System.Drawing.Point(6, 61);
            this.btnAcquire.Name = "btnAcquire";
            this.btnAcquire.Size = new System.Drawing.Size(121, 23);
            this.btnAcquire.TabIndex = 2;
            this.btnAcquire.Text = "&Acquire";
            this.btnAcquire.UseVisualStyleBackColor = true;
            this.btnAcquire.Click += new System.EventHandler(this.btnAcquire_Click);
            // 
            // btnDescriptor
            // 
            this.btnDescriptor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDescriptor.BackgroundImage")));
            this.btnDescriptor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDescriptor.GradientEndColor = System.Drawing.Color.DarkRed;
            this.btnDescriptor.GradientStartColor = System.Drawing.Color.White;
            this.btnDescriptor.Location = new System.Drawing.Point(6, 32);
            this.btnDescriptor.Name = "btnDescriptor";
            this.btnDescriptor.Size = new System.Drawing.Size(121, 23);
            this.btnDescriptor.TabIndex = 1;
            this.btnDescriptor.Text = "&Device Descriptor";
            this.btnDescriptor.UseVisualStyleBackColor = true;
            this.btnDescriptor.Click += new System.EventHandler(this.btnDescriptor_Click);
            // 
            // btnRefreshDevices
            // 
            this.btnRefreshDevices.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshDevices.BackgroundImage")));
            this.btnRefreshDevices.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefreshDevices.GradientEndColor = System.Drawing.Color.DarkRed;
            this.btnRefreshDevices.GradientStartColor = System.Drawing.Color.White;
            this.btnRefreshDevices.Location = new System.Drawing.Point(6, 3);
            this.btnRefreshDevices.Name = "btnRefreshDevices";
            this.btnRefreshDevices.Size = new System.Drawing.Size(121, 23);
            this.btnRefreshDevices.TabIndex = 0;
            this.btnRefreshDevices.Text = "&Refresh Devices";
            this.btnRefreshDevices.UseVisualStyleBackColor = true;
            this.btnRefreshDevices.Click += new System.EventHandler(this.btnRefreshDevices_Click);
            // 
            // FingerScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 413);
            this.Controls.Add(this.pnlLoadDataBox);
            this.Controls.Add(this.mnuMain);
            this.KeyPreview = true;
            this.Name = "FingerScanForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Status = "";
            this.Text = "";
            this.Load += new System.EventHandler(this.FingerScanForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FingerScanForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FingerScanForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FingerScanForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FingerScanForm_MouseUp);
            this.Controls.SetChildIndex(this.statusMain, 0);
            this.Controls.SetChildIndex(this.lblFormTitle, 0);
            this.Controls.SetChildIndex(this.pnlFormLeft, 0);
            this.Controls.SetChildIndex(this.pnlFormRight, 0);
            this.Controls.SetChildIndex(this.pnlFormContent, 0);
            this.Controls.SetChildIndex(this.mnuMain, 0);
            this.Controls.SetChildIndex(this.pnlLoadDataBox, 0);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.pnlLoadDataBox.ResumeLayout(false);
            this.pnlLoadDataBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFingerprint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Figlut.Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuApply;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private Figlut.Server.Toolkit.Winforms.GradientPanel pnlLoadDataBox;
        private Figlut.Server.Toolkit.Winforms.GradientButton btnRefreshDevices;
        private Figlut.Server.Toolkit.Winforms.GradientButton btnDescriptor;
        private Figlut.Server.Toolkit.Winforms.GradientButton btnAcquire;
        private Figlut.Server.Toolkit.Winforms.GradientButton btnCancel;
        private System.Windows.Forms.PictureBox picFingerprint;
        private System.Windows.Forms.ComboBox cboDevices;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtQuality;
        private Figlut.Server.Toolkit.Winforms.CustomProgressBar progressQuality;
        private System.Windows.Forms.ComboBox cboFinger;
    }
}