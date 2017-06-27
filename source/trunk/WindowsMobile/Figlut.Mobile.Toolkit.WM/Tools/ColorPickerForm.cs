namespace Figlut.Mobile.Toolkit.Tools
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
    using System.Reflection;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    /// <summary>
    /// A form that displays a list of all the system colors which the user can choose from.
    /// If the ShowDialog() method of this form returns a dialog of OK, the SelectedColor
    /// property will have been set to the color that the user selected.
    /// </summary>
    public partial class ColorPickerForm : Form
    {
        #region Constructors

        /// <summary>
        /// A form that displays a list of all the system colors which the user can choose from.
        /// If the ShowDialog() method of this form returns a dialog of OK, the SelectedColor
        /// property will have been set to the color that the user selected.
        /// </summary>
        /// <param name="logo">The image logo to display at the top of the form.</param>
        /// <param name="sizemode">The size mode of the picture on which the logo will be displayed.</param>
        public ColorPickerForm(Image logo, PictureBoxSizeMode logoSizeMode)
        {
            InitializeComponent();
            Information.GetPsionDeviceId();
            Logo = logo;
            picLogo.SizeMode = logoSizeMode;
        }

        #endregion //Constructors

        #region Fields

        protected Dictionary<string, Color> _colors;
        protected Color _selectedColor;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// The color selected by the user. This property will only be set by this form 
        /// once the user has selected a color and the form has closed with a dialog result of OK.
        /// </summary>
        public Color SelectedColor
        {
            get { return _selectedColor; }
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
                    throw new NullReferenceException("Logo for color picker form may not be null.");
                }
                picLogo.Image = value;
                Application.DoEvents();
            }
        }

        #endregion //Properties

        #region Methods

        private void RefreshSystemColors()
        {
            string originalStatus = statusMain.Text;
            try
            {
                using (WaitProcess w = new WaitProcess(mnuMain))
                {
                    statusMain.Text = "Refreshing colors ...";
                    Application.DoEvents();
                    _colors = Information.GetSystemColors();
                    lstColors.Items.Clear();
                    _colors.Keys.ToList().ForEach(c => lstColors.Items.Add(c));
                }
            }
            finally
            {
                statusMain.Text = originalStatus;
            }
        }

        #endregion //Methods

        #region Event Handlers

        private void ColorPickerForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.BeginInvoke(new Action(RefreshSystemColors));
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

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstColors.SelectedIndex < 0)
                {
                    lstColors.Focus();
                    UIHelper.DisplayError("No color selected.");
                    return;
                }
                string colorName = lstColors.SelectedItem.ToString();
                if (!_colors.ContainsKey(colorName))
                {
                    throw new ArgumentException(string.Format("Could not find color with name {0} in cache.", colorName));
                }
                _selectedColor = _colors[colorName];
                this.DialogResult = DialogResult.OK;
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