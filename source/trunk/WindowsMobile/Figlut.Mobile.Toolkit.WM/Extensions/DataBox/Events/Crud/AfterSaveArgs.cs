namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class AfterSaveArgs : AfterDataBoxArgs
    {
        #region Constructors

        public AfterSaveArgs(string messageResult, bool saveSuccessful)
        {
            _messageResult = messageResult;
            _saveSuccessful = saveSuccessful;
        }

        #endregion //Constructors

        #region Fields

        protected string _messageResult;
        protected bool _saveSuccessful;

        #endregion //Fields

        #region Properties

        public string MessageResult
        {
            get { return _messageResult; }
        }

        public bool SaveSuccessful
        {
            get { return _saveSuccessful; }
        }

        #endregion //Properties
    }
}
