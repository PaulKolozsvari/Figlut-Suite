namespace Figlut.Mobile.Toolkit.Tools.IM
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Text;
    using System.Windows.Forms;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    /// <summary>
    /// A Windows Forms control that can used on a form to add and maintain an Image Map i.e.
    /// similar to how an image map works in HTML.
    /// </summary>
    public partial class ImageMap : UserControl
    {
        #region Inner Types

        public delegate void HotspotClickedHandler(Hotspot hotspot, Vertex mousePosition);
        public delegate void MousePositionChangedHandler(Vertex mousePosition);

        #endregion //Inner Types

        #region Constructors

        /// <summary>
        /// A Windows Forms control that can used on a form to add and maintain an Image Map i.e.
        /// similar to how an image map works in HTML.
        /// </summary>
        public ImageMap()
        {
            InitializeComponent();
            Information.GetPsionDeviceId();
            _hotspotsCache = new HotspotsCache();
            _showImageResetCountInStatus = false;
            _showMousePositionInStatus = false;
            _enableHotspotHighlightOnMouseMove = false;
            _singleHotspotHighlight = false;
            _requireImageReset = false;
            _creatingHotspot = false;
            _brush = new SolidBrush(Color.Red);
            _pen = new Pen(Color.Red, 5);
        }

        #endregion //Constructors

        #region Events

        /// <summary>
        /// An event that gets fired when a hotspot is clicked on the Image Control.
        /// Provides the hotspot as well as the mouse position.
        /// </summary>
        public event HotspotClickedHandler OnHotspotClick;

        /// <summary>
        /// An event that gets fired when the position of the mouse changes i.e. MouseMove when
        /// the user clicks and drags across the Image Map control.
        /// Provides the mouse position too.
        /// </summary>
        public event MousePositionChangedHandler OnMousePositionChanged;

        #endregion //Events

        #region Fields

        protected Hotspot _activeHotspot;
        protected bool _requireImageReset;
        protected int _imageResetCount = 0;

        protected Graphics _graphics;
        protected HotspotsCache _hotspotsCache;
        protected StatusBar _statusBar;

        protected bool _showMousePositionInStatus;
        protected bool _showImageResetCountInStatus;
        protected bool _enableHotspotHighlight;
        protected bool _enableHotspotHighlightOnMouseMove;
        protected bool _singleHotspotHighlight;
        protected Color _hotspotColor;
        protected SolidBrush _brush;
        protected Pen _pen;

        protected bool _creatingHotspot;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// Gets or sets the status bar to which the Image Map control should update the status to.
        /// If not status bar is available just won't be written i.e. this property is not mandatory.
        /// </summary>
        public StatusBar StatusBar
        {
            get { return _statusBar; }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("Status Bar supplied to ImageMap may not be null.");
                }
                _statusBar = value;
            }
        }

        /// <summary>
        /// The background image that should be displayed. The hotspots will drawn, highlighted etc.
        /// onto this image.
        /// </summary>
        public Image Image
        {
            get { return pictureBox.Image; }
            set
            {
                try
                {
                    if (value == null)
                    {
                        throw new NullReferenceException("Image supplied to ImageMap may not be null.");
                    }
                    pictureBox.Image = null;
                    pictureBox.Image = value;
                    _graphics = pictureBox.CreateGraphics();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    UIHelper.DisplayError(ex.Message);
                }
            }
        }

        /// <summary>
        /// Indicates whether the mouse position should written to the status bar when it moves
        /// or is released (MouseUp) from the Image Map control.
        /// </summary>
        public bool ShowMousePositionInStatus
        {
            get { return _showMousePositionInStatus; }
            set { _showMousePositionInStatus = value; }
        }

        /// <summary>
        /// Indicates whether hotspots should be highlighted i.e. colored in.
        /// </summary>
        public bool EnableHotspotHighlight
        {
            get { return _enableHotspotHighlight; }
            set 
            { 
                _enableHotspotHighlight = value;
                tmrRefreshHotspotHighlights.Enabled = _enableHotspotHighlight;
            }
        }

        /// <summary>
        /// The color used to highlight/color in the hotspots.
        /// </summary>
        public Color HotspotColor
        {
            get { return _hotspotColor; }
            set 
            { 
                _hotspotColor = value;
                _brush.Color = _hotspotColor;
                _pen = new Pen(_hotspotColor, 5);
            }
        }

        /// <summary>
        /// Indicates whether hotspots should be highlighted when the user clicks
        /// and drags the mouse over the highlights i.e. MouseMove.
        /// </summary>
        public bool EnableHotspotHighlightOnMouseMove
        {
            get { return _enableHotspotHighlightOnMouseMove; }
            set { _enableHotspotHighlightOnMouseMove = value; }
        }

        /// <summary>
        /// Indicates whether only a single hotspot should be highlighted/colored at a time (the last activated hotspot)
        /// or whether multiple hotspots should be allowed to be highlighted until the highlights are cleared.
        /// </summary>
        public bool SingleHotspotHighlight
        {
            get { return _singleHotspotHighlight; }
            set 
            { 
                _singleHotspotHighlight = value;
            }
        }

        /// <summary>
        /// The amount of times the image on the Image Map control was reset i.e. invalidated.
        /// This happens everytime the image changes e.g. when a hotspot is activated/deactivated.
        /// </summary>
        public int ImageResetCount
        {
            get { return _imageResetCount; }
        }

        /// <summary>
        /// Indicates whether the Image Reset Count should be displayed writen to the status bar
        /// when the mouse moves (gets dragged) on the Image Map control or is released (MouseUp).
        /// </summary>
        public bool ShowImageResetCountInStatus
        {
            get { return _showImageResetCountInStatus; }
            set { _showImageResetCountInStatus = value; }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Add a hotspot to the ImageMap.
        /// </summary>
        /// <param name="hotspot">The hotspot to add.</param>
        public void AddHotspot(Hotspot hotspot)
        {
            if (hotspot == null)
            {
                throw new NullReferenceException("Supplied hotspot to be added to the Image Map map not be null.");
            }
            _hotspotsCache.Add(hotspot);
        }

        /// <summary>
        /// Deletes the hotspot with the specified key and then it refreshes the hotspot highlights.
        /// If a hotspot with the specified key does not exist an exception is thrown.
        /// </summary>
        /// <param name="key">The key of the hotspot to be deleted.</param>
        public void DeleteHotspot(string key)
        {
            if (!_hotspotsCache.Exists(key))
            {
                throw new ArgumentException(string.Format("Could not find hotspot with key {0} to remove.", key));
            }
            Hotspot hotspot = _hotspotsCache[key];
            if (hotspot.Key == _activeHotspot.Key)
            {
                _activeHotspot = null;
            }
            _hotspotsCache.Delete(hotspot.Key);
            RefreshHotspotHighlights();
        }

        /// <summary>
        /// Deletes all the hotspots from the cache that don't have any tags attached to them
        /// and then it refreshes the hotspot highlights. Does not delete any hotspots with
        /// keys that are listed in the hotspotsToKeep.
        /// </summary>
        public void DeleteHotspotsWithoutTags(List<Hotspot> hotspotsToKeep)
        {
            List<Hotspot> hotspotsWithoutTags = _hotspotsCache.Where(p => p.Tag == null).ToList();
            foreach (Hotspot hotspot in hotspotsWithoutTags)
            {
                if (hotspotsToKeep.Contains(hotspot))
                {
                    continue;
                }
                if (_activeHotspot != null && hotspot.Key == _activeHotspot.Key)
                {
                    _activeHotspot = null;
                }
                _hotspotsCache.Delete(hotspot.Key);
            }
            GC.Collect();
            RefreshHotspotHighlights();
        }

        /// <summary>
        /// Returns the hotspot from the cache with the specified key.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="key">The key of the hotspot in cache.</param>
        /// <returns>The hotspot with the specified key or null if it doesn't exist.</returns>
        public Hotspot GetHotspot(string key)
        {
            return _hotspotsCache[key];
        }

        /// <summary>
        /// Gets the currently active hotspot. If there is currently no hotspot active
        /// (the user clicked outside of any hotspot) then null is returned.
        /// </summary>
        /// <returns>the last hotspot the user activated.</returns>
        public Hotspot GetActiveHotspot()
        {
            return _activeHotspot;
        }

        /// <summary>
        /// A hotspot that contains the given vertex is made as the active hotspot.
        /// If the given vertex is not located inside any of the hotspots the active
        /// hotspot is set to null.
        /// </summary>
        /// <param name="vertex">The vertex to check which hotspot it is located in.</param>
        /// <returns>Returns the hotspot where the vertex is located in, otherwise returns null.</returns>
        protected Hotspot SetActiveHotspotAtPoint(Vertex vertex)
        {
            Hotspot result = null;
            foreach (Hotspot hotspot in _hotspotsCache)
            {
                if (hotspot.IsPointVisible(vertex))
                {
                    result = hotspot;
                }
            }
            if (result != _activeHotspot)
            {
                _activeHotspot = result;
                _requireImageReset = true;
            }
            if (_activeHotspot != null)
            {
                _activeHotspot.Highlighted = true;
            }
            return result;
        }

        /// <summary>
        /// Sets the status of the status bar that is assigned to this Image Map control.
        /// If there is not status bar this method does nothing.
        /// </summary>
        /// <param name="status">The status string to assign to the status bar.</param>
        public void SetStatus(string status)
        {
            if (_statusBar == null)
            {
                return;
            }
            _statusBar.Text = status;
            Application.DoEvents();
        }

        /// <summary>
        /// Sets the status of the status bar that is assigned to this Image Map control
        /// as well as displays the mouse co position and image reset count if this Image
        /// Map control is configured to do so i.e. display these fields.
        /// If there is not status bar this method does nothing.
        /// </summary>
        /// <param name="status">The status string to assign to the status bar.</param>
        /// <param name="mousePosition">The position (Vertex) of the mouse.</param>
        public void SetStatus(string status, Vertex mousePosition)
        {
            if (_statusBar == null)
            {
                return;
            }
            status = _showMousePositionInStatus ?
                string.Format("X={0} Y={1} {2}", mousePosition.X, mousePosition.Y, status) :
                status;
            if (_showImageResetCountInStatus)
            {
                status += string.Format(" RC = {0}", _imageResetCount);
            }
            _statusBar.Text = status;
            Application.DoEvents();
        }

        /// <summary>
        /// Invalidates the image thereby clearing the drawn hotspot highlights and can also
        /// clear the flags on all the highlights that are flagged to be highlighted.
        /// Additional may only invalidate/reset the image only it is required i.e. if the image has
        /// changed because a highlight has become active or has been deactivated. The parameter
        /// forceImageReset controls this behaviour.
        /// </summary>
        /// <param name="forceImageReset">Disregards whether the image indeed needs to be invalidated/reset and forces the reset to occur anyway.</param>
        /// <param name="resetHotspotHighlightFlags">Clears the Highlighted flag on all hotspots to ensure they are not highlighted again until they get activated again by the user.</param>
        public void ClearHotspotHighlights(bool forceImageReset, bool resetHotspotHighlightFlags)
        {
            if (resetHotspotHighlightFlags)
            {
                foreach (Hotspot h in _hotspotsCache)
                {
                    h.Highlighted = false;
                }
            }
            if (!forceImageReset && //We might not have to reset the image.
                (!_singleHotspotHighlight || //Allow multiple hotspots to be highlighted.
                !_requireImageReset)) //No change to the image has been done.
            {
                return;
            }
            pictureBox.Invalidate();
            Application.DoEvents();
            _requireImageReset = false;
            _imageResetCount++;
        }

        /// <summary>
        /// Invalidates the image thereby clearing the drawn hotspot highlights.
        /// An image reset image reset is forced and all hotspot Highlighted flags are cleared.
        /// </summary>
        public void ClearHotspotHighlights()
        {
            ClearHotspotHighlights(true, true);
        }

        /// <summary>
        /// Clears the hotspot highlights on the image (i.e. invalidates the image) without
        /// clearing the hotspots' Highlighted flags. Then rehighlights all the hotspots
        /// that should be highlighted unless the SingleHotspotHighlight property on this
        /// control is set to true, in which case only the currently activated hotspot
        /// will be highlighted. This method does nothing if the EnableHotspotHighlights
        /// property is not set to true on this control
        /// </summary>
        public void RefreshHotspotHighlights()
        {
            ClearHotspotHighlights(false, false);
            if (!_enableHotspotHighlight)
            {
                return;
            }
            foreach (Hotspot hotspot in _hotspotsCache)
            {
                if (_singleHotspotHighlight)
                {
                    if (_activeHotspot == null)
                    {
                        return;
                    }
                    if (hotspot.Key == _activeHotspot.Key &&
                        hotspot.EnableHotspotHighlight) //Check to highlight only the active hotspot.
                    {
                        _activeHotspot.HighlightHotspot(_graphics, _brush);
                        return;
                    }
                }
                else
                {
                    if (hotspot.Highlighted &&
                        hotspot.EnableHotspotHighlight) //Check to highlight all hotspots flagged to be highlighted.
                    {
                        hotspot.HighlightHotspot(_graphics, _brush);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether a hotspot with a provided key has already been added to this control.
        /// </summary>
        /// <param name="key">The hotspot key to look against the hotspots in this control.</param>
        /// <returns>Returns true if a hotspot has already been added to this control with a key matching the given key.</returns>
        public bool HotspotKeyExists(string key)
        {
            return _hotspotsCache.Exists(key);
        }

        /// <summary>
        /// Cancels the creation of a new hotspot on this control.
        /// </summary>
        public void CancelHotspot()
        {
            if (!_creatingHotspot)
            {
                throw new Exception("Cannot cancel hotspot that has not begun.");
            }
            if (_activeHotspot == null)
            {
                throw new NullReferenceException("No active hotspot to cancel.");
            }
            SetStatus(_activeHotspot.CancelHotspot());
            _creatingHotspot = false;
            ClearHotspotHighlights(true, true);
        }

        /// <summary>
        /// Begins the creation of a new hotspot to be later added to this control.
        /// N.B. This method does not add the given hotspot to the existing hotspots
        /// on this control, but merely initiates the creation of this given hotspot i.e.
        /// sets the given hotspot into a creation state thereby allowing for vertices
        /// to be added to it. While the given hotspot is in the creation state it is
        /// set as the active hotspot for this control.
        /// </summary>
        /// <param name="hotspot">The hotspot to set in the creation state to allow vertices to be added to it. This hotspot will also be set as the active hotspot for this control.</param>
        public void BeginHotspot(Hotspot hotspot)
        {
            if (hotspot == null)
            {
                throw new NullReferenceException("Supplied new hotspot to begin may not be null.");
            }
            SetStatus(hotspot.BeginHotspot());
            _creatingHotspot = true;
            _activeHotspot = hotspot;
        }

        /// <summary>
        /// Adds the given vertex to the hotspot if the active hotspot and this control is in the creation state.
        /// Otherwise throws an exception. This method is called by this control on a MouseUp event.
        /// </summary>
        /// <param name="mousePostion">The vertex to add to the active hotspot that should be in the creation state.</param>
        protected void AddHotspotVertex(Vertex mousePostion)
        {
            if (!_creatingHotspot)
            {
                UIHelper.DisplayError("Hotspot creation has not begun. Cannot add vertex.");
                return;
            }
            if (_activeHotspot == null)
            {
                throw new NullReferenceException("No active hotspot to add vertex to.");
            }
            if (_enableHotspotHighlight)
            {
                SetStatus(_activeHotspot.AddHotspotVertex(mousePostion, _graphics, _pen));
            }
            else
            {
                SetStatus(_activeHotspot.AddHotspotVertex(mousePostion));
            }
        }

        /// <summary>
        /// Determines what the active hotspot is at the given mousePosition vertex, 
        /// sets the status and refreshes the hotspot highlights for the control.
        /// </summary>
        /// <param name="mousePosition"></param>
        protected void MouseMoveSetActiveHotspot(Vertex mousePosition)
        {
            SetActiveHotspotAtPoint(mousePosition);
            string status = _activeHotspot == null ? string.Empty : _activeHotspot.Key;
            SetStatus(status, mousePosition);
            RefreshHotspotHighlights();
            if ((_activeHotspot != null) && (OnHotspotClick != null))
            {
                OnHotspotClick(_activeHotspot, mousePosition);
            }
        }

        /// <summary>
        /// Ends the active hotspot that should be in the creation state when this method is called.
        /// Otherwise throws an exception. After the active hotspot has been ended (completed) this
        /// method also adds the hotspot to hotspots cache of this control.
        /// </summary>
        public void EndHotspot()
        {
            if (!_creatingHotspot)
            {
                throw new Exception("Hotspot creation has not begun. Cannot end hotspot.");
            }
            if (_activeHotspot == null)
            {
                throw new NullReferenceException("Cannot add vertex to hotspot that is null.");
            }
            if (_enableHotspotHighlight)
            {
                SetStatus(_activeHotspot.EndHotspot(_graphics, _brush));
            }
            else
            {
                SetStatus(_activeHotspot.EndHotspot());
            }
            _hotspotsCache.Add(_activeHotspot);
            _creatingHotspot = false;
        }

        /// <summary>
        /// Clears all the hotspots that have neen added to this control.
        /// If these hotspots have not been exported to file they will be lost.
        /// </summary>
        public void ClearHotspots()
        {
            _hotspotsCache.Clear();
            _creatingHotspot = false;
            ClearHotspotHighlights(true, true);
        }

        /// <summary>
        /// Exports all the hotspots that have been added to this control to
        /// an XML file. All the types of hotspots that have been added need 
        /// to be included in order for all the hotspots to be serialized to XML successfully.
        /// </summary>
        /// <param name="filePath">The file path to the XML file to where the hotspots will be exported to.</param>
        /// <param name="hotspotDerivedTypes">The hotspot types that have been added to this control e.g. RectangleHotspot/CircleHotspot etc.</param>
        public void ExportHotspots(string filePath, Type[] hotspotDerivedTypes)
        {
            _hotspotsCache.Export(filePath, hotspotDerivedTypes);
        }

        /// <summary>
        /// Imports hotspots from an XML file into this control. Existing hotspot are overwritten with the new ones.
        /// All the types (e.g. RectangleHotspot/CircleHotspot etc.) 
        /// of hotspots that are in the XML file need to be specified in order
        /// for all the hotspots to be deserialized from XML successfully.
        /// </summary>
        /// <param name="filePath">The file path to the XML file from where the hotspots should be imported from.</param>
        /// <param name="hotspotDerivedTypes">The hotspot types that exist in the XML file to be imported e.g. RectangleHotspot/CircleHotspot etc.</param>
        public void ImportHotspots(string filePath, Type[] hotspotDerivedTypes)
        {
            _hotspotsCache = HotspotsCache.Import(filePath, hotspotDerivedTypes);
        }

        #endregion //Methods

        #region Event Handlers

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Vertex mousePosition = new Vertex() { X = e.X, Y = e.Y };
                if (_creatingHotspot)
                {
                    AddHotspotVertex(mousePosition);
                    return;
                }
                else
                {
                    MouseMoveSetActiveHotspot(mousePosition);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_creatingHotspot)
                {
                    return;
                }
                Vertex mousePosition = new Vertex() { X = e.X, Y = e.Y };
                if (OnMousePositionChanged != null)
                {
                    OnMousePositionChanged(mousePosition);
                }
                if (!_enableHotspotHighlightOnMouseMove)
                {
                    return;
                }
                SetActiveHotspotAtPoint(mousePosition);
                string status = _activeHotspot == null ? string.Empty : _activeHotspot.Key;
                SetStatus(status, mousePosition);
                RefreshHotspotHighlights();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        private void tmrRefreshHotspotHighlights_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!_enableHotspotHighlight)
                {
                    tmrRefreshHotspotHighlights.Enabled = false;
                    return;
                }
                if (_creatingHotspot)
                {
                    if (_activeHotspot == null)
                    {
                        throw new Exception("No active hotspot. Cannot refresh incomplete hotspot outline. System failure.");
                    }
                    ClearHotspotHighlights(true, false);
                    _activeHotspot.RefreshIncompleteHotspotOutline(_graphics, _pen);
                }
                {
                    RefreshHotspotHighlights();
                }
            }
            catch (Exception ex)
            {
                tmrRefreshHotspotHighlights.Enabled = false;
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            tmrRefreshHotspotHighlights.Enabled = false;
            base.Dispose(disposing);
        }

        #endregion //Event Handlers
    }
}