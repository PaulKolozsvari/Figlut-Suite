namespace Figlut.Mobile.Configuration.Manager.UI.AuxilaryUI
{
    partial class EditBoolSettingForm
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
            this.mnuSave = new System.Windows.Forms.MenuItem();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblSetting = new System.Windows.Forms.Label();
            this.txtSetting = new System.Windows.Forms.TextBox();
            this.chkValue = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuCancel);
            this.mnuMain.MenuItems.Add(this.mnuSave);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Text = "Save";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // lblValue
            // 
            this.lblValue.Location = new System.Drawing.Point(16, 114);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(58, 20);
            this.lblValue.Text = "Value:";
            // 
            // lblSetting
            // 
            this.lblSetting.Location = new System.Drawing.Point(16, 88);
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(58, 20);
            this.lblSetting.Text = "Setting:";
            // 
            // txtSetting
            // 
            this.txtSetting.Location = new System.Drawing.Point(80, 87);
            this.txtSetting.Name = "txtSetting";
            this.txtSetting.ReadOnly = true;
            this.txtSetting.Size = new System.Drawing.Size(144, 21);
            this.txtSetting.TabIndex = 14;
            // 
            // chkValue
            // 
            this.chkValue.Location = new System.Drawing.Point(80, 114);
            this.chkValue.Name = "chkValue";
            this.chkValue.Size = new System.Drawing.Size(144, 20);
            this.chkValue.TabIndex = 18;
            // 
            // EditBoolSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.chkValue);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblSetting);
            this.Controls.Add(this.txtSetting);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "EditBoolSettingForm";
            this.Text = "Update Setting";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBoolSettingForm_KeyDown);
            this.Load += new System.EventHandler(this.EditBoolSettingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuSave;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblSetting;
        private System.Windows.Forms.TextBox txtSetting;
        private System.Windows.Forms.CheckBox chkValue;
    }
}