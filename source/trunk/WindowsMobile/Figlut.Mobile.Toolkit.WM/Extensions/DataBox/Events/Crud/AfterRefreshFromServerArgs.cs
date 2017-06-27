namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;

    #endregion //Using Directives

    public class AfterRefreshFromServerArgs : AfterDataBoxArgs
    {
        public AfterRefreshFromServerArgs(bool presentLoadDataBoxForm, SqlCeDatabaseTable currentTable)
        {
            _presentLoadDataBoxForm = presentLoadDataBoxForm;
            _currentTable = currentTable;
        }

        #region Fields

        protected bool _presentLoadDataBoxForm;
        protected SqlCeDatabaseTable _currentTable;

        #endregion //Fields

        #region Properties

        public bool PresentLoadDataBoxForm
        {
            get { return _presentLoadDataBoxForm; }
        }

        public SqlCeDatabaseTable CurrentTable
        {
            get { return _currentTable; }
        }

        #endregion //Properties
    }
}
