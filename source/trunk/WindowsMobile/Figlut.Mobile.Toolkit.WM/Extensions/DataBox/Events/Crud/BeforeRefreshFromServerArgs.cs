namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;

    #endregion //Using Directives

    public class BeforeRefreshFromServerArgs : BeforeDataBoxArgs
    {
        #region Constructors

        public BeforeRefreshFromServerArgs(bool presentLoadDataBoxForm, SqlCeDatabaseTable selectedTable)
        {
            _presentLoadDataBoxForm = presentLoadDataBoxForm;
            _selectedTable = selectedTable;
        }

        #endregion //Constructors

        #region Fields

        protected bool _presentLoadDataBoxForm;
        protected SqlCeDatabaseTable _selectedTable;

        #endregion //Fields

        #region Properties

        public bool PresentLoadDataBoxForm
        {
            get { return _presentLoadDataBoxForm; }
        }

        public SqlCeDatabaseTable SelectedTable
        {
            get { return _selectedTable; }
        }

        #endregion //Properties
    }
}
