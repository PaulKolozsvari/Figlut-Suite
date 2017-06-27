namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    partial class LoadDataBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadDataBoxForm));
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLoadDataBox = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.lstDataBoxTable = new Figlut.Server.Toolkit.Winforms.CustomListBox();
            this.chkApplyFilters = new System.Windows.Forms.CheckBox();
            this.txtDataBoxTable = new System.Windows.Forms.TextBox();
            this.lblDataBoxTable = new System.Windows.Forms.Label();
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
            this.pnlFormContent.Size = new System.Drawing.Size(626, 480);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(656, 40);
            this.pnlFormRight.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlFormRight.Size = new System.Drawing.Size(30, 480);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlFormLeft.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlFormLeft.Size = new System.Drawing.Size(30, 480);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Margin = new System.Windows.Forms.Padding(24, 0, 24, 0);
            this.lblFormTitle.Size = new System.Drawing.Size(686, 40);
            this.lblFormTitle.Text = "Load DataBox ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoadDataBoxForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadDataBoxForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LoadDataBoxForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 520);
            this.statusMain.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.statusMain.Size = new System.Drawing.Size(686, 40);
            this.statusMain.Text = "Select DataBox to load.";
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
            this.mnuMain.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.mnuMain.Size = new System.Drawing.Size(626, 47);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "customMainMenu1";
            // 
            // mnuLoad
            // 
            this.mnuLoad.Name = "mnuLoad";
            this.mnuLoad.Size = new System.Drawing.Size(82, 39);
            this.mnuLoad.Text = "&Load";
            this.mnuLoad.Click += new System.EventHandler(this.mnuLoad_Click);
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
            this.pnlLoadDataBox.Controls.Add(this.lstDataBoxTable);
            this.pnlLoadDataBox.Controls.Add(this.chkApplyFilters);
            this.pnlLoadDataBox.Controls.Add(this.txtDataBoxTable);
            this.pnlLoadDataBox.Controls.Add(this.lblDataBoxTable);
            this.pnlLoadDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoadDataBox.GradientEndColor = System.Drawing.Color.WhiteSmoke;
            this.pnlLoadDataBox.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlLoadDataBox.GradientStartColor = System.Drawing.Color.White;
            this.pnlLoadDataBox.Location = new System.Drawing.Point(0, 47);
            this.pnlLoadDataBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlLoadDataBox.Name = "pnlLoadDataBox";
            this.pnlLoadDataBox.Size = new System.Drawing.Size(626, 433);
            this.pnlLoadDataBox.TabIndex = 1;
            // 
            // lstDataBoxTable
            // 
            this.lstDataBoxTable.BackColor = System.Drawing.Color.White;
            this.lstDataBoxTable.BackEndColor = System.Drawing.Color.WhiteSmoke;
            this.lstDataBoxTable.BackStartColor = System.Drawing.Color.White;
            this.lstDataBoxTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDataBoxTable.FormattingEnabled = true;
            this.lstDataBoxTable.ItemHeight = 25;
            this.lstDataBoxTable.Location = new System.Drawing.Point(0, 57);
            this.lstDataBoxTable.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lstDataBoxTable.Name = "lstDataBoxTable";
            this.lstDataBoxTable.Size = new System.Drawing.Size(626, 346);
            this.lstDataBoxTable.TabIndex = 4;
            // 
            // chkApplyFilters
            // 
            this.chkApplyFilters.AutoSize = true;
            this.chkApplyFilters.BackColor = System.Drawing.Color.Transparent;
            this.chkApplyFilters.Checked = true;
            this.chkApplyFilters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkApplyFilters.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkApplyFilters.Location = new System.Drawing.Point(0, 403);
            this.chkApplyFilters.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkApplyFilters.Name = "chkApplyFilters";
            this.chkApplyFilters.Size = new System.Drawing.Size(626, 30);
            this.chkApplyFilters.TabIndex = 3;
            this.chkApplyFilters.Text = "Apply Filters";
            this.chkApplyFilters.UseVisualStyleBackColor = false;
            // 
            // txtDataBoxTable
            // 
            this.txtDataBoxTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDataBoxTable.Location = new System.Drawing.Point(0, 26);
            this.txtDataBoxTable.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtDataBoxTable.Name = "txtDataBoxTable";
            this.txtDataBoxTable.Size = new System.Drawing.Size(626, 31);
            this.txtDataBoxTable.TabIndex = 1;
            this.txtDataBoxTable.TextChanged += new System.EventHandler(this.txtDataBoxTable_TextChanged);
            // 
            // lblDataBoxTable
            // 
            this.lblDataBoxTable.AutoSize = true;
            this.lblDataBoxTable.BackColor = System.Drawing.Color.Transparent;
            this.lblDataBoxTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDataBoxTable.Location = new System.Drawing.Point(0, 0);
            this.lblDataBoxTable.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblDataBoxTable.Name = "lblDataBoxTable";
            this.lblDataBoxTable.Size = new System.Drawing.Size(161, 26);
            this.lblDataBoxTable.TabIndex = 0;
            this.lblDataBoxTable.Text = "DataBox Table:";
            // 
            // LoadDataBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 560);
            this.FormTitle = "Load DataBox";
            this.MainMenuStrip = this.mnuMain;
            this.Margin = new System.Windows.Forms.Padding(24, 23, 24, 23);
            this.Name = "LoadDataBoxForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Status = "Select DataBox to load.";
            this.Text = "Load DataBox";
            this.Load += new System.EventHandler(this.LoadDataBoxForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LoadDataBoxForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoadDataBoxForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadDataBoxForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LoadDataBoxForm_MouseUp);
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
        private System.Windows.Forms.ToolStripMenuItem mnuLoad;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private Server.Toolkit.Winforms.GradientPanel pnlLoadDataBox;
        private System.Windows.Forms.Label lblDataBoxTable;
        private System.Windows.Forms.TextBox txtDataBoxTable;
        private System.Windows.Forms.CheckBox chkApplyFilters;
        private Server.Toolkit.Winforms.CustomListBox lstDataBoxTable;
    }
}