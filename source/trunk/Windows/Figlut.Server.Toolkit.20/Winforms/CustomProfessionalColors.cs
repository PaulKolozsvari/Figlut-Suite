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

    public class CustomProfessionalColors : ProfessionalColorTable
    {
        #region Constructors

        public CustomProfessionalColors()
        {
            _toolStripGradientBegin = Color.White;
            _toolStripGradientMiddle = Color.WhiteSmoke;
            _toolStripGradientEnd = Color.LightGray;
            _menuStripGradientBegin = Color.White;
            _menuStripGradientEnd = Color.White;
        }

        public CustomProfessionalColors(
            Color toolStripGradientBegin,
            Color toolStripGradientMiddle,
            Color toolStripGradientEnd,
            Color menuStripGradientBegin,
            Color menuStripGradientEnd)
        {
            _toolStripGradientBegin = toolStripGradientBegin;
            _toolStripGradientMiddle = toolStripGradientMiddle;
            _toolStripGradientEnd = toolStripGradientEnd;
            _menuStripGradientBegin = menuStripGradientBegin;
            _menuStripGradientEnd = menuStripGradientEnd;
        }

        #endregion //Constructors

        #region Fields

        protected Color _toolStripGradientBegin;
        protected Color _toolStripGradientMiddle;
        protected Color _toolStripGradientEnd;
        protected Color _menuStripGradientBegin;
        protected Color _menuStripGradientEnd;

        #endregion //Fields

        #region Properties

        public override Color ToolStripGradientBegin
        {
            get { return _toolStripGradientBegin; }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return _toolStripGradientMiddle; }
        }

        public override Color ToolStripGradientEnd
        {
            get { return _toolStripGradientEnd; }
        }

        public override Color MenuStripGradientBegin
        {
            get { return _menuStripGradientBegin; }
        }

        public override Color MenuStripGradientEnd
        {
            get { return _menuStripGradientEnd; }
        }

        #endregion //Properties

        #region Methods

        public void SetToolStripGradientBegin(Color color)
        {
            _toolStripGradientBegin = color;
        }

        public void SetToolStripGradientMiddle(Color color)
        {
            _toolStripGradientMiddle = color;
        }

        public void SetToolStripGradientEnd(Color color)
        {
            _toolStripGradientEnd = color;
        }

        public void SetMenuStripGradientBegin(Color color)
        {
            _menuStripGradientBegin = color;
        }

        public void SetMenuStripGradientEnd(Color color)
        {
            _menuStripGradientEnd = color;
        }

        #endregion //Methods
    }
}