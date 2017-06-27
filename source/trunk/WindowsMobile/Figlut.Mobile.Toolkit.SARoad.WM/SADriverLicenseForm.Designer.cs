namespace Figlut.Mobile.Toolkit.SARoad.WM
{
    partial class SADriverLicenseForm
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
            this.mnuBack = new System.Windows.Forms.MenuItem();
            this.mnuPhoto = new System.Windows.Forms.MenuItem();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.statusMain = new System.Windows.Forms.StatusBar();
            this.txtInitials = new System.Windows.Forms.TextBox();
            this.lblSurname = new System.Windows.Forms.Label();
            this.lblInitials = new System.Windows.Forms.Label();
            this.txtIdNumber = new System.Windows.Forms.TextBox();
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.lblValidUntil = new System.Windows.Forms.Label();
            this.txtValidUntil = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.txtGender = new System.Windows.Forms.TextBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.lblIdNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuBack);
            this.mnuMain.MenuItems.Add(this.mnuPhoto);
            // 
            // mnuBack
            // 
            this.mnuBack.Text = "Back";
            this.mnuBack.Click += new System.EventHandler(this.mnuBack_Click);
            // 
            // mnuPhoto
            // 
            this.mnuPhoto.Text = "Photo";
            this.mnuPhoto.Click += new System.EventHandler(this.mnuPhoto_Click);
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
            this.statusMain.Text = "Driver license.";
            // 
            // txtInitials
            // 
            this.txtInitials.Location = new System.Drawing.Point(104, 84);
            this.txtInitials.Name = "txtInitials";
            this.txtInitials.Size = new System.Drawing.Size(133, 21);
            this.txtInitials.TabIndex = 69;
            // 
            // lblSurname
            // 
            this.lblSurname.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblSurname.Location = new System.Drawing.Point(3, 111);
            this.lblSurname.Name = "lblSurname";
            this.lblSurname.Size = new System.Drawing.Size(95, 20);
            this.lblSurname.Text = "Surname:";
            // 
            // lblInitials
            // 
            this.lblInitials.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblInitials.Location = new System.Drawing.Point(3, 84);
            this.lblInitials.Name = "lblInitials";
            this.lblInitials.Size = new System.Drawing.Size(95, 20);
            this.lblInitials.Text = "Initials:";
            // 
            // txtIdNumber
            // 
            this.txtIdNumber.Location = new System.Drawing.Point(104, 137);
            this.txtIdNumber.Name = "txtIdNumber";
            this.txtIdNumber.Size = new System.Drawing.Size(133, 21);
            this.txtIdNumber.TabIndex = 72;
            // 
            // txtCountry
            // 
            this.txtCountry.Location = new System.Drawing.Point(104, 191);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(133, 21);
            this.txtCountry.TabIndex = 75;
            // 
            // lblValidUntil
            // 
            this.lblValidUntil.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblValidUntil.Location = new System.Drawing.Point(3, 219);
            this.lblValidUntil.Name = "lblValidUntil";
            this.lblValidUntil.Size = new System.Drawing.Size(95, 20);
            this.lblValidUntil.Text = "Valid Until:";
            // 
            // txtValidUntil
            // 
            this.txtValidUntil.Location = new System.Drawing.Point(104, 218);
            this.txtValidUntil.Name = "txtValidUntil";
            this.txtValidUntil.Size = new System.Drawing.Size(133, 21);
            this.txtValidUntil.TabIndex = 77;
            // 
            // lblGender
            // 
            this.lblGender.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblGender.Location = new System.Drawing.Point(3, 165);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(95, 20);
            this.lblGender.Text = "Gender:";
            // 
            // txtGender
            // 
            this.txtGender.Location = new System.Drawing.Point(104, 164);
            this.txtGender.Name = "txtGender";
            this.txtGender.Size = new System.Drawing.Size(133, 21);
            this.txtGender.TabIndex = 74;
            // 
            // lblCountry
            // 
            this.lblCountry.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblCountry.Location = new System.Drawing.Point(3, 192);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(95, 20);
            this.lblCountry.Text = "Country:";
            // 
            // txtSurname
            // 
            this.txtSurname.Location = new System.Drawing.Point(104, 111);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.ReadOnly = true;
            this.txtSurname.Size = new System.Drawing.Size(133, 21);
            this.txtSurname.TabIndex = 70;
            // 
            // lblIdNumber
            // 
            this.lblIdNumber.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblIdNumber.Location = new System.Drawing.Point(3, 138);
            this.lblIdNumber.Name = "lblIdNumber";
            this.lblIdNumber.Size = new System.Drawing.Size(95, 20);
            this.lblIdNumber.Text = "ID Number:";
            // 
            // SADriverLicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.txtInitials);
            this.Controls.Add(this.lblSurname);
            this.Controls.Add(this.lblInitials);
            this.Controls.Add(this.txtIdNumber);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.lblValidUntil);
            this.Controls.Add(this.txtValidUntil);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.txtGender);
            this.Controls.Add(this.lblCountry);
            this.Controls.Add(this.txtSurname);
            this.Controls.Add(this.lblIdNumber);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.picLogo);
            this.Menu = this.mnuMain;
            this.Name = "SADriverLicenseForm";
            this.Text = "Driver License";
            this.Load += new System.EventHandler(this.SADriverLicenseForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.MenuItem mnuBack;
        private System.Windows.Forms.StatusBar statusMain;
        private System.Windows.Forms.TextBox txtInitials;
        private System.Windows.Forms.Label lblSurname;
        private System.Windows.Forms.Label lblInitials;
        private System.Windows.Forms.TextBox txtIdNumber;
        private System.Windows.Forms.TextBox txtCountry;
        private System.Windows.Forms.Label lblValidUntil;
        private System.Windows.Forms.TextBox txtValidUntil;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.TextBox txtGender;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.Label lblIdNumber;
        private System.Windows.Forms.MenuItem mnuPhoto;
    }
}