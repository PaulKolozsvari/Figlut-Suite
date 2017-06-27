namespace Figlut.Server.Toolkit.Web.Service.REST.Events
{
    #region Using Directives

    using Figlut.Server.Toolkit.Data.DB.LINQ;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class RestServicePostEntityEventArgs : RestServiceEventArgs
    {
         #region Constructors

        public RestServicePostEntityEventArgs(
            string entityName,
            Nullable<Guid> userId,
            string userName,
            EntityContext entityContext,
            Type entityType,
            object inputEntity)
            : base(entityName, userId, userName, entityContext, entityType)
        {
            _inputEntity = inputEntity;
        }

        #endregion //Constructors

        #region Fields

        private object _inputEntity;

        #endregion //Fields

        #region Properties

        public object InputEntity
        {
            get { return _inputEntity; }
        }

        #endregion //Properties
    }
}
