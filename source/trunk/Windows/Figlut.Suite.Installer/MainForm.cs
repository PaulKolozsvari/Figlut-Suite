namespace Figlut.Suite.Installer
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Winforms;

    #endregion //Using Directives

    public partial class MainForm : BorderlessForm
    {
        #region Constructors

        public MainForm()
        {
            InitializeComponent();
            _installerFilePaths = new List<string>();
            _componentControls = new List<CheckBox>();
        }

        #endregion //Constructors

        #region Fields

        private List<string> _installerFilePaths;
        private List<CheckBox> _componentControls;
        private bool _forceClose;

        #endregion //Fields

        #region Methods

        private void InitComponents()
        {
            chkFiglutConfigurationManager.Tag = "Figlut.Configuration.Manager.exe";
            chkFiglutWebService.Tag = "Figlut.Web.Service.exe";
            chkFiglutDesktopDataBox.Tag = "Figlut.Desktop.DataBox.exe";
            chkFiglutServerToolkit.Tag = "Figlut.Server.Toolkit.exe";
            chkFiglutMobileToolkitWM.Tag = "Figlut.Mobile.Toolkit.WM.exe";
            _componentControls.Clear();
            List<CheckBox> componentControls = new List<CheckBox>();
            foreach (CheckBox chk in gpbInstallComponents.Controls)
            {
                componentControls.Add(chk);
            }
            _componentControls = DataHelper.ReverseListOrder(componentControls);
        }

        private void CheckComponentsSelected()
        {
            _installerFilePaths.Clear();
            foreach (CheckBox chk in _componentControls)
            {
                if (chk.Checked)
                {
                    _installerFilePaths.Add(chk.Tag.ToString());
                }
            }
            btnInstall.Enabled = _installerFilePaths.Count > 0;
        }

        private void InstallComponent(string fileName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName) { WorkingDirectory = Information.GetExecutingDirectory() };
            using (Process p = new Process() { StartInfo = startInfo })
            {
                if (!p.Start())
                {
                    throw new Exception(string.Format("Failed to install {0}.", fileName));
                }
                p.WaitForExit();
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitComponents();
            CheckComponentsSelected();
            chkFiglutConfigurationManager.Focus();
        }

        private void chkComponentCheckChanged_CheckStateChanged(object sender, EventArgs e)
        {
            CheckComponentsSelected();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            foreach (string filePath in _installerFilePaths)
            {
                InstallComponent(filePath);
            }
            _forceClose = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_forceClose || UIHelper.AskQuestion("Are you sure you want to cancel the Figlut Suite Installer?") ==
                System.Windows.Forms.DialogResult.Yes)
            {
                AnimateHideForm();
            }
            else
            {
                e.Cancel = true;
            }
        }

        #endregion //Event Handlers
    }
}
