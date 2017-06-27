namespace Figlut.Mobile.DataBox.UI.Base
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
    using Figlut.Mobile.DataBox.Configuration;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.DataBox.Utilities;

    #endregion //Using Directives

    public partial class BaseForm : Form
    {
        #region Constructors

        public BaseForm()
        {
            InitializeComponent();
            if (FiglutDataBoxApplication.Instance.ThemeColor != null)
            {
                pnlLeftBorder.BackColor = pnlRightBorder.BackColor = statusMain.BackColor =
                    FiglutDataBoxApplication.Instance.ThemeColor;
            }
            if (FiglutDataBoxApplication.Instance.ApplicationBannerImage != null)
            {
                picLogo.Image = FiglutDataBoxApplication.Instance.ApplicationBannerImage;
            }
        }

        #endregion //Constructors
    }
}