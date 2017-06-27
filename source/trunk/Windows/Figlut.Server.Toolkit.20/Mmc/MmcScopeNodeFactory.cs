namespace Figlut.Server.Toolkit.Mmc
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.ManagementConsole;
    using Figlut.Server.Toolkit.Mmc.Forms;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public class MmcScopeNodeFactory
    {
        #region Methods

        public static ScopeNode BuildRootNode(
            string displayName,
            Nullable<int> imageIndex,
            Nullable<int> selectedImageIndex,
            Settings settings)
        {
            ScopeNode result = new ScopeNode() { DisplayName = displayName };
            result.Tag = settings;
            ValidateMandatoryNodeFields(result);
            SetImageIndices(result, imageIndex, selectedImageIndex);
            return result;
        }

        public static ScopeNode BuildSettingsCategoryNode(
            string displayName,
            string formViewDescriptionDisplayName,
            SettingsCategoryInfo settingsCategoryInfo,
            Nullable<int> imageIndex,
            Nullable<int> selectedImageIndex,
            bool hideExpandIcon)
        {
            ScopeNode result = new ScopeNode(hideExpandIcon) { DisplayName = displayName };
            ValidateMandatoryNodeFields(result);
            result.ViewDescriptions.Add(new FormViewDescription() //Tells the snap-in how to construct a form and what control to to add it to.
            {
                DisplayName = formViewDescriptionDisplayName,
                ViewType = typeof(SettingsFormView), //The Type of the form to instantiate.
                ControlType = typeof(SettingsControl), //The Type of the control to instantiate and place onto the form.
                Tag = settingsCategoryInfo, //Informs the SettingsControl which CATEGORY of settings to display i.e. i.e. the properties of the Settings class which have an attribute with the same Category name set.
            });
            result.ViewDescriptions.DefaultIndex = 0;
            SetImageIndices(result, imageIndex, selectedImageIndex);
            return result;
        }

        private static void SetImageIndices(
            ScopeNode node,
            Nullable<int> imageIndex, 
            Nullable<int> selectedImageIndex)
        {
            if (imageIndex.HasValue)
            {
                node.ImageIndex = imageIndex.Value;
            }
            if (selectedImageIndex.HasValue)
            {
                node.SelectedImageIndex = selectedImageIndex.Value;
            }
        }

        private static void ValidateMandatoryNodeFields(ScopeNode node)
        {
            if (string.IsNullOrEmpty(node.DisplayName))
            {
                throw new NullReferenceException(string.Format(
                    "DisplayName may not be null or empty on MMC {0}.",
                    typeof(ScopeNode).FullName));
            }
        }

        #endregion //Methods
    }
}