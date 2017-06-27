namespace Figlut.Mobile.Toolkit.SARoad.WM
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Face.DrivingLicenseCard.RSA;
    using Summus.WiSDK;
    using System.Drawing;
    using System.IO;
    using System.Drawing.Imaging;

    #endregion //Using Directives

    public class SADriverLicense
    {
        #region Constructors

        public SADriverLicense(byte[] barcodeRawData)
        {
            DrivingLicenseCard lic = DLCSerializerRSA.Deserialize(barcodeRawData);
            Bitmap photo = WaveletImage.Decompress(
                lic.Photo.ImageData,
                lic.Photo.IsTopDownImage);

            LicenseCertificateNumber = lic.DrivingLicense.CertificateNumber;
            CountryOfIssue = lic.IdentityDocument.CountryOfIssue;
            Initials = lic.Person.Initials;
            Surname = lic.Person.Surname;
            IdNumber = lic.IdentityDocument.Number;
            Gender = lic.Person.Gender;
            DateOfBirth = lic.Person.DateOfBirth;
            DateValidFrom = lic.Card.DateValidFrom;
            DateValidUntil = lic.Card.DateValidUntil;
            ProfessionalDrivingPermitCategory = lic.ProfessionalDrivingPermit.Category;
            ProfessionalDrivingPermitDateValidUntil = lic.ProfessionalDrivingPermit.DateValidUntil;
            DriverImage = photo;
            using (MemoryStream ms = new MemoryStream())
            {
                photo.Save(ms, ImageFormat.Jpeg);
                DriverImageBytes = ms.ToArray();
            }
        }

        #endregion //Constructors

        #region Properties

        public string LicenseCertificateNumber { get; set; }

        public string Initials { get; set; }

        public string Surname { get; set; }

        public string IdNumber { get; set; }

        public string Gender { get; set; }

        public string CountryOfIssue { get; set; }

        public string DateOfBirth { get; set; }

        public Image DriverImage { get; set; }

        public byte[] DriverImageBytes { get; set; }

        public string DateValidFrom { get; set; }

        public string DateValidUntil { get; set; }

        public string ProfessionalDrivingPermitCategory { get; set; }

        public string ProfessionalDrivingPermitDateValidUntil { get; set; }

        #endregion //Properties
    }
}