namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Synchronization;

    #endregion //Using Directives

    public class SyncSessionProgressEventArgs : SyncEventArgs
    {
        #region Constructors

        public SyncSessionProgressEventArgs(
            string eventMessage,
            SessionProgressEventArgs sessionProgressEventArgs)
            : base(eventMessage)
        {
            _sessionProgressEventArgs = sessionProgressEventArgs;
        }

        #endregion //Constructors

        #region Fields

        protected SessionProgressEventArgs _sessionProgressEventArgs;

        #endregion //Fields

        #region Properties

        public SessionProgressEventArgs SessionProgressEventArgs
        {
            get { return _sessionProgressEventArgs; }
        }

        #endregion //Properties
    }
}