namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    partial class ManageFilterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageFilterForm));
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuApply = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLoadDataBox = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.cboLogicalOperator = new System.Windows.Forms.ComboBox();
            this.lblLogicalOperator = new System.Windows.Forms.Label();
            this.pnlColumnValue = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.lblColumnValue = new System.Windows.Forms.Label();
            this.cboComparisonOperator = new System.Windows.Forms.ComboBox();
            this.lblComparisonOperator = new System.Windows.Forms.Label();
            this.cboColumnName = new System.Windows.Forms.ComboBox();
            this.lblColumnName = new System.Windows.Forms.Label();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.pnlLoadDataBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.pnlLoadDataBox);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Size = new System.Drawing.Size(493, 187);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(508, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 187);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 187);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(523, 21);
            this.lblFormTitle.Text = "Filter ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ManageFilterForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ManageFilterForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManageFilterForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 208);
            this.statusMain.Size = new System.Drawing.Size(523, 21);
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuApply,
            this.mnuCancel});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MenuStripGradientBegin = System.Drawing.Color.White;
            this.mnuMain.MenuStripGradientEnd = System.Drawing.Color.White;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(493, 24);
            this.mnuMain.TabIndex = 2;
            this.mnuMain.Text = "customMainMenu1";
            // 
            // mnuApply
            // 
            this.mnuApply.Name = "mnuApply";
            this.mnuApply.Size = new System.Drawing.Size(50, 20);
            this.mnuApply.Text = "&Apply";
            this.mnuApply.Click += new System.EventHandler(this.mnuApply_Click);
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
            this.pnlLoadDataBox.Controls.Add(this.cboLogicalOperator);
            this.pnlLoadDataBox.Controls.Add(this.lblLogicalOperator);
            this.pnlLoadDataBox.Controls.Add(this.pnlColumnValue);
            this.pnlLoadDataBox.Controls.Add(this.lblColumnValue);
            this.pnlLoadDataBox.Controls.Add(this.cboComparisonOperator);
            this.pnlLoadDataBox.Controls.Add(this.lblComparisonOperator);
            this.pnlLoadDataBox.Controls.Add(this.cboColumnName);
            this.pnlLoadDataBox.Controls.Add(this.lblColumnName);
            this.pnlLoadDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoadDataBox.GradientEndColor = System.Drawing.Color.WhiteSmoke;
            this.pnlLoadDataBox.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlLoadDataBox.GradientStartColor = System.Drawing.Color.White;
            this.pnlLoadDataBox.Location = new System.Drawing.Point(0, 24);
            this.pnlLoadDataBox.Name = "pnlLoadDataBox";
            this.pnlLoadDataBox.Size = new System.Drawing.Size(493, 163);
            this.pnlLoadDataBox.TabIndex = 3;
            // 
            // cboLogicalOperator
            // 
            this.cboLogicalOperator.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboLogicalOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLogicalOperator.FormattingEnabled = true;
            this.cboLogicalOperator.Location = new System.Drawing.Point(0, 120);
            this.cboLogicalOperator.Name = "cboLogicalOperator";
            this.cboLogicalOperator.Size = new System.Drawing.Size(493, 21);
            this.cboLogicalOperator.TabIndex = 8;
            this.cboLogicalOperator.SelectedIndexChanged += new System.EventHandler(this.cboLogicalOperator_SelectedIndexChanged);
            // 
            // lblLogicalOperator
            // 
            this.lblLogicalOperator.AutoSize = true;
            this.lblLogicalOperator.BackColor = System.Drawing.Color.Transparent;
            this.lblLogicalOperator.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLogicalOperator.Location = new System.Drawing.Point(0, 107);
            this.lblLogicalOperator.Name = "lblLogicalOperator";
            this.lblLogicalOperator.Size = new System.Drawing.Size(88, 13);
            this.lblLogicalOperator.TabIndex = 7;
            this.lblLogicalOperator.Text = "Logical Operator:";
            // 
            // pnlColumnValue
            // 
            this.pnlColumnValue.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlColumnValue.BackgroundImage")));
            this.pnlColumnValue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlColumnValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlColumnValue.GradientEndColor = System.Drawing.Color.WhiteSmoke;
            this.pnlColumnValue.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlColumnValue.GradientStartColor = System.Drawing.Color.White;
            this.pnlColumnValue.Location = new System.Drawing.Point(0, 81);
            this.pnlColumnValue.Name = "pnlColumnValue";
            this.pnlColumnValue.Size = new System.Drawing.Size(493, 26);
            this.pnlColumnValue.TabIndex = 6;
            // 
            // lblColumnValue
            // 
            this.lblColumnValue.AutoSize = true;
            this.lblColumnValue.BackColor = System.Drawing.Color.Transparent;
            this.lblColumnValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblColumnValue.Location = new System.Drawing.Point(0, 68);
            this.lblColumnValue.Name = "lblColumnValue";
            this.lblColumnValue.Size = new System.Drawing.Size(75, 13);
            this.lblColumnValue.TabIndex = 5;
            this.lblColumnValue.Text = "Column Value:";
            // 
            // cboComparisonOperator
            // 
            this.cboComparisonOperator.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboComparisonOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboComparisonOperator.FormattingEnabled = true;
            this.cboComparisonOperator.Location = new System.Drawing.Point(0, 47);
            this.cboComparisonOperator.Name = "cboComparisonOperator";
            this.cboComparisonOperator.Size = new System.Drawing.Size(493, 21);
            this.cboComparisonOperator.TabIndex = 3;
            this.cboComparisonOperator.SelectedIndexChanged += new System.EventHandler(this.cboComparisonOperator_SelectedIndexChanged);
            // 
            // lblComparisonOperator
            // 
            this.lblComparisonOperator.AutoSize = true;
            this.lblComparisonOperator.BackColor = System.Drawing.Color.Transparent;
            this.lblComparisonOperator.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblComparisonOperator.Location = new System.Drawing.Point(0, 34);
            this.lblComparisonOperator.Name = "lblComparisonOperator";
            this.lblComparisonOperator.Size = new System.Drawing.Size(109, 13);
            this.lblComparisonOperator.TabIndex = 2;
            this.lblComparisonOperator.Text = "Comparison Operator:";
            // 
            // cboColumnName
            // 
            this.cboColumnName.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColumnName.FormattingEnabled = true;
            this.cboColumnName.Location = new System.Drawing.Point(0, 13);
            this.cboColumnName.Name = "cboColumnName";
            this.cboColumnName.Size = new System.Drawing.Size(493, 21);
            this.cboColumnName.TabIndex = 1;
            this.cboColumnName.SelectedIndexChanged += new System.EventHandler(this.cboColumnName_SelectedIndexChanged);
            // 
            // lblColumnName
            // 
            this.lblColumnName.AutoSize = true;
            this.lblColumnName.BackColor = System.Drawing.Color.Transparent;
            this.lblColumnName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblColumnName.Location = new System.Drawing.Point(0, 0);
            this.lblColumnName.Name = "lblColumnName";
            this.lblColumnName.Size = new System.Drawing.Size(76, 13);
            this.lblColumnName.TabIndex = 0;
            this.lblColumnName.Text = "Column Name:";
            // 
            // ManageFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 229);
            this.FormTitle = "Filter";
            this.Name = "ManageFilterForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "";
            this.Load += new System.EventHandler(this.ManageFilterForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ManageFilterForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ManageFilterForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ManageFilterForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManageFilterForm_MouseUp);
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
        private System.Windows.Forms.ToolStripMenuItem mnuApply;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private Server.Toolkit.Winforms.GradientPanel pnlLoadDataBox;
        private System.Windows.Forms.Label lblColumnName;
        private System.Windows.Forms.ComboBox cboColumnName;
        private System.Windows.Forms.ComboBox cboComparisonOperator;
        private System.Windows.Forms.Label lblComparisonOperator;
        private System.Windows.Forms.Label lblColumnValue;
        private Server.Toolkit.Winforms.GradientPanel pnlColumnValue;
        private System.Windows.Forms.ComboBox cboLogicalOperator;
        private System.Windows.Forms.Label lblLogicalOperator;
    }
}