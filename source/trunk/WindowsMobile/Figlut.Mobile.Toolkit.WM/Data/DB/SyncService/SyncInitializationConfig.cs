namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Microsoft.Synchronization.Data;
    using Microsoft.Synchronization;

    #endregion //Using Directives

    public class SyncInitializationConfig
    {
        #region Constructors

        public SyncInitializationConfig(
            object mobileSyncServiceProxy,
            SyncAgent syncAgent,
            bool openLocalDbConnectionAfterInitialization,
            bool clearTablesAfterInitialization)
        {
            _mobileSyncServiceProxy = mobileSyncServiceProxy;
            _remoteProvider = new ServerSyncProviderProxy(_mobileSyncServiceProxy);
            _syncAgent = syncAgent;
            _syncAgent.RemoteProvider = _remoteProvider;

            _openConnectionAfterInitialization = openLocalDbConnectionAfterInitialization;
            _clearTablesAfterInitialization = clearTablesAfterInitialization;
            VerifyMandatoryProperties();
        }

        public SyncInitializationConfig(
            object mobileSyncServiceProxy,
            SyncAgent syncAgent,
            bool openLocalDbConnectionAfterInitialization,
            bool clearTablesAfterInitialization,
            bool reinitializeSubscriptionIfExpired,
            string subscriptionExpiredExpectedMessage)
        {
            _mobileSyncServiceProxy = mobileSyncServiceProxy;
            _remoteProvider = new ServerSyncProviderProxy(_mobileSyncServiceProxy);
            _syncAgent = syncAgent;
            _syncAgent.RemoteProvider = _remoteProvider;

            _openConnectionAfterInitialization = openLocalDbConnectionAfterInitialization;
            _clearTablesAfterInitialization = clearTablesAfterInitialization;
            _reinitializeSubscriptionIfExpired = reinitializeSubscriptionIfExpired;
            _subscriptionExpiredExpectedMessage = subscriptionExpiredExpectedMessage;
            VerifyMandatoryProperties();
        }

        #endregion //Constructors

        #region Fields

        protected object _mobileSyncServiceProxy;
        protected ServerSyncProviderProxy _remoteProvider;
        protected SyncAgent _syncAgent;
        protected bool _openConnectionAfterInitialization;
        protected bool _clearTablesAfterInitialization;
        protected bool _reinitializeSubscriptionIfExpired;
        protected string _subscriptionExpiredExpectedMessage;

        #endregion //Fields

        #region Properties

        public object MobileSyncServiceProxy
        {
            get { return _mobileSyncServiceProxy; }
            set { _mobileSyncServiceProxy = value; }
        }

        public ServerSyncProviderProxy RemoteProvider
        {
            get { return _remoteProvider; }
            set { _remoteProvider = value; }
        }

        public SyncAgent SyncAgent
        {
            get { return _syncAgent; }
            set { _syncAgent = value; }
        }

        public bool OpenConnectionAfterInitialization
        {
            get { return _openConnectionAfterInitialization; }
            set { _openConnectionAfterInitialization = value; }
        }

        public bool ClearTablesAfterInitialization
        {
            get { return _clearTablesAfterInitialization; }
            set { _clearTablesAfterInitialization = value; }
        }

        public bool ReinitializeSubscriptionIfExpired
        {
            get { return _reinitializeSubscriptionIfExpired; }
            set { _reinitializeSubscriptionIfExpired = value; }
        }

        public string SubscriptionExpiredExpectedMessage
        {
            get { return _subscriptionExpiredExpectedMessage; }
            set { _subscriptionExpiredExpectedMessage = value; }
        }

        #endregion //Properties

        #region Methods

        private void VerifyMandatoryProperties()
        {
            if (MobileSyncServiceProxy == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set in {1}.",
                    EntityReader<SyncInitializationConfig>.GetPropertyName(p => p.MobileSyncServiceProxy, false),
                    this.GetType().FullName));
            }
            if (SyncAgent == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} not set in {1}.",
                    EntityReader<SyncInitializationConfig>.GetPropertyName(p => p.SyncAgent, false),
                    this.GetType().FullName));
            }
            if (ReinitializeSubscriptionIfExpired && 
                string.IsNullOrEmpty(SubscriptionExpiredExpectedMessage))
            {
                throw new NullReferenceException(string.Format(
                    "If {0} is set to true on {1}, then {2} must also be set.",
                    EntityReader<SyncInitializationConfig>.GetPropertyName(p => ReinitializeSubscriptionIfExpired, false),
                    this.GetType().FullName,
                    EntityReader<SyncInitializationConfig>.GetPropertyName(p => p.SubscriptionExpiredExpectedMessage, false)));
            }
        }

        #endregion //Methods
    }
}