namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    partial class SyncForm
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
            this.mnuSynchronize = new System.Windows.Forms.MenuItem();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.statusMain = new System.Windows.Forms.StatusBar();
            this.progressSynchronization = new System.Windows.Forms.ProgressBar();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuBack);
            this.mnuMain.MenuItems.Add(this.mnuSynchronize);
            // 
            // mnuBack
            // 
            this.mnuBack.Text = "Back";
            this.mnuBack.Click += new System.EventHandler(this.mnuBack_Click);
            // 
            // mnuSynchronize
            // 
            this.mnuSynchronize.Text = "Synchronize";
            this.mnuSynchronize.Click += new System.EventHandler(this.mnuSynchronize_Click);
            // 
            // picLogo
            // 
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(240, 78);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 246);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(240, 22);
            this.statusMain.Text = "Synchronize database with server.";
            // 
            // progressSynchronization
            // 
            this.progressSynchronization.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressSynchronization.Location = new System.Drawing.Point(0, 226);
            this.progressSynchronization.Name = "progressSynchronization";
            this.progressSynchronization.Size = new System.Drawing.Size(240, 20);
            // 
            // txtStatus
            // 
            this.txtStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStatus.Location = new System.Drawing.Point(0, 78);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(240, 148);
            this.txtStatus.TabIndex = 18;
            // 
            // SyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.progressSynchronization);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.picLogo);
            this.Menu = this.mnuMain;
            this.Name = "SyncForm";
            this.Text = "Synchronize";
            this.Load += new System.EventHandler(this.SyncForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.MenuItem mnuBack;
        private System.Windows.Forms.MenuItem mnuSynchronize;
        private System.Windows.Forms.StatusBar statusMain;
        private System.Windows.Forms.ProgressBar progressSynchronization;
        private System.Windows.Forms.TextBox txtStatus;
    }
}