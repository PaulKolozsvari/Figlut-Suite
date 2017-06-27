namespace Figlut.Mobile.Toolkit.Web.Client.DataProgress
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class ServiceTransactionEventArgs : EventArgs
    {
        #region Constructors

        public ServiceTransactionEventArgs(
            ServiceDataTransactionType serviceOperation,
            int transactionCount,
            int transactionCompletedIndex,
            object entity)
        {
            _serviceOperation = serviceOperation;
            _transactionCount = transactionCount;
            _transactionCompletedIndex = transactionCompletedIndex;
            _entity = entity;
        }

        #endregion //Constructors

        #region Fields

        private ServiceDataTransactionType _serviceOperation;
        private int _transactionCount;
        private int _transactionCompletedIndex;
        private object _entity;

        #endregion //Fields

        #region Properties

        public ServiceDataTransactionType ServiceOperation
        {
            get { return _serviceOperation; }
        }

        public int TransactionCount
        {
            get { return _transactionCount; }
        }

        public int TransactionCompletedIndex
        {
            get { return _transactionCompletedIndex; }
        }

        public object Entity
        {
            get { return _entity; }
        }

        #endregion //Properties
    }
}
