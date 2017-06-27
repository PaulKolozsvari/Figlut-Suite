namespace Figlut.Mobile.DataBox.UI
{
    partial class DataBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataBoxForm));
            this.mnuMain = new System.Windows.Forms.MainMenu();
            this.mnuCancel = new System.Windows.Forms.MenuItem();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.picAdd = new System.Windows.Forms.PictureBox();
            this.picUpdate = new System.Windows.Forms.PictureBox();
            this.picDelete = new System.Windows.Forms.PictureBox();
            this.picRefresh = new System.Windows.Forms.PictureBox();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.svdExport = new System.Windows.Forms.SaveFileDialog();
            this.opdImport = new System.Windows.Forms.OpenFileDialog();
            this.grdData = new System.Windows.Forms.DataGrid();
            this.picChildren = new System.Windows.Forms.PictureBox();
            this.picParent = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.Add(this.mnuCancel);
            this.mnuMain.MenuItems.Add(this.mnuOptions);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Text = "Options";
            // 
            // picAdd
            // 
            this.picAdd.Image = ((System.Drawing.Image)(resources.GetObject("picAdd.Image")));
            this.picAdd.Location = new System.Drawing.Point(18, 86);
            this.picAdd.Name = "picAdd";
            this.picAdd.Size = new System.Drawing.Size(24, 24);
            this.picAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAdd.Click += new System.EventHandler(this.picAdd_Click);
            // 
            // picUpdate
            // 
            this.picUpdate.Image = ((System.Drawing.Image)(resources.GetObject("picUpdate.Image")));
            this.picUpdate.Location = new System.Drawing.Point(48, 86);
            this.picUpdate.Name = "picUpdate";
            this.picUpdate.Size = new System.Drawing.Size(24, 24);
            this.picUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUpdate.Click += new System.EventHandler(this.picUpdate_Click);
            // 
            // picDelete
            // 
            this.picDelete.Image = ((System.Drawing.Image)(resources.GetObject("picDelete.Image")));
            this.picDelete.Location = new System.Drawing.Point(78, 86);
            this.picDelete.Name = "picDelete";
            this.picDelete.Size = new System.Drawing.Size(24, 24);
            this.picDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDelete.Click += new System.EventHandler(this.picDelete_Click);
            // 
            // picRefresh
            // 
            this.picRefresh.Image = ((System.Drawing.Image)(resources.GetObject("picRefresh.Image")));
            this.picRefresh.Location = new System.Drawing.Point(168, 86);
            this.picRefresh.Name = "picRefresh";
            this.picRefresh.Size = new System.Drawing.Size(24, 24);
            this.picRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRefresh.Click += new System.EventHandler(this.picRefresh_Click);
            // 
            // picSave
            // 
            this.picSave.Image = ((System.Drawing.Image)(resources.GetObject("picSave.Image")));
            this.picSave.Location = new System.Drawing.Point(198, 86);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(24, 24);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSave.Click += new System.EventHandler(this.picSave_Click);
            // 
            // svdExport
            // 
            this.svdExport.Filter = "CSV files|*.csv";
            // 
            // opdImport
            // 
            this.opdImport.Filter = "CSV files|*.csv";
            // 
            // grdData
            // 
            this.grdData.BackColor = System.Drawing.Color.White;
            this.grdData.BackgroundColor = System.Drawing.Color.White;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular);
            this.grdData.ForeColor = System.Drawing.Color.Black;
            this.grdData.GridLineColor = System.Drawing.Color.SteelBlue;
            this.grdData.HeaderBackColor = System.Drawing.Color.SteelBlue;
            this.grdData.HeaderForeColor = System.Drawing.Color.White;
            this.grdData.Location = new System.Drawing.Point(18, 116);
            this.grdData.Name = "grdData";
            this.grdData.PreferredRowHeight = 8;
            this.grdData.RowHeadersVisible = false;
            this.grdData.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.grdData.SelectionForeColor = System.Drawing.Color.White;
            this.grdData.Size = new System.Drawing.Size(204, 120);
            this.grdData.TabIndex = 6;
            // 
            // picChildren
            // 
            this.picChildren.Image = ((System.Drawing.Image)(resources.GetObject("picChildren.Image")));
            this.picChildren.Location = new System.Drawing.Point(108, 86);
            this.picChildren.Name = "picChildren";
            this.picChildren.Size = new System.Drawing.Size(24, 24);
            this.picChildren.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picChildren.Visible = false;
            this.picChildren.Click += new System.EventHandler(this.picChildren_Click);
            // 
            // picParent
            // 
            this.picParent.Image = ((System.Drawing.Image)(resources.GetObject("picParent.Image")));
            this.picParent.Location = new System.Drawing.Point(138, 86);
            this.picParent.Name = "picParent";
            this.picParent.Size = new System.Drawing.Size(24, 24);
            this.picParent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picParent.Visible = false;
            this.picParent.Click += new System.EventHandler(this.picParent_Click);
            // 
            // DataBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.picParent);
            this.Controls.Add(this.picChildren);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.picSave);
            this.Controls.Add(this.picRefresh);
            this.Controls.Add(this.picDelete);
            this.Controls.Add(this.picUpdate);
            this.Controls.Add(this.picAdd);
            this.KeyPreview = true;
            this.Menu = this.mnuMain;
            this.Name = "DataBoxForm";
            this.Text = "DataBox";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataBoxForm_KeyDown);
            this.Load += new System.EventHandler(this.DataBoxForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuOptions;
        private System.Windows.Forms.PictureBox picAdd;
        private System.Windows.Forms.PictureBox picUpdate;
        private System.Windows.Forms.PictureBox picDelete;
        private System.Windows.Forms.PictureBox picRefresh;
        private System.Windows.Forms.PictureBox picSave;
        private System.Windows.Forms.SaveFileDialog svdExport;
        private System.Windows.Forms.OpenFileDialog opdImport;
        private System.Windows.Forms.DataGrid grdData;
        private System.Windows.Forms.PictureBox picChildren;
        private System.Windows.Forms.PictureBox picParent;
    }
}