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

    #endregion //Using Directives

    public partial class CustomTab : TabControl
    {
        #region Constructors

        public CustomTab()
        {
            InitializeComponent();
        }

        #endregion //Constructors

        #region Fields

        private Color _selectedForeBrushColor;
        private Color _unselectedForBrushColor;
        private Color _selectedBackStartColor;
        private Color _selectedBackEndColor;
        private Color _unselectedBackStartColor;
        private Color _unselectedBackEndColor;

        #endregion //Fields

        #region Properties

        public Color SelectedForeBrushColor
        {
            get { return _selectedForeBrushColor; }
            set { _selectedForeBrushColor = value; }
        }

        public Color SelectedBackStartColor
        {
            get { return _selectedBackStartColor; }
            set { _selectedBackStartColor = value; }
        }

        public Color SelectedBackEndColor
        {
            get { return _selectedBackEndColor; }
            set { _selectedBackEndColor = value; }
        }

        public Color UnselectedForeBrushColor
        {
            get { return _unselectedForBrushColor; }
            set { _unselectedForBrushColor = value; }
        }

        public Color UnselectedBackStartColor
        {
            get { return _unselectedBackStartColor; }
            set { _unselectedBackStartColor = value; }
        }

        public Color UnselectedBackEndColor
        {
            get { return _unselectedBackEndColor; }
            set { _unselectedBackEndColor = value; }
        }

        #endregion //Properties

        #region Methods

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Font f = new Font(e.Font, FontStyle.Italic | FontStyle.Bold);
            Brush backBrush;
            Brush foreBrush;
            if (e.Index == this.SelectedIndex)
            {
                backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    e.Bounds,
                    _selectedBackStartColor,
                    _selectedBackEndColor,
                    System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                foreBrush = new SolidBrush(_selectedForeBrushColor);
            }
            else
            {
                //f = e.Font;
                backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    e.Bounds,
                    _unselectedBackStartColor,
                    _unselectedBackEndColor,
                    System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                foreBrush = new SolidBrush(_unselectedForBrushColor);
            }
            string tabName = this.TabPages[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            Rectangle r = e.Bounds;
            r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
            e.Graphics.DrawString(tabName, f, foreBrush, r, sf);
            sf.Dispose();
            if (e.Index == this.SelectedIndex)
            {
                f.Dispose();
                backBrush.Dispose();
            }
            else
            {
                backBrush.Dispose();
                foreBrush.Dispose();
            }
            base.OnDrawItem(e);
        }

        #endregion //Methods
    }
}