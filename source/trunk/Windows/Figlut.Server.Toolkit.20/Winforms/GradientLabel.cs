namespace Figlut.Server.Toolkit
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing.Drawing2D;

    #endregion //Using Directives

    public partial class GradientLabel : Label
    {
        #region Constructors

        public GradientLabel()
        {
            InitializeComponent();
            _gradientStartColor = Color.White;
            _gradientEndColor = Color.White;
        }

        #endregion //Constructors

        #region Fields

        private Color _gradientStartColor;
        private Color _gradientEndColor;

        #endregion //Fields

        #region Properties

        public Color GradientStartColor
        {
            get { return _gradientStartColor; }
            set
            {
                _gradientStartColor = value;
                PaintGradient();
            }
        }

        public Color GradientEndColor
        {
            get { return _gradientEndColor; }
            set
            {
                _gradientEndColor = value;
                PaintGradient();
            }
        }

        #endregion //Properties

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void PaintGradient()
        {
            //System.Drawing.Drawing2D.LinearGradientBrush gradBrush;
            //gradBrush = new LinearGradientBrush(new
            //  Point(0, 0), new Point(this.Width, this.Height),
            //  _gradientStartColor, _gradientEndColor);

            LinearGradientBrush gradBrush = new LinearGradientBrush(
                new Rectangle(0, 0, this.Width, this.Height),
                _gradientStartColor,
                _gradientEndColor, LinearGradientMode.Horizontal);

            Bitmap bmp = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(gradBrush, new Rectangle(0, 0,
                            this.Width, this.Height));
            this.BackgroundImage = bmp;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion //Methods
    }
}