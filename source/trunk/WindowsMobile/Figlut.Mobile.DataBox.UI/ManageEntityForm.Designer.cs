namespace Figlut.Mobile.DataBox.UI
{
    partial class ManageEntityForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mnuMain;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuCancel = new System.Windows.Forms.MenuItem();
            this.mnuApply = new System.Windows.Forms.MenuItem();
            this.pnlInputControls = new System.Windows.Forms.Panel();
            this.lnkSelectParent = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuCancel);
            this.mnuMain.MenuItems.Add(this.mnuApply);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnuApply
            // 
            this.mnuApply.Text = "Apply";
            this.mnuApply.Click += new System.EventHandler(this.mnuApply_Click);
            // 
            // pnlInputControls
            // 
            this.pnlInputControls.AutoScroll = true;
            this.pnlInputControls.Location = new System.Drawing.Point(18, 116);
            this.pnlInputControls.Name = "pnlInputControls";
            this.pnlInputControls.Size = new System.Drawing.Size(204, 127);
            // 
            // lnkSelectParent
            // 
            this.lnkSelectParent.Location = new System.Drawing.Point(125, 93);
            this.lnkSelectParent.Name = "lnkSelectParent";
            this.lnkSelectParent.Size = new System.Drawing.Size(97, 20);
            this.lnkSelectParent.TabIndex = 4;
            this.lnkSelectParent.Text = "SELECT PARENT";
            this.lnkSelectParent.Visible = false;
            this.lnkSelectParent.Click += new System.EventHandler(this.lnkSelectParent_Click);
            // 
            // ManageEntityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lnkSelectParent);
            this.Controls.Add(this.pnlInputControls);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "ManageEntityForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManageEntityForm_KeyDown);
            this.Load += new System.EventHandler(this.ManageEntityForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuApply;
        private System.Windows.Forms.Panel pnlInputControls;
        private System.Windows.Forms.LinkLabel lnkSelectParent;
    }
}