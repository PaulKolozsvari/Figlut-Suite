namespace Figlut.Mobile.Configuration.Manager.UI
{
    partial class EditSettingsForm
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
            this.mnuCancel = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.grdSettingValues = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuCancel);
            this.mnuMain.MenuItems.Add(this.mnuEdit);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Text = "Edit";
            this.mnuEdit.Click += new System.EventHandler(this.mnuEdit_Click);
            // 
            // grdSettingValues
            // 
            this.grdSettingValues.BackColor = System.Drawing.Color.White;
            this.grdSettingValues.BackgroundColor = System.Drawing.Color.White;
            this.grdSettingValues.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular);
            this.grdSettingValues.ForeColor = System.Drawing.Color.Black;
            this.grdSettingValues.GridLineColor = System.Drawing.Color.SteelBlue;
            this.grdSettingValues.HeaderBackColor = System.Drawing.Color.SteelBlue;
            this.grdSettingValues.HeaderForeColor = System.Drawing.Color.White;
            this.grdSettingValues.Location = new System.Drawing.Point(18, 116);
            this.grdSettingValues.Name = "grdSettingValues";
            this.grdSettingValues.PreferredRowHeight = 8;
            this.grdSettingValues.RowHeadersVisible = false;
            this.grdSettingValues.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.grdSettingValues.SelectionForeColor = System.Drawing.Color.White;
            this.grdSettingValues.Size = new System.Drawing.Size(204, 120);
            this.grdSettingValues.TabIndex = 0;
            this.grdSettingValues.CurrentCellChanged += new System.EventHandler(this.grdSettingValues_CurrentCellChanged);
            // 
            // EditSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.grdSettingValues);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "EditSettingsForm";
            this.Text = "Edit Settings";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditSettingsForm_KeyDown);
            this.Load += new System.EventHandler(this.EditSettingsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuEdit;
        private System.Windows.Forms.DataGrid grdSettingValues;
    }
}