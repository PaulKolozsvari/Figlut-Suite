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

    public partial class CustomDataGridView : DataGridView
    {
        #region Constructors

        public CustomDataGridView()
        {
            InitializeComponent();
            _backgroundStartColor = Color.White;
            _backgroundEndColor = Color.White;
            this.DoubleBuffered = true;
        }

        #endregion //Constructors

        #region Fields

        private Color _backgroundStartColor;
        private Color _backgroundEndColor;

        #endregion //Fields

        public Color BackgroundStartColor
        {
            get { return _backgroundStartColor; }
            set
            {
                _backgroundStartColor = value;
            }
        }

        public Color BackgroundEndColor
        {
            get { return _backgroundEndColor; }
            set
            {
                _backgroundEndColor = value;
            }
        }

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            graphics.FillRectangle(new LinearGradientBrush(
                gridBounds,
                _backgroundStartColor,
                _backgroundEndColor,
                LinearGradientMode.Horizontal),
                gridBounds);
        }

        private void CustomDataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            Refresh();
        }

        #endregion //Methods
    }
}
