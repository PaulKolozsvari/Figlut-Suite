namespace Figlut.Mobile.DataBox.UI
{
    partial class ViewChildrenEntitiesForm
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
            this.grdData = new System.Windows.Forms.DataGrid();
            this.lblChildTableName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuCancel);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // grdData
            // 
            this.grdData.BackColor = System.Drawing.Color.White;
            this.grdData.BackgroundColor = System.Drawing.Color.White;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular);
            this.grdData.ForeColor = System.Drawing.Color.Black;
            this.grdData.GridLineColor = System.Drawing.Color.SteelBlue;
            this.grdData.HeaderBackColor = System.Drawing.Color.SteelBlue;
            this.grdData.HeaderForeColor = System.Drawing.Color.White;
            this.grdData.Location = new System.Drawing.Point(18, 116);
            this.grdData.Name = "grdData";
            this.grdData.PreferredRowHeight = 8;
            this.grdData.RowHeadersVisible = false;
            this.grdData.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.grdData.SelectionForeColor = System.Drawing.Color.White;
            this.grdData.Size = new System.Drawing.Size(204, 120);
            this.grdData.TabIndex = 7;
            // 
            // lblChildTableName
            // 
            this.lblChildTableName.Location = new System.Drawing.Point(18, 93);
            this.lblChildTableName.Name = "lblChildTableName";
            this.lblChildTableName.Size = new System.Drawing.Size(74, 20);
            this.lblChildTableName.Text = "Children:";
            // 
            // ViewChildrenEntitiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lblChildTableName);
            this.Controls.Add(this.grdData);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "ViewChildrenEntitiesForm";
            this.Text = "View Children Entities";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ViewChildrenForm_KeyDown);
            this.Load += new System.EventHandler(this.ViewChildrenForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.DataGrid grdData;
        private System.Windows.Forms.Label lblChildTableName;
    }
}