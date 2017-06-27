namespace Figlut.Server.Toolkit.Utilities.SMS
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    [Serializable]
    public class SmsInfo
    {
        #region Constructors

        public SmsInfo()
        {
        }

        public SmsInfo(string cellPhoneNumber, string smsMessage)
        {
            _cellPhoneNumber = cellPhoneNumber;
            _smsMessage = smsMessage;
        }

        #endregion //Constructors

        #region Fields

        private string _cellPhoneNumber;
        private string _smsMessage;

        #endregion //Fields

        #region Properties

        public string CellPhoneNumber
        {
            get { return _cellPhoneNumber; }
            set { _cellPhoneNumber = value; }
        }

        public string SmsMessage
        {
            get { return _smsMessage; }
            set { _smsMessage = value; }
        }

        #endregion //Properties
    }
}