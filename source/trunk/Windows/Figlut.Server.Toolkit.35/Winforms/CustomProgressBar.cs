namespace Figlut.Server.Toolkit.Winforms
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing.Drawing2D;

    #endregion //Using Directives

    public partial class CustomProgressBar : ProgressBar
    {
        #region Constructors

        public CustomProgressBar()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
        }

        #endregion //Constructors

        #region Fields

        protected Color _progressStartColor;
        protected Color _progressEndColor;

        #endregion //Fields

        #region Properties

        public Color ProgressStartColor
        {
            get { return _progressStartColor; }
            set
            {
                _progressStartColor = value;
            }
        }

        public Color ProgressEndColor
        {
            get { return _progressEndColor; }
            set
            {
                _progressEndColor = value;
            }
        }

        #endregion //Properties

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
            {
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            }
            rec.Height = rec.Height - 4;
            LinearGradientBrush gradBrush = new LinearGradientBrush(
                new Rectangle(0, 0, this.Width, this.Height),
                _progressStartColor,
                _progressEndColor, LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(gradBrush, 2, 2, rec.Width, rec.Height);
        }

        #endregion //Methods
    }
}
