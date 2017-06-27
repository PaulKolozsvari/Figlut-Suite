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
    /// A hotspot the shape of a polygon (multiple vertices) that can be added to an Image Map control.
    /// </summary>
    [Serializable]
    public class PolygonHotspot : Hotspot
    {
        #region Constructors

        /// <summary>
        /// Creates a new polygon hotspot.
        /// </summary>
        public PolygonHotspot()
        {
            _vertices = new List<Vertex>();
        }

        /// <summary>
        /// Creates a new polygon hotspot.
        /// </summary>
        /// <param name="key">A unique key that will be used by the Image Map control to store the hotspot in a hotspot cache.</param>
        /// <param name="enableHotspotHighlight">If set false the hotspot will not be highlighted even of the Image Map control is set to highlight hotspots.</param>
        /// <param name="vertices">A list of polygon vertices.</param>
        public PolygonHotspot(string key, bool enableHotspotHighlight, List<Vertex> vertices)
            : base(key, enableHotspotHighlight, true)
        {
            _vertices = vertices;
        }

        #endregion //Constructors

        #region Fields

        protected List<Vertex> _vertices;

        #endregion //Fields

        /// <summary>
        /// The list of vertices making up the polygon.
        /// </summary>
        public List<Vertex> Vertices
        {
            get { return _vertices; }
            set
            {
                _vertices = value;
            }
        }

        #region Methods

        /// <summary>
        /// Determines if a supplied vertex is located inside this polygon.
        /// </summary>
        /// <param name="_vertex">The vertex to test for e.g. where the user clicked on the Image Map control.</param>
        /// <returns>Returns true if the vertex is inside the polygon.</returns>
        public override bool IsPointVisible(Vertex vertex)
        {
            return Formula.IsVertexInsideConcavePolygon(vertex, _vertices);
        }

        /// <summary>
        /// Colors in the polygon with the provided brush on the provided graphics.
        /// </summary>
        /// <param name="graphics">The graphics to color in with.</param>
        /// <param name="brush">Brush to use to color in.</param>
        public override void HighlightHotspot(Graphics graphics, Brush brush)
        {
            if (_vertices == null || _vertices.Count < 1)
            {
                throw new Exception(string.Format("No points allocated to polygon {0}.", Key));
            }
            List<Point> points = new List<Point>();
            _vertices.ForEach(v => points.Add(v.ToPoint()));
            graphics.FillPolygon(brush, points.ToArray());
        }

        /// <summary>
        /// Cancels the creation of the polygon hotspot by clearing the captured vertices.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string CancelHotspot()
        {
            if (_hotspotComplete == true)
            {
                throw new Exception("Cannot a cancel a hotspot that is already complete.");
            }
            _vertices.Clear();
            _hotspotComplete = true;
            return "Polygon hotspot complete.";
        }

        /// <summary>
        /// Begins the creation of a polygon hotspot. Sets the the hotspot in a state to accept new vertices to be added.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string BeginHotspot()
        {
            if (_vertices == null)
            {
                _vertices = new List<Vertex>();
            }
            _vertices.Clear();
            _hotspotComplete = false;
            return "Select at least 3 vertices of polygon.";
        }

        /// <summary>
        /// Adds the vertex to this polygon hotspot that should be in the creation state.
        /// Call BeginHotspot to ensure hotspot is in the creation state.
        /// </summary>
        /// <param name="vertex">The new vertex to be added to the hotspot.</param>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string AddHotspotVertex(Vertex vertex)
        {
            return AddHotspotVertex(vertex, null, null);
        }

        /// <summary>
        /// Adds the vertex to this polygon hotspot that should be in the creation state.
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
            if (_vertices.Count > 0 && graphics != null)
            {
                Vertex lastVertex = _vertices[_vertices.Count - 1];
                graphics.DrawLine(pen, lastVertex.X, lastVertex.Y, vertex.X, vertex.Y);
            }
            else //This is the first point
            {
                graphics.DrawLine(pen, vertex.X, vertex.Y, vertex.X + 5, vertex.Y + 5);
            }
            _vertices.Add(vertex);
            return "Select next vertex.";
        }

        /// <summary>
        /// Checks that at least three vertices have been added to the polygon
        /// and then ends the creation of the hotspot by setting the Hotspot Complete flag to true.
        /// May not add more vertices after calling this method.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string EndHotspot()
        {
            return EndHotspot(null, null);
        }

        /// <summary>
        /// Checks that at least three vertices have been added to the polygon
        /// and then ends the creation of the hotspot by setting the Hotspot Complete flag to true.
        /// May not add more vertices after calling this method.
        /// Colors in the polygon if the supplied graphics is not null with the supplied brush.
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
            if (_vertices.Count < 3)
            {
                throw new Exception("Polygon needs to at least 3 vertices.");
            }
            if (graphics != null)
            {
                Vertex firstVertex = _vertices[0];
                Vertex lastVertex = _vertices[_vertices.Count - 1];
                HighlightHotspot(graphics, brush);
            }
            _hotspotComplete = true;
            return "Polygon hotspot complete.";
        }

        /// <summary>
        /// Refreshes the outline of this polygon. Does nothing if the supplied graphics is null.
        /// </summary>
        /// <param name="graphics">The graphics to use to draw on.</param>
        /// <param name="pen">The pen to use to draw the outline.</param>
        public override void RefreshIncompleteHotspotOutline(Graphics graphics, Pen pen)
        {
            if (graphics == null)
            {
                return;
            }
            if (_vertices.Count == 1)
            {
                Vertex onlyVertex = _vertices[0];
                graphics.DrawLine(pen, onlyVertex.X, onlyVertex.Y, onlyVertex.X + 5, onlyVertex.Y + 5);
            }
            int lastVertex = _vertices.Count - 1; ////The lastVertex is the previous one to the first.
            for (int i = 0; i < _vertices.Count; i++)
            {
                if (!_hotspotComplete && i == lastVertex)
                {
                    break; //Don't draw the last line completing the hotspot. Wait for the user to complete the hotspot.
                }
                Vertex startOfLine = _vertices[i];
                Vertex endOfLine = i == lastVertex ? _vertices[0] : _vertices[i + 1];
                graphics.DrawLine(pen, startOfLine.X, startOfLine.Y, endOfLine.X, endOfLine.Y);
            }
        }

        #endregion //Methods
    }
}