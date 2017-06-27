namespace Figlut.Server.Toolkit.Extensions.DataBox.Events.MainMenu
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Extensions.DataBox;

    #endregion //Using Directives

    public class MenuClickArgs : EventArgs
    {
        #region Constructors

        public MenuClickArgs(
            SqlDatabaseTable currentTable,
            object selectedEntity,
            bool dataBoxInUpdateMode)
        {
            _currentTable = currentTable;
            _selectedEntity = selectedEntity;
            _dataBoxInUpdateMode = dataBoxInUpdateMode;
        }

        #endregion //Constructors

        #region Fields

        protected SqlDatabaseTable _currentTable;
        protected object _selectedEntity;
        protected bool _dataBoxInUpdateMode;

        #endregion //Fields

        #region Properties

        public SqlDatabaseTable CurrentTable
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
