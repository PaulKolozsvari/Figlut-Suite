namespace Figlut.Server.Toolkit.Mmc.Forms
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Text;
    using System.Windows.Forms;
    using Microsoft.ManagementConsole;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public partial class SettingsControl : UserControl, IFormViewControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            lstvSettings.View = System.Windows.Forms.View.Details; //How the list view should display items.

            //Column headers displayed on the list view.
            lstvSettings.Columns.Add(new ColumnHeader()
            {
                Text = "Setting",
                Width = 250
            });
            lstvSettings.Columns.Add(new ColumnHeader()
            {
                Text = "Value",
                Width = 250
            });
            lstvSettings.Columns.Add(new ColumnHeader()
            {
                Text = "Description",
                Width = 1000
            });
        }

        #region Fields

        private SettingsFormView _settingsFormView = null; //The form to which this control is associated with.

        #endregion //Fields

        public ListView ListView
        {
            get { return lstvSettings; }
        }

        #region Methods

        /// <summary>
        /// Cache the associated Form View and add the actions
        /// </summary>
        /// <param name="parentSelectionFormView">Containing form</param>
        ///         
        void IFormViewControl.Initialize(FormView parentSelectionFormView)
        {
            _settingsFormView = (SettingsFormView)parentSelectionFormView;

            // Add the actions
            _settingsFormView.SelectionData.ActionsPaneItems.Clear();
            _settingsFormView.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action(
                DataShaper.ShapeCamelCaseString(SnapInAction.Edit.ToString()), 
                "Shows the Names of the selected Items in the FormView's ListView.", 
                -1,
                SnapInAction.Edit)); //The tag is the action identifier.
        }

        /// <summary>
        /// Populate the list with sample data
        /// </summary>
        /// <param name="users">array of user data to add to the list</param>
        public void RefreshData()
        {
            lstvSettings.Items.Clear();
            SettingsCategoryInfo settingsCategoryInfo = _settingsFormView.ViewDescriptionTag as SettingsCategoryInfo; //Provides the settings and the category in the settings to be used to populate the list view.
            if (settingsCategoryInfo == null)
            {
                throw new NullReferenceException(string.Format(
                    "Expected a {0} in the tag of the {1}, but none was supplied.",
                    typeof(SettingsCategoryInfo).FullName,
                    typeof(SettingsFormView).FullName));
            }
            foreach (SettingItem s in settingsCategoryInfo.Settings.GetSettingsByCategory(settingsCategoryInfo, this))
            {
                lstvSettings.Items.Add(s.ListViewItem);
            }
        }

        /// <summary>
        /// Build string of selected users
        /// </summary>
        /// <returns></returns>
        private SettingItem GetSelectedSetting()
        {
            if (lstvSettings.SelectedItems.Count < 1)
            {
                return null;
            }
            return (SettingItem)lstvSettings.SelectedItems[0].Tag;
        }

        #endregion //Methods

        #region Event Handlers

        /// <summary>
        /// Updates the FormView's selected data context when the user clicks on a setting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstvSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstvSettings.SelectedItems.Count == 0)
            {
                _settingsFormView.SelectionData.Clear();
            }
            else
            {
                // update MMC with the current selection information (tell the form what has been selected.
                _settingsFormView.SelectionData.Update(GetSelectedSetting(), lstvSettings.SelectedItems.Count > 1, null, null);

                // update action pane (right hand side) selected data menu's title
                _settingsFormView.SelectionData.DisplayName = 
                    ((lstvSettings.SelectedItems.Count == 1) ? 
                    lstvSettings.SelectedItems[0].Text : 
                    "Selected a setting to edit");
            }
        }

        /// <summary>
        /// Handle mouseclick and use MMC to show context menu if necessary 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstvSettings_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            bool rightClickedOnSelection = false;
            ListViewItem rightClickedItem = lstvSettings.GetItemAt(e.X, e.Y);
            if (rightClickedItem == null || rightClickedItem.Selected == false) //User needs have clicked on an item and that item needs to be selected.
            {
                rightClickedOnSelection = false;
            }
            else
            {
                rightClickedOnSelection = true;
            }
            rightClickedOnSelection = rightClickedItem != null && rightClickedItem.Selected; //User needs have clicked on an item and that item needs to be selected.
            _settingsFormView.ShowContextMenu(PointToScreen(e.Location), rightClickedOnSelection); //Show context menu (containing the defined actions) at the point where the user clicked.
        } 

        #endregion //Event Handlers
    }
}
