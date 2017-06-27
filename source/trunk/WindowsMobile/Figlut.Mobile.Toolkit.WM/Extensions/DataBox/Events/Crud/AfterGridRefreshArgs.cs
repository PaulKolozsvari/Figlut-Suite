namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    #endregion //Using Directives

    public class AfterGridRefreshArgs : AfterDataBoxArgs
    {
        #region Constructors

        public AfterGridRefreshArgs(
            int selectedRowIndex,
            DataGrid currentGrid,
            bool filtersEnabled,
            bool refreshFromServer,
            List<string> hiddenProperties,
            List<Type> hiddenTypes)
        {
            _selectedRowId = selectedRowIndex;
            _currentGrid = currentGrid;
            _refreshFromServer = refreshFromServer;
            _hiddenProperties = hiddenProperties;
            _hiddenTypes = hiddenTypes;
        }

        #endregion //Constructors

        #region Fields

        protected int _selectedRowId;
        protected DataGrid _currentGrid;
        protected bool _filtersEnabled;
        protected bool _refreshFromServer;
        protected List<string> _hiddenProperties;
        protected List<Type> _hiddenTypes;

        #endregion //Fields

        #region Properties

        public int SelectedRowIndex
        {
            get { return _selectedRowId; }
        }

        public DataGrid CurrentGrid
        {
            get { return _currentGrid; }
        }

        public bool FiltersEnabled
        {
            get { return _filtersEnabled; }
        }

        public bool RefreshFromServer
        {
            get { return _refreshFromServer; }
        }

        public List<string> HiddenProperties
        {
            get { return _hiddenProperties; }
        }

        public List<Type> HiddenTypes
        {
            get { return _hiddenTypes; }
        }

        #endregion //Properties
    }
}
