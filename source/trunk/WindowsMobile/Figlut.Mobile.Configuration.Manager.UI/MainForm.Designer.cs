namespace Figlut.Mobile.Configuration.Manager.UI
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
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuUpdate = new System.Windows.Forms.MenuItem();
            this.trvComponentSettings = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuExit);
            this.mnuMain.MenuItems.Add(this.mnuUpdate);
            // 
            // mnuExit
            // 
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuUpdate
            // 
            this.mnuUpdate.Text = "Update";
            this.mnuUpdate.Click += new System.EventHandler(this.mnuUpdate_Click);
            // 
            // trvComponentSettings
            // 
            this.trvComponentSettings.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.trvComponentSettings.ForeColor = System.Drawing.Color.Black;
            this.trvComponentSettings.Location = new System.Drawing.Point(13, 94);
            this.trvComponentSettings.Name = "trvComponentSettings";
            this.trvComponentSettings.Size = new System.Drawing.Size(214, 137);
            this.trvComponentSettings.TabIndex = 4;
            this.trvComponentSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvComponentSettings_AfterSelect);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.trvComponentSettings);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "MainForm";
            this.Text = "Figlut Configuration Manager";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuUpdate;
        private System.Windows.Forms.TreeView trvComponentSettings;
    }
}