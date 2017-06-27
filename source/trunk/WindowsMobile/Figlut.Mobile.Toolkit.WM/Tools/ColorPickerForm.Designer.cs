namespace Figlut.Mobile.Toolkit.Tools
{
    partial class ColorPickerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPickerForm));
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuBack = new System.Windows.Forms.MenuItem();
            this.mnuSelect = new System.Windows.Forms.MenuItem();
            this.lblColors = new System.Windows.Forms.Label();
            this.statusMain = new System.Windows.Forms.StatusBar();
            this.lstColors = new System.Windows.Forms.ListBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuBack);
            this.mnuMain.MenuItems.Add(this.mnuSelect);
            // 
            // mnuBack
            // 
            this.mnuBack.Text = "Back";
            this.mnuBack.Click += new System.EventHandler(this.mnuBack_Click);
            // 
            // mnuSelect
            // 
            this.mnuSelect.Text = "Select";
            this.mnuSelect.Click += new System.EventHandler(this.mnuSelect_Click);
            // 
            // lblColors
            // 
            this.lblColors.Location = new System.Drawing.Point(0, 81);
            this.lblColors.Name = "lblColors";
            this.lblColors.Size = new System.Drawing.Size(100, 20);
            this.lblColors.Text = "Colors:";
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 246);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(240, 22);
            this.statusMain.Text = "Select color.";
            // 
            // lstColors
            // 
            this.lstColors.Location = new System.Drawing.Point(3, 103);
            this.lstColors.Name = "lstColors";
            this.lstColors.Size = new System.Drawing.Size(234, 142);
            this.lstColors.TabIndex = 2;
            // 
            // picLogo
            // 
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(240, 78);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // ColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.lstColors);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.lblColors);
            this.Menu = this.mnuMain;
            this.Name = "ColorPickerForm";
            this.Text = "Color Picker";
            this.Load += new System.EventHandler(this.ColorPickerForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuBack;
        private System.Windows.Forms.Label lblColors;
        private System.Windows.Forms.StatusBar statusMain;
        private System.Windows.Forms.ListBox lstColors;
        private System.Windows.Forms.MenuItem mnuSelect;
        private System.Windows.Forms.PictureBox picLogo;
    }
}