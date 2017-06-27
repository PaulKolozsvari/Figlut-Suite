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
    /// A hotspot the shape of a circle that can be added to an Image Map control.
    /// </summary>
    [Serializable]
    public class CircleHotspot : Hotspot
    {
        #region Constructors

        /// <summary>
        /// Creates a new circle hotspot.
        /// </summary>
        public CircleHotspot()
        {
        }

        /// <summary>
        /// Creates a new circle hotspot.
        /// </summary>
        /// <param name="key">A unique key that will be used by the Image Map control to store the hotspot in a hotspot cache.</param>
        /// <param name="enableHotspotHighlight">If set false the hotspot will not be highlighted even of the Image Map control is set to highlight hotspots.</param>
        /// <param name="centerVertex">The center vertex of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public CircleHotspot(
            string key, 
            bool enableHotspotHighlight, 
            Vertex centerVertex, 
            int radius)
            : base(key, enableHotspotHighlight, true)
        {
            _centerVertex = centerVertex;
            _radius = radius;
        }

        #endregion //Constructors

        #region Fields

        protected Vertex _centerVertex;
        protected int _radius;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// The center vertex of th circle.
        /// </summary>
        public Vertex CenterVertex
        {
            get { return _centerVertex; }
            set { _centerVertex = value; }
        }

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public int Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Determines if a supplied vertex is located inside this circle.
        /// </summary>
        /// <param name="vertex">The vertex to test for e.g. where the user clicked on the Image Map control.</param>
        /// <returns>Returns true if the vertex is inside the circle.</returns>
        public override bool IsPointVisible(Vertex vertex)
        {
            return Formula.IsVertexInsideCircle(vertex, _centerVertex, _radius);
        }

        /// <summary>
        /// Colors in the circle with the provided brush on the provided graphics.
        /// </summary>
        /// <param name="graphics">The graphics to color in on.</param>
        /// <param name="brush">Brush to use to color in.</param>
        public override void HighlightHotspot(Graphics graphics, Brush brush)
        {
            int xStartingPoint = _centerVertex.X - _radius;
            int yStartingPoint = _centerVertex.Y - _radius;
            int diameter = _radius * 2;
            graphics.FillEllipse(brush, xStartingPoint, yStartingPoint, diameter, diameter);
        }

        /// <summary>
        /// Cancels the creation of the circle hotspot by clearing the captured vertices.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string CancelHotspot()
        {
            if (_hotspotComplete == true)
            {
                throw new Exception("Cannot a cancel a hotspot that is already complete.");
            }
            _centerVertex = null;
            _radius = 0;
            _hotspotComplete = true;
            return "Circle hotspot complete.";
        }

        /// <summary>
        /// Begins the creation of a circle hotspot. Sets the the hotspot in a state to accept new vertices to be added.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string BeginHotspot()
        {
            _centerVertex = null;
            _radius = 0;
            _hotspotComplete = false;
            return "Select circle center.";
        }

        /// <summary>
        /// Adds the vertex to this circle hotspot that should be in the creation state.
        /// Call BeginHotspot to ensure hotspot is in the creation state.
        /// </summary>
        /// <param name="vertex">The new vertex to be added to the hotspot.</param>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string AddHotspotVertex(Vertex vertex)
        {
            return AddHotspotVertex(vertex, null, null);
        }

        /// <summary>
        /// Adds the vertex to this circle hotspot that should be in the creation state.
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
            if (_centerVertex == null && _radius == 0) //Center
            {
                _centerVertex = vertex;
                if (graphics != null)
                {
                    graphics.DrawLine(pen, _centerVertex.X, _centerVertex.Y, _centerVertex.X + 5, _centerVertex.Y + 5);
                }
                return "Select circle edge.";
            }
            else if (_centerVertex != null && _radius == 0) //Radius
            {
                _radius = Formula.GetRadius(vertex, _centerVertex);
                if (graphics != null)
                {
                    int xStartingPoint = _centerVertex.X - _radius;
                    int yStartingPoint = _centerVertex.Y - _radius;
                    int diameter = _radius * 2;
                    graphics.DrawEllipse(pen, xStartingPoint, yStartingPoint, diameter, diameter);
                }
                return "Complete the hotspot to add the circle.";
            }
            else if (_centerVertex != null && _radius != 0) //Another vertex
            {
                throw new Exception("Center and circle edge already added.");
            }
            else
            {
                throw new Exception("Center not set and circle edge vertix is set. This is a bug. This should never happen.");
            }
        }

        /// <summary>
        /// Checks that both the center and edge of the circle vertices have been added
        /// and then ends the creation of the hotspot by setting the Hotspot Complete flag to true.
        /// May not add more vertices after calling this method.
        /// </summary>
        /// <returns>Returns the status string to be displayed on a status bar i.e. what the next step for the user is.</returns>
        public override string EndHotspot()
        {
            return EndHotspot(null, null); ;
        }

        /// <summary>
        /// Checks that both the center and edge of the circle vertices have been added
        /// and then ends the creation of the hotspot by setting the Hotspot Complete flag to true.
        /// May not add more vertices after calling this method. 
        /// Colors in the circle if the supplied graphics is not null with the supplied brush.
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
            if (_centerVertex == null || _radius == 0)
            {
                throw new Exception("Center and circle edge vertices have not been set.");
            }
            if (graphics != null)
            {
                HighlightHotspot(graphics, brush);
            }
            _hotspotComplete = true;
            return "Circle hotspot complete.";
        }

        /// <summary>
        /// Refreshes the outline of this circle. Does nothing if the supplied graphics is null.
        /// </summary>
        /// <param name="graphics">The graphics to use to draw on.</param>
        /// <param name="pen">The pen to use to draw the outline.</param>
        public override void RefreshIncompleteHotspotOutline(Graphics graphics, Pen pen)
        {
            if (graphics == null)
            {
                return;
            }
            if (_centerVertex != null) //Center vertex has been addeded.
            {
                graphics.DrawLine(pen, _centerVertex.X, _centerVertex.Y, _centerVertex.X + 5, _centerVertex.Y + 5);
            }
            if (_centerVertex != null && _radius != 0) //Center vertex has been added and radius has been calculated.
            {
                int xStartingPoint = _centerVertex.X - _radius;
                int yStartingPoint = _centerVertex.Y - _radius;
                int diameter = _radius * 2;
                graphics.DrawEllipse(pen, xStartingPoint, yStartingPoint, diameter, diameter);
            }
        }

        #endregion //Methods
    }
}