namespace Figlut.Server.Toolkit.Web.MVC.Models
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class InformationModel
    {
        #region Properties

        /// <summary>
        /// The controller action that the confirmation dialog needs to be posted back to.
        /// </summary>
        public string PostBackControllerAction { get; set; }

        /// <summary>
        /// The controller that the confirmation dialog needs to be posted back to.
        /// </summary>
        public string PostBackControllerName { get; set; }

        /// <summary>
        /// The HTML div element that encapsulates the confirmation dialog.
        /// </summary>
        public string DialogDivId { get; set; }

        /// <summary>
        /// The message to display on the wait dialog.
        /// </summary>
        public string InformationMessage { get; set; }

        /// <summary>
        /// To be used when confirming for a single entity.
        /// </summary>
        public Nullable<Guid> Identifier { get; set; }

        #endregion //Properties
    }
}
