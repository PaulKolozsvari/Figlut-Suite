namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    public class SyncEventArgs
    {
        #region Constructors

        public SyncEventArgs(string eventMessage)
        {
            _eventMessage = eventMessage;
        }

        #endregion //Constructors

        #region Fields

        private string _eventMessage;

        #endregion //Fields

        #region Properties

        public string EventMessage
        {
            get { return _eventMessage; }
        }

        #endregion //Properties
    }
}