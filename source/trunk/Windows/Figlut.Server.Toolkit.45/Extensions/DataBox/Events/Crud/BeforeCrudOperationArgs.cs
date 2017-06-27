namespace Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class BeforeCrudOperationArgs : BeforeDataBoxArgs
    {
        #region Constructors

        public BeforeCrudOperationArgs(object entity)
        {
            _entity = entity;
        }

        #endregion //Constructors

        #region Fields

        protected object _entity;
        protected bool _cancelDefaultBindingBehaviour;

        #endregion //Fields

        #region Properties

        public object Entity
        {
            get { return _entity; }
        }

        public bool CancelDefaultBindingBehaviour
        {
            get { return _cancelDefaultBindingBehaviour; }
            set { _cancelDefaultBindingBehaviour = value; }
        }

        #endregion //Properties
    }
}
