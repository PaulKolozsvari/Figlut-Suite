namespace Figlut.Mobile.Toolkit.Demo.WM
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
    using Figlut.Mobile.Toolkit.Tools.IM;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    public partial class BeginHotspotForm : Form
    {
        #region Inner Types

        public delegate bool HotspotKeyExists(string key);

        #endregion //Inner Types

        #region Constructors

        public BeginHotspotForm(HotspotKeyExists keyExistsMethod)
        {
            InitializeComponent();
            if (keyExistsMethod == null)
            {
                throw new NullReferenceException("Must provide a method to be called that determines whether a key entered already exists.");
            }
            _keyExistsMethod = keyExistsMethod;
        }

        #endregion //Constructors

        #region Fields

        private HotspotKeyExists _keyExistsMethod;
        private Hotspot _hotspot;

        #endregion //Fields

        #region Properties

        public Hotspot Hotspot
        {
            get { return _hotspot; }
        }

        #endregion //Properties

        #region Event Handlers

        private void mnuBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtKey.Text))
                {
                    txtKey.Focus();
                    UIHelper.DisplayError("Key not entered.");
                    return;
                }
                if (_keyExistsMethod(txtKey.Text))
                {
                    txtKey.Focus();
                    UIHelper.DisplayError(string.Format("A hotspot with key {0} already exists.", txtKey.Text));
                    return;
                }
                if (radPolygon.Checked)
                {
                    _hotspot = new PolygonHotspot();
                }
                else if (radCircle.Checked)
                {
                    _hotspot = new CircleHotspot();
                }
                else if (radRectangle.Checked)
                {
                    _hotspot = new RectangleHotspot();
                }
                else
                {
                    UIHelper.DisplayError("No hotspot type selected.");
                    return;
                }
                _hotspot.Key = txtKey.Text;
                _hotspot.EnableHotspotHighlight = chkEnableHotspotHighlight.Checked;
                if(!string.IsNullOrEmpty(txtTag.Text))
                {
                    _hotspot.Tag = txtTag.Text;
                }
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Event Handlers
    }
}