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

    public partial class CustomToolStrip : ToolStrip
    {
        #region Constructors

        public CustomToolStrip()
        {
            InitializeComponent();
            _colors = new CustomProfessionalColors();
            this.Renderer = new ToolStripProfessionalRenderer(_colors);
        }

        public CustomToolStrip(CustomProfessionalColors colors)
        {
            _colors = colors;
            this.Renderer = new ToolStripProfessionalRenderer(_colors);
        }

        #endregion //Constructors

        #region Fields

        protected CustomProfessionalColors _colors;

        #endregion //Fields

        #region Methods

        public void SetColors(CustomProfessionalColors colors)
        {
            _colors = colors;
            this.Renderer = new ToolStripProfessionalRenderer(_colors);
        }

        #endregion //Methods

        #region Properties

        public Color ToolStripGradientBegin
        {
            get { return _colors.ToolStripGradientBegin; }
            set
            {
                _colors.SetToolStripGradientBegin(value);
                this.Renderer = new ToolStripProfessionalRenderer(_colors);
                this.Refresh();
            }
        }

        public Color ToolStripGradientMiddle
        {
            get { return _colors.ToolStripGradientMiddle; }
            set
            {
                _colors.SetToolStripGradientMiddle(value);
                this.Renderer = new ToolStripProfessionalRenderer(_colors);
                this.Refresh();
            }
        }

        public Color ToolStripGradientEnd
        {
            get { return _colors.ToolStripGradientEnd; }
            set
            {
                _colors.SetToolStripGradientEnd(value);
                this.Renderer = new ToolStripProfessionalRenderer(_colors);
                this.Refresh();
            }
        }

        #endregion //Properties
    }
}