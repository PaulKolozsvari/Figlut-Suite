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

    public partial class CustomListBox : ListBox
    {
        #region Constructors

        public CustomListBox()
        {
            InitializeComponent();
            _backStartColor = Color.White;
            _backEndColor = Color.White;
        }

        #endregion //Constructors

        #region Properties

        #region Fields

        private Color _backStartColor;
        private Color _backEndColor;

        #endregion //Fields

        public Color BackStartColor
        {
            get { return _backStartColor; }
            set
            {
                _backStartColor = value;
                this.Refresh();
            }
        }

        public Color BackEndColor
        {
            get { return _backEndColor; }
            set
            {
                _backEndColor = value;
                this.Refresh();
            }
        }

        #endregion //Properties

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clientRectangle = new Rectangle(0, 0, this.Width, this.Height);
            e.Graphics.FillRectangle(new LinearGradientBrush(
                clientRectangle,
                _backStartColor,
                _backEndColor,
                LinearGradientMode.Horizontal),
                clientRectangle);
            base.OnPaint(e);
        }

        #endregion //Methods
    }
}