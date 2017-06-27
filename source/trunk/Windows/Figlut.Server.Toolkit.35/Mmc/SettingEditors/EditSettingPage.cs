namespace Figlut.Server.Toolkit.Mmc.SettingEditors
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.ManagementConsole;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;

    #endregion //Using Directives

    public class EditSettingPage : PropertyPage
    {
        #region Constructors

        public EditSettingPage(object selectionObject)
        {
            SettingItem selectedSetting = (SettingItem)selectionObject;
            if (selectedSetting.SettingType.Equals(typeof(string)))
            {
                _childSettingControl = new EditTextSettingControl(this);
            }
            else if (selectedSetting.SettingType.IsEnum)
            {
                _childSettingControl = new EditEnumSettingControl(this);
            }
            else if (selectedSetting.SettingType.Equals(typeof(Boolean)))
            {
                _childSettingControl = new EditBoolSettingControl(this);
            }
            else if (selectedSetting.SettingType.Equals(typeof(Int64)))
            {
                _childSettingControl = new EditLongSettingControl(this);
            }
            else
            {
                throw new NotImplementedException(string.Format(
                    "No supported type of {0} for setting {1}.",
                    selectedSetting.SettingType.FullName,
                    selectedSetting.SettingDisplayName));
            }
            this.Title = "Edit setting";
            this.Control = (UserControl)_childSettingControl;
        }

        #endregion //Constructors

        #region Fields

        private IEditSetting _childSettingControl;

        #endregion //Fields

        #region Methods

        protected override void OnInitialize()
        {
            try
            {
                base.OnInitialize();
                _childSettingControl.RefreshData(this.ParentSheet.SelectionObject);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        /// <summary>
        /// Sent to every page in the property sheet to indicate that the user has clicked 
        /// the Apply button and wants all changes to take effect.
        /// </summary>
        protected override bool OnApply()
        {
            try
            {
                if (this.Dirty && _childSettingControl.CanApplyChanges())
                {
                    _childSettingControl.UpdateData(this.ParentSheet.SelectionObject); //Save changes.
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return false; //Something invalid was entered.
        }

        /// <summary>
        /// Sent to every page in the property sheet to indicate that the user has clicked the OK 
        /// or Close button and wants all changes to take effect.
        /// </summary>
        protected override bool OnOK()
        {
            try
            {
                return this.OnApply();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return false;
        }

        /// <summary>
        /// Indicates that the user wants to cancel the property sheet.
        /// Default implementation allows cancel operation.
        /// </summary>
        protected override void OnCancel()
        {
            try
            {
                _childSettingControl.RefreshData(this.ParentSheet.SelectionObject);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        /// <summary>
        /// Notifies a page that the property sheet is getting destoyed. 
        /// Use this notification message as an opportunity to perform cleanup operations.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        #endregion //Methods
    }
}