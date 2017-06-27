namespace Figlut.Mobile.DataBox.UI
{
    partial class SelectChildDataBoxForm
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
            this.lstChildrenDataBoxes = new System.Windows.Forms.ListBox();
            this.lblChildren = new System.Windows.Forms.Label();
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
            // lstChildrenDataBoxes
            // 
            this.lstChildrenDataBoxes.BackColor = System.Drawing.Color.White;
            this.lstChildrenDataBoxes.Location = new System.Drawing.Point(17, 123);
            this.lstChildrenDataBoxes.Name = "lstChildrenDataBoxes";
            this.lstChildrenDataBoxes.Size = new System.Drawing.Size(204, 114);
            this.lstChildrenDataBoxes.TabIndex = 6;
            // 
            // lblChildren
            // 
            this.lblChildren.Location = new System.Drawing.Point(17, 100);
            this.lblChildren.Name = "lblChildren";
            this.lblChildren.Size = new System.Drawing.Size(126, 20);
            this.lblChildren.Text = "Children DataBoxes:";
            // 
            // SelectChildDataBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lblChildren);
            this.Controls.Add(this.lstChildrenDataBoxes);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "SelectChildDataBoxForm";
            this.Text = "Select Child DataBox";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectChildTableForm_KeyDown);
            this.Load += new System.EventHandler(this.SelectChildTableForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuSelect;
        private System.Windows.Forms.ListBox lstChildrenDataBoxes;
        private System.Windows.Forms.Label lblChildren;
    }
}