namespace Figlut.Mobile.Toolkit.Demo.WM
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuDemo = new System.Windows.Forms.MenuItem();
            this.statusMain = new System.Windows.Forms.StatusBar();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lstDemos = new System.Windows.Forms.ListBox();
            this.lblDemos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuExit);
            this.mnuMain.MenuItems.Add(this.mnuDemo);
            // 
            // mnuExit
            // 
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuDemo
            // 
            this.mnuDemo.Text = "Demo";
            this.mnuDemo.Click += new System.EventHandler(this.mnuDemo_Click);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 246);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(240, 22);
            this.statusMain.Text = "Select demo.";
            // 
            // picLogo
            // 
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(240, 78);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // lstDemos
            // 
            this.lstDemos.Location = new System.Drawing.Point(3, 104);
            this.lstDemos.Name = "lstDemos";
            this.lstDemos.Size = new System.Drawing.Size(234, 128);
            this.lstDemos.TabIndex = 3;
            // 
            // lblDemos
            // 
            this.lblDemos.Location = new System.Drawing.Point(3, 81);
            this.lblDemos.Name = "lblDemos";
            this.lblDemos.Size = new System.Drawing.Size(100, 20);
            this.lblDemos.Text = "Demos:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lblDemos);
            this.Controls.Add(this.lstDemos);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.statusMain);
            this.Menu = this.mnuMain;
            this.Name = "MainForm";
            this.Text = "Figlut Mobile Toolkit";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.StatusBar statusMain;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.MenuItem mnuDemo;
        private System.Windows.Forms.ListBox lstDemos;
        private System.Windows.Forms.Label lblDemos;
    }
}