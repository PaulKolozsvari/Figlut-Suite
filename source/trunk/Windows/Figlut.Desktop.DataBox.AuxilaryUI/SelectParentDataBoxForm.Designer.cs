namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    partial class SelectParentDataBoxForm
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
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.lstParentDataBoxes = new System.Windows.Forms.ListBox();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.lstParentDataBoxes);
            this.pnlFormContent.Size = new System.Drawing.Size(313, 249);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(328, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 249);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 249);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(343, 21);
            this.lblFormTitle.Text = "Select Parent DataBox ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstParentDataBoxes_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstParentDataBoxes_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstParentDataBoxes_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 270);
            this.statusMain.Size = new System.Drawing.Size(343, 21);
            this.statusMain.Text = "Select parent DataBox.";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSelect,
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(15, 21);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(313, 24);
            this.mnuMain.TabIndex = 159;
            this.mnuMain.Text = "customMainMenu1";
            // 
            // mnuSelect
            // 
            this.mnuSelect.Name = "mnuSelect";
            this.mnuSelect.Size = new System.Drawing.Size(50, 20);
            this.mnuSelect.Text = "&Select";
            this.mnuSelect.Click += new System.EventHandler(this.mnuSelect_Click);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Name = "mnuCancel";
            this.mnuCancel.Size = new System.Drawing.Size(55, 20);
            this.mnuCancel.Text = "&Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // lstParentDataBoxes
            // 
            this.lstParentDataBoxes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstParentDataBoxes.FormattingEnabled = true;
            this.lstParentDataBoxes.Location = new System.Drawing.Point(0, 24);
            this.lstParentDataBoxes.Name = "lstParentDataBoxes";
            this.lstParentDataBoxes.Size = new System.Drawing.Size(313, 225);
            this.lstParentDataBoxes.TabIndex = 0;
            // 
            // SelectParentDataBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 291);
            this.Controls.Add(this.mnuMain);
            this.FormTitle = "Select Parent DataBox";
            this.Name = "SelectParentDataBoxForm";
            this.ShowInTaskbar = false;
            this.Status = "Select parent DataBox.";
            this.Text = "Select Parent DataBox";
            this.Load += new System.EventHandler(this.SelectParentDataBoxForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SelectParentDataBoxForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstParentDataBoxes_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstParentDataBoxes_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstParentDataBoxes_MouseUp);
            this.Controls.SetChildIndex(this.statusMain, 0);
            this.Controls.SetChildIndex(this.lblFormTitle, 0);
            this.Controls.SetChildIndex(this.pnlFormLeft, 0);
            this.Controls.SetChildIndex(this.pnlFormRight, 0);
            this.Controls.SetChildIndex(this.pnlFormContent, 0);
            this.Controls.SetChildIndex(this.mnuMain, 0);
            this.pnlFormContent.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuSelect;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private System.Windows.Forms.ListBox lstParentDataBoxes;

    }
}