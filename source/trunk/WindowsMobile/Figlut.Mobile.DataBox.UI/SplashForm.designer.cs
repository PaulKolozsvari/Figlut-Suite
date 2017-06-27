namespace Figlut.Mobile.DataBox.UI
{
    partial class SplashForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.picApplicationImage = new System.Windows.Forms.PictureBox();
            this.lblApplicationTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tmrMain = new System.Windows.Forms.Timer();
            this.progressMain = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // picApplicationImage
            // 
            this.picApplicationImage.Image = ((System.Drawing.Image)(resources.GetObject("picApplicationImage.Image")));
            this.picApplicationImage.Location = new System.Drawing.Point(173, 87);
            this.picApplicationImage.Name = "picApplicationImage";
            this.picApplicationImage.Size = new System.Drawing.Size(52, 52);
            this.picApplicationImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblApplicationTitle.Location = new System.Drawing.Point(16, 172);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(209, 44);
            this.lblApplicationTitle.Text = "Figlut Mobile DataBox";
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblVersion.Location = new System.Drawing.Point(16, 216);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(209, 20);
            this.lblVersion.Text = "Version 1.2.0.0";
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 10;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // progressMain
            // 
            this.progressMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressMain.Location = new System.Drawing.Point(0, 274);
            this.progressMain.Name = "progressMain";
            this.progressMain.Size = new System.Drawing.Size(240, 20);
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.progressMain);
            this.Controls.Add(this.lblApplicationTitle);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.picApplicationImage);
            this.Name = "SplashForm";
            this.Text = "Welcome";
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picApplicationImage;
        private System.Windows.Forms.Label lblApplicationTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.ProgressBar progressMain;
    }
}