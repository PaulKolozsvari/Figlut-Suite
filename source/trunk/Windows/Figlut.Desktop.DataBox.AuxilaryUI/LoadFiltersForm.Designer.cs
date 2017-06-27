namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    partial class LoadFiltersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadFiltersForm));
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripData = new Figlut.Server.Toolkit.Winforms.CustomToolStrip();
            this.tsAdd = new System.Windows.Forms.ToolStripButton();
            this.tsUpdate = new System.Windows.Forms.ToolStripButton();
            this.tsDelete = new System.Windows.Forms.ToolStripButton();
            this.pnlLoadDataBox = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.lstFilters = new Figlut.Server.Toolkit.Winforms.CustomListBox();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.toolStripData.SuspendLayout();
            this.pnlLoadDataBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.pnlLoadDataBox);
            this.pnlFormContent.Controls.Add(this.toolStripData);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Size = new System.Drawing.Size(716, 249);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(731, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 249);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 249);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(746, 21);
            this.lblFormTitle.Text = "Select Filters ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoadFiltersForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadFiltersForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LoadFiltersForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 270);
            this.statusMain.Size = new System.Drawing.Size(746, 21);
            this.statusMain.Text = "Add filters to be applied when loading the DataBox.";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLoad,
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(716, 24);
            this.mnuMain.TabIndex = 1;
            this.mnuMain.Text = "customMainMenu1";
            // 
            // mnuLoad
            // 
            this.mnuLoad.Name = "mnuLoad";
            this.mnuLoad.Size = new System.Drawing.Size(45, 20);
            this.mnuLoad.Text = "&Load";
            this.mnuLoad.Click += new System.EventHandler(this.mnuLoad_Click);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Name = "mnuCancel";
            this.mnuCancel.Size = new System.Drawing.Size(55, 20);
            this.mnuCancel.Text = "&Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // toolStripData
            // 
            this.toolStripData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAdd,
            this.tsUpdate,
            this.tsDelete});
            this.toolStripData.Location = new System.Drawing.Point(0, 24);
            this.toolStripData.Name = "toolStripData";
            this.toolStripData.Size = new System.Drawing.Size(716, 25);
            this.toolStripData.TabIndex = 6;
            this.toolStripData.Text = "customToolStrip1";
            this.toolStripData.ToolStripGradientBegin = System.Drawing.Color.White;
            this.toolStripData.ToolStripGradientEnd = System.Drawing.Color.WhiteSmoke;
            this.toolStripData.ToolStripGradientMiddle = System.Drawing.Color.Gainsboro;
            // 
            // tsAdd
            // 
            this.tsAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsAdd.Image")));
            this.tsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsAdd.Name = "tsAdd";
            this.tsAdd.Size = new System.Drawing.Size(23, 22);
            this.tsAdd.Text = "Add";
            this.tsAdd.Click += new System.EventHandler(this.tsAdd_Click);
            // 
            // tsUpdate
            // 
            this.tsUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsUpdate.Image = ((System.Drawing.Image)(resources.GetObject("tsUpdate.Image")));
            this.tsUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUpdate.Name = "tsUpdate";
            this.tsUpdate.Size = new System.Drawing.Size(23, 22);
            this.tsUpdate.Text = "Update";
            this.tsUpdate.Click += new System.EventHandler(this.tsUpdate_Click);
            // 
            // tsDelete
            // 
            this.tsDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsDelete.Image")));
            this.tsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDelete.Name = "tsDelete";
            this.tsDelete.Size = new System.Drawing.Size(23, 22);
            this.tsDelete.Text = "Delete";
            this.tsDelete.Click += new System.EventHandler(this.tsDelete_Click);
            // 
            // pnlLoadDataBox
            // 
            this.pnlLoadDataBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLoadDataBox.BackgroundImage")));
            this.pnlLoadDataBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlLoadDataBox.Controls.Add(this.lstFilters);
            this.pnlLoadDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoadDataBox.GradientEndColor = System.Drawing.Color.WhiteSmoke;
            this.pnlLoadDataBox.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlLoadDataBox.GradientStartColor = System.Drawing.Color.White;
            this.pnlLoadDataBox.Location = new System.Drawing.Point(0, 49);
            this.pnlLoadDataBox.Name = "pnlLoadDataBox";
            this.pnlLoadDataBox.Size = new System.Drawing.Size(716, 200);
            this.pnlLoadDataBox.TabIndex = 7;
            // 
            // lstFilters
            // 
            this.lstFilters.BackColor = System.Drawing.Color.White;
            this.lstFilters.BackEndColor = System.Drawing.Color.WhiteSmoke;
            this.lstFilters.BackStartColor = System.Drawing.Color.White;
            this.lstFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFilters.FormattingEnabled = true;
            this.lstFilters.Location = new System.Drawing.Point(0, 0);
            this.lstFilters.Name = "lstFilters";
            this.lstFilters.Size = new System.Drawing.Size(716, 200);
            this.lstFilters.TabIndex = 4;
            // 
            // LoadFiltersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 291);
            this.FormTitle = "Select Filters";
            this.Name = "LoadFiltersForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Status = "Add filters to be applied when loading the DataBox.";
            this.Text = "Select Filters";
            this.Load += new System.EventHandler(this.LoadFiltersForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LoadFiltersForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoadFiltersForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadFiltersForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LoadFiltersForm_MouseUp);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.toolStripData.ResumeLayout(false);
            this.toolStripData.PerformLayout();
            this.pnlLoadDataBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuLoad;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private Server.Toolkit.Winforms.CustomToolStrip toolStripData;
        private System.Windows.Forms.ToolStripButton tsAdd;
        private System.Windows.Forms.ToolStripButton tsUpdate;
        private System.Windows.Forms.ToolStripButton tsDelete;
        private Server.Toolkit.Winforms.GradientPanel pnlLoadDataBox;
        private Server.Toolkit.Winforms.CustomListBox lstFilters;
    }
}