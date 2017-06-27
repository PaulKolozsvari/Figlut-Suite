namespace Figlut.Server.Toolkit.Winforms
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing.Drawing2D;

    #endregion //Using Directives

    public partial class GradientPanel : Panel
    {
        #region Constructors

        public GradientPanel()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            _gradientStartColor = Color.White;
            _gradientEndColor = Color.White;
            PaintGradient();
        }

        #endregion //Constructors

        #region Fields

        protected Color _gradientStartColor;
        protected Color _gradientEndColor;
        protected LinearGradientMode _gradientMode;

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

        public LinearGradientMode GradientMode
        {
            get { return _gradientMode; }
            set
            {
                _gradientMode = value;
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
            LinearGradientBrush gradBrush = new LinearGradientBrush(
                new Rectangle(0, 0, this.Width, this.Height),
                _gradientStartColor,
                _gradientEndColor, _gradientMode);

            Bitmap bmp = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(gradBrush, new Rectangle(0, 0, this.Width, this.Height));
            this.BackgroundImage = bmp;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion //Methods
    }
}