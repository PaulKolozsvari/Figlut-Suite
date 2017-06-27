namespace Figlut.Server.Toolkit.Mmc.Forms
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.ManagementConsole;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Mmc.SettingEditors;

    #endregion //Using Directives

    /// <summary>
    /// FormView to display Winforms controls
    /// </summary>
    public class SettingsFormView : FormView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsFormView()
        {
        }

        #region Fields

        private SettingsControl _settingsControl = null;

        #endregion //Fields

        #region Methods

        /// <summary>
        /// Handle any setup necessary
        /// </summary>
        /// <param name="status">asynchronous status for updating the console</param>
        protected override void OnInitialize(AsyncStatus status)
        {
            base.OnInitialize(status);
            _settingsControl = (SettingsControl)this.Control; //Get typed reference to the hosted control setup by the FormViewDescription
            _settingsControl.RefreshData(); // Tell the control to load the data (load settings by category).
            this.DescriptionBarText = string.Format("Figlut Suite {0} settings", this.ViewDescriptionTag.ToString().ToLower());
        }

        /// <summary>
        /// Handle triggered action
        /// </summary>
        /// <param name="action">triggered action</param>
        /// <param name="status">asynchronous status to update console</param>
        protected override void OnSelectionAction(Microsoft.ManagementConsole.Action action, AsyncStatus status)
        {
            try
            {
                SnapInAction settingAction = (SnapInAction)action.Tag;
                switch (settingAction)
                {
                    case SnapInAction.Edit:
                        this.SelectionData.ShowPropertySheet("Setting");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        /// <summary>
        /// OnAddPropertyPages is used to get the property pages to show. 
        /// (triggered by SelectionData.ShowPropertySheet)
        /// </summary>
        /// <param name="propertyPageCollection">property pages</param>
        protected override void OnAddPropertyPages(PropertyPageCollection propertyPageCollection)
        {
            try
            {
                if (_settingsControl.ListView.SelectedItems.Count < 1)
                {
                    throw new Exception("there should be at least one selection");
                }
                propertyPageCollection.Add(new EditSettingPage(this.SelectionData.SelectionObject));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion //Methods
    }
}
