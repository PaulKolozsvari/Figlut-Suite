namespace Figlut.Server.Toolkit.Data.QueryRunners
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    [Serializable]
    public class SqlQueryRunnerOutput
    {
        #region Constructors

        public SqlQueryRunnerOutput()
        {
        }

        public SqlQueryRunnerOutput(
            bool success,
            string resultMessage)
        {
            _success = success;
            _resultMessage = resultMessage;
        }

        #endregion //Constructors

        #region Fields

        protected bool _success;
        protected string _resultMessage;

        #endregion //Fields

        #region Properties

        public bool Success
        {
            get { return _success; }
        }

        public string ResultMessage
        {
            get { return _resultMessage; }
        }

        #endregion //Properties
    }
}