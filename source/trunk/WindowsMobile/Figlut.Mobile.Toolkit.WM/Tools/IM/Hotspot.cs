namespace Figlut.Mobile.Toolkit.Tools.IM
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    /// <summary>
    /// An abstract class to be implemented by for specific hotspot shapes e.g. circle, rectangle etc.,
    /// which can then be added on an Image Map control.
    /// </summary>
    [Serializable]
    public abstract class Hotspot
    {
        #region Constructors

        /// <summary>
        /// Creates a new hotspot.
        /// </summary>
        protected Hotspot()
        {
            _hotspotComplete = true;
            _highlighted = false;
        }

        /// <summary>
        /// Creates a new hotspot.
        /// </summary>
        /// <param name="key">A unique key that will be used by the Image Map control to store the hotspot in a hotspot cache.</param>
        /// <param name="enableHotspotHighlight">If set false the hotspot will not be highlighted even of the Image Map control is set to highlight hotspots.</param>
        /// <param name="hotspotComplete">Sets the flag on the hotspot whether or no it is in the creation state i.e. if hotspot is complete it is not in the creation state.</param>
        protected Hotspot(string key, bool enableHotspotHighlight, bool hotspotComplete)
        {
            _key = key;
            _enableHotspotHighlight = enableHotspotHighlight;
            _hotspotComplete = hotspotComplete;
        }

        #endregion //Constructors

        #region Fields

        protected string _key;
        protected object _tag;
        protected bool _enableHotspotHighlight;
        protected bool _hotspotComplete;
        protected bool _highlighted;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// A unique key that will be used by the Image Map control to store the hotspot in 
        /// a hotspot cache and identify it.
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Any object you'd like stored linked to this hotspot.
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// If set false the hotspot will not be highlighted even of the Image Map control is set to highlight hotspots.
        /// </summary>
        public bool EnableHotspotHighlight
        {
            get { return _enableHotspotHighlight; }
            set { _enableHotspotHighlight = value; }
        }

        /// <summary>
        /// Whether or not the hotspot is in the creation state.
        /// i.e. if hotspot is complete it is not in the creation state.
        /// </summary>
        public bool HotspotComplete
        {
            get { return _hotspotComplete; }
            set { _hotspotComplete = value; }
        }

        /// <summary>
        /// A flag used by the Image Map control to flag a hotspot that should highlighted i.e. colored in.
        /// </summary>
        public bool Highlighted
        {
            get { return _highlighted; }
            set { _highlighted = value; }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Determines if a supplied vertex is located inside the hotspot.
        /// </summary>
        /// <param name="vertex">The vertex to test for e.g. where the user clicked on the Image Map control.</param>
        /// <returns>Returns true if the vertex is inside the hotspot.</returns>
        public abstract bool IsPointVisible(Vertex vertex);

        /// <summary>
        /// Colors in the hotspot with the provided brush on the provided graphics.
        /// </summary>
        /// <param name="graphics">The graphics to color in on.</param>
        /// <param name="brush">Brush to use to color in.</param>
        public abstract void HighlightHotspot(Graphics graphics, Brush brush);

        /// <summary>
        /// Cancels the creation of the hotspot by clearing the captured vertices.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public abstract string CancelHotspot();

        /// <summary>
        /// Begins the creation of a hotspot. Sets the the hotspot in a state to accept new vertices to be added.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public abstract string BeginHotspot();

        /// <summary>
        /// Adds the vertex to this hotspot that should be in the creation state.
        /// </summary>
        /// <param name="vertex">The new vertex to be added to the hotspot.</param>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public abstract string AddHotspotVertex(Vertex vertex);

        /// <summary>
        /// Adds the vertex to this hotspot that should be in the creation state.
        /// Call BeginHotspot to ensure hotspot is in the creation state. If the graphics object is not null
        /// the outline of the the hotspot will also be drawn on the provided graphics with the
        /// provided pen.
        /// </summary>
        /// <param name="vertex">The new vertex to be added to the hotspot.</param>
        /// <param name="graphics">The graphics to draw the outline on.</param>
        /// <param name="pen">The pen to use for drawing the outline.</param>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public abstract string AddHotspotVertex(Vertex vertex, Graphics graphics, Pen pen);

        /// <summary>
        /// Checks that all the necessary vertices have been added
        /// and then ends the creation of the hotspot by setting the HotspotComplet flag to true.
        /// May not add more vertices after calling this method.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public abstract string EndHotspot();

        /// <summary>
        /// Checks that all the necessary vertices have been added
        /// and then ends the creation of the hotspot by setting the HotspotComplet flag to true.
        /// May not add more vertices after calling this method.
        /// Implementation should color in the hotspot if the supplied graphics is not null with the supplied brush.
        /// </summary>
        /// <param name="graphics">The brush to you use to color in.</param>
        /// <param name="brush">The brush to you use to color in.</param>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public abstract string EndHotspot(Graphics graphics, Brush brush);

        /// <summary>
        /// Refreshes the outline of this hotspot. Implementation should do nothing if the supplied graphics is null.
        /// </summary>
        /// <param name="graphics">The graphics to use to draw on.</param>
        /// <param name="pen">The pen to use to draw the outline.</param>
        public abstract void RefreshIncompleteHotspotOutline(Graphics graphics, Pen pen);

        #endregion //Methods
    }
}