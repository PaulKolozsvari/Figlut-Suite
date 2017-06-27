namespace Figlut.Mobile.Toolkit.Demo.WM
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Tools;
    using System.IO;

    #endregion //Using Directives

    public partial class MainForm : Form
    {
        #region Constructors

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion //Constructors

        #region Constants

        private const string Figlut_LOGO = "FiglutLogo.jpg";

        private const string IMAGE_MAP = "ImageMap";
        private const string CAMERA = "Camera";
        private const string COLOR_PICKER = "ColorPicker";
        private const string SIGNATURE = "Signature";

        #endregion //Constants
    
        #region Fields
		
        private Image _logo;
		
        #endregion //Fields

        #region Methods
		
        public void GetLogo()
        {
            string filePath = Path.Combine(Information.GetExecutingDirectory(), Figlut_LOGO);
            _logo = new Bitmap(filePath);
        }
		
        #endregion //Methods

        #region Event Handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                lstDemos.Items.Add(IMAGE_MAP);
                lstDemos.Items.Add(CAMERA);
                lstDemos.Items.Add(COLOR_PICKER);
                lstDemos.Items.Add(SIGNATURE);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            try
            {
                if(UIHelper.AskQuestion("Are you sure you want to exit?") == DialogResult.Yes)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuDemo_Click(object sender, EventArgs e)
        {
            try
            {
                if(lstDemos.SelectedIndex < 0)
                {
                    lstDemos.Focus();
                    UIHelper.DisplayError("No demo selected.");
                    return;
                }
                string demo = lstDemos.SelectedItem.ToString();
                switch (demo)
                {
                    case IMAGE_MAP:
                        using (DemoImageMapForm f = new DemoImageMapForm()) { f.ShowDialog(); }
                        break;
                    case CAMERA:
                        Image image = null; 
                        byte[] imageBytes = null;
                        (new CameraHelper()).CameraCapture(false, this, "Camera Demo", out image, out imageBytes);
                        break;
                    case COLOR_PICKER:
                        using (ColorPickerForm f = new ColorPickerForm(_logo, PictureBoxSizeMode.StretchImage)) { f.ShowDialog(); }
                        break;
                    case SIGNATURE:
                        using (SignatureForm f = new SignatureForm()) { f.ShowDialog(); }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Event Handlers
    }
}