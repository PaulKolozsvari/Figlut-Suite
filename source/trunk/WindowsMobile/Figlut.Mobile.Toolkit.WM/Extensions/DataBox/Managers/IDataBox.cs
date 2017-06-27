namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Managers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public interface IDataBox
    {
        #region Events

        event OnDataBoxPropertiesChanged OnDataBoxPropertiesChanged;

        #endregion //Events

        #region Methods

        bool RefreshDataBox(bool fromServer, bool fromFile, bool presentLoadDataBoxForm, bool presentOpenFileDialogBox, bool refreshInputControls);

        void CancelEntityUpdate();

        void PrepareForEntityUpdate();

        void AddEntity();

        void DeleteEntity();

        void ManageChildren();

        void ViewParent();

        void Save();

        void SetFiltersEnabled(bool enable);

        void SelectEntity(Guid dataBoxEntityId);

        #endregion //Methods
    }
}