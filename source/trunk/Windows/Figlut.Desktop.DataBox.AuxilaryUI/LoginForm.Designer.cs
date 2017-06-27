namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLoadDataBox = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.pnlLoadDataBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.pnlLoadDataBox);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlFormContent.Size = new System.Drawing.Size(640, 205);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(670, 40);
            this.pnlFormRight.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlFormRight.Size = new System.Drawing.Size(30, 205);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlFormLeft.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlFormLeft.Size = new System.Drawing.Size(30, 205);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Margin = new System.Windows.Forms.Padding(24, 0, 24, 0);
            this.lblFormTitle.Size = new System.Drawing.Size(700, 40);
            this.lblFormTitle.Text = "Login ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 245);
            this.statusMain.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.statusMain.Size = new System.Drawing.Size(700, 40);
            this.statusMain.Text = "Enter windows user name and password to login to server.";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLogin,
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.mnuMain.Size = new System.Drawing.Size(640, 47);
            this.mnuMain.TabIndex = 1;
            this.mnuMain.Text = "customMainMenu1";
            // 
            // mnuLogin
            // 
            this.mnuLogin.Name = "mnuLogin";
            this.mnuLogin.Size = new System.Drawing.Size(89, 39);
            this.mnuLogin.Text = "&Login";
            this.mnuLogin.Click += new System.EventHandler(this.mnuLogin_Click);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Name = "mnuCancel";
            this.mnuCancel.Size = new System.Drawing.Size(100, 39);
            this.mnuCancel.Text = "&Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // pnlLoadDataBox
            // 
            this.pnlLoadDataBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLoadDataBox.BackgroundImage")));
            this.pnlLoadDataBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlLoadDataBox.Controls.Add(this.txtPassword);
            this.pnlLoadDataBox.Controls.Add(this.lblPassword);
            this.pnlLoadDataBox.Controls.Add(this.txtUserName);
            this.pnlLoadDataBox.Controls.Add(this.lblUserName);
            this.pnlLoadDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoadDataBox.GradientEndColor = System.Drawing.Color.WhiteSmoke;
            this.pnlLoadDataBox.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlLoadDataBox.GradientStartColor = System.Drawing.Color.White;
            this.pnlLoadDataBox.Location = new System.Drawing.Point(0, 47);
            this.pnlLoadDataBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlLoadDataBox.Name = "pnlLoadDataBox";
            this.pnlLoadDataBox.Size = new System.Drawing.Size(640, 158);
            this.pnlLoadDataBox.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPassword.Location = new System.Drawing.Point(0, 83);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(640, 31);
            this.txtPassword.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPassword.Location = new System.Drawing.Point(0, 57);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(114, 26);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";
            // 
            // txtUserName
            // 
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtUserName.Location = new System.Drawing.Point(0, 26);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(640, 31);
            this.txtUserName.TabIndex = 1;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUserName.Location = new System.Drawing.Point(0, 0);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(129, 26);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "User Name:";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 285);
            this.FormTitle = "Login";
            this.Margin = new System.Windows.Forms.Padding(24, 23, 24, 23);
            this.Name = "LoginForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Status = "Enter windows user name and password to login to server.";
            this.Text = "LoginForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseUp);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.pnlLoadDataBox.ResumeLayout(false);
            this.pnlLoadDataBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuLogin;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private Server.Toolkit.Winforms.GradientPanel pnlLoadDataBox;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
    }
}