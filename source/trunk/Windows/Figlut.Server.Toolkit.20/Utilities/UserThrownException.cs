namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public class UserThrownException : Exception
    {
        #region Constructors

        public UserThrownException(string message) : 
            base(message)
        {
            _closeApplication = false;
            _loggingLevel = LoggingLevel.Normal;
        }

        public UserThrownException(string message, LoggingLevel loggingLevel) :
            base(message)
        {
            _closeApplication = false;
            _loggingLevel = loggingLevel;
        }

        public UserThrownException(string message, bool closeApplication)
            : base(message)
        {
            _loggingLevel = LoggingLevel.Normal;
            _closeApplication = closeApplication;
        }

        public UserThrownException(string message, LoggingLevel loggingLevel, bool closeApplication) 
            : base(message)
        {
            _loggingLevel = loggingLevel;
            _closeApplication = closeApplication;
        }

        #endregion //Constructors

        #region Fields

        protected bool _closeApplication;
        protected LoggingLevel _loggingLevel;

        #endregion //Fields

        #region Properties

        public virtual bool CloseApplication
        {
            get { return _closeApplication; }
        }

        public virtual LoggingLevel LoggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        #endregion //Properties
    }
}