namespace Figlut.Mobile.Toolkit.Demo.WM
{
    partial class BeginHotspotForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BeginHotspotForm));
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuBack = new System.Windows.Forms.MenuItem();
            this.mnuSelect = new System.Windows.Forms.MenuItem();
            this.statusMain = new System.Windows.Forms.StatusBar();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.radPolygon = new System.Windows.Forms.RadioButton();
            this.radCircle = new System.Windows.Forms.RadioButton();
            this.radRectangle = new System.Windows.Forms.RadioButton();
            this.lblKey = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtTag = new System.Windows.Forms.TextBox();
            this.lblTag = new System.Windows.Forms.Label();
            this.chkEnableHotspotHighlight = new System.Windows.Forms.CheckBox();
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
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 246);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(240, 22);
            this.statusMain.Text = "Select hotspot type.";
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
            // radPolygon
            // 
            this.radPolygon.Location = new System.Drawing.Point(64, 164);
            this.radPolygon.Name = "radPolygon";
            this.radPolygon.Size = new System.Drawing.Size(100, 20);
            this.radPolygon.TabIndex = 2;
            this.radPolygon.Text = "Polygon";
            // 
            // radCircle
            // 
            this.radCircle.Location = new System.Drawing.Point(64, 190);
            this.radCircle.Name = "radCircle";
            this.radCircle.Size = new System.Drawing.Size(100, 20);
            this.radCircle.TabIndex = 3;
            this.radCircle.Text = "Circle";
            // 
            // radRectangle
            // 
            this.radRectangle.Location = new System.Drawing.Point(64, 216);
            this.radRectangle.Name = "radRectangle";
            this.radRectangle.Size = new System.Drawing.Size(100, 20);
            this.radRectangle.TabIndex = 4;
            this.radRectangle.Text = "Rectangle";
            // 
            // lblKey
            // 
            this.lblKey.Location = new System.Drawing.Point(3, 85);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(85, 20);
            this.lblKey.Text = "Key:";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(64, 84);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(173, 21);
            this.txtKey.TabIndex = 8;
            // 
            // txtTag
            // 
            this.txtTag.Location = new System.Drawing.Point(64, 111);
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(173, 21);
            this.txtTag.TabIndex = 10;
            // 
            // lblTag
            // 
            this.lblTag.Location = new System.Drawing.Point(3, 112);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(85, 20);
            this.lblTag.Text = "Tag:";
            // 
            // chkEnableHotspotHighlight
            // 
            this.chkEnableHotspotHighlight.Checked = true;
            this.chkEnableHotspotHighlight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableHotspotHighlight.Location = new System.Drawing.Point(64, 138);
            this.chkEnableHotspotHighlight.Name = "chkEnableHotspotHighlight";
            this.chkEnableHotspotHighlight.Size = new System.Drawing.Size(173, 20);
            this.chkEnableHotspotHighlight.TabIndex = 12;
            this.chkEnableHotspotHighlight.Text = "Enable Hotspot Highlight";
            // 
            // BeginHotspotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.chkEnableHotspotHighlight);
            this.Controls.Add(this.txtTag);
            this.Controls.Add(this.lblTag);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.radRectangle);
            this.Controls.Add(this.radCircle);
            this.Controls.Add(this.radPolygon);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.statusMain);
            this.Menu = this.mnuMain;
            this.Name = "BeginHotspotForm";
            this.Text = "Begin Hotspot";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusBar statusMain;
        private System.Windows.Forms.MenuItem mnuBack;
        private System.Windows.Forms.MenuItem mnuSelect;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.RadioButton radPolygon;
        private System.Windows.Forms.RadioButton radCircle;
        private System.Windows.Forms.RadioButton radRectangle;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Label lblTag;
        private System.Windows.Forms.CheckBox chkEnableHotspotHighlight;
    }
}