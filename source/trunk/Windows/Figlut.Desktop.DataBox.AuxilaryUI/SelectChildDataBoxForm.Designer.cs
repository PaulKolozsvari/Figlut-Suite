namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    partial class SelectChildDataBoxForm
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
            this.lstChildrenDataBoxes = new System.Windows.Forms.ListBox();
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.lstChildrenDataBoxes);
            this.pnlFormContent.Controls.Add(this.mnuMain);
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
            this.lblFormTitle.Text = "Select Child DataBox ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SelectChildDataBoxForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectChildDataBoxForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SelectChildDataBoxForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 270);
            this.statusMain.Size = new System.Drawing.Size(343, 21);
            this.statusMain.Text = "Select child DataBox.";
            // 
            // lstChildrenDataBoxes
            // 
            this.lstChildrenDataBoxes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstChildrenDataBoxes.FormattingEnabled = true;
            this.lstChildrenDataBoxes.Location = new System.Drawing.Point(0, 24);
            this.lstChildrenDataBoxes.Name = "lstChildrenDataBoxes";
            this.lstChildrenDataBoxes.Size = new System.Drawing.Size(313, 225);
            this.lstChildrenDataBoxes.TabIndex = 160;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSelect,
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(313, 24);
            this.mnuMain.TabIndex = 161;
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
            // SelectChildDataBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 291);
            this.FormTitle = "Select Child DataBox";
            this.Name = "SelectChildDataBoxForm";
            this.ShowInTaskbar = false;
            this.Status = "Select child DataBox.";
            this.Text = "Select Child DataBox";
            this.Load += new System.EventHandler(this.SelectChildDataBoxForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SelectChildDataBoxForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SelectChildDataBoxForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectChildDataBoxForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SelectChildDataBoxForm_MouseUp);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstChildrenDataBoxes;
        private Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuSelect;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
    }
}