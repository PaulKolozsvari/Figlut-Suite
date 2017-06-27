using System.Windows.Forms;

namespace Figlut.Mobile.Toolkit.Tools
{
    partial class SignatureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.mnuCancel = new System.Windows.Forms.MenuItem();
            this.menuItemMenu = new System.Windows.Forms.MenuItem();
            this.mnuCapture = new System.Windows.Forms.MenuItem();
            this.mnuWidth = new System.Windows.Forms.MenuItem();
            this.mnuWidth1Pixel = new System.Windows.Forms.MenuItem();
            this.mnuWidth3Pixels = new System.Windows.Forms.MenuItem();
            this.mnuWidth5Pixels = new System.Windows.Forms.MenuItem();
            this.mnuClear = new System.Windows.Forms.MenuItem();
            this.picSignature = new System.Windows.Forms.PictureBox();
            this.pictureBoxBlack = new System.Windows.Forms.PictureBox();
            this.pictureBoxRed = new System.Windows.Forms.PictureBox();
            this.pictureBoxGreen = new System.Windows.Forms.PictureBox();
            this.pictureBoxBlue = new System.Windows.Forms.PictureBox();
            this.pictureBoxYellow = new System.Windows.Forms.PictureBox();
            this.pictureBoxWhite = new System.Windows.Forms.PictureBox();
            this.panelRed = new System.Windows.Forms.Panel();
            this.panelGreen = new System.Windows.Forms.Panel();
            this.panelBlue = new System.Windows.Forms.Panel();
            this.panelWhite = new System.Windows.Forms.Panel();
            this.panelBlack = new System.Windows.Forms.Panel();
            this.panelSignature = new System.Windows.Forms.Panel();
            this.timer = new System.Windows.Forms.Timer();
            this.panelYellow = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.mnuCancel);
            this.mainMenu.MenuItems.Add(this.menuItemMenu);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // menuItemMenu
            // 
            this.menuItemMenu.MenuItems.Add(this.mnuCapture);
            this.menuItemMenu.MenuItems.Add(this.mnuWidth);
            this.menuItemMenu.MenuItems.Add(this.mnuClear);
            this.menuItemMenu.Text = "&Menu";
            // 
            // mnuCapture
            // 
            this.mnuCapture.Enabled = false;
            this.mnuCapture.Text = "Capture";
            this.mnuCapture.Click += new System.EventHandler(this.mnuCapture_Click);
            // 
            // mnuWidth
            // 
            this.mnuWidth.MenuItems.Add(this.mnuWidth1Pixel);
            this.mnuWidth.MenuItems.Add(this.mnuWidth3Pixels);
            this.mnuWidth.MenuItems.Add(this.mnuWidth5Pixels);
            this.mnuWidth.Text = "&Width";
            // 
            // mnuWidth1Pixel
            // 
            this.mnuWidth1Pixel.Checked = true;
            this.mnuWidth1Pixel.Text = "&1 pixel";
            this.mnuWidth1Pixel.Click += new System.EventHandler(this.mnuWidth1Pixel_Click);
            // 
            // mnuWidth3Pixels
            // 
            this.mnuWidth3Pixels.Text = "&3 pixels";
            this.mnuWidth3Pixels.Click += new System.EventHandler(this.mnuWidth3Pixels_Click);
            // 
            // mnuWidth5Pixels
            // 
            this.mnuWidth5Pixels.Text = "&5 pixels";
            this.mnuWidth5Pixels.Click += new System.EventHandler(this.mnuWidth5Pixels_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Text = "&Clear";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // picSignature
            // 
            this.picSignature.BackColor = System.Drawing.Color.White;
            this.picSignature.Location = new System.Drawing.Point(4, 4);
            this.picSignature.Name = "picSignature";
            this.picSignature.Size = new System.Drawing.Size(232, 221);
            this.picSignature.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.picSignature.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSignature_MouseUp);
            // 
            // pictureBoxBlack
            // 
            this.pictureBoxBlack.BackColor = System.Drawing.Color.Black;
            this.pictureBoxBlack.Location = new System.Drawing.Point(9, 233);
            this.pictureBoxBlack.Name = "pictureBoxBlack";
            this.pictureBoxBlack.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxBlack.Click += new System.EventHandler(this.pictureBoxBlack_Click);
            // 
            // pictureBoxRed
            // 
            this.pictureBoxRed.BackColor = System.Drawing.Color.Red;
            this.pictureBoxRed.Location = new System.Drawing.Point(47, 233);
            this.pictureBoxRed.Name = "pictureBoxRed";
            this.pictureBoxRed.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxRed.Click += new System.EventHandler(this.pictureBoxRed_Click);
            // 
            // pictureBoxGreen
            // 
            this.pictureBoxGreen.BackColor = System.Drawing.Color.Green;
            this.pictureBoxGreen.Location = new System.Drawing.Point(85, 233);
            this.pictureBoxGreen.Name = "pictureBoxGreen";
            this.pictureBoxGreen.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxGreen.Click += new System.EventHandler(this.pictureBoxGreen_Click);
            // 
            // pictureBoxBlue
            // 
            this.pictureBoxBlue.BackColor = System.Drawing.Color.Blue;
            this.pictureBoxBlue.Location = new System.Drawing.Point(123, 233);
            this.pictureBoxBlue.Name = "pictureBoxBlue";
            this.pictureBoxBlue.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxBlue.Click += new System.EventHandler(this.pictureBoxBlue_Click);
            // 
            // pictureBoxYellow
            // 
            this.pictureBoxYellow.BackColor = System.Drawing.Color.Yellow;
            this.pictureBoxYellow.Location = new System.Drawing.Point(162, 233);
            this.pictureBoxYellow.Name = "pictureBoxYellow";
            this.pictureBoxYellow.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxYellow.Click += new System.EventHandler(this.pictureBoxYellow_Click);
            // 
            // pictureBoxWhite
            // 
            this.pictureBoxWhite.BackColor = System.Drawing.Color.White;
            this.pictureBoxWhite.Location = new System.Drawing.Point(199, 233);
            this.pictureBoxWhite.Name = "pictureBoxWhite";
            this.pictureBoxWhite.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxWhite.Click += new System.EventHandler(this.pictureBoxWhite_Click);
            // 
            // panelRed
            // 
            this.panelRed.BackColor = System.Drawing.Color.Black;
            this.panelRed.Location = new System.Drawing.Point(46, 232);
            this.panelRed.Name = "panelRed";
            this.panelRed.Size = new System.Drawing.Size(34, 34);
            // 
            // panelGreen
            // 
            this.panelGreen.BackColor = System.Drawing.Color.Black;
            this.panelGreen.Location = new System.Drawing.Point(84, 232);
            this.panelGreen.Name = "panelGreen";
            this.panelGreen.Size = new System.Drawing.Size(34, 34);
            // 
            // panelBlue
            // 
            this.panelBlue.BackColor = System.Drawing.Color.Black;
            this.panelBlue.Location = new System.Drawing.Point(123, 232);
            this.panelBlue.Name = "panelBlue";
            this.panelBlue.Size = new System.Drawing.Size(34, 34);
            // 
            // panelWhite
            // 
            this.panelWhite.BackColor = System.Drawing.Color.Black;
            this.panelWhite.Location = new System.Drawing.Point(198, 232);
            this.panelWhite.Name = "panelWhite";
            this.panelWhite.Size = new System.Drawing.Size(34, 34);
            // 
            // panelBlack
            // 
            this.panelBlack.BackColor = System.Drawing.Color.Gray;
            this.panelBlack.Location = new System.Drawing.Point(8, 232);
            this.panelBlack.Name = "panelBlack";
            this.panelBlack.Size = new System.Drawing.Size(34, 34);
            // 
            // panelSignature
            // 
            this.panelSignature.BackColor = System.Drawing.Color.Black;
            this.panelSignature.Location = new System.Drawing.Point(3, 3);
            this.panelSignature.Name = "panelSignature";
            this.panelSignature.Size = new System.Drawing.Size(234, 223);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panelYellow
            // 
            this.panelYellow.BackColor = System.Drawing.Color.Black;
            this.panelYellow.Location = new System.Drawing.Point(161, 232);
            this.panelYellow.Name = "panelYellow";
            this.panelYellow.Size = new System.Drawing.Size(34, 34);
            // 
            // SignatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.picSignature);
            this.Controls.Add(this.pictureBoxWhite);
            this.Controls.Add(this.pictureBoxYellow);
            this.Controls.Add(this.panelYellow);
            this.Controls.Add(this.pictureBoxBlue);
            this.Controls.Add(this.pictureBoxGreen);
            this.Controls.Add(this.pictureBoxRed);
            this.Controls.Add(this.pictureBoxBlack);
            this.Controls.Add(this.panelRed);
            this.Controls.Add(this.panelGreen);
            this.Controls.Add(this.panelBlue);
            this.Controls.Add(this.panelWhite);
            this.Controls.Add(this.panelBlack);
            this.Controls.Add(this.panelSignature);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "SignatureForm";
            this.Text = "Mobile Signature";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picSignature;
        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem menuItemMenu;
        private System.Windows.Forms.MenuItem mnuClear;
        private System.Windows.Forms.PictureBox pictureBoxBlack;
        private System.Windows.Forms.PictureBox pictureBoxRed;
        private System.Windows.Forms.PictureBox pictureBoxGreen;
        private System.Windows.Forms.PictureBox pictureBoxBlue;
        private System.Windows.Forms.PictureBox pictureBoxYellow;
        private System.Windows.Forms.PictureBox pictureBoxWhite;
        private System.Windows.Forms.Panel panelRed;
        private System.Windows.Forms.Panel panelGreen;
        private System.Windows.Forms.Panel panelBlue;
        private System.Windows.Forms.Panel panelWhite;
        private System.Windows.Forms.Panel panelBlack;
        private System.Windows.Forms.Panel panelSignature;
        private System.Windows.Forms.MenuItem mnuWidth;
        private System.Windows.Forms.MenuItem mnuWidth1Pixel;
        private System.Windows.Forms.MenuItem mnuWidth3Pixels;
        private System.Windows.Forms.MenuItem mnuWidth5Pixels;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panelYellow;
        private System.Windows.Forms.MenuItem mnuCapture;
    }
}

