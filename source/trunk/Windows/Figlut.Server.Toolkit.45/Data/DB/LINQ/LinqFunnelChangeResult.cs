namespace Figlut.Server.Toolkit.Data.DB.LINQ
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class LinqFunnelChangeResult
    {
        #region Properties

        /// <summary>
        /// A surrogate key which uniquely identifies the entity e.g. a GUID.
        /// </summary>
        public object SurrogateKey { get; set; }

        /// <summary>
        /// A value that uniquely identifies the entity.
        /// </summary>
        public object EntityIdentifier { get; set; }

        /// <summary>
        /// The function which was being performed.
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// The date on which the change was made.
        /// </summary>
        public DateTime DateChanged { get; set; }

        /// <summary>
        /// The entity on which the change was made.
        /// </summary>
        public string EntityChanged { get; set; }

        /// <summary>
        /// The field on the entity which was changed.
        /// </summary>
        public string FieldChanged { get; set; }

        /// <summary>
        /// The original value of the field which was changed on the entity.
        /// </summary>
        public object OriginalValue { get; set; }

        /// <summary>
        /// The new value that was set on the field which was changed on the entity.
        /// </summary>
        public object NewValue { get; set; }

        #endregion //Properties
    }
}