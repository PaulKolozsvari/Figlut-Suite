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

    public class AfterRefreshFromServerArgs : AfterDataBoxArgs
    {
        public AfterRefreshFromServerArgs(bool presentLoadDataBoxForm, SqlDatabaseTable currentTable)
        {
            _presentLoadDataBoxForm = presentLoadDataBoxForm;
            _currentTable = currentTable;
        }

        #region Fields

        protected bool _presentLoadDataBoxForm;
        protected SqlDatabaseTable _currentTable;

        #endregion //Fields

        #region Properties

        public bool PresentLoadDataBoxForm
        {
            get { return _presentLoadDataBoxForm; }
        }

        public SqlDatabaseTable CurrentTable
        {
            get { return _currentTable; }
        }

        #endregion //Properties
    }
}
