namespace Figlut.Mobile.Toolkit.Peripherals.Scanning
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    public class DeviceScanEventArgs
    {
        #region Constructors

        public DeviceScanEventArgs(
            string barcodeText,
            byte[] rawData,
            string symbology)
        {
            _barcodeText = barcodeText;
            _rawData = rawData;
            _symbology = symbology;
        }

        #endregion //Constructors

        #region Fields

        private string _barcodeText;
        private byte[] _rawData;
        private string _symbology;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// The text representation of the barcode that was scanned.
        /// </summary>
        public string BarcodeText
        {
            get { return _barcodeText; }
        }

        /// <summary>
        /// The bytes of the barcode that was scanned.
        /// </summary>
        public byte[] RawData
        {
            get { return _rawData; }
        }

        /// <summary>
        /// The symbology of the scanned barcode.
        /// </summary>
        public string Symbology
        {
            get { return _symbology; }
        }

        #endregion //Properties
    }
}