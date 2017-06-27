namespace Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Winforms;

    #endregion //Using Directives

    public class BeforeGridRefreshArgs : BeforeDataBoxArgs
    {
        #region Constructors

        public BeforeGridRefreshArgs(
            int selectedRowIndex,
            CustomDataGridView currentGrid,
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
        protected CustomDataGridView _currentGrid;
        protected bool _filtersEnabled;
        protected bool _refreshFromServer;

        #endregion //Fields

        #region Properties

        public int SelectedRowIndex
        {
            get { return _selectedRowIndex; }
        }

        public CustomDataGridView CurrentGrid
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
