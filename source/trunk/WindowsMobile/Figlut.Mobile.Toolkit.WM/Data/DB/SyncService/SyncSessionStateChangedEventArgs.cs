namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Synchronization;

    #endregion //Using Directives

    public class SyncSessionStateChangedEventArgs : SyncEventArgs
    {
        #region Constructors

        public SyncSessionStateChangedEventArgs(
            string eventMessage,
            SessionStateChangedEventArgs sessionStateChangedEventArgs)
            : base(eventMessage)
        {
            _sessionStateChangedEventArgs = sessionStateChangedEventArgs;
        }

        #endregion //Constructors

        #region Fields

        protected SessionStateChangedEventArgs _sessionStateChangedEventArgs;

        #endregion //Fields

        #region Properties

        public SessionStateChangedEventArgs SessionSateChangedEventArgs
        {
            get { return _sessionStateChangedEventArgs; }
        }

        #endregion //Properties
    }
}