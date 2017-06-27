namespace Figlut.Server.Toolkit.Mmc.SettingEditors
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public interface IEditSetting
    {
        #region Methods

        void RefreshData(object selectionObject);

        bool CanApplyChanges();

        void UpdateData(object selectionObject);

        #endregion //Methods
    }
}
