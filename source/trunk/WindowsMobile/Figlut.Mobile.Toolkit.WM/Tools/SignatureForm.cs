namespace Figlut.Mobile.Toolkit.Tools
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
    using System.Reflection;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.IO;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    /// <summary>
    /// A form that allows a user capture a signature. The CapturedSignatureImage and CapturedSignatureImageData
    /// properties will only be set if the user captures a the signature and thereby causing the form to close
    /// with a dialog result of OK.
    /// </summary>
    public partial class SignatureForm : Form
    {
        #region Constructors

        /// <summary>
        /// A form that allows a user to capture a signature. The CapturedSignatureImage and CapturedSignatureImageData
        /// propertie will only be set if the user captures a the signature and thereby causing the form to close
        /// with a dialog result of OK.
        /// </summary>
        public SignatureForm()
        {
            InitializeComponent();
            Clear();
        }

        #endregion //Constructors

        #region Fields

        protected int _x1 = -1;
        protected int _y1 = -1;
        protected int _x2 = -1;
        protected int _y2 = -1;

        protected Color _signatureColor = Color.Black;
        protected Pen _pen = new Pen(Color.Black);

        protected int _width = 1;

        protected Bitmap _capturedSignatureImage;
        protected byte[] _capturedSignatureImageData;
        protected Graphics _picGraphics;

        #endregion //Fields

        /// <summary>
        /// The image containing the signature that was captured. This property will only be 
        /// set if the user captures a the signature and thereby causing the form to close
        /// with a dialog result of OK.
        /// </summary>
        public Image CapturedSignatureImage
        {
            get { return _capturedSignatureImage; }
        }

        /// <summary>
        /// The binary data of the image containing the signature that was captured. This property will only be 
        /// set if the user captures a the signature and thereby causing the form to close
        /// with a dialog result of OK.
        /// </summary>
        public byte[] CapturedSignatureImageData
        {
            get { return _capturedSignatureImageData; }
        }

        #region Event Handlers

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_pen.Color != _signatureColor)
                {
                    _pen = new Pen(_signatureColor);
                }
                if (_pen.Width != _width)
                {
                    _pen.Width = _width;
                }
                _x1 = _x2;
                _y1 = _y2;
                _x2 = e.X;
                _y2 = e.Y;
                if (_x1 == -1 && _y1 == -1)
                {
                    return;
                }
                _picGraphics.DrawLine(_pen, _x1, _y1, _x2, _y2);
                mnuCapture.Enabled = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void pictureBoxBlack_Click(object sender, EventArgs e)
        {
            try
            {
                _signatureColor = Color.Black;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void pictureBoxRed_Click(object sender, EventArgs e)
        {
            try
            {
                _signatureColor = Color.Red;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void pictureBoxGreen_Click(object sender, EventArgs e)
        {
            try
            {
                _signatureColor = Color.Green;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void pictureBoxBlue_Click(object sender, EventArgs e)
        {
            try
            {
                _signatureColor = Color.Blue;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void pictureBoxYellow_Click(object sender, EventArgs e)
        {
            try
            {
                _signatureColor = Color.Yellow;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void pictureBoxWhite_Click(object sender, EventArgs e)
        {
            try
            {
                _signatureColor = Color.White;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void pictureBoxSignature_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuWidth1Pixel_Click(object sender, EventArgs e)
        {
            try
            {
                _width = 1;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuWidth3Pixels_Click(object sender, EventArgs e)
        {
            try
            {
                _width = 3;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuWidth5Pixels_Click(object sender, EventArgs e)
        {
            try
            {
                _width = 5;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                panelBlack.Tag = true;
                panelRed.Tag = true;
                panelGreen.Tag = true;
                panelBlue.Tag = true;
                panelYellow.Tag = true;
                panelWhite.Tag = true;

                switch (GetColorName(_signatureColor))
                {
                    case "Black":
                        panelBlack.Tag = !panelBlack.Visible;
                        break;
                    case "Red":
                        panelRed.Tag = !panelRed.Visible;
                        break;
                    case "Green":
                        panelGreen.Tag = !panelGreen.Visible;
                        break;
                    case "Blue":
                        panelBlue.Tag = !panelBlue.Visible;
                        break;
                    case "Yellow":
                        panelYellow.Tag = !panelYellow.Visible;
                        break;
                    case "White":
                        panelWhite.Tag = !panelWhite.Visible;
                        break;
                }

                panelBlack.Visible = (bool)panelBlack.Tag;
                panelRed.Visible = (bool)panelRed.Tag;
                panelGreen.Visible = (bool)panelGreen.Tag;
                panelBlue.Visible = (bool)panelBlue.Tag;
                panelYellow.Visible = (bool)panelYellow.Tag;
                panelWhite.Visible = (bool)panelWhite.Tag;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuCapture_Click(object sender, EventArgs e)
        {
            try
            {
                string signatureImageFilePath = Path.Combine(Information.GetExecutingDirectory(), "signature.jpg");
                Save(signatureImageFilePath, _picGraphics, picSignature.Bounds);
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Event Handlers

        #region Methods

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
            if (_picGraphics != null)
            {
                _picGraphics.Dispose();
            }
            if (timer != null && timer.Enabled)
            {
                timer.Enabled = false;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Clears the image containing the signature thereby allowing the user to recapture
        /// the image.
        /// </summary>
        protected void Clear()
        {
            _capturedSignatureImage = new Bitmap(picSignature.Width, picSignature.Height);
            _picGraphics = picSignature.CreateGraphics();
            _picGraphics.Clear(Color.White);
            mnuCapture.Enabled = false;
        }

        /// <summary>
        /// Resets the X and Y co-ordinates/points.
        /// </summary>
        protected void Reset()
        {
            _x1 = -1;
            _y1 = -1;
            _x2 = -1;
            _y2 = -1;
        }

        private static string GetName(Type enumType, object value)
        {
            return Enum.ToObject(enumType, value).ToString();
        }

        private static string GetColorName(Color inputColor)
        {
            string colorName = string.Empty;
            Type colorType = inputColor.GetType();
            if (colorType != null)
            {
                PropertyInfo[] propInfoList = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
                for (int i = 0; i < propInfoList.Length; i++)
                {
                    PropertyInfo propInfo = (PropertyInfo)propInfoList[i];
                    Color color = (Color)propInfo.GetValue(null, null);
                    if (color == inputColor)
                    {
                        colorName = propInfo.Name;
                    }
                }
            }

            return colorName;
        }

        /// <summary>
        /// Saves the image to a file.
        /// Passing a null or empty filename will bypass the saving to tne file and only update this form's result properties.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="gx"></param>
        /// <param name="rect"></param>
        protected void Save(string filename, Graphics gx, Rectangle rect)
        {
            using (Bitmap bmp = new Bitmap(rect.Width, rect.Height))
            {
                // Create compatible graphics
                using (Graphics gxComp = Graphics.FromImage(bmp))
                {
                    // Blit the image data
                    BitBlt(gxComp.GetHdc(), 0, 0, rect.Width, rect.Height, gx.GetHdc(), rect.Left, rect.Top, SRCCOPY);
                    _capturedSignatureImage = bmp;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Jpeg);
                        _capturedSignatureImageData = ms.ToArray();
                    }
                    if (!string.IsNullOrEmpty(filename))
                    {
                        bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
        }

        #region P/Invokes

        // P/Invoke declaration
        [DllImport("coredll.dll")]
        public static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        const int SRCCOPY = 0x00CC0020;

        #endregion //P/Invokes 

        #endregion //Methods
    }
}