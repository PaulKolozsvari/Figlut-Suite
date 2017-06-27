namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Events.MainMenu
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Mobile.Toolkit.Extensions.DataBox;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;

    #endregion //Using Directives

    public class MenuClickArgs : EventArgs
    {
        #region Constructors

        public MenuClickArgs(
            SqlCeDatabaseTable currentTable,
            object selectedEntity,
            bool dataBoxInUpdateMode)
        {
            _currentTable = currentTable;
            _selectedEntity = selectedEntity;
            _dataBoxInUpdateMode = dataBoxInUpdateMode;
        }

        #endregion //Constructors

        #region Fields

        protected SqlCeDatabaseTable _currentTable;
        protected object _selectedEntity;
        protected bool _dataBoxInUpdateMode;

        #endregion //Fields

        #region Properties

        public SqlCeDatabaseTable CurrentTable
        {
            get { return _currentTable; }
        }

        public object SelectedEntity
        {
            get { return _selectedEntity; }
        }

        public bool DataBoxInUpdateMode
        {
            get { return _dataBoxInUpdateMode; }
        }

        #endregion //Properties
    }
}
