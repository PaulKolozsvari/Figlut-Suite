namespace Figlut.Server.Toolkit.Web.Client.SOAP
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public class SoapClientMessageInspector : IClientMessageInspector
    {
        #region Constructors

        public SoapClientMessageInspector(bool traceSoapMessages, bool traceSoapMessageHeaders)
        {
            _traceSoapMessages = traceSoapMessages;
            _traceSoapMessageHeaders = traceSoapMessageHeaders;
        }

        #endregion //Constructors

        #region Fields

        private bool _traceSoapMessages;
        private bool _traceSoapMessageHeaders;

        #endregion //Fields

        #region Properties

        public bool TraceSoapMessages
        {
            get { return _traceSoapMessages; }
            set { _traceSoapMessages = value; }
        }

        public bool TraceSoapMessageHeaders
        {
            get { return _traceSoapMessageHeaders; }
            set { _traceSoapMessageHeaders = value; }
        }

        #endregion //Properties

        #region Methods

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine("Received Reply:");
            logMessage.AppendLine();
            if (_traceSoapMessageHeaders)
            {
                foreach (MessageHeader header in reply.Headers)
                {
                    string headerString = header.ToString();
                    logMessage.AppendLine(headerString);
                }
            }
            if (_traceSoapMessages)
            {
                logMessage.AppendLine();
                string soapMessage = reply.ToString();
                logMessage.AppendLine(soapMessage);
            }
            string logMessageString = logMessage.ToString();
            if (logMessageString.Contains("SOAP:Fault"))
            {
                if (logMessageString.Contains("CPAChannelStoppedException"))
                {
                    GOC.Instance.Logger.LogMessage(new LogMessage(string.Format("Sap Service Offline: {0}", logMessageString), LogMessageType.Error, LoggingLevel.Normal));
                }
                else
                {
                    GOC.Instance.Logger.LogMessage(new LogMessage(string.Format("Sap Error: {0}", logMessageString), LogMessageType.Error, LoggingLevel.Normal));
                }
            }
            else if (!_traceSoapMessages && !_traceSoapMessageHeaders)
            {
                return;
            }
            else
            {
                GOC.Instance.Logger.LogMessage(new LogMessage(logMessageString, LogMessageType.Information, LoggingLevel.Maximum));
            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            if (!_traceSoapMessages && !_traceSoapMessageHeaders)
            {
                return null;
            }
            StringBuilder logMessageHeader = new StringBuilder();
            logMessageHeader.AppendLine("Sending Request:");
            logMessageHeader.AppendLine();
            if (_traceSoapMessageHeaders)
            {
                foreach (MessageHeader header in request.Headers)
                {
                    string headerString = header.ToString();
                    logMessageHeader.AppendLine(headerString);
                }
            }
            StringBuilder logMessageMessages = new StringBuilder();
            if (_traceSoapMessages)
            {
                string soapMessage = request.ToString();
                logMessageMessages.AppendLine(soapMessage);
            }
            string logMessageHeaderString = logMessageHeader.ToString();
            string logMessageMessagesString = logMessageMessages.ToString();
            GOC.Instance.Logger.LogMessage(new LogMessage(logMessageHeaderString, LogMessageType.Information, LoggingLevel.Maximum));
            return null;
        }

        #endregion //Methods
    }
}
