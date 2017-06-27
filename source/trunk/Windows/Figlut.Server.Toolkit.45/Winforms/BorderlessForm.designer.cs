namespace Figlut.Server.Toolkit.Winforms
{
    partial class BorderlessForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BorderlessForm));
            this.tmrTextAnimator = new System.Windows.Forms.Timer(this.components);
            this.tmrWaitForAnimation = new System.Windows.Forms.Timer(this.components);
            this.statusMain = new Figlut.Server.Toolkit.GradientLabel();
            this.lblFormTitle = new Figlut.Server.Toolkit.GradientLabel();
            this.pnlFormLeft = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.pnlFormRight = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.pnlFormContent = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tmrTextAnimator
            // 
            this.tmrTextAnimator.Interval = 500;
            // 
            // tmrWaitForAnimation
            // 
            this.tmrWaitForAnimation.Interval = 500;
            // 
            // statusMain
            // 
            this.statusMain.BackColor = System.Drawing.Color.DimGray;
            this.statusMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.statusMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusMain.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusMain.ForeColor = System.Drawing.Color.White;
            this.statusMain.GradientEndColor = System.Drawing.Color.White;
            this.statusMain.GradientStartColor = System.Drawing.Color.White;
            this.statusMain.Location = new System.Drawing.Point(0, 533);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(866, 21);
            this.statusMain.TabIndex = 150;
            this.statusMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.BackColor = System.Drawing.Color.DimGray;
            this.lblFormTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFormTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.GradientEndColor = System.Drawing.Color.White;
            this.lblFormTitle.GradientStartColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(0, 0);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(866, 21);
            this.lblFormTitle.TabIndex = 152;
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.BackColor = System.Drawing.Color.DimGray;
            this.pnlFormLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormLeft.BackgroundImage")));
            this.pnlFormLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlFormLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFormLeft.GradientEndColor = System.Drawing.Color.White;
            this.pnlFormLeft.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlFormLeft.GradientStartColor = System.Drawing.Color.White;
            this.pnlFormLeft.Location = new System.Drawing.Point(0, 21);
            this.pnlFormLeft.Name = "pnlFormLeft";
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 512);
            this.pnlFormLeft.TabIndex = 155;
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.BackColor = System.Drawing.Color.DimGray;
            this.pnlFormRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormRight.BackgroundImage")));
            this.pnlFormRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlFormRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFormRight.GradientEndColor = System.Drawing.Color.White;
            this.pnlFormRight.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlFormRight.GradientStartColor = System.Drawing.Color.White;
            this.pnlFormRight.Location = new System.Drawing.Point(851, 21);
            this.pnlFormRight.Name = "pnlFormRight";
            this.pnlFormRight.Size = new System.Drawing.Size(15, 512);
            this.pnlFormRight.TabIndex = 156;
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFormContent.Location = new System.Drawing.Point(15, 21);
            this.pnlFormContent.Name = "pnlFormContent";
            this.pnlFormContent.Size = new System.Drawing.Size(836, 512);
            this.pnlFormContent.TabIndex = 157;
            // 
            // BorderlessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(866, 554);
            this.Controls.Add(this.pnlFormContent);
            this.Controls.Add(this.pnlFormRight);
            this.Controls.Add(this.pnlFormLeft);
            this.Controls.Add(this.lblFormTitle);
            this.Controls.Add(this.statusMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BorderlessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BorderlessForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BorderlessForm_FormClosing);
            this.Load += new System.EventHandler(this.BorderlessForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BorderlessForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BorderlessForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BorderlessForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrTextAnimator;
        private System.Windows.Forms.Timer tmrWaitForAnimation;
        protected System.Windows.Forms.Panel pnlFormContent;
        public GradientPanel pnlFormRight;
        public GradientPanel pnlFormLeft;
        public GradientLabel lblFormTitle;
        public GradientLabel statusMain;
    }
}