namespace Figlut.Desktop.DataBox.Controls
{
    partial class DataBoxControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataBoxControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.svdExport = new System.Windows.Forms.SaveFileDialog();
            this.opdImport = new System.Windows.Forms.OpenFileDialog();
            this.svdExportItem = new System.Windows.Forms.SaveFileDialog();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.btnTimesheetAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.splTimesheet = new System.Windows.Forms.SplitContainer();
            this.pnlDataInputs = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.tabData = new Figlut.Server.Toolkit.Winforms.CustomTab();
            this.tabPageDataInput = new System.Windows.Forms.TabPage();
            this.pnlDataInput = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.pnlDataBox = new Figlut.Server.Toolkit.Winforms.GradientPanel();
            this.grdData = new Figlut.Server.Toolkit.Winforms.CustomDataGridView();
            this.lblData = new System.Windows.Forms.Label();
            this.toolStripData = new Figlut.Server.Toolkit.Winforms.CustomToolStrip();
            this.tsAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsUpdate = new System.Windows.Forms.ToolStripButton();
            this.tsUpdateCommit = new System.Windows.Forms.ToolStripButton();
            this.tsUpdateCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsChildren = new System.Windows.Forms.ToolStripButton();
            this.tsParent = new System.Windows.Forms.ToolStripButton();
            this.tsSelectParent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFiltersEnable = new System.Windows.Forms.ToolStripButton();
            this.tsFiltersDisable = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.tsExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExtensionsMainMenu = new System.Windows.Forms.ToolStripDropDownButton();
            ((System.ComponentModel.ISupportInitialize)(this.splTimesheet)).BeginInit();
            this.splTimesheet.Panel1.SuspendLayout();
            this.splTimesheet.Panel2.SuspendLayout();
            this.splTimesheet.SuspendLayout();
            this.pnlDataInputs.SuspendLayout();
            this.tabData.SuspendLayout();
            this.tabPageDataInput.SuspendLayout();
            this.pnlDataBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.toolStripData.SuspendLayout();
            this.SuspendLayout();
            // 
            // svdExport
            // 
            this.svdExport.DefaultExt = "csv";
            this.svdExport.Filter = "Csv Files|*.csv";
            // 
            // opdImport
            // 
            this.opdImport.DefaultExt = "csv";
            this.opdImport.Filter = "Csv Files|*.csv";
            // 
            // svdExportItem
            // 
            this.svdExportItem.DefaultExt = "pdf";
            this.svdExportItem.Filter = "PDF Files|*.pdf";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.BackColor = System.Drawing.Color.Transparent;
            this.checkBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox2.Enabled = false;
            this.checkBox2.ForeColor = System.Drawing.Color.Black;
            this.checkBox2.Location = new System.Drawing.Point(137, 0);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(47, 27);
            this.checkBox2.TabIndex = 426;
            this.checkBox2.Text = "Paid";
            this.checkBox2.UseVisualStyleBackColor = false;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.BackColor = System.Drawing.Color.Transparent;
            this.checkBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox3.Enabled = false;
            this.checkBox3.ForeColor = System.Drawing.Color.Black;
            this.checkBox3.Location = new System.Drawing.Point(62, 0);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(75, 27);
            this.checkBox3.TabIndex = 425;
            this.checkBox3.Text = "Sunk Cost";
            this.checkBox3.UseVisualStyleBackColor = false;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.BackColor = System.Drawing.Color.Transparent;
            this.checkBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox4.ForeColor = System.Drawing.Color.Black;
            this.checkBox4.Location = new System.Drawing.Point(0, 0);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(62, 27);
            this.checkBox4.TabIndex = 424;
            this.checkBox4.Text = "Billable ";
            this.checkBox4.UseVisualStyleBackColor = false;
            // 
            // btnTimesheetAdd
            // 
            this.btnTimesheetAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTimesheetAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnTimesheetAdd.Image")));
            this.btnTimesheetAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTimesheetAdd.Name = "btnTimesheetAdd";
            this.btnTimesheetAdd.Size = new System.Drawing.Size(23, 22);
            this.btnTimesheetAdd.Text = "Add";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // splTimesheet
            // 
            this.splTimesheet.BackColor = System.Drawing.Color.WhiteSmoke;
            this.splTimesheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splTimesheet.Location = new System.Drawing.Point(0, 25);
            this.splTimesheet.Margin = new System.Windows.Forms.Padding(6);
            this.splTimesheet.Name = "splTimesheet";
            // 
            // splTimesheet.Panel1
            // 
            this.splTimesheet.Panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.splTimesheet.Panel1.Controls.Add(this.pnlDataInputs);
            this.splTimesheet.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splTimesheet.Panel2
            // 
            this.splTimesheet.Panel2.Controls.Add(this.pnlDataBox);
            this.splTimesheet.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splTimesheet.Size = new System.Drawing.Size(1380, 629);
            this.splTimesheet.SplitterDistance = 458;
            this.splTimesheet.SplitterWidth = 8;
            this.splTimesheet.TabIndex = 2;
            // 
            // pnlDataInputs
            // 
            this.pnlDataInputs.AutoScroll = true;
            this.pnlDataInputs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlDataInputs.BackgroundImage")));
            this.pnlDataInputs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlDataInputs.Controls.Add(this.tabData);
            this.pnlDataInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataInputs.GradientEndColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDataInputs.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlDataInputs.GradientStartColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDataInputs.Location = new System.Drawing.Point(0, 0);
            this.pnlDataInputs.Margin = new System.Windows.Forms.Padding(6);
            this.pnlDataInputs.Name = "pnlDataInputs";
            this.pnlDataInputs.Size = new System.Drawing.Size(458, 629);
            this.pnlDataInputs.TabIndex = 267;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.tabPageDataInput);
            this.tabData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabData.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabData.HotTrack = true;
            this.tabData.Location = new System.Drawing.Point(0, 0);
            this.tabData.Margin = new System.Windows.Forms.Padding(6);
            this.tabData.Name = "tabData";
            this.tabData.SelectedBackEndColor = System.Drawing.Color.Gainsboro;
            this.tabData.SelectedBackStartColor = System.Drawing.Color.WhiteSmoke;
            this.tabData.SelectedForeBrushColor = System.Drawing.Color.DimGray;
            this.tabData.SelectedIndex = 0;
            this.tabData.Size = new System.Drawing.Size(458, 629);
            this.tabData.TabIndex = 378;
            this.tabData.UnselectedBackEndColor = System.Drawing.Color.WhiteSmoke;
            this.tabData.UnselectedBackStartColor = System.Drawing.Color.Gainsboro;
            this.tabData.UnselectedForeBrushColor = System.Drawing.Color.DimGray;
            // 
            // tabPageDataInput
            // 
            this.tabPageDataInput.BackColor = System.Drawing.Color.LightGray;
            this.tabPageDataInput.Controls.Add(this.pnlDataInput);
            this.tabPageDataInput.Location = new System.Drawing.Point(4, 34);
            this.tabPageDataInput.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageDataInput.Name = "tabPageDataInput";
            this.tabPageDataInput.Padding = new System.Windows.Forms.Padding(6);
            this.tabPageDataInput.Size = new System.Drawing.Size(450, 591);
            this.tabPageDataInput.TabIndex = 2;
            this.tabPageDataInput.Text = "Input";
            this.tabPageDataInput.UseVisualStyleBackColor = true;
            // 
            // pnlDataInput
            // 
            this.pnlDataInput.AutoScroll = true;
            this.pnlDataInput.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlDataInput.BackgroundImage")));
            this.pnlDataInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlDataInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDataInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataInput.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.pnlDataInput.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlDataInput.GradientStartColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDataInput.Location = new System.Drawing.Point(6, 6);
            this.pnlDataInput.Margin = new System.Windows.Forms.Padding(6);
            this.pnlDataInput.Name = "pnlDataInput";
            this.pnlDataInput.Size = new System.Drawing.Size(438, 579);
            this.pnlDataInput.TabIndex = 1;
            // 
            // pnlDataBox
            // 
            this.pnlDataBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlDataBox.BackgroundImage")));
            this.pnlDataBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlDataBox.Controls.Add(this.grdData);
            this.pnlDataBox.Controls.Add(this.lblData);
            this.pnlDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataBox.GradientEndColor = System.Drawing.Color.Gainsboro;
            this.pnlDataBox.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.pnlDataBox.GradientStartColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDataBox.Location = new System.Drawing.Point(0, 0);
            this.pnlDataBox.Margin = new System.Windows.Forms.Padding(6);
            this.pnlDataBox.Name = "pnlDataBox";
            this.pnlDataBox.Size = new System.Drawing.Size(914, 629);
            this.pnlDataBox.TabIndex = 0;
            // 
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.AllowUserToResizeRows = false;
            this.grdData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.grdData.BackgroundEndColor = System.Drawing.Color.Gainsboro;
            this.grdData.BackgroundGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.grdData.BackgroundStartColor = System.Drawing.Color.WhiteSmoke;
            this.grdData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdData.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdData.EnableHeadersVisualStyles = false;
            this.grdData.GridColor = System.Drawing.Color.DimGray;
            this.grdData.Location = new System.Drawing.Point(0, 26);
            this.grdData.Margin = new System.Windows.Forms.Padding(6);
            this.grdData.MultiSelect = false;
            this.grdData.Name = "grdData";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdData.Size = new System.Drawing.Size(914, 603);
            this.grdData.TabIndex = 265;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.BackColor = System.Drawing.Color.Transparent;
            this.lblData.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblData.ForeColor = System.Drawing.Color.Black;
            this.lblData.Location = new System.Drawing.Point(0, 0);
            this.lblData.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(64, 26);
            this.lblData.TabIndex = 264;
            this.lblData.Text = "Data:";
            // 
            // toolStripData
            // 
            this.toolStripData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAdd,
            this.toolStripSeparator1,
            this.tsUpdate,
            this.tsUpdateCommit,
            this.tsUpdateCancel,
            this.toolStripSeparator2,
            this.tsDelete,
            this.toolStripSeparator3,
            this.tsChildren,
            this.tsParent,
            this.tsSelectParent,
            this.toolStripSeparator9,
            this.tsFiltersEnable,
            this.tsFiltersDisable,
            this.toolStripSeparator4,
            this.tsRefresh,
            this.tsSave,
            this.tsExport,
            this.toolStripSeparator8,
            this.mnuExtensionsMainMenu});
            this.toolStripData.Location = new System.Drawing.Point(0, 0);
            this.toolStripData.Name = "toolStripData";
            this.toolStripData.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripData.Size = new System.Drawing.Size(1380, 25);
            this.toolStripData.TabIndex = 1;
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
            this.tsAdd.Text = "Add (CTRL + SHIFT + A)";
            this.tsAdd.Click += new System.EventHandler(this.tsAdd_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsUpdate
            // 
            this.tsUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsUpdate.Image = ((System.Drawing.Image)(resources.GetObject("tsUpdate.Image")));
            this.tsUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUpdate.Name = "tsUpdate";
            this.tsUpdate.Size = new System.Drawing.Size(23, 22);
            this.tsUpdate.Text = "Update (CTRL + SHIFT + U)";
            this.tsUpdate.Click += new System.EventHandler(this.tsUpdate_Click);
            // 
            // tsUpdateCommit
            // 
            this.tsUpdateCommit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsUpdateCommit.Enabled = false;
            this.tsUpdateCommit.Image = ((System.Drawing.Image)(resources.GetObject("tsUpdateCommit.Image")));
            this.tsUpdateCommit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUpdateCommit.Name = "tsUpdateCommit";
            this.tsUpdateCommit.Size = new System.Drawing.Size(23, 22);
            this.tsUpdateCommit.Text = "Commit Update (CTRL + SHIFT + U)";
            this.tsUpdateCommit.Click += new System.EventHandler(this.tsUpdateCommit_Click);
            // 
            // tsUpdateCancel
            // 
            this.tsUpdateCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsUpdateCancel.Enabled = false;
            this.tsUpdateCancel.Image = ((System.Drawing.Image)(resources.GetObject("tsUpdateCancel.Image")));
            this.tsUpdateCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUpdateCancel.Name = "tsUpdateCancel";
            this.tsUpdateCancel.Size = new System.Drawing.Size(23, 22);
            this.tsUpdateCancel.Text = "Cancel Update (CTRL + SHIFT + B)";
            this.tsUpdateCancel.Click += new System.EventHandler(this.tsUpdateCancel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsDelete
            // 
            this.tsDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsDelete.Image")));
            this.tsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDelete.Name = "tsDelete";
            this.tsDelete.Size = new System.Drawing.Size(23, 22);
            this.tsDelete.Text = "Delete (CTRL + SHIFT + D)";
            this.tsDelete.Click += new System.EventHandler(this.tsDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsChildren
            // 
            this.tsChildren.BackColor = System.Drawing.Color.Transparent;
            this.tsChildren.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsChildren.Image = ((System.Drawing.Image)(resources.GetObject("tsChildren.Image")));
            this.tsChildren.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsChildren.Name = "tsChildren";
            this.tsChildren.Size = new System.Drawing.Size(23, 22);
            this.tsChildren.Text = "View children of selected row (CTRL + SHIFT + C)";
            this.tsChildren.Visible = false;
            this.tsChildren.Click += new System.EventHandler(this.tsChildren_Click);
            // 
            // tsParent
            // 
            this.tsParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsParent.Image = ((System.Drawing.Image)(resources.GetObject("tsParent.Image")));
            this.tsParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsParent.Name = "tsParent";
            this.tsParent.Size = new System.Drawing.Size(23, 22);
            this.tsParent.Text = "View parent of selected row (CTRL + SHIFT + P)";
            this.tsParent.Visible = false;
            this.tsParent.Click += new System.EventHandler(this.tsParent_Click);
            // 
            // tsSelectParent
            // 
            this.tsSelectParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSelectParent.Image = ((System.Drawing.Image)(resources.GetObject("tsSelectParent.Image")));
            this.tsSelectParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSelectParent.Name = "tsSelectParent";
            this.tsSelectParent.Size = new System.Drawing.Size(23, 22);
            this.tsSelectParent.Text = "Select parent as input (CTRL + SHIFT + R)";
            this.tsSelectParent.Visible = false;
            this.tsSelectParent.Click += new System.EventHandler(this.tsSelectParent_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // tsFiltersEnable
            // 
            this.tsFiltersEnable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFiltersEnable.Image = ((System.Drawing.Image)(resources.GetObject("tsFiltersEnable.Image")));
            this.tsFiltersEnable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsFiltersEnable.Name = "tsFiltersEnable";
            this.tsFiltersEnable.Size = new System.Drawing.Size(23, 22);
            this.tsFiltersEnable.Text = "Enable Filters";
            this.tsFiltersEnable.Visible = false;
            // 
            // tsFiltersDisable
            // 
            this.tsFiltersDisable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFiltersDisable.Enabled = false;
            this.tsFiltersDisable.Image = ((System.Drawing.Image)(resources.GetObject("tsFiltersDisable.Image")));
            this.tsFiltersDisable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsFiltersDisable.Name = "tsFiltersDisable";
            this.tsFiltersDisable.Size = new System.Drawing.Size(23, 22);
            this.tsFiltersDisable.Text = "Disable Filters";
            this.tsFiltersDisable.Visible = false;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator4.Visible = false;
            // 
            // tsRefresh
            // 
            this.tsRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsRefresh.Image")));
            this.tsRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRefresh.Name = "tsRefresh";
            this.tsRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsRefresh.Text = "Load DataBox (CTRL + SHIFT + L)";
            this.tsRefresh.Click += new System.EventHandler(this.tsRefresh_Click);
            // 
            // tsSave
            // 
            this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSave.Image = ((System.Drawing.Image)(resources.GetObject("tsSave.Image")));
            this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSave.Name = "tsSave";
            this.tsSave.Size = new System.Drawing.Size(23, 22);
            this.tsSave.Text = "Save (CTRL + SHIFT + S)";
            this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // tsExport
            // 
            this.tsExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsExport.Image = ((System.Drawing.Image)(resources.GetObject("tsExport.Image")));
            this.tsExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsExport.Name = "tsExport";
            this.tsExport.Size = new System.Drawing.Size(23, 22);
            this.tsExport.Text = "Export to File";
            this.tsExport.Click += new System.EventHandler(this.tsExport_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // mnuExtensionsMainMenu
            // 
            this.mnuExtensionsMainMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuExtensionsMainMenu.Image = ((System.Drawing.Image)(resources.GetObject("mnuExtensionsMainMenu.Image")));
            this.mnuExtensionsMainMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuExtensionsMainMenu.Name = "mnuExtensionsMainMenu";
            this.mnuExtensionsMainMenu.Size = new System.Drawing.Size(29, 22);
            this.mnuExtensionsMainMenu.Text = "Menu";
            this.mnuExtensionsMainMenu.Visible = false;
            // 
            // DataBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splTimesheet);
            this.Controls.Add(this.toolStripData);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "DataBoxControl";
            this.Size = new System.Drawing.Size(1380, 654);
            this.Load += new System.EventHandler(this.DataBoxControl_Load);
            this.splTimesheet.Panel1.ResumeLayout(false);
            this.splTimesheet.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splTimesheet)).EndInit();
            this.splTimesheet.ResumeLayout(false);
            this.pnlDataInputs.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.tabPageDataInput.ResumeLayout(false);
            this.pnlDataBox.ResumeLayout(false);
            this.pnlDataBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.toolStripData.ResumeLayout(false);
            this.toolStripData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog svdExport;
        private System.Windows.Forms.OpenFileDialog opdImport;
        private System.Windows.Forms.SaveFileDialog svdExportItem;
        protected System.Windows.Forms.CheckBox checkBox2;
        protected System.Windows.Forms.CheckBox checkBox3;
        protected System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.ToolStripButton btnTimesheetAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private Figlut.Server.Toolkit.Winforms.CustomToolStrip toolStripData;
        private System.Windows.Forms.ToolStripButton tsAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsUpdate;
        private System.Windows.Forms.ToolStripButton tsUpdateCommit;
        private System.Windows.Forms.ToolStripButton tsUpdateCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsFiltersEnable;
        private System.Windows.Forms.ToolStripButton tsFiltersDisable;
        private System.Windows.Forms.SplitContainer splTimesheet;
        private Figlut.Server.Toolkit.Winforms.GradientPanel pnlDataInputs;
        private Figlut.Server.Toolkit.Winforms.CustomTab tabData;
        private System.Windows.Forms.TabPage tabPageDataInput;
        private Figlut.Server.Toolkit.Winforms.GradientPanel pnlDataInput;
        private Figlut.Server.Toolkit.Winforms.GradientPanel pnlDataBox;
        private Figlut.Server.Toolkit.Winforms.CustomDataGridView grdData;
        protected System.Windows.Forms.Label lblData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsSave;
        private System.Windows.Forms.ToolStripDropDownButton mnuExtensionsMainMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton tsParent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton tsChildren;
        private System.Windows.Forms.ToolStripButton tsSelectParent;
        private System.Windows.Forms.ToolStripButton tsRefresh;
        private System.Windows.Forms.ToolStripButton tsExport;




    }
}
