namespace Figlut.Mobile.DataBox.UI
{
    partial class ViewParentEntityForm
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
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.lblParentTableName = new System.Windows.Forms.Label();
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
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(18, 116);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(204, 121);
            this.txtInfo.TabIndex = 0;
            // 
            // lblParentTableName
            // 
            this.lblParentTableName.Location = new System.Drawing.Point(18, 93);
            this.lblParentTableName.Name = "lblParentTableName";
            this.lblParentTableName.Size = new System.Drawing.Size(100, 20);
            this.lblParentTableName.Text = "Parent:";
            // 
            // ViewParentEntityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lblParentTableName);
            this.Controls.Add(this.txtInfo);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "ViewParentEntityForm";
            this.Text = "View Parent Entity";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ViewParentForm_KeyDown);
            this.Load += new System.EventHandler(this.ViewParentForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Label lblParentTableName;
    }
}