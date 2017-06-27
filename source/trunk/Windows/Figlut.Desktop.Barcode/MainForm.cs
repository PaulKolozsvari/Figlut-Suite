namespace Figlut.Desktop.Barcode
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.Barcode.Configuration;
    using Figlut.Desktop.BaseUI;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using ZXing;

    #endregion //Using Directives

    public partial class MainForm : FiglutBaseForm
    {
        #region Constructors

        public MainForm(FiglutDesktopBarcodeSettings settings)
        {
            InitializeComponent();
            _initialFormSize = this.Size;
            _settings = settings;
            _barcodeWriter = new BarcodeWriter();
            _imageFormats = new Dictionary<string, ImageFormat>();
            InitializeImageFormats();
            opdImport.Filter = string.Format(
                "{0} Files|*.{1}|{2} Files|*.{3}|{4} Files|*.{5}|{6} Files|*.{7}|{8} Files|*.{9}|{10} Files|*.{11}|{12} Files|*.{13}|{14} Files|*.{15}|All files|*.*",
                BMP_IMAGE_FORMAT.ToUpper(), //0
                BMP_IMAGE_FORMAT, //1
                EMF_IMAGE_FORMAT.ToUpper(), //2
                EMF_IMAGE_FORMAT, //3
                GIF_IMAGE_FORMAT.ToUpper(), //4
                GIF_IMAGE_FORMAT, //5
                ICON_IMAGE_FORMAT.ToUpper(), //6
                ICON_IMAGE_FORMAT, //7
                JPEG_IMAGE_FORMAT.ToUpper(), //8
                JPEG_IMAGE_FORMAT, //9
                PNG_IMAGE_FORMAT.ToUpper(), //10
                PNG_IMAGE_FORMAT, //11
                TIFF_IMAGE_FORMAT.ToUpper(), //12
                TIFF_IMAGE_FORMAT, //13
                WMF_IMAGE_FORMAT.ToUpper(), //14
                WMF_IMAGE_FORMAT); //15
        }

        #endregion //Constructors

        #region Constants

        private const string BMP_IMAGE_FORMAT = "bmp";
        private const string EMF_IMAGE_FORMAT = "emf";
        private const string GIF_IMAGE_FORMAT = "gif";
        private const string ICON_IMAGE_FORMAT = "icon";
        private const string JPEG_IMAGE_FORMAT = "jpg";
        private const string PNG_IMAGE_FORMAT = "png";
        private const string TIFF_IMAGE_FORMAT = "tiff";
        private const string WMF_IMAGE_FORMAT = "wmf";

        private const int DEFAULT_WIDTH = 300;
        private const int DEFAULT_HEIGHT = 300;

        #endregion //Constants

        #region Fields

        private FiglutDesktopBarcodeSettings _settings;
        private bool _forceClose;
        private Dictionary<string, ImageFormat> _imageFormats;
        private BarcodeWriter _barcodeWriter;
        private Size _initialFormSize;

        #endregion //Fields

        #region Properties

        public bool ForceClose
        {
            get { return _forceClose; }
            set { _forceClose = true; }
        }

        #endregion //Properties

        #region Methods

        private void InitializeImageFormats()
        {
            _imageFormats.Clear();
            _imageFormats.Add(BMP_IMAGE_FORMAT, ImageFormat.Bmp);
            _imageFormats.Add(EMF_IMAGE_FORMAT, ImageFormat.Emf);
            _imageFormats.Add(GIF_IMAGE_FORMAT, ImageFormat.Gif);
            _imageFormats.Add(ICON_IMAGE_FORMAT, ImageFormat.Icon);
            _imageFormats.Add(JPEG_IMAGE_FORMAT, ImageFormat.Jpeg);
            _imageFormats.Add(PNG_IMAGE_FORMAT, ImageFormat.Png);
            _imageFormats.Add(TIFF_IMAGE_FORMAT, ImageFormat.Tiff);
            _imageFormats.Add(WMF_IMAGE_FORMAT, ImageFormat.Wmf);
        }

        private void ResetInputControls()
        {
            this.Size = _initialFormSize;
            txtBarcodeContent.Text = string.Empty;
            RefreshBarcodeFormats();
            RefreshImageFormats();
            nudWidth.Value = DEFAULT_WIDTH;
            nudHeight.Value = DEFAULT_HEIGHT;
            picBarcode.Image = null;
            txtBarcodeContent.Focus();
        }

        private void RefreshBarcodeFormats()
        {
            cboBarcodeFormat.Items.Clear();
            Array barcodeFormats = EnumHelper.GetEnumValues(typeof(BarcodeFormat));
            foreach (Enum e in barcodeFormats)
            {
                cboBarcodeFormat.Items.Add(e);
            }
            cboBarcodeFormat.SelectedItem = BarcodeFormat.QR_CODE;
        }

        private void RefreshImageFormats()
        {
            cboImageFormat.Items.Clear();
            _imageFormats.Keys.ToList().ForEach(p => cboImageFormat.Items.Add(p));
            cboImageFormat.SelectedItem = PNG_IMAGE_FORMAT;
        }

        private void Generate()
        {
            if (string.IsNullOrEmpty(txtBarcodeContent.Text))
            {
                txtBarcodeContent.Focus();
                throw new UserThrownException("Barcode Content not entered.", LoggingLevel.None);
            }
            if (cboBarcodeFormat.SelectedIndex < 0)
            {
                cboBarcodeFormat.Focus();
                throw new UserThrownException(string.Format("{0} not selected.", DataShaper.ShapeCamelCaseString(typeof(BarcodeFormat).Name)), LoggingLevel.None);
            }
            if (nudWidth.Value <= 0)
            {
                nudWidth.Focus();
                throw new UserThrownException("Width not entered.", LoggingLevel.None);
            }
            if (nudHeight.Value <= 0)
            {
                nudHeight.Focus();
                throw new UserThrownException("Height not entered.", LoggingLevel.None);
            }
            BarcodeFormat barcodeFormat = (BarcodeFormat)cboBarcodeFormat.SelectedItem;
            int width = Convert.ToInt32(nudWidth.Value);
            int height = Convert.ToInt32(nudHeight.Value);

            _barcodeWriter.Format = barcodeFormat;
            _barcodeWriter.Options = new ZXing.Common.EncodingOptions() { Height = height, Width = width };
            picBarcode.Image = _barcodeWriter.Write(txtBarcodeContent.Text);

            txtBarcodeContent.Focus();
        }

        private void Decode()
        {

        }

        private void CopyToClipBoard()
        {
            if (picBarcode.Image == null)
            {
                throw new UserThrownException("No current image to copy to clipboard.", LoggingLevel.None);
            }
            Clipboard.SetImage((Bitmap)picBarcode.Image);
        }

        private void Export()
        {
            if (cboImageFormat.SelectedIndex < 0)
            {
                cboImageFormat.Focus();
                throw new UserThrownException(string.Format("{0} not selected.", DataShaper.ShapeCamelCaseString(typeof(ImageFormat).Name)), LoggingLevel.None);
            }
            if (picBarcode.Image == null)
            {
                throw new UserThrownException("No current image to export.", LoggingLevel.None);
            }
            string selectedImageFormatName = cboImageFormat.SelectedItem.ToString();
            ImageFormat imageFormat = _imageFormats[selectedImageFormatName];
            svdExport.DefaultExt = selectedImageFormatName;
            svdExport.Filter = string.Format("{0} Files|*.{1}", selectedImageFormatName.ToUpper(), selectedImageFormatName);
            if (svdExport.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            ((Bitmap)picBarcode.Image).Save(svdExport.FileName, imageFormat);
        }

        private void Import()
        {
            //if (opdImport.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            //{
            //    return;
            //}
            //picBarcode.Image = Bitmap.FromFile(opdImport.FileName);
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

        private void picResizeWindow_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderLessFormResize(sender, e);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_forceClose && UIHelper.AskQuestion("Are you sure you want to exit?") != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                AnimateHideForm();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ResetInputControls();
        }


        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.R) & e.Control & e.Shift)
            {
                ResetInputControls();
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                Generate();
            }
            else if ((e.KeyCode == Keys.D) & e.Control & e.Shift)
            {
                Decode();
            }
            else if ((e.KeyCode == Keys.C) & e.Control & e.Shift)
            {
                CopyToClipBoard();
            }
            else if ((e.KeyCode == Keys.E) & e.Control & e.Shift)
            {
                Export();
            }
            else if ((e.KeyCode == Keys.I) & e.Control & e.Shift)
            {
                Import();
            }
        }

        private void lnkResetInputControls_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetInputControls();
        }

        private void mnuBarcodeGenerate_Click(object sender, EventArgs e)
        {
            Generate();
        }

        private void mnuBarcodeDecode_Click(object sender, EventArgs e)
        {
            Decode();
        }

        private void mnuBarcodeCopyToClipboard_Click(object sender, EventArgs e)
        {
            CopyToClipBoard();
        }

        private void mnuExport_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void mnuImport_Click(object sender, EventArgs e)
        {
            Import();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion //Event Handlers
    }
}
