namespace Figlut.Mobile.Toolkit.SARoad.WM
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
    using Figlut.Mobile.Toolkit.Data;

    #endregion //Using Directives

    public partial class SAVehicleLicenseForm : Form
    {
        #region Constructors

        public SAVehicleLicenseForm(Image logo, PictureBoxSizeMode logoSizeMode, SAVehicleLicense license)
        {
            InitializeComponent();
            if (license == null)
            {
                throw new NullReferenceException("Supplied SA Vehicle License may not be null.");
            }
            _license = license;
            Logo = logo;
            picLogo.SizeMode = logoSizeMode;
        }

        #endregion //Constructors

        #region Fields

        protected SAVehicleLicense _license;

        #endregion //Fields

        #region Properties

        public SAVehicleLicense License
        {
            get { return _license; }
        }

        /// <summary>
        /// The image logo to display at the top of the form.
        /// </summary>
        public Image Logo
        {
            get { return picLogo.Image; }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException(
                        string.Format("{0} for {1} may not be null.",
                        EntityReader<SAVehicleLicenseForm>.GetPropertyName(p => p.Logo, false),
                        this.GetType().FullName));
                }
                picLogo.Image = value;
                Application.DoEvents();
            }
        }

        #endregion //Properties

        #region Event Handlers

        private void SAVehicleLicenseForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtRegistration.Text = _license.RegistrationNumber;
                txtMake.Text = _license.Make;
                txtColour.Text = _license.Colour;
                txtVIN.Text = _license.VIN;
                txtLicenseNumber.Text = _license.LicenseNumber;
                txtEngineNumber.Text = _license.EngineNumber;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuBack_Click(object sender, EventArgs e)
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

        #endregion //Event Handlers
    }
}