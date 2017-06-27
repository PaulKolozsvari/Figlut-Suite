namespace OneStopShop.FiglutExtensions.Biometric
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.BaseUI;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Sagem.MorphoKit;

    #endregion //Using Directives

    public partial class FingerScanForm : FiglutBaseForm
    {
        #region Constructors

        public FingerScanForm(bool enrolment, FingerId defaultFingerId, string formTitle)
        {
            try
            {
                InitializeComponent();
                _enrolment = enrolment;
                _defaultFingerId = defaultFingerId;
                _device = new AcquisitionDevice();
                _device.FingerEvent += _device_FingerEvent;
                _device.EnrolmentEvent += _device_EnrolmentEvent;
                _device.QualityEvent += _device_QualityEvent;
                _device.ImageEvent += _device_ImageEvent;
                _device.Display = this.picFingerprint.Handle;
                _formTitle = formTitle;
            }
            catch (Exception ex)
            {
                UIHelper.DisplayError(ex.Message);
            }
        }

        #endregion //Constructors

        #region Fields

        private bool _enrolment;
        private FingerId _defaultFingerId;
        private string _formTitle;
        private AcquisitionDevice _device;
        private Bitmap _bitmap;
        private byte[] _template;
        private byte[] _acquisitionImageBytes;
        private FingerId _fingerId;

        #endregion //Fields

        #region Properties

        public bool Enrolment
        {
            get { return _enrolment; }
        }

        public Bitmap BitmapResult
        {
            get { return _bitmap; }
        }

        public byte[] TemplateResult
        {
            get { return _template; }
        }

        public byte[] AcquisitionImageBytesResult
        {
            get { return _acquisitionImageBytes; }
        }

        public FingerId FingerIdResult
        {
            get { return _fingerId; }
        }

        #endregion //Properties

        #region Methods

        private Bitmap CreateGreyscaleBitmap(byte[] buffer, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //Copy the acquire image data to our bitmap. This works because the width of all MSO images is a multiple of 4.
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            Marshal.Copy(buffer, 0, bmpData.Scan0, width * height); //Copies the image data from an unmanaged memory (buffer - unmanaged pointer) to the managed bmpData.
            bmp.UnlockBits(bmpData);

            //Setup a greyscale palette
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
            {
                pal.Entries[i] = Color.FromArgb(i, i, i);
            }
            bmp.Palette = pal;
            _bitmap = bmp;
            return _bitmap;
        }

        private void PerformEnrolment(string serialNumber)
        {
            _device.TimeOut = 10;
            IConsolidatedAcquisitionResult consolidatedAcquisitionResult = _device.AcquireConsolidated(serialNumber);
            UIHelper.DisplayInformation(string.Format("Acquisition Status: {0}.", consolidatedAcquisitionResult.Status));
            if (consolidatedAcquisitionResult.Status != 0) //Quality too poor.
            {
                return;
            }
            picFingerprint.Image = CreateGreyscaleBitmap(consolidatedAcquisitionResult.ImageBuffer1, consolidatedAcquisitionResult.Width, consolidatedAcquisitionResult.Height);
            _acquisitionImageBytes = consolidatedAcquisitionResult.ImageBuffer1;
            Coder coder = new Coder();
            int fingerIdInt = (int)_fingerId;
            IConsolidationResult consolidationResult = coder.EnrollConsolidated( //Consolidate the 3 scans into a single image.
                consolidatedAcquisitionResult.ImageBuffer1,
                consolidatedAcquisitionResult.ImageBuffer2,
                consolidatedAcquisitionResult.ImageBuffer3,
                consolidatedAcquisitionResult.Width,
                consolidatedAcquisitionResult.Height,
                (byte)fingerIdInt);
            if (!consolidationResult.Success)
            {
                UIHelper.DisplayError("Consolidation failed.");
                return;
            }
            _template = consolidationResult.Template;
            mnuApply.Enabled = true;
        }

        private void PerformAcquisition(string serialNumber)
        {
            IAcquisitionResult acquisitionResult = _device.Acquire(serialNumber);
            UIHelper.DisplayInformation(string.Format("Acquisition Result: {0}.", acquisitionResult.Status));
            if (acquisitionResult.Status != 0) //Quality too poor.
            {
                return;
            }
            picFingerprint.Image = CreateGreyscaleBitmap(
                acquisitionResult.ImageBuffer,
                acquisitionResult.Width,
                acquisitionResult.Height);
            _acquisitionImageBytes = acquisitionResult.ImageBuffer;
            Coder coder = new Coder(); //Encode the acquisition into a template.
            int fingerIdInt = (int)_fingerId;
            ICoderResult coderResult = coder.EnrollBitmap(_bitmap, (byte)fingerIdInt);
            _template = coderResult.Template;
            mnuApply.Enabled = true;
        }

        private void RefreshFingers()
        {
            cboFinger.Items.Clear();
            Array fingers = EnumHelper.GetEnumValues(typeof(FingerId));
            foreach (FingerId f in fingers)
            {
                if (f == FingerId.None)
                {
                    continue;
                }
                string fingerName = DataShaper.ShapeCamelCaseString(f.ToString());
                cboFinger.Items.Add((fingerName));
            }
            cboFinger.SelectedIndex = (int)_defaultFingerId - 1;
        }

        private FingerId GetSelectedFingerId()
        {
            if (cboFinger.SelectedIndex < 0)
            {
                return FingerId.None;
            }
            string fingerIdName = DataShaper.RestoreStringToCamelCase(cboFinger.SelectedItem.ToString());
            FingerId result = (FingerId)Enum.Parse(typeof(FingerId), fingerIdName);
            return result;
        }

        #endregion //Methods

        #region Event Handlers

        private void FingerScanForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_formTitle))
            {
                this.FormTitle = _enrolment ? "Enrol Fingerprint" : "Identify Fingerprint";
            }
            else
            {
                this.FormTitle = _formTitle;
            }
            Status = _enrolment ? "Acquire 3 fingerprints for enrolment." : "Acquire fingerprint to identify it.";
            btnRefreshDevices.PerformClick();
            RefreshFingers();
            this.Refresh();
        }

        private void _device_ImageEvent(byte[] buffer, int width, int height, int resolution)
        {
            //Nothing to do here for now.
        }

        private void _device_QualityEvent(byte quality)
        {
            txtQuality.Text = string.Format("Live quality: {0}", quality);
            progressQuality.Value = quality < progressQuality.Maximum ? quality : progressQuality.Maximum;
        }

        private void _device_EnrolmentEvent(int captureIndex)
        {
            txtStatus.Text = string.Format("Enrolment Event: {0}", captureIndex);
        }

        private void _device_FingerEvent(int status)
        {
            string message = string.Empty;
            FingerEventStatus fingerEventStatus = (FingerEventStatus)status;
            switch (fingerEventStatus)
            {
                case FingerEventStatus.MOVE_FINGER_DOWN:
                    message = "Move down.";
                    break;
                case FingerEventStatus.MOVE_FINGER_LEFT:
                    message = "Move left.";
                    break;
                case FingerEventStatus.MOVE_FINGER_RIGHT:
                    message = "Move right.";
                    break;
                case FingerEventStatus.MOVE_FINGER_UP:
                    message = "Move up.";
                    break;
                case FingerEventStatus.NO_FINGER_DETECTED:
                    message = "No finger detected.";
                    break;
                case FingerEventStatus.OK:
                    message = "OK.";
                    break;
                case FingerEventStatus.PRESS_FINGER_HARDER:
                    message = "Press harder.";
                    break;
                case FingerEventStatus.REMOVE_FINGER:
                    message = "Remove finger.";
                    break;
                default:
                    break;
            }
            txtStatus.Text = message;
        }

        private void FingerScanForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void FingerScanForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void FingerScanForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void btnRefreshDevices_Click(object sender, EventArgs e)
        {
            cboDevices.Items.Clear();
            int deviceCount = _device.GetNumberOfDevices();
            if (deviceCount < 1)
            {
                throw new UserThrownException("No fingerprint devices found.", LoggingLevel.Maximum);
            }
            List<IAcquisitionDeviceInfo> deviceInfos = _device.EnumerateDevices().ToList();
            deviceInfos.ForEach(p => cboDevices.Items.Add(p.SerialNumber));
            if (cboDevices.Items.Count > 0)
            {
                cboDevices.SelectedIndex = 0;
            }
            cboDevices.SelectedIndex = cboDevices.Items.Count > 0 ? 0 : -1;
        }

        private void btnDescriptor_Click(object sender, EventArgs e)
        {
            if (cboDevices.SelectedIndex < 0)
            {
                btnRefreshDevices.PerformClick();
                if (_device.GetNumberOfDevices() < 1)
                {
                    return;
                }
            }
            string serialNumber = cboDevices.SelectedItem.ToString();
            IAcquisitionDeviceDescriptor descriptor = _device.GetDescriptor(serialNumber);
            using (DeviceDescriptionForm f = new DeviceDescriptionForm(
                descriptor.ProductDescriptor,
                descriptor.SensorDescriptor,
                descriptor.SoftwareDescriptor))
            {
                f.ShowDialog();
            }
        }

        private void btnAcquire_Click(object sender, EventArgs e)
        {
            if (cboDevices.SelectedIndex < 0)
            {
                btnRefreshDevices.PerformClick();
                if (_device.GetNumberOfDevices() < 1)
                {
                    return;
                }
            }
            _fingerId = GetSelectedFingerId();
            if (_fingerId == FingerId.None)
            {
                cboDevices.Focus();
                UIHelper.DisplayError("No finger selected.");
                return;
            }
            string serialNumber = cboDevices.SelectedItem.ToString();
            if (_enrolment) //3 acquisitions required and are then consolidated.
            {
                PerformEnrolment(serialNumber);
            }
            else //1 acquisition performed only.
            {
                PerformAcquisition(serialNumber);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_device.CancelAcquisition() != 0)
            {
                throw new UserThrownException("Failed to cancel fingerprint acquisition.", LoggingLevel.Normal);
            }
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void mnuApply_Click(object sender, EventArgs e)
        {
            if (_template == null ||
                _bitmap == null ||
                _acquisitionImageBytes == null)
            {
                UIHelper.DisplayError("No acquisition performed.");
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void FingerScanForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
            else if ((e.KeyCode == Keys.Enter) & e.Control & e.Shift)
            {
                mnuApply.PerformClick();
            }
            else if ((e.KeyCode == Keys.R) & e.Control & e.Shift)
            {
                btnRefreshDevices.PerformClick();
            }
            else if ((e.KeyCode == Keys.D) & e.Control & e.Shift)
            {
                btnDescriptor.PerformClick();
            }
            else if ((e.KeyCode == Keys.A) & e.Control & e.Shift)
            {
                btnAcquire.PerformClick();
            }
            else if ((e.KeyCode == Keys.Cancel) & e.Control & e.Shift)
            {
                btnCancel.PerformClick();
            }
        }

        #endregion //Event Handlers
    }
}
