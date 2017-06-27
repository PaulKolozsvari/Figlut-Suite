namespace Figlut.Mobile.Toolkit.SARoad.WM
{
    partial class SADriverLicensePhotoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SADriverLicensePhotoForm));
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuBack = new System.Windows.Forms.MenuItem();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.statusMain = new System.Windows.Forms.StatusBar();
            this.picDriverPhoto = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuBack);
            // 
            // mnuBack
            // 
            this.mnuBack.Text = "Back";
            this.mnuBack.Click += new System.EventHandler(this.mnuBack_Click);
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
            this.statusMain.Text = "Driver license photo.";
            // 
            // picDriverPhoto
            // 
            this.picDriverPhoto.Image = ((System.Drawing.Image)(resources.GetObject("picDriverPhoto.Image")));
            this.picDriverPhoto.Location = new System.Drawing.Point(3, 84);
            this.picDriverPhoto.Name = "picDriverPhoto";
            this.picDriverPhoto.Size = new System.Drawing.Size(234, 156);
            this.picDriverPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            // 
            // SADriverLicensePhotoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.picDriverPhoto);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.picLogo);
            this.Menu = this.mnuMain;
            this.Name = "SADriverLicensePhotoForm";
            this.Text = "Driver Photo";
            this.Load += new System.EventHandler(this.SADriverLicensePhotoForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuBack;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.StatusBar statusMain;
        private System.Windows.Forms.PictureBox picDriverPhoto;
    }
}