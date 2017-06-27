namespace Figlut.Server.Toolkit.Web.Service.Inspector
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class ServiceMessageInspector : IDispatchMessageInspector
    {
        #region Constructors

        public ServiceMessageInspector(bool traceMessages, bool traceMessageHeaders)
        {
            _traceMessages = traceMessages;
            _traceMessageHeaders = traceMessageHeaders;
        }

        #endregion //Constructors

        #region Fields

        private bool _traceMessages;
        private bool _traceMessageHeaders;

        #endregion //Fields

        #region Properties

        public bool TraceSoapMessages
        {
            get { return _traceMessages; }
            set { _traceMessages = value; }
        }

        public bool TraceSoapMessageHeaders
        {
            get { return _traceMessageHeaders; }
            set { _traceMessageHeaders = value; }
        }

        #endregion //Properties

        #region Methods

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            if (!_traceMessages && !_traceMessageHeaders)
            {
                return null;
            }
            StringBuilder logMessage = new StringBuilder();
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            request = buffer.CreateMessage();
            Message originalMessage = buffer.CreateMessage();
            logMessage.AppendLine("Received:");
            logMessage.AppendLine();
            if (_traceMessageHeaders)
            {
                foreach (MessageHeader header in originalMessage.Headers)
                {
                    string headerString = header.ToString();
                    logMessage.AppendLine(headerString);
                }
            }
            if (_traceMessages)
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
            if (!_traceMessages && !_traceMessageHeaders)
            {
                return;
            }
            StringBuilder logMessage = new StringBuilder();
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            reply = buffer.CreateMessage();
            Message originalMessage = buffer.CreateMessage();
            logMessage.AppendLine("Sending:");
            logMessage.AppendLine();
            if (_traceMessageHeaders)
            {
                foreach (MessageHeader header in originalMessage.Headers)
                {
                    string headerString = header.ToString();
                    logMessage.AppendLine(headerString);
                }
            }
            if (_traceMessages)
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
