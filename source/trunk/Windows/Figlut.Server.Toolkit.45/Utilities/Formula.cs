namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using System.Drawing;

    #endregion //Using Directives

    /// <summary>
    /// A helper class containing geometry and other maths methods.
    /// </summary>
    public class Formula
    {
        #region Methods

        /// <summary>
        /// Creates a rectangle given the top left and bottom right vertices.
        /// </summary>
        /// <param name="topLeftVertex">The vertex of the top left hand corner of the rectangle.</param>
        /// <param name="bottomRightVertex">The vertex of the bottom right hand corner of the rectangle.</param>
        /// <returns></returns>
        public static Rectangle GetRectangleFromVertices(Vertex topLeftVertex, Vertex bottomRightVertex)
        {
            return new Rectangle(
                Convert.ToInt32(topLeftVertex.X),
                Convert.ToInt32(topLeftVertex.Y),
                Convert.ToInt32((bottomRightVertex.X - topLeftVertex.X)),
                Convert.ToInt32((bottomRightVertex.Y - topLeftVertex.Y)));
        }

        /// <summary>
        /// Calculates the area of any irregular polygon given its vertices.
        /// http://www.mathopenref.com/coordpolygonarea2.html
        /// </summary>
        /// <param name="vertices">The list of vertices of the polygon.</param>
        /// <returns>Returns the area of the polygon.</returns>
        public static double GetPolygonArea(List<Vertex> vertices)
        {
            double result = 0;
            int lastVertex = vertices.Count - 1; //The lastVertex is the previous one to the first.
            for (int i = 0; i < vertices.Count; i++)
            {
                result = result + ((vertices[lastVertex].X + vertices[i].X) * (vertices[lastVertex].Y - vertices[i].Y));
                lastVertex = i;
            }
            return Math.Abs(result / 2);
        }

        /// <summary>
        /// Calculates the area of any triangle given it's three vertices.
        /// http://www.mathopenref.com/coordtrianglearea.html
        /// </summary>
        /// <returns>Returns the area of the triangle.</returns>
        public static double GetTriangleArea(Vertex a, Vertex b, Vertex c)
        {
            double result = (a.X * (b.Y - c.Y)) +
                            (b.X * (c.Y - a.Y)) +
                            (c.X * (a.Y - b.Y));
            result = Math.Abs(result / 2);
            return result;
        }

        /// <summary>
        /// Determines the distance from the center of a circle to the given vertex.
        /// It basically gets the distance between two points.
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="centerVertex"></param>
        /// <returns></returns>
        public static int GetRadius(Vertex vertex, Vertex centerVertex)
        {
            /*Use Pythagoras to determine the distance from the point to the center of the circle i.e. radius.
            * If that distance is not less than the radius than the point is outside the circle.*/
            double xDistance = centerVertex.X - vertex.X;
            double yDistance = centerVertex.Y - vertex.Y;
            double hypotenuseSquared = Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2);
            return Convert.ToInt32(Math.Sqrt(hypotenuseSquared));
        }

        /// <summary>
        /// Determines if a supplied vertex is located inside this circle.
        /// </summary>
        /// <param name="vertex">The vertex to test for e.g. where the user clicked on the Image Map control.</param>
        /// <param name="centerVertex">The vertex marking the center of the circle.</param>
        /// <param name="circleRadius">The radius of the circle.</param>
        /// <returns>Returns true if the vertex is located inside circle.</returns>
        public static bool IsVertexInsideCircle(Vertex vertex, Vertex centerVertex, int circleRadius)
        {
            /*Use Pythagoras to determine the distance from the point to the center of the circle.
            * If that distance is not less than the radius than the point is outside the circle.*/
            double xDistance = centerVertex.X - vertex.X;
            double yDistance = centerVertex.Y - vertex.Y;
            double hypotenuseSquared = Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2);
            return hypotenuseSquared <= Math.Pow(circleRadius, 2);
        }

        /// <summary>
        /// Determines whether a given vertex is inside a convex polygon made up of the provided vertices.
        /// http://www.mathopenref.com/polygonconvex.html
        /// This method is not accurate for concave polygons.
        /// </summary>
        /// <param name="vertex">The vertex to test for e.g. where the user clicked on the Image Map control.</param>
        /// <param name="polygonVertices">The vertices of the making up the polygon.</param>
        /// <returns>Returns true if the vertix is inside the polygon.</returns>
        public static bool IsVertexInsideNonConvexPolygon(Vertex vertex, List<Vertex> polygonVertices)
        {
            double realPolygonArea = Formula.GetPolygonArea(polygonVertices);
            double totalTriangleAreas = 0;
            int lastVertex = polygonVertices.Count - 1; ////The lastVertex is the previous one to the first.
            /*Slice the polygon into triangles and calculate each of their areas. If the sum of the 
             * triangle areas is equal to the realPolygonArea then point is inside the polygon*/
            for (int i = 0; i < polygonVertices.Count; i++)
            {
                Vertex a = polygonVertices[i];
                Vertex b = polygonVertices[lastVertex];
                Vertex c = vertex;
                totalTriangleAreas += Formula.GetTriangleArea(a, b, c);
                lastVertex = i;
            }
            bool result = Math.Round(realPolygonArea, 11) == Math.Round(totalTriangleAreas, 11);
            return result;
        }

        /// <summary>
        /// Determines whether a polygon is Convex.
        /// http://stackoverflow.com/questions/471962/how-do-determine-if-a-polygon-is-complex-convex-nonconvex
        /// </summary>
        /// <param name="polygonVertices"></param>
        /// <returns></returns>
        public static bool IsPolygonConvex(List<Vertex> polygonVertices)
        {
            if (polygonVertices.Count < 4)
            {
                return true;
            }
            bool sign = false;
            int n = polygonVertices.Count;
            for (int i = 0; i < n; i++)
            {
                double dx1 = polygonVertices[(i + 2) % n].X - polygonVertices[(i + 1) % n].X;
                double dy1 = polygonVertices[(i + 2) % n].Y - polygonVertices[(i + 1) % n].Y;
                double dx2 = polygonVertices[i].X - polygonVertices[(i + 1) % n].X;
                double dy2 = polygonVertices[i].Y - polygonVertices[(i + 1) % n].Y;
                double zcrossproduct = dx1 * dy2 - dy1 * dx2;
                if (i == 0)
                    sign = zcrossproduct > 0;
                else
                {
                    if (sign != (zcrossproduct > 0))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether a given vertex is inside a concave polygon made up of the provided vertices.
        /// http://alienryderflex.com/polygon/
        /// http://www.mathopenref.com/polygonconcave.html
        /// </summary>
        /// <param name="vertex">The vertex to test for e.g. where the user clicked on the Image Map control.</param>
        /// <param name="polygonVertices">The vertices of the making up the polygon.</param>
        /// <returns>Returns true if the vertix is inside the polygon.</returns>
        public static bool IsVertexInsideConcavePolygon(Vertex vertex, List<Vertex> polygonVertices)
        {
            double x = vertex.X;
            double y = vertex.Y;
            List<double> polyX = new List<double>();
            List<double> polyY = new List<double>();
            foreach (Vertex v in polygonVertices) //It's more accurate working with double or decimal numbers.
            {
                polyX.Add(v.X);
                polyY.Add(v.Y);
            }

            int i, j = polygonVertices.Count - 1;
            bool oddNodes = false;
            for (i = 0; i < polygonVertices.Count; i++)
            {
                if ((polyY[i] < y && polyY[j] >= y
                || polyY[j] < y && polyY[i] >= y)
                && (polyX[i] <= x || polyX[j] <= x))
                {
                    oddNodes ^= (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x);
                }
                j = i;
            }
            return oddNodes;
        }

        /// <summary>
        /// Determines whether a given vertex is inside given a rectangle.
        /// </summary>
        /// <param name="vertex">The vertex to test for e.g. where the user clicked on the Image Map control.</param>
        /// <param name="rectangle">The rectangle to check against i.e. if the vertex is located inside of it.</param>
        /// <returns>Returns true if the vertex is inside the rectangle.</returns>
        public static bool IsVertexInsideRectangle(Vertex vertex, Rectangle rectangle)
        {
            Region region = new Region(rectangle);
            return region.IsVisible(vertex.ToPoint());
        }

        /// <summary>
        /// Calculates the percentage of a value out of a total value e.g. 8 (value) out of 10 (totalValue) = 80.
        /// </summary>
        /// <param name="value">The value in question.</param>
        /// <param name="totalValue">The total value to calculate the percentage out of.</param>
        /// <param name="decimals">The number of decimal places to include in the percentage result.</param>
        /// <returns></returns>
        public static double GetPercentage(int value, int totalValue, int decimals)
        {
            return Math.Round((Convert.ToDouble(value) / totalValue) * 100, decimals);
        }

        /// <summary>
        /// Calculates the percentage of a value out of a total value e.g. 8 (value) out of 10 (totalValue) = 80.
        /// </summary>
        /// <param name="value">The value in question.</param>
        /// <param name="totalValue">The total value to calculate the percentage out of.</param>
        /// <param name="decimals">The number of decimal places to include in the percentage result.</param>
        /// <returns></returns>
        public static double GetPercentage(long value, long totalValue, int decimals)
        {
            return Math.Round((Convert.ToDouble(value) / totalValue) * 100, decimals);
        }

        #endregion //Methods
    }
}