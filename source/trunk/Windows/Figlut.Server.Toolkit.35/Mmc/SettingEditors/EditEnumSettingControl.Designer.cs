namespace Figlut.Server.Toolkit.Mmc.SettingEditors
{
    partial class EditEnumSettingControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblValue = new System.Windows.Forms.Label();
            this.txtSetting = new System.Windows.Forms.TextBox();
            this.lblSetting = new System.Windows.Forms.Label();
            this.UserInfo = new System.Windows.Forms.GroupBox();
            this.cboValue = new System.Windows.Forms.ComboBox();
            this.UserInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValue
            // 
            this.lblValue.Location = new System.Drawing.Point(43, 72);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(74, 23);
            this.lblValue.TabIndex = 17;
            this.lblValue.Text = "Value:";
            // 
            // txtSetting
            // 
            this.txtSetting.Location = new System.Drawing.Point(123, 40);
            this.txtSetting.Name = "txtSetting";
            this.txtSetting.ReadOnly = true;
            this.txtSetting.Size = new System.Drawing.Size(250, 20);
            this.txtSetting.TabIndex = 16;
            // 
            // lblSetting
            // 
            this.lblSetting.Location = new System.Drawing.Point(43, 40);
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(74, 23);
            this.lblSetting.TabIndex = 15;
            this.lblSetting.Text = "Setting:";
            // 
            // UserInfo
            // 
            this.UserInfo.Controls.Add(this.cboValue);
            this.UserInfo.Location = new System.Drawing.Point(19, 16);
            this.UserInfo.Name = "UserInfo";
            this.UserInfo.Size = new System.Drawing.Size(378, 89);
            this.UserInfo.TabIndex = 19;
            this.UserInfo.TabStop = false;
            this.UserInfo.Text = "Edit Setting";
            // 
            // cboValue
            // 
            this.cboValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboValue.FormattingEnabled = true;
            this.cboValue.Location = new System.Drawing.Point(104, 53);
            this.cboValue.Name = "cboValue";
            this.cboValue.Size = new System.Drawing.Size(250, 21);
            this.cboValue.TabIndex = 0;
            this.cboValue.SelectedIndexChanged += new System.EventHandler(this.cboValue_SelectedIndexChanged);
            // 
            // EditEnumSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.txtSetting);
            this.Controls.Add(this.lblSetting);
            this.Controls.Add(this.UserInfo);
            this.Name = "EditEnumSettingControl";
            this.Size = new System.Drawing.Size(416, 119);
            this.UserInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtSetting;
        private System.Windows.Forms.Label lblSetting;
        private System.Windows.Forms.GroupBox UserInfo;
        private System.Windows.Forms.ComboBox cboValue;
    }
}
