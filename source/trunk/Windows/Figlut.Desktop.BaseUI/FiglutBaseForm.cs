namespace Figlut.Desktop.BaseUI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.DataBox.Configuration;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Winforms;

    #endregion //Using Directives

    public partial class FiglutBaseForm : BorderlessForm
    {
        #region Constructors

        public FiglutBaseForm()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                FiglutDesktopDataBoxSettings settings = GOC.Instance.GetSettings<FiglutDesktopDataBoxSettings>();
                TitleBarStartColor =
                    settings.ThemeStartColor == null ?
                    Color.White :
                    Color.FromName(settings.ThemeStartColor);

                TitleBarEndColor =
                    settings.ThemeStartColor == null ?
                    Color.SteelBlue :
                    Color.FromName(settings.ThemeEndColor);
            }
            this.KeyPreview = true;
        }

        #endregion //Constructors

        #region Fields

        private Color _titleBarStartColor;
        private Color _titleBarEndColor;

        #endregion //Fields

        #region Properties

        public Color TitleBarStartColor
        {
            get { return _titleBarStartColor; }
            set
            {
                _titleBarStartColor = value;
                lblFormTitle.GradientStartColor = _titleBarStartColor;
                Application.DoEvents();
            }
        }

        public Color TitleBarEndColor
        {
            get { return _titleBarEndColor; }
            set
            {
                _titleBarEndColor = value;
                lblFormTitle.GradientEndColor = _titleBarEndColor;
            }
        }

        #endregion //Properties
    }
}
