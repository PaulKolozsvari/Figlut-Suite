namespace Figlut.Desktop.DataBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.lblApplicationTitle = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.progressMain = new Figlut.Server.Toolkit.Winforms.CustomProgressBar();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pnlFormContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.BackColor = System.Drawing.Color.White;
            this.pnlFormContent.Controls.Add(this.lblVersion);
            this.pnlFormContent.Controls.Add(this.progressMain);
            this.pnlFormContent.Controls.Add(this.lblApplicationTitle);
            this.pnlFormContent.Controls.Add(this.picLogo);
            this.pnlFormContent.Size = new System.Drawing.Size(458, 363);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormRight.BackgroundImage")));
            this.pnlFormRight.Location = new System.Drawing.Point(473, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 363);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormLeft.BackgroundImage")));
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 363);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(488, 21);
            this.lblFormTitle.Text = "Figlut DataBox ";
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 384);
            this.statusMain.Size = new System.Drawing.Size(488, 21);
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 10;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblApplicationTitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationTitle.Location = new System.Drawing.Point(0, 261);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(458, 35);
            this.lblApplicationTitle.TabIndex = 11;
            this.lblApplicationTitle.Text = "Application Title";
            this.lblApplicationTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.White;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(458, 261);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLogo.TabIndex = 10;
            this.picLogo.TabStop = false;
            // 
            // progressMain
            // 
            this.progressMain.BackColor = System.Drawing.Color.DarkGray;
            this.progressMain.ForeColor = System.Drawing.Color.Silver;
            this.progressMain.Location = new System.Drawing.Point(28, 335);
            this.progressMain.Name = "progressMain";
            this.progressMain.ProgressEndColor = System.Drawing.Color.SteelBlue;
            this.progressMain.ProgressStartColor = System.Drawing.Color.WhiteSmoke;
            this.progressMain.Size = new System.Drawing.Size(406, 15);
            this.progressMain.Step = 1;
            this.progressMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressMain.TabIndex = 13;
            // 
            // lblVersion
            // 
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblVersion.Location = new System.Drawing.Point(0, 296);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(458, 20);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "Version 0.0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(488, 405);
            this.ControlBox = false;
            this.FormTitle = "Figlut DataBox";
            this.Name = "SplashForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Figlut DataBox";
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.pnlFormContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrMain;
        private Figlut.Server.Toolkit.Winforms.CustomProgressBar progressMain;
        private System.Windows.Forms.Label lblApplicationTitle;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblVersion;
    }
}