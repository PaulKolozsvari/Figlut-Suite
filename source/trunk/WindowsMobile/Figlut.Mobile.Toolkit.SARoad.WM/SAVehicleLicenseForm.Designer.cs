namespace Figlut.Mobile.Toolkit.SARoad.WM
{
    partial class SAVehicleLicenseForm
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
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.statusMain = new System.Windows.Forms.StatusBar();
            this.mnuBack = new System.Windows.Forms.MenuItem();
            this.txtVIN = new System.Windows.Forms.TextBox();
            this.lblVIN = new System.Windows.Forms.Label();
            this.txtColour = new System.Windows.Forms.TextBox();
            this.lblColour = new System.Windows.Forms.Label();
            this.txtMake = new System.Windows.Forms.TextBox();
            this.lblMake = new System.Windows.Forms.Label();
            this.txtRegistration = new System.Windows.Forms.TextBox();
            this.lblRegistration = new System.Windows.Forms.Label();
            this.txtLicenseNumber = new System.Windows.Forms.TextBox();
            this.lblLicenseNumber = new System.Windows.Forms.Label();
            this.txtEngineNumber = new System.Windows.Forms.TextBox();
            this.lblEngineNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuBack);
            // 
            // picLogo
            // 
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(240, 78);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 246);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(240, 22);
            this.statusMain.Text = "Vehicle license.";
            // 
            // mnuBack
            // 
            this.mnuBack.Text = "Back";
            this.mnuBack.Click += new System.EventHandler(this.mnuBack_Click);
            // 
            // txtVIN
            // 
            this.txtVIN.Location = new System.Drawing.Point(104, 165);
            this.txtVIN.Name = "txtVIN";
            this.txtVIN.Size = new System.Drawing.Size(133, 21);
            this.txtVIN.TabIndex = 50;
            // 
            // lblVIN
            // 
            this.lblVIN.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblVIN.Location = new System.Drawing.Point(3, 165);
            this.lblVIN.Name = "lblVIN";
            this.lblVIN.Size = new System.Drawing.Size(95, 20);
            this.lblVIN.Text = "VIN:";
            // 
            // txtColour
            // 
            this.txtColour.Location = new System.Drawing.Point(104, 137);
            this.txtColour.Name = "txtColour";
            this.txtColour.Size = new System.Drawing.Size(133, 21);
            this.txtColour.TabIndex = 49;
            // 
            // lblColour
            // 
            this.lblColour.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblColour.Location = new System.Drawing.Point(3, 138);
            this.lblColour.Name = "lblColour";
            this.lblColour.Size = new System.Drawing.Size(95, 20);
            this.lblColour.Text = "Colour:";
            // 
            // txtMake
            // 
            this.txtMake.Location = new System.Drawing.Point(104, 110);
            this.txtMake.Name = "txtMake";
            this.txtMake.Size = new System.Drawing.Size(133, 21);
            this.txtMake.TabIndex = 48;
            // 
            // lblMake
            // 
            this.lblMake.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblMake.Location = new System.Drawing.Point(3, 111);
            this.lblMake.Name = "lblMake";
            this.lblMake.Size = new System.Drawing.Size(95, 20);
            this.lblMake.Text = "Make:";
            // 
            // txtRegistration
            // 
            this.txtRegistration.Location = new System.Drawing.Point(104, 83);
            this.txtRegistration.Name = "txtRegistration";
            this.txtRegistration.Size = new System.Drawing.Size(133, 21);
            this.txtRegistration.TabIndex = 47;
            // 
            // lblRegistration
            // 
            this.lblRegistration.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblRegistration.Location = new System.Drawing.Point(3, 84);
            this.lblRegistration.Name = "lblRegistration";
            this.lblRegistration.Size = new System.Drawing.Size(95, 20);
            this.lblRegistration.Text = "Registration:";
            // 
            // txtLicenseNumber
            // 
            this.txtLicenseNumber.Location = new System.Drawing.Point(104, 192);
            this.txtLicenseNumber.Name = "txtLicenseNumber";
            this.txtLicenseNumber.Size = new System.Drawing.Size(133, 21);
            this.txtLicenseNumber.TabIndex = 56;
            // 
            // lblLicenseNumber
            // 
            this.lblLicenseNumber.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblLicenseNumber.Location = new System.Drawing.Point(3, 193);
            this.lblLicenseNumber.Name = "lblLicenseNumber";
            this.lblLicenseNumber.Size = new System.Drawing.Size(95, 20);
            this.lblLicenseNumber.Text = "License Number:";
            // 
            // txtEngineNumber
            // 
            this.txtEngineNumber.Location = new System.Drawing.Point(104, 219);
            this.txtEngineNumber.Name = "txtEngineNumber";
            this.txtEngineNumber.Size = new System.Drawing.Size(133, 21);
            this.txtEngineNumber.TabIndex = 59;
            // 
            // lblEngineNumber
            // 
            this.lblEngineNumber.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblEngineNumber.Location = new System.Drawing.Point(3, 220);
            this.lblEngineNumber.Name = "lblEngineNumber";
            this.lblEngineNumber.Size = new System.Drawing.Size(95, 20);
            this.lblEngineNumber.Text = "Engine Number:";
            // 
            // SAVehicleLicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.txtEngineNumber);
            this.Controls.Add(this.lblEngineNumber);
            this.Controls.Add(this.txtLicenseNumber);
            this.Controls.Add(this.lblLicenseNumber);
            this.Controls.Add(this.txtVIN);
            this.Controls.Add(this.lblVIN);
            this.Controls.Add(this.txtColour);
            this.Controls.Add(this.lblColour);
            this.Controls.Add(this.txtMake);
            this.Controls.Add(this.lblMake);
            this.Controls.Add(this.txtRegistration);
            this.Controls.Add(this.lblRegistration);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.picLogo);
            this.Menu = this.mnuMain;
            this.Name = "SAVehicleLicenseForm";
            this.Text = "Vehicle License";
            this.Load += new System.EventHandler(this.SAVehicleLicenseForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.StatusBar statusMain;
        private System.Windows.Forms.MenuItem mnuBack;
        private System.Windows.Forms.TextBox txtVIN;
        private System.Windows.Forms.Label lblVIN;
        private System.Windows.Forms.TextBox txtColour;
        private System.Windows.Forms.Label lblColour;
        private System.Windows.Forms.TextBox txtMake;
        private System.Windows.Forms.Label lblMake;
        private System.Windows.Forms.TextBox txtRegistration;
        private System.Windows.Forms.Label lblRegistration;
        private System.Windows.Forms.TextBox txtLicenseNumber;
        private System.Windows.Forms.Label lblLicenseNumber;
        private System.Windows.Forms.TextBox txtEngineNumber;
        private System.Windows.Forms.Label lblEngineNumber;
    }
}