namespace Figlut.Server.Toolkit.Mmc.SettingEditors
{
    partial class EditTextSettingControl
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
            this.txtValue = new System.Windows.Forms.TextBox();
            this.UserInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValue
            // 
            this.lblValue.Location = new System.Drawing.Point(43, 72);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(74, 23);
            this.lblValue.TabIndex = 12;
            this.lblValue.Text = "Value:";
            // 
            // txtSetting
            // 
            this.txtSetting.Location = new System.Drawing.Point(123, 40);
            this.txtSetting.Name = "txtSetting";
            this.txtSetting.ReadOnly = true;
            this.txtSetting.Size = new System.Drawing.Size(250, 20);
            this.txtSetting.TabIndex = 11;
            // 
            // lblSetting
            // 
            this.lblSetting.Location = new System.Drawing.Point(43, 40);
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(74, 23);
            this.lblSetting.TabIndex = 10;
            this.lblSetting.Text = "Setting:";
            // 
            // UserInfo
            // 
            this.UserInfo.Controls.Add(this.txtValue);
            this.UserInfo.Location = new System.Drawing.Point(19, 16);
            this.UserInfo.Name = "UserInfo";
            this.UserInfo.Size = new System.Drawing.Size(378, 89);
            this.UserInfo.TabIndex = 14;
            this.UserInfo.TabStop = false;
            this.UserInfo.Text = "Edit Setting";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(104, 53);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(250, 20);
            this.txtValue.TabIndex = 0;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // EditTextSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.txtSetting);
            this.Controls.Add(this.lblSetting);
            this.Controls.Add(this.UserInfo);
            this.Name = "EditTextSettingControl";
            this.Size = new System.Drawing.Size(416, 119);
            this.UserInfo.ResumeLayout(false);
            this.UserInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtSetting;
        private System.Windows.Forms.Label lblSetting;
        private System.Windows.Forms.GroupBox UserInfo;
        private System.Windows.Forms.TextBox txtValue;
    }
}
