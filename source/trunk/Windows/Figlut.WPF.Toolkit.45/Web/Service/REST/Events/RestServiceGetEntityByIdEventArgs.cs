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

    public class RestServiceGetEntityByIdEventArgs : RestServiceEventArgs
    {
        #region Constructors

        public RestServiceGetEntityByIdEventArgs(
            string entityName,
            Nullable<Guid> userId,
            string userName,
            EntityContext entityContext,
            Type entityType,
            string entityId,
            object outputEntity)
            : base(entityName, userId, userName, entityContext, entityType)
        {
            _entityId = entityId;
            _outputEntity = outputEntity;
        }

        #endregion //Constructors

        #region Fields

        private string _entityId;
        private object _outputEntity;

        #endregion //Fields

        #region Properties

        public string EntityId
        {
            get { return _entityId; }
        }

        public object OutputEntity
        {
            get { return _outputEntity; }
        }

        #endregion //Properties
    }
}
