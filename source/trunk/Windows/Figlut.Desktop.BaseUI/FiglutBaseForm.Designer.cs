namespace Figlut.Desktop.BaseUI
{
    partial class FiglutBaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiglutBaseForm));
            this.SuspendLayout();
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormRight.BackgroundImage")));
            this.pnlFormRight.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.pnlFormRight.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFormLeft.BackgroundImage")));
            this.pnlFormLeft.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.pnlFormLeft.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.GradientEndColor = System.Drawing.Color.SteelBlue;
            this.lblFormTitle.Text = " ";
            // 
            // statusMain
            // 
            this.statusMain.ForeColor = System.Drawing.Color.DimGray;
            this.statusMain.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.statusMain.GradientStartColor = System.Drawing.Color.Gainsboro;
            // 
            // FiglutDataBoxBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 554);
            this.Name = "FiglutDataBoxBaseForm";
            this.Text = "FiglutDataBoxBaseForm";
            this.ResumeLayout(false);

        }

        #endregion
    }
}