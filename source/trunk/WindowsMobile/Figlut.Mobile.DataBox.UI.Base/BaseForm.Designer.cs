namespace Figlut.Mobile.DataBox.UI.Base
{
    partial class BaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.pnlLeftBorder = new System.Windows.Forms.Panel();
            this.pnlRightBorder = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.statusMain = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlLeftBorder
            // 
            this.pnlLeftBorder.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlLeftBorder.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftBorder.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftBorder.Name = "pnlLeftBorder";
            this.pnlLeftBorder.Size = new System.Drawing.Size(10, 294);
            // 
            // pnlRightBorder
            // 
            this.pnlRightBorder.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlRightBorder.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightBorder.Location = new System.Drawing.Point(230, 0);
            this.pnlRightBorder.Name = "pnlRightBorder";
            this.pnlRightBorder.Size = new System.Drawing.Size(10, 294);
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.White;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(10, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(220, 81);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // statusMain
            // 
            this.statusMain.BackColor = System.Drawing.Color.SteelBlue;
            this.statusMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusMain.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.statusMain.ForeColor = System.Drawing.Color.White;
            this.statusMain.Location = new System.Drawing.Point(10, 269);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(220, 25);
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.pnlRightBorder);
            this.Controls.Add(this.pnlLeftBorder);
            this.Name = "BaseForm";
            this.Text = "BaseForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeftBorder;
        private System.Windows.Forms.Panel pnlRightBorder;
        protected System.Windows.Forms.PictureBox picLogo;
        protected System.Windows.Forms.Label statusMain;

    }
}