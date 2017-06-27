namespace Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class AfterCrudOperationArgs : AfterDataBoxArgs
    {
        #region Constructors

        public AfterCrudOperationArgs(object entity)
        {
            _entity = entity;
        }

        #endregion //Constructors

        #region Fields

        protected object _entity;

        #endregion //Fields

        #region Properties

        public object Entity
        {
            get { return _entity; }
        }

        #endregion //Properties
    }
}