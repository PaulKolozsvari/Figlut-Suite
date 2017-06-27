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

    public partial class SADriverLicensePhotoForm : Form
    {
        #region Constructors

        public SADriverLicensePhotoForm(Image logo, PictureBoxSizeMode logoSizeMode, SADriverLicense license)
        {
            InitializeComponent();
            if (license == null)
            {
                throw new NullReferenceException("Supplied SA Driver License may not be null.");
            }
            _license = license;
            Logo = logo;
            picLogo.SizeMode = logoSizeMode;
        }

        #endregion //Constructors

        #region Fields

        protected SADriverLicense _license;

        #endregion //Fields

        #region Properties

        public SADriverLicense License
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
                    EntityReader<SADriverLicensePhotoForm>.GetPropertyName(p => p.Logo, false),
                    this.GetType().FullName));
                }
                picLogo.Image = value;
                Application.DoEvents();
            }
        }

        private void SADriverLicensePhotoForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (_license.DriverImage != null)
                {
                    picDriverPhoto.Image = _license.DriverImage;
                }
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

        #endregion //Properties
    }
}