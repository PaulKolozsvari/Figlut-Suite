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

    public class RestServiceEventArgs : EventArgs
    {
        #region Constructors

        public RestServiceEventArgs(
            string entityName,
            Nullable<Guid> userId,
            string userName,
            EntityContext entityContext,
            Type entityType)
        {
            _entityName = entityName;
            _userId = userId;
            _userName = userName;
            _entityContext = entityContext;
            _entityType = entityType;
        }

        #endregion //Constructors

        #region Fields

        private string _entityName;
        private Nullable<Guid> _userId;
        private string _userName;
        private EntityContext _entityContext;
        private Type _entityType;

        #endregion //Fields

        #region Properties

        public string EntityName
        {
            get { return _entityName; }
        }

        public Nullable<Guid> UserId
        {
            get { return _userId; }
        }

        public string UserName
        {
            get { return _userName; }
        }

        public EntityContext EntityContext
        {
            get { return _entityContext; }
        }

        public Type EntityType
        {
            get { return _entityType; }
        }

        #endregion //Properties
    }
}
