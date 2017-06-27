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

    public partial class SADriverLicenseForm : Form
    {
        public SADriverLicenseForm(Image logo, PictureBoxSizeMode logoSizeMode, SADriverLicense license)
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
                        EntityReader<SADriverLicenseForm>.GetPropertyName(p => p.Logo, false),
                        this.GetType().FullName));
                }
                picLogo.Image = value;
                Application.DoEvents();
            }
        }

        private void SADriverLicenseForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtInitials.Text = _license.Initials;
                txtSurname.Text = _license.Surname;
                txtIdNumber.Text = _license.IdNumber;
                txtGender.Text = _license.Gender;
                txtCountry.Text = _license.CountryOfIssue;
                txtValidUntil.Text = _license.DateValidUntil;
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

        private void mnuPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                using (SADriverLicensePhotoForm f = new SADriverLicensePhotoForm(
                    Logo, PictureBoxSizeMode.StretchImage, _license))
                {
                    f.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Properties
    }
}