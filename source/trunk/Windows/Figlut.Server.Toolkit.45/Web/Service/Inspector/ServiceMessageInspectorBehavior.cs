namespace Figlut.Server.Toolkit.Web.Service.Inspector
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Description;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class ServiceMessageInspectorBehavior : IEndpointBehavior
    {
        #region Constructors

        public ServiceMessageInspectorBehavior(bool traceMessages, bool traceMessageHeaders)
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

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {   
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new ServiceMessageInspector(_traceMessages, _traceMessageHeaders));
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion //Methods
    }
}
