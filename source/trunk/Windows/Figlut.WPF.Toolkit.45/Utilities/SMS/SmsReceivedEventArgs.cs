namespace Figlut.Server.Toolkit.Utilities.SMS
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class SmsReceivedEventArgs : EventArgs
    {
        #region Constructors

        public SmsReceivedEventArgs(SmsReceivedInfo smsReceivedInfo)
        {
            _smsReceivedInfo = smsReceivedInfo;
        }

        #endregion //Constructors

        #region Fields

        private SmsReceivedInfo _smsReceivedInfo;

        #endregion //Fields

        #region Properties

        public SmsReceivedInfo SmsReceivedInfo
        {
            get { return _smsReceivedInfo; }
        }

        #endregion //Properties
    }
}