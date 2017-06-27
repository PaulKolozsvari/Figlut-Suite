namespace Figlut.Configuration.Manager.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;

    #endregion //Using Directives

    public class ClientView
    {
        #region Constructors

        public ClientView(ClientViewType clientType, EntityCache<Guid, FiglutClientInfo> clients)
        {
            _clientType = clientType;
            _clients = clients;
        }

        #endregion //Constructors

        #region Fields

        private ClientViewType _clientType;
        private EntityCache<Guid, FiglutClientInfo> _clients;

        #endregion //Fields

        #region Properties

        public ClientViewType ClientType
        {
            get { return _clientType; }
        }

        public EntityCache<Guid, FiglutClientInfo> Clients
        {
            get { return _clients; }
        }

        #endregion //Properties
    }
}
