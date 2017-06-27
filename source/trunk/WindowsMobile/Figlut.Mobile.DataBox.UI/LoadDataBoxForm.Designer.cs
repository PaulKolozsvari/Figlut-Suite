namespace Figlut.Mobile.DataBox.UI
{
    partial class LoadDataBoxForm
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
            this.mnuLoad = new System.Windows.Forms.MenuItem();
            this.lblDataBoxTable = new System.Windows.Forms.Label();
            this.txtDataBoxTable = new System.Windows.Forms.TextBox();
            this.lstDataBoxTable = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuCancel);
            this.mnuMain.MenuItems.Add(this.mnuLoad);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnuLoad
            // 
            this.mnuLoad.Text = "Load";
            this.mnuLoad.Click += new System.EventHandler(this.mnuLoad_Click);
            // 
            // lblDataBoxTable
            // 
            this.lblDataBoxTable.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblDataBoxTable.Location = new System.Drawing.Point(20, 88);
            this.lblDataBoxTable.Name = "lblDataBoxTable";
            this.lblDataBoxTable.Size = new System.Drawing.Size(58, 20);
            this.lblDataBoxTable.Text = "DataBox:";
            // 
            // txtDataBoxTable
            // 
            this.txtDataBoxTable.Location = new System.Drawing.Point(84, 87);
            this.txtDataBoxTable.Name = "txtDataBoxTable";
            this.txtDataBoxTable.Size = new System.Drawing.Size(140, 21);
            this.txtDataBoxTable.TabIndex = 0;
            this.txtDataBoxTable.TextChanged += new System.EventHandler(this.txtDataBoxTable_TextChanged);
            // 
            // lstDataBoxTable
            // 
            this.lstDataBoxTable.BackColor = System.Drawing.Color.White;
            this.lstDataBoxTable.ForeColor = System.Drawing.Color.Black;
            this.lstDataBoxTable.Location = new System.Drawing.Point(18, 116);
            this.lstDataBoxTable.Name = "lstDataBoxTable";
            this.lstDataBoxTable.Size = new System.Drawing.Size(204, 128);
            this.lstDataBoxTable.TabIndex = 1;
            // 
            // LoadDataBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lstDataBoxTable);
            this.Controls.Add(this.txtDataBoxTable);
            this.Controls.Add(this.lblDataBoxTable);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "LoadDataBoxForm";
            this.Text = "Load DataBox";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoadDataBoxForm_KeyDown);
            this.Load += new System.EventHandler(this.LoadDataBoxForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuLoad;
        private System.Windows.Forms.Label lblDataBoxTable;
        private System.Windows.Forms.TextBox txtDataBoxTable;
        private System.Windows.Forms.ListBox lstDataBoxTable;
    }
}