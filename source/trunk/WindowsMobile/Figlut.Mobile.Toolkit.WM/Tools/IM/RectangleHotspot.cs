namespace Figlut.Mobile.Toolkit.Tools.IM
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using System.Diagnostics;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    /// <summary>
    /// A hotspot the shape of a rectangle that can be added to an Image Map control.
    /// </summary>
    [Serializable]
    public class RectangleHotspot : Hotspot
    {
        #region Constructors

        /// <summary>
        /// Creates a new rectangle hotspot.
        /// </summary>
        public RectangleHotspot()
        {
        }

        /// <summary>
        /// Creates a new rectangle hotspot.
        /// </summary>
        /// <param name="key">A unique key that will be used by the Image Map control to store the hotspot in a hotspot cache.</param>
        /// <param name="enableHotspotHighlight">If set false the hotspot will not be highlighted even of the Image Map control is set to highlight hotspots.</param>
        /// <param name="topLeftVertex">The vertex of the top left hand corner of the rectangle.</param>
        /// <param name="bottomRightVertex">The vertex of the bottom right hand corner of the rectangle.</param>
        public RectangleHotspot(
            string key, bool 
            enableHotspotHighlight, 
            Vertex topLeftVertex, 
            Vertex bottomRightVertex)
            : base(key, enableHotspotHighlight, true)
        {
            _topLeftVertex = topLeftVertex;
            _bottomRightVertex = bottomRightVertex;
            RefreshRectangle();
        }

        #endregion //Constructors

        #region Fields

        protected Vertex _topLeftVertex;
        protected Vertex _bottomRightVertex;
        protected Rectangle _rectangle;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// The vertex of the top left hand corner of the rectangle.
        /// </summary>
        public Vertex TopLeftVertex
        {
            get { return _topLeftVertex; }
            set { _topLeftVertex = value; }
        }

        /// <summary>
        /// The vertex of the bottom right hand corner of the rectangle.
        /// </summary>
        public Vertex BottomRightVertex
        {
            get { return _bottomRightVertex; }
            set { _bottomRightVertex = value; }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Refreshes the rectangle object from the already captured top left and bottom right vertices.
        /// Throws an exception if the two vertices have not been already been supplied.
        /// </summary>
        protected void RefreshRectangle()
        {
            if (_topLeftVertex == null || _bottomRightVertex == null)
            {
                throw new NullReferenceException("Both top left and bottom right rectangle vertices have not been set on this hotspot.");
            }
            _rectangle = Formula.GetRectangleFromVertices(_topLeftVertex, _bottomRightVertex);
        }

        /// <summary>
        /// Determines whether a given vertex is inside given the rectangle.
        /// </summary>
        /// <param name="vertex">The vertex to test for e.g. where the user clicked on the Image Map control.</param>
        /// <returns>Returns true if the vertex is inside the rectangle.</returns>
        public override bool IsPointVisible(Vertex vertex)
        {
            RefreshRectangle();
            return Formula.IsVertexInsideRectangle(vertex, _rectangle);
        }

        /// <summary>
        /// Colors in the rectangle with the provided brush on the provided graphics.
        /// </summary>
        /// <param name="graphics">The graphics to color in on.</param>
        /// <param name="brush">Brush to use to color in.</param>
        public override void HighlightHotspot(Graphics graphics, Brush brush)
        {
            graphics.FillRectangle(brush, _rectangle);
        }

        /// <summary>
        /// Cancels the creation of the rectangle hotspot by clearing the captured vertices.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string CancelHotspot()
        {
            if (_hotspotComplete == true)
            {
                throw new Exception("Cannot a cancel a hotspot that is already complete.");
            }
            _topLeftVertex = null;
            _bottomRightVertex = null;
            _hotspotComplete = true;
            return "Rectangle hotspot canceled.";
        }

        /// <summary>
        /// Begins the creation of a rectangle hotspot. Sets the the hotspot in a state to accept new vertices to be added.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string BeginHotspot()
        {
            _topLeftVertex = null;
            _bottomRightVertex = null;
            _hotspotComplete = false;
            return "Select top left vertex of rectangle.";
        }

        /// <summary>
        /// Adds the vertex to this rectangle hotspot that should be in the creation state.
        /// Call BeginHotspot to ensure hotspot is in the creation state.
        /// </summary>
        /// <param name="vertex">The new vertex to be added to the hotspot.</param>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string AddHotspotVertex(Vertex vertex)
        {
            return AddHotspotVertex(vertex, null, null);
        }

        /// <summary>
        /// Adds the vertex to this rectangle hotspot that should be in the creation state.
        /// Call BeginHotspot to ensure hotspot is in the creation state. If the graphics object is not null
        /// the outline of the the hotspot will also be drawn on the provided graphics with the
        /// provided pen.
        /// </summary>
        /// <param name="vertex">The new vertex to be added to the hotspot.</param>
        /// <param name="graphics">The graphics to draw the outline on.</param>
        /// <param name="pen">The pen to use for drawing the outline.</param>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string AddHotspotVertex(Vertex vertex, Graphics graphics, Pen pen)
        {
            if (_hotspotComplete)
            {
                throw new Exception("May not add vertex to hotspot that is already complete.");
            }
            if (_topLeftVertex == null && _bottomRightVertex == null) //Top left vertix
            {
                _topLeftVertex = vertex;
                if (graphics != null)
                {
                    graphics.DrawLine(pen, _topLeftVertex.X, _topLeftVertex.Y, _topLeftVertex.X + 5, _topLeftVertex.Y + 5);
                }
                return "Select bottom right vertex.";
            }
            else if (_topLeftVertex != null && _bottomRightVertex == null) //Bottom right vertex
            {
                _bottomRightVertex = vertex;
                if (graphics != null)
                {
                    RefreshRectangle();
                    graphics.DrawRectangle(pen, _rectangle);
                }
                return "Complete the hotspot to add the rectangle.";
            }
            else if (_topLeftVertex != null && _bottomRightVertex != null) //Another vertex
            {
                throw new Exception("Top left and bottom right vertices of rectangle already added.");
            }
            else
            {
                throw new Exception("Top left vertix is not set and bottom right vertix is set. This is a bug. This should never happen.");
            }
        }

        /// <summary>
        /// Checks that both the top left and bottom right vertices have been added
        /// and then ends the creation of the hotspot by setting the Hotspot Complete flag to true.
        /// May not add more vertices after calling this method.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string EndHotspot()
        {
            return EndHotspot(null, null);
        }

        /// <summary>
        /// Checks that both the top left and bottom right vertices have been added
        /// and then ends the creation of the hotspot by setting the Hotspot Complete flag to true.
        /// May not add more vertices after calling this method. 
        /// Colors in the rectangle if the supplied graphics is not null with the supplied brush.
        /// </summary>
        /// <param name="graphics">The graphics to color in on.</param>
        /// <param name="brush">The brush to you use to color in.</param>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string EndHotspot(Graphics graphics, Brush brush)
        {
            if (_hotspotComplete)
            {
                throw new Exception("May not end hotspot that is already complete.");
            }
            if (_topLeftVertex == null || _bottomRightVertex == null)
            {
                throw new Exception("Top left and bottom right vertices have not been set.");
            }
            if (graphics != null)
            {
                RefreshRectangle();
                HighlightHotspot(graphics, brush);
            }
            _hotspotComplete = true;
            return "Rectangle hotspot complete.";
        }

        /// <summary>
        /// Refreshes the outline of this rectangle. Does nothing if the supplied graphics is null.
        /// </summary>
        /// <param name="graphics">The graphics to use to draw on.</param>
        /// <param name="pen">The pen to use to draw the outline.</param>
        public override void RefreshIncompleteHotspotOutline(Graphics graphics, Pen pen)
        {
            if (graphics == null)
            {
                return;
            }
            if (_topLeftVertex != null && _bottomRightVertex == null) //Only top left vertex has been added.
            {
                graphics.DrawLine(pen, _topLeftVertex.X, _topLeftVertex.Y, _topLeftVertex.X + 5, _topLeftVertex.Y + 5);
            }
            else if (_topLeftVertex != null && _bottomRightVertex != null && _rectangle != null) //Both top left and bottom right vertices have been added and rectangle has been calculated.
            {
                graphics.DrawRectangle(pen, _rectangle);
            }
        }

        #endregion //Methods
    }
}