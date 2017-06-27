namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    partial class ViewParentEntityForm
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
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.txtInfo);
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
            this.lblFormTitle.Text = "View Parent Entity ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewParentEntityForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewParentEntityForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ViewParentEntityForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 270);
            this.statusMain.Size = new System.Drawing.Size(343, 21);
            this.statusMain.Text = "View parent entity.";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(313, 24);
            this.mnuMain.TabIndex = 159;
            this.mnuMain.Text = "customMainMenu1";
            // 
            // mnuCancel
            // 
            this.mnuCancel.Name = "mnuCancel";
            this.mnuCancel.Size = new System.Drawing.Size(55, 20);
            this.mnuCancel.Text = "&Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // txtInfo
            // 
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.Location = new System.Drawing.Point(0, 24);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(313, 225);
            this.txtInfo.TabIndex = 160;
            // 
            // ViewParentEntityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 291);
            this.FormTitle = "View Parent Entity";
            this.Name = "ViewParentEntityForm";
            this.ShowInTaskbar = false;
            this.Status = "View parent entity.";
            this.Text = "View Parent Entity";
            this.Load += new System.EventHandler(this.ViewParentEntityForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ViewParentEntityForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewParentEntityForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewParentEntityForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ViewParentEntityForm_MouseUp);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private System.Windows.Forms.TextBox txtInfo;
    }
}