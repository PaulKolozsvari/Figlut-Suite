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
    using System.IO;
    using Figlut.Mobile.Toolkit.Tools.IM;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Tools;

    #endregion //Using Directives

    public partial class DemoImageMapForm : Form
    {
        #region Constructors

        public DemoImageMapForm()
        {
            InitializeComponent();
            GetLogo();
            InitHotspotDerivedTypes();
            RefreshImageControlProperties();
        }

        #endregion //Constructors

        #region Constants

        private const string Figlut_LOGO = "FiglutLogo.jpg";

        #endregion //Constants

        #region Fields

        private Type[] _hotspotDerivedTypes;
        private Image _logo;

        #endregion //Fields

        #region Methods

        public void GetLogo()
        {
            string filePath = Path.Combine(Information.GetExecutingDirectory(), Figlut_LOGO);
            _logo = new Bitmap(filePath);
        }

        private void InitHotspotDerivedTypes()
        {
            _hotspotDerivedTypes = new Type[]
            {
                typeof(CircleHotspot),
                typeof(RectangleHotspot),
                typeof(PolygonHotspot)
            };
        }

        private void RefreshImageControlProperties()
        {
            mnuShowResetCount.Checked = imageMap.ShowImageResetCountInStatus;
            mnuSingleHotspotHighlight.Checked = imageMap.SingleHotspotHighlight;
            mnuShowMousePosition.Checked = imageMap.ShowMousePositionInStatus;
            mnuEnableHotspotHighlight.Checked = imageMap.EnableHotspotHighlight;
            mnuEnableHotspotHighlightOnMouseMove.Checked = imageMap.EnableHotspotHighlightOnMouseMove;
        }

        #endregion //Methods

        #region Event Handlers

        private void mnuBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuSelectImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (opdSelectImage.ShowDialog() == DialogResult.OK)
                {
                    imageMap.Image = new Bitmap(opdSelectImage.FileName);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void imageMap_OnHotspotClick(Hotspot hotspot, Vertex mousePosition)
        {
            try
            {
                if (hotspot.Tag != null)
                {
                    UIHelper.DisplayInformation(hotspot.Tag.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuResetHotspotHighlights_Click(object sender, EventArgs e)
        {
            try
            {
                imageMap.ClearHotspotHighlights();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuShowResetCount_Click(object sender, EventArgs e)
        {
            try
            {
                imageMap.ShowImageResetCountInStatus = !imageMap.ShowImageResetCountInStatus;
                RefreshImageControlProperties();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuSingleHotspotHighlight_Click(object sender, EventArgs e)
        {
            try
            {
                imageMap.SingleHotspotHighlight = !imageMap.SingleHotspotHighlight;
                RefreshImageControlProperties();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuShowMousePosition_Click(object sender, EventArgs e)
        {
            try
            {
                imageMap.ShowMousePositionInStatus = !imageMap.ShowMousePositionInStatus;
                RefreshImageControlProperties();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuEnableHotspotHighlight_Click(object sender, EventArgs e)
        {
            try
            {
                imageMap.EnableHotspotHighlight = !imageMap.EnableHotspotHighlight;
                RefreshImageControlProperties();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuEnableHotspotHighlightOnMouseMove_Click(object sender, EventArgs e)
        {
            try
            {
                imageMap.EnableHotspotHighlightOnMouseMove = !imageMap.EnableHotspotHighlightOnMouseMove;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuSetHotspotColor_Click(object sender, EventArgs e)
        {
            try
            {
                using (ColorPickerForm f = new ColorPickerForm(_logo, PictureBoxSizeMode.StretchImage))
                {
                    if (f.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    imageMap.HotspotColor = f.SelectedColor;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuExportHotspots_Click(object sender, EventArgs e)
        {
            try
            {
                if (svdExport.ShowDialog() == DialogResult.OK)
                {
                    imageMap.ExportHotspots(svdExport.FileName, _hotspotDerivedTypes);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuImportHotspots_Click(object sender, EventArgs e)
        {
            try
            {
                if (opdImport.ShowDialog() == DialogResult.OK)
                {
                    imageMap.ImportHotspots(opdImport.FileName, _hotspotDerivedTypes);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuClearHotspots_Click(object sender, EventArgs e)
        {
            try
            {
                if (UIHelper.AskQuestion(
                    "All hotspots will be cleared. Any unsaved hotspots will be lost. Are you sure?") 
                    != DialogResult.Yes)
                {
                    return;
                }
                imageMap.ClearHotspots();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuBeginHotspot_Click(object sender, EventArgs e)
        {
            try
            {
                using(BeginHotspotForm f = new BeginHotspotForm(imageMap.HotspotKeyExists))
                {
                    if (f.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    imageMap.BeginHotspot(f.Hotspot);
                    mnuCancelHotspot.Enabled = true;
                    mnuEndHotspot.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuEndHotspot_Click(object sender, EventArgs e)
        {
            try
            {
                imageMap.EndHotspot();
                mnuCancelHotspot.Enabled = false;
                mnuEndHotspot.Enabled = false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void mnuCancelHotspot_Click(object sender, EventArgs e)
        {
            try
            {
                if (UIHelper.AskQuestion(
                    "Are you sure you want cancel hotspot?")
                    != DialogResult.Yes)
                {
                    return;
                }
                imageMap.CancelHotspot();
                mnuCancelHotspot.Enabled = false;
                mnuEndHotspot.Enabled = false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //Event Handlers
    }
}