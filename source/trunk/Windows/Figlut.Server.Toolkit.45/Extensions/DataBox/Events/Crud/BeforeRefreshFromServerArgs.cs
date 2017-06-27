namespace Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;

    #endregion //Using Directives

    public class BeforeRefreshFromServerArgs : BeforeDataBoxArgs
    {
        #region Constructors

        public BeforeRefreshFromServerArgs(bool presentLoadDataBoxForm, SqlDatabaseTable selectedTable)
        {
            _presentLoadDataBoxForm = presentLoadDataBoxForm;
            _selectedTable = selectedTable;
        }

        #endregion //Constructors

        #region Fields

        protected bool _presentLoadDataBoxForm;
        protected SqlDatabaseTable _selectedTable;

        #endregion //Fields

        #region Properties

        public bool PresentLoadDataBoxForm
        {
            get { return _presentLoadDataBoxForm; }
        }

        public SqlDatabaseTable SelectedTable
        {
            get { return _selectedTable; }
        }

        #endregion //Properties
    }
}
