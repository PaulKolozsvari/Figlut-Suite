namespace Figlut.Mobile.Toolkit.Peripherals
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Mobile.Toolkit.Peripherals.Scanning;

    #endregion //Using Directives

    /// <summary>
    /// A singleton class for storing peripheral objects for the lifetime
    /// of an application.
    /// </summary>
    public class Peripheral
    {
        #region Singleton Setup

        private static Peripheral _instance;

        /// <summary>
        /// Gets the one and only instance of the Peripheral class for storing peripheral 
        /// objects for the lifetime of an application.
        /// </summary>
        public static Peripheral Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Peripheral();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private Peripheral()
        {
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Get or set the OEM specific scanner object implementing the generic 
        /// abstract DeviceScanner class.
        /// </summary>
        public DeviceScanner Scanner { get; set; }

        #endregion //Properties
    }
}