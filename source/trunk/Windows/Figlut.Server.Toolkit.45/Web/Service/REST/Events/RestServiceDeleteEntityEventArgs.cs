namespace Figlut.Server.Toolkit.Web.Service.REST.Events
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.DB.LINQ;

    #endregion //Using Directives

    public class RestServiceDeleteEntityEventArgs : RestServiceEventArgs
    {
        #region Constructors

        public RestServiceDeleteEntityEventArgs(
            string entityName,
            Nullable<Guid> userId,
            string userName,
            EntityContext entityContext,
            Type entityType,
            string entityId)
            : base(entityName, userId, userName, entityContext, entityType)
        {
            _entityId = entityId;
        }

        #endregion //Constructors

        #region Fields

        private string _entityId;

        #endregion //Fields

        #region Properties

        public string EntityId
        {
            get { return _entityId; }
        }

        #endregion //Properties
    }
}
