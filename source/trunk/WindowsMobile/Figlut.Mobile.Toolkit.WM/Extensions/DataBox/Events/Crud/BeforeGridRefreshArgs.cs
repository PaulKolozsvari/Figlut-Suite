namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    #endregion //Using Directives

    public class BeforeGridRefreshArgs : BeforeDataBoxArgs
    {
        #region Constructors

        public BeforeGridRefreshArgs(
            int selectedRowIndex,
            DataGrid currentGrid,
            bool filtersEnabled,
            bool refreshFromServer)
        {
            _selectedRowIndex = selectedRowIndex;
            _currentGrid = currentGrid;
            _filtersEnabled = filtersEnabled;
            _refreshFromServer = refreshFromServer;
        }

        #endregion //Constructors

        #region Fields

        protected int _selectedRowIndex;
        protected DataGrid _currentGrid;
        protected bool _filtersEnabled;
        protected bool _refreshFromServer;

        #endregion //Fields

        #region Properties

        public int SelectedRowIndex
        {
            get { return _selectedRowIndex; }
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

        #endregion //Properties
    }
}
