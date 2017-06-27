namespace Figlut.Mobile.Toolkit.Peripherals.Scanning
{
    #region region name

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using PsionTeklogix.Barcode;
    using PsionTeklogix.Barcode.ScannerServices;
    using Figlut.Mobile.Toolkit.Peripherals;

    #endregion //region name

    /// <summary>
    /// An abstract class that can be implemented for each OEM (Orginal Equipment Manufacturer)
    /// based on the the OEM SDK e.g. FiglutDeviceScanner. The user (developer) that needs to interact
    /// with the scanner hardware only needs to interact with an instance of this class i.e. DeviceScanner
    /// therefore not allowing OEM SDK specific code to be mixed with any business logic. An instance
    /// of this class DeviceScanner should be set in inside the Peripheral singleton class.
    /// </summary>
    public abstract class DeviceScanner : IDisposable
    {
        #region Constructors

        /// <summary>
        /// An abstract class that can be implemented for each OEM (Orginal Equipment Manufacturer)
        /// based on the the OEM SDK e.g. FiglutDeviceScanner. The user (developer) that needs to interact
        /// with the scanner hardware only needs to interact with an instance of this class i.e. DeviceScanner
        /// therefore not allowing OEM SDK specific code to be mixed with any business logic. An instance
        /// of this class DeviceScanner should be set in inside the Peripheral singleton class.
        /// </summary>
        public DeviceScanner()
        {
        }

        /// <summary>
        /// An abstract class that can be implemented for each OEM (Orginal Equipment Manufacturer)
        /// based on the the OEM SDK e.g. FiglutDeviceScanner. The user (developer) that needs to interact
        /// with the scanner hardware only needs to interact with an instance of this class i.e. DeviceScanner
        /// therefore not allowing OEM SDK specific code to be mixed with any business logic. An instance
        /// of this class DeviceScanner should be set in inside the Peripheral singleton class.
        /// </summary>
        /// <param name="manufacturerBaseScanner">The OEM specific scanner object acquired from the OEM SDK e.g. FiglutTeklogix.Barcode.Scanner.</param>
        public DeviceScanner(object manufacturerBaseScanner)
        {
            _manufacturerBaseScanner = manufacturerBaseScanner;
        }

        #endregion //Constructors

        #region Fields

        /// <summary>
        /// The OEM specific scanner object acquired from the OEM SDK e.g. FiglutTeklogix.Barcode.Scanner
        /// </summary>
        protected object _manufacturerBaseScanner;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// The OEM specific scanner object acquired from the OEM SDK e.g. FiglutTeklogix.Barcode.Scanner
        /// This value should be set in the constructor of this class.
        /// </summary>
        public object ManufacturerBaseScanner
        {
            get { return _manufacturerBaseScanner; }
        }

        #endregion //Properties

        #region Events

        /// <summary>
        /// An event fired when scannner scans a barcode.
        /// </summary>
        public event DeviceScanHander OnScan;

        #endregion //Events

        #region Methods

        /// <summary>
        /// Enables the scanner.
        /// </summary>
        public abstract void Enable();

        /// <summary>
        /// Disables the scanner.
        /// </summary>
        public abstract void Disable();

        /// <summary>
        /// Call this method from a derived class implenting the OEM specific SDK
        /// in order to fire the OnScan event which users should be subscribing to.
        /// </summary>
        /// <param name="e"></param>
        protected void FireOnScanEvent(DeviceScanEventArgs e)
        {
            if (OnScan != null)
            {
                OnScan(this, e);
            }
        }

        /// <summary>
        /// Cleans up the OEM specific scanner i.e. Manufacturer Based Scanner object.
        /// </summary>
        public abstract void Dispose();

        #endregion //Methods
    }
}