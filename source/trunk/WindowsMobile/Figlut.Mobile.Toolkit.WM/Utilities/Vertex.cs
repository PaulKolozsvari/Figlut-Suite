namespace Figlut.Mobile.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;

    #endregion //Using Directives

    /// <summary>
    /// A helper class containing an X and Y co-ordinate. This class can used
    /// instead of a .NET Point if one wants to ensure that a set of 
    /// co-ordinates will be serialized/deserialized successfully.
    /// </summary>
    [Serializable]
    public class Vertex
    {
        #region Properties

        /// <summary>
        /// The X co-ordinate of the Vertex.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y co-ordinate of the Vertex.
        /// </summary>
        public int Y { get; set; }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Creates a .NET Point object based on this Vertex's
        /// X and Y co-ordinates.
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        #endregion //Methods
    }
}   