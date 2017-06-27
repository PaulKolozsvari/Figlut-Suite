namespace OneStopShop.FiglutExtensions.Biometric
{
    partial class DeviceDescriptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceDescriptionForm));
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLoadDataBox = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.gpbSoftware = new System.Windows.Forms.GroupBox();
            this.lblSoftware1 = new System.Windows.Forms.Label();
            this.gpbSensor = new System.Windows.Forms.GroupBox();
            this.lblSensor1 = new System.Windows.Forms.Label();
            this.lblSensor2 = new System.Windows.Forms.Label();
            this.gpbProduct = new System.Windows.Forms.GroupBox();
            this.lblProduct1 = new System.Windows.Forms.Label();
            this.lblProduct2 = new System.Windows.Forms.Label();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.pnlLoadDataBox.SuspendLayout();
            this.gpbSoftware.SuspendLayout();
            this.gpbSensor.SuspendLayout();
            this.gpbProduct.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.pnlLoadDataBox);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Size = new System.Drawing.Size(319, 188);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(334, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 188);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 188);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(349, 21);
            this.lblFormTitle.Text = "Device Description ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DeviceDescriptionForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DeviceDescriptionForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DeviceDescriptionForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 209);
            this.statusMain.Size = new System.Drawing.Size(349, 21);
            this.statusMain.Text = "Description of the currently connected fingerprint device.";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(319, 24);
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
            // pnlLoadDataBox
            // 
            this.pnlLoadDataBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLoadDataBox.BackgroundImage")));
            this.pnlLoadDataBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlLoadDataBox.Controls.Add(this.gpbSoftware);
            this.pnlLoadDataBox.Controls.Add(this.gpbSensor);
            this.pnlLoadDataBox.Controls.Add(this.gpbProduct);
            this.pnlLoadDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoadDataBox.GradientEndColor = System.Drawing.Color.WhiteSmoke;
            this.pnlLoadDataBox.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlLoadDataBox.GradientStartColor = System.Drawing.Color.White;
            this.pnlLoadDataBox.Location = new System.Drawing.Point(0, 24);
            this.pnlLoadDataBox.Name = "pnlLoadDataBox";
            this.pnlLoadDataBox.Size = new System.Drawing.Size(319, 164);
            this.pnlLoadDataBox.TabIndex = 160;
            // 
            // gpbSoftware
            // 
            this.gpbSoftware.AutoSize = true;
            this.gpbSoftware.BackColor = System.Drawing.Color.Transparent;
            this.gpbSoftware.Controls.Add(this.lblSoftware1);
            this.gpbSoftware.Location = new System.Drawing.Point(6, 106);
            this.gpbSoftware.Name = "gpbSoftware";
            this.gpbSoftware.Size = new System.Drawing.Size(304, 45);
            this.gpbSoftware.TabIndex = 7;
            this.gpbSoftware.TabStop = false;
            this.gpbSoftware.Text = "Software Descriptor";
            // 
            // lblSoftware1
            // 
            this.lblSoftware1.AutoSize = true;
            this.lblSoftware1.Location = new System.Drawing.Point(6, 16);
            this.lblSoftware1.Name = "lblSoftware1";
            this.lblSoftware1.Size = new System.Drawing.Size(0, 13);
            this.lblSoftware1.TabIndex = 5;
            // 
            // gpbSensor
            // 
            this.gpbSensor.AutoSize = true;
            this.gpbSensor.BackColor = System.Drawing.Color.Transparent;
            this.gpbSensor.Controls.Add(this.lblSensor1);
            this.gpbSensor.Controls.Add(this.lblSensor2);
            this.gpbSensor.Location = new System.Drawing.Point(6, 56);
            this.gpbSensor.Name = "gpbSensor";
            this.gpbSensor.Size = new System.Drawing.Size(304, 45);
            this.gpbSensor.TabIndex = 6;
            this.gpbSensor.TabStop = false;
            this.gpbSensor.Text = "Sensor Descriptor";
            // 
            // lblSensor1
            // 
            this.lblSensor1.AutoSize = true;
            this.lblSensor1.Location = new System.Drawing.Point(6, 16);
            this.lblSensor1.Name = "lblSensor1";
            this.lblSensor1.Size = new System.Drawing.Size(0, 13);
            this.lblSensor1.TabIndex = 5;
            // 
            // lblSensor2
            // 
            this.lblSensor2.AutoSize = true;
            this.lblSensor2.Location = new System.Drawing.Point(148, 16);
            this.lblSensor2.Name = "lblSensor2";
            this.lblSensor2.Size = new System.Drawing.Size(0, 13);
            this.lblSensor2.TabIndex = 6;
            // 
            // gpbProduct
            // 
            this.gpbProduct.AutoSize = true;
            this.gpbProduct.BackColor = System.Drawing.Color.Transparent;
            this.gpbProduct.Controls.Add(this.lblProduct1);
            this.gpbProduct.Controls.Add(this.lblProduct2);
            this.gpbProduct.Location = new System.Drawing.Point(6, 3);
            this.gpbProduct.Name = "gpbProduct";
            this.gpbProduct.Size = new System.Drawing.Size(304, 45);
            this.gpbProduct.TabIndex = 5;
            this.gpbProduct.TabStop = false;
            this.gpbProduct.Text = "Product Descriptor";
            // 
            // lblProduct1
            // 
            this.lblProduct1.AutoSize = true;
            this.lblProduct1.Location = new System.Drawing.Point(6, 16);
            this.lblProduct1.Name = "lblProduct1";
            this.lblProduct1.Size = new System.Drawing.Size(0, 13);
            this.lblProduct1.TabIndex = 0;
            // 
            // lblProduct2
            // 
            this.lblProduct2.AutoSize = true;
            this.lblProduct2.Location = new System.Drawing.Point(148, 16);
            this.lblProduct2.Name = "lblProduct2";
            this.lblProduct2.Size = new System.Drawing.Size(0, 13);
            this.lblProduct2.TabIndex = 1;
            // 
            // DeviceDescriptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 230);
            this.FormTitle = "Device Description";
            this.KeyPreview = true;
            this.Name = "DeviceDescriptionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Status = "Description of the currently connected fingerprint device.";
            this.Text = "";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DeviceDescriptionForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DeviceDescriptionForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DeviceDescriptionForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DeviceDescriptionForm_MouseUp);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.pnlLoadDataBox.ResumeLayout(false);
            this.pnlLoadDataBox.PerformLayout();
            this.gpbSoftware.ResumeLayout(false);
            this.gpbSoftware.PerformLayout();
            this.gpbSensor.ResumeLayout(false);
            this.gpbSensor.PerformLayout();
            this.gpbProduct.ResumeLayout(false);
            this.gpbProduct.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Figlut.Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private Figlut.Server.Toolkit.Winforms.GradientPanel pnlLoadDataBox;
        private System.Windows.Forms.GroupBox gpbSoftware;
        private System.Windows.Forms.Label lblSoftware1;
        private System.Windows.Forms.GroupBox gpbSensor;
        private System.Windows.Forms.Label lblSensor1;
        private System.Windows.Forms.Label lblSensor2;
        private System.Windows.Forms.GroupBox gpbProduct;
        private System.Windows.Forms.Label lblProduct1;
        private System.Windows.Forms.Label lblProduct2;
    }
}