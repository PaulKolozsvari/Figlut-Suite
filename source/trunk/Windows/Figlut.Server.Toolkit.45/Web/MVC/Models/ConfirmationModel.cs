namespace Figlut.Server.Toolkit.Web.MVC.Models
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class ConfirmationModel
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
        /// To be used when confirming for a single entity.
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// It is the search text specified by the user. To be used when confirming for multiple entities. i.e. used to find the entities.
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// To be used when confirming for multiple entities in a date range.
        /// </summary>
        public Nullable<DateTime> StartDate { get; set; }

        /// <summary>
        /// To be used when confirming for multiple entities in a date range.
        /// </summary>
        public Nullable<DateTime> EndDate { get; set; }

        /// <summary>
        /// The confirmation message in the confirmation dialog to be presented to the user i.e. the question being asked.
        /// </summary>
        public string ConfirmationMessage { get; set; }

        /// <summary>
        /// The ID of the parent i.e. to be used when a group of entities belonging to a parent is loaded and needs to be identified/deleted.
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// A description of the to be used when a group of entities belonging to a parent is loaded and needs to be identified/deleted.
        /// </summary>
        public string ParentCaption { get; set; }

        /// <summary>
        /// Whether or not to display a wait dialog while posting back to the PostBackControllerAction.
        /// </summary>
        public bool ShowWaitDialog { get; set; }

        /// <summary>
        /// The wait message to display on the wait dialog.
        /// </summary>
        public string WaitMessage { get; set; }

        /// <summary>
        /// The message to display after wait dialog has been closed after a successful postback to the PostBackControllerAction.
        /// </summary>
        public string WaitDialogSuccessMessage { get; set; }

        #endregion //Properties
    }
}
