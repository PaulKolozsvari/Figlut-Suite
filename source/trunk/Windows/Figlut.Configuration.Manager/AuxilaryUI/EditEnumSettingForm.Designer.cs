﻿namespace Figlut.Configuration.Manager.AuxilaryUI
{
    partial class EditEnumSettingForm
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
            this.cboValue = new System.Windows.Forms.ComboBox();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.lblValue);
            this.pnlFormContent.Controls.Add(this.txtSetting);
            this.pnlFormContent.Controls.Add(this.lblSetting);
            this.pnlFormContent.Controls.Add(this.cboValue);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Size = new System.Drawing.Size(357, 109);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(372, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 109);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 109);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(387, 21);
            this.lblFormTitle.Text = "Update Setting ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EditSettingForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 130);
            this.statusMain.Size = new System.Drawing.Size(387, 21);
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
            this.mnuMain.Size = new System.Drawing.Size(357, 24);
            this.mnuMain.TabIndex = 1;
            // 
            // mnuUpdate
            // 
            this.mnuUpdate.Name = "mnuUpdate";
            this.mnuUpdate.Size = new System.Drawing.Size(57, 20);
            this.mnuUpdate.Text = "&Update";
            this.mnuUpdate.Click += new System.EventHandler(this.mnuUpdate_Click);
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
            this.lblValue.Location = new System.Drawing.Point(6, 70);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(74, 23);
            this.lblValue.TabIndex = 21;
            this.lblValue.Text = "Value:";
            // 
            // txtSetting
            // 
            this.txtSetting.Location = new System.Drawing.Point(86, 38);
            this.txtSetting.Name = "txtSetting";
            this.txtSetting.ReadOnly = true;
            this.txtSetting.Size = new System.Drawing.Size(250, 20);
            this.txtSetting.TabIndex = 20;
            // 
            // lblSetting
            // 
            this.lblSetting.Location = new System.Drawing.Point(6, 38);
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(74, 23);
            this.lblSetting.TabIndex = 19;
            this.lblSetting.Text = "Setting:";
            // 
            // cboValue
            // 
            this.cboValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboValue.FormattingEnabled = true;
            this.cboValue.Location = new System.Drawing.Point(86, 67);
            this.cboValue.Name = "cboValue";
            this.cboValue.Size = new System.Drawing.Size(250, 21);
            this.cboValue.TabIndex = 18;
            // 
            // EditEnumSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 151);
            this.FormTitle = "Update Setting";
            this.KeyPreview = true;
            this.Name = "EditEnumSettingForm";
            this.ShowInTaskbar = false;
            this.Status = "Update setting value.";
            this.Text = "EditSettingForm";
            this.Load += new System.EventHandler(this.EditEnumSettingForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EditEnumSettingForm_KeyUp);
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
        private System.Windows.Forms.ComboBox cboValue;
    }
}