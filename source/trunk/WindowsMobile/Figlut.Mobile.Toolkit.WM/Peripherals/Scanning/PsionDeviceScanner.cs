namespace Figlut.Mobile.Toolkit.Peripherals.Scanning
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using PsionTeklogix.Barcode;
    using PsionTeklogix.Barcode.ScannerServices;

    #endregion //Using Directives

    /// <summary>
    /// A Figlut specific scanner wrapper class implementing the the generic DeviceScanner
    /// abstract class. An instance of this class should be stored in the Peripheral 
    /// singleton class.
    /// </summary>
    public class PsionDeviceScanner : DeviceScanner
    {
        #region Constructors

        /// <summary>
        /// A Figlut specific scanner wrapper class implementing the the generic DeviceScanner
        /// abstract class. An instance of this class should be stored in the Peripheral 
        /// singleton class.
        /// </summary>
        /// <param name="enabled">Whether the scanner should be enabled or disabled.</param>
        public PsionDeviceScanner(bool enabled)
            : base(new Scanner(new ScannerServicesDriver()))
        {
            Scanner scanner = ((Scanner)_manufacturerBaseScanner);
            scanner.Enabled = enabled;
            ((Scanner)_manufacturerBaseScanner).ScanCompleteEvent += 
                new ScanCompleteEventHandler(_manufacturerBaseScanner_ScanCompleteEvent);
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// Enables the Figlut scanner.
        /// </summary>
        public override void Enable()
        {
            ((Scanner)_manufacturerBaseScanner).Enabled = true;
        }

        /// <summary>
        /// Dsiables the Figlut scanner.
        /// </summary>
        public override void Disable()
        {
            ((Scanner)_manufacturerBaseScanner).Enabled = true;
        }

        /// <summary>
        /// Cleans up the OEM specific scanner i.e. Manufacturer Base Scanner object.
        /// </summary>
        public override void Dispose()
        {
            Scanner FiglutScanner = ((Scanner)_manufacturerBaseScanner);
            FiglutScanner.ScanCompleteEvent -= new ScanCompleteEventHandler(_manufacturerBaseScanner_ScanCompleteEvent);
            FiglutScanner.Dispose();
        }

        #endregion //Methods

        #region Event Handlers

        private void _manufacturerBaseScanner_ScanCompleteEvent(object sender, ScanCompleteEventArgs e)
        {
            base.FireOnScanEvent(new DeviceScanEventArgs(e.Text, e.RawData, e.Symbology.ToString()));
        }

        #endregion //Event Handlers
    }
}