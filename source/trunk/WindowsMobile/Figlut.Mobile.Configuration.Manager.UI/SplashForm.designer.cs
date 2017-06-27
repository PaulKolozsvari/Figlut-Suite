namespace Figlut.Mobile.Configuration.Manager.UI
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tmrMain = new System.Windows.Forms.Timer();
            this.progressMain = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // picApplicationImage
            // 
            this.picApplicationImage.Image = ((System.Drawing.Image)(resources.GetObject("picApplicationImage.Image")));
            this.picApplicationImage.Location = new System.Drawing.Point(172, 87);
            this.picApplicationImage.Name = "picApplicationImage";
            this.picApplicationImage.Size = new System.Drawing.Size(52, 52);
            this.picApplicationImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(17, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 50);
            this.label1.Text = "Figlut Mobile Configuration Manager";
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblVersion.Location = new System.Drawing.Point(16, 213);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(115, 20);
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
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.picApplicationImage);
            this.Name = "SplashForm";
            this.Text = "Welcome";
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picApplicationImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.ProgressBar progressMain;
    }
}