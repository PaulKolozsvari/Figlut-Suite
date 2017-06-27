namespace Figlut.Mobile.DataBox.UI
{
    partial class SelectParentDataBoxForm
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
            this.mnuSelect = new System.Windows.Forms.MenuItem();
            this.lstParentDataBoxes = new System.Windows.Forms.ListBox();
            this.lblParents = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuCancel);
            this.mnuMain.MenuItems.Add(this.mnuSelect);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnuSelect
            // 
            this.mnuSelect.Text = "Select";
            this.mnuSelect.Click += new System.EventHandler(this.mnuSelect_Click);
            // 
            // lstParentDataBoxes
            // 
            this.lstParentDataBoxes.BackColor = System.Drawing.Color.White;
            this.lstParentDataBoxes.Location = new System.Drawing.Point(20, 123);
            this.lstParentDataBoxes.Name = "lstParentDataBoxes";
            this.lstParentDataBoxes.Size = new System.Drawing.Size(204, 114);
            this.lstParentDataBoxes.TabIndex = 5;
            // 
            // lblParents
            // 
            this.lblParents.Location = new System.Drawing.Point(20, 100);
            this.lblParents.Name = "lblParents";
            this.lblParents.Size = new System.Drawing.Size(120, 20);
            this.lblParents.Text = "Parent DataBoxes:";
            // 
            // SelectParentDataBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lblParents);
            this.Controls.Add(this.lstParentDataBoxes);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "SelectParentDataBoxForm";
            this.Text = "Select Parent DataBox";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectParentTableForm_KeyDown);
            this.Load += new System.EventHandler(this.SelectParentTableForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuSelect;
        private System.Windows.Forms.ListBox lstParentDataBoxes;
        private System.Windows.Forms.Label lblParents;
    }
}