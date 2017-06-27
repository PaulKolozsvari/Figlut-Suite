namespace Figlut.Server.Toolkit.Web.Service.SOAP
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

    //http://weblogs.asp.net/paolopia/writing-a-wcf-message-inspector
    public class SoapServerMessageInspector : IDispatchMessageInspector
    {
        #region Constructors

        public SoapServerMessageInspector(bool traceSoapMessages, bool traceSoapMessageHeaders)
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

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            if (!_traceSoapMessages && !_traceSoapMessageHeaders)
            {
                return null;
            }
            StringBuilder logMessage = new StringBuilder();
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            request = buffer.CreateMessage();
            Message originalMessage = buffer.CreateMessage();
            logMessage.AppendLine("Received:");
            logMessage.AppendLine();
            if (_traceSoapMessageHeaders)
            {
                foreach (MessageHeader header in originalMessage.Headers)
                {
                    string headerString = header.ToString();
                    logMessage.AppendLine(headerString);
                }
            }
            if (_traceSoapMessages)
            {
                logMessage.AppendLine();
                string soapMessage = originalMessage.ToString();
                logMessage.AppendLine(soapMessage);
            }
            string logMessageString = logMessage.ToString();
            GOC.Instance.Logger.LogMessage(new LogMessage(logMessageString, LogMessageType.Information, LoggingLevel.Maximum));
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (!_traceSoapMessages && !_traceSoapMessageHeaders)
            {
                return;
            }
            StringBuilder logMessage = new StringBuilder();
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            reply = buffer.CreateMessage();
            Message originalMessage = buffer.CreateMessage();
            logMessage.AppendLine("Sending:");
            logMessage.AppendLine();
            if (_traceSoapMessageHeaders)
            {
                foreach (MessageHeader header in originalMessage.Headers)
                {
                    string headerString = header.ToString();
                    logMessage.AppendLine(headerString);
                }
            }
            if (_traceSoapMessages)
            {
                logMessage.AppendLine();
                string soapMessage = originalMessage.ToString();
                logMessage.AppendLine(soapMessage);
            }
            string logMessageString = logMessage.ToString();
            GOC.Instance.Logger.LogMessage(new LogMessage(logMessageString, LogMessageType.Information, LoggingLevel.Maximum));
        }

        #endregion //Methods
    }
}
