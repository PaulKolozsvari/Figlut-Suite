namespace Figlut.Desktop.DataBox.AuxilaryUI
{
    partial class SelectParentEntityForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mnuMain = new Figlut.Server.Toolkit.Winforms.CustomMainMenu();
            this.mnuApply = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.grdData = new Figlut.Server.Toolkit.Winforms.CustomDataGridView();
            this.pnlFormContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFormContent
            // 
            this.pnlFormContent.Controls.Add(this.grdData);
            this.pnlFormContent.Controls.Add(this.mnuMain);
            this.pnlFormContent.Size = new System.Drawing.Size(470, 258);
            // 
            // pnlFormRight
            // 
            this.pnlFormRight.Location = new System.Drawing.Point(485, 21);
            this.pnlFormRight.Size = new System.Drawing.Size(15, 258);
            // 
            // pnlFormLeft
            // 
            this.pnlFormLeft.Size = new System.Drawing.Size(15, 258);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Size = new System.Drawing.Size(500, 21);
            this.lblFormTitle.Text = "Select Parent Entity ";
            this.lblFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SelectParentEntityForm_MouseDown);
            this.lblFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectParentEntityForm_MouseMove);
            this.lblFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SelectParentEntityForm_MouseUp);
            // 
            // statusMain
            // 
            this.statusMain.Location = new System.Drawing.Point(0, 279);
            this.statusMain.Size = new System.Drawing.Size(500, 21);
            this.statusMain.Text = "Select parent entity.";
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
            this.mnuMain.Size = new System.Drawing.Size(470, 24);
            this.mnuMain.TabIndex = 159;
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
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.AllowUserToResizeRows = false;
            this.grdData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.grdData.BackgroundEndColor = System.Drawing.Color.WhiteSmoke;
            this.grdData.BackgroundStartColor = System.Drawing.Color.White;
            this.grdData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdData.DefaultCellStyle = dataGridViewCellStyle5;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdData.EnableHeadersVisualStyles = false;
            this.grdData.GridColor = System.Drawing.Color.DimGray;
            this.grdData.Location = new System.Drawing.Point(0, 24);
            this.grdData.MultiSelect = false;
            this.grdData.Name = "grdData";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdData.Size = new System.Drawing.Size(470, 234);
            this.grdData.TabIndex = 266;
            // 
            // SelectParentEntityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.FormTitle = "Select Parent Entity";
            this.Name = "SelectParentEntityForm";
            this.ShowInTaskbar = false;
            this.Status = "Select parent entity.";
            this.Text = "Select Parent Entity";
            this.Load += new System.EventHandler(this.SelectParentEntityForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SelectParentEntityForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SelectParentEntityForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectParentEntityForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SelectParentEntityForm_MouseUp);
            this.pnlFormContent.ResumeLayout(false);
            this.pnlFormContent.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Server.Toolkit.Winforms.CustomMainMenu mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuApply;
        private System.Windows.Forms.ToolStripMenuItem mnuCancel;
        private Server.Toolkit.Winforms.CustomDataGridView grdData;
    }
}