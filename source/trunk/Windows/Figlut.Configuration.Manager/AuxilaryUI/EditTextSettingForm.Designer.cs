namespace Figlut.Configuration.Manager.AuxilaryUI
{
    partial class EditTextSettingForm
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
            this.mnuUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.lblValue = new System.Windows.Forms.Label();
            this.txtSetting = new System.Windows.Forms.TextBox();
            this.lblSetting = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.lblValue);
            this.pnlFormContent.Controls.Add(this.txtSetting);
            this.pnlFormContent.Controls.Add(this.lblSetting);
            this.pnlFormContent.Controls.Add(this.txtValue);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Size = new System.Drawing.Size(350, 111);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(365, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 111);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 111);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(380, 21);
            this.lblFormTitle.Text = "Update Setting";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 132);
            this.statusMain.Size = new System.Drawing.Size(380, 21);
            this.statusMain.Text = "Update setting value.";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUpdate,
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(350, 24);
            this.mnuMain.TabIndex = 1;
            // 
            // mnuUpdate
            // 
            this.mnuUpdate.Name = "mnuUpdate";
            this.mnuUpdate.Size = new System.Drawing.Size(57, 20);
            this.mnuUpdate.Text = "&Update";
            this.mnuUpdate.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Name = "mnuCancel";
            this.mnuCancel.Size = new System.Drawing.Size(55, 20);
            this.mnuCancel.Text = "&Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // lblValue
            // 
            this.lblValue.Location = new System.Drawing.Point(6, 73);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(74, 23);
            this.lblValue.TabIndex = 16;
            this.lblValue.Text = "Value:";
            // 
            // txtSetting
            // 
            this.txtSetting.Location = new System.Drawing.Point(86, 41);
            this.txtSetting.Name = "txtSetting";
            this.txtSetting.ReadOnly = true;
            this.txtSetting.Size = new System.Drawing.Size(250, 20);
            this.txtSetting.TabIndex = 15;
            // 
            // lblSetting
            // 
            this.lblSetting.Location = new System.Drawing.Point(6, 41);
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(74, 23);
            this.lblSetting.TabIndex = 14;
            this.lblSetting.Text = "Setting:";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(86, 70);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(250, 20);
            this.txtValue.TabIndex = 13;
            // 
            // EditTextSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 153);
            this.FormTitle = "Update Setting";
            this.KeyPreview = true;
            this.Name = "EditTextSettingForm";
            this.ShowInTaskbar = false;
            this.Status = "Update setting value.";
            this.Text = "EditSettingForm";
            this.Load += new System.EventHandler(this.EditTextSettingForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EditTextSettingForm_KeyUp);
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
        private System.Windows.Forms.ToolStripMenuItem mnuUpdate;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtSetting;
        private System.Windows.Forms.Label lblSetting;
        private System.Windows.Forms.TextBox txtValue;
    }
}