namespace Figlut.Mobile.DataBox.UI
{
    partial class MainMenuForm
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
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuSelect = new System.Windows.Forms.MenuItem();
            this.lstMenu = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuExit);
            this.mnuMain.MenuItems.Add(this.mnuSelect);
            // 
            // mnuExit
            // 
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuSelect
            // 
            this.mnuSelect.Text = "Select";
            this.mnuSelect.Click += new System.EventHandler(this.mnuSelect_Click);
            // 
            // lstMenu
            // 
            this.lstMenu.BackColor = System.Drawing.Color.White;
            this.lstMenu.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lstMenu.ForeColor = System.Drawing.Color.Black;
            this.lstMenu.FullRowSelect = true;
            this.lstMenu.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstMenu.Location = new System.Drawing.Point(18, 116);
            this.lstMenu.Name = "lstMenu";
            this.lstMenu.Size = new System.Drawing.Size(204, 120);
            this.lstMenu.TabIndex = 1;
            this.lstMenu.View = System.Windows.Forms.View.Details;
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lstMenu);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "MainMenuForm";
            this.Text = "Figlut Mobile DataBox";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainMenuForm_KeyDown);
            this.Load += new System.EventHandler(this.MainMenuForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mnuMain;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuSelect;
        private System.Windows.Forms.ListView lstMenu;

    }
}