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
    public class SmsReceivedInfo : SmsInfo
    {
        #region Constructors

        public SmsReceivedInfo()
        {
        }

        public SmsReceivedInfo(string cellPhoneNumber, string smsMessage, string receivedDateTimeRaw)
            : base(cellPhoneNumber, smsMessage)
        {
            _receivedDateTimeRaw = receivedDateTimeRaw;
        }

        #endregion //Constructors

        #region Fields

        private string _receivedDateTimeRaw;
        private Nullable<DateTime> _receivedDate;

        #endregion //Fields

        #region Properties

        public string ReceivedDateTimeRaw
        {
            get { return _receivedDateTimeRaw; }
            set { _receivedDateTimeRaw = value; }
        }

        public Nullable<DateTime> ReceivedDate
        {
            get { return _receivedDate; }
            set { _receivedDate = value; }
        }

        #endregion //Properties

        #region Methods

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(string.Format("SMS Received: {0} - {1}", CellPhoneNumber, ReceivedDate.ToString()));
            result.AppendLine(SmsMessage);
            return result.ToString();
        }

        #endregion //Methods
    }
}