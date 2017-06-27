namespace Figlut.Configuration.Manager
{
    partial class EditSettingForm
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
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Size = new System.Drawing.Size(470, 179);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(485, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 179);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 179);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(500, 21);
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 200);
            this.statusMain.Size = new System.Drawing.Size(500, 21);
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSave,
            this.cancelToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(470, 24);
            this.mnuMain.TabIndex = 1;
            // 
            // mnuSave
            // 
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(43, 20);
            this.mnuSave.Text = "&Save";
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.cancelToolStripMenuItem.Text = "&Cancel";
            // 
            // EditSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 221);
            this.Name = "EditSettingForm";
            this.ShowInTaskbar = false;
            this.Text = "EditSettingForm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseUp);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
    }
}