namespace Figlut.Mobile.Toolkit.SARoad.WM
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    public class SAVehicleLicense
    {
        public SAVehicleLicense(string barcodeText)
        {
            string[] args = barcodeText.Split('%');
            if (args.Length != 16)
            {
                throw new ArgumentException("Invalid car license format.");
            }
            LicenseNumber = args[1];
            ControlNumber = args[5];
            RegistrationNumber = args[6];
            RegisterNumber = args[7];
            Description = args[8];
            Make = args[9];
            SeriesName = args[10];
            Colour = args[11];
            VIN = args[12];
            ExpiryDate = DateTime.ParseExact(args[14], "yyyy-MM-dd", null);
        }

        #region Properties

        public string LicenseNumber { get; set; }

        public string ControlNumber { get; set; }

        public string RegistrationNumber { get; set; }

        public string RegisterNumber { get; set; }

        public string VIN { get; set; }

        public string EngineNumber { get; set; }

        public string Description { get; set; }

        public string Make { get; set; }

        public string SeriesName { get; set; }

        public string Colour { get; set; }

        public DateTime ExpiryDate { get; set; }

        #endregion //Properties
    }
}