namespace Figlut.Server.Toolkit.Web.Client.SOAP
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Description;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class SoapClientMessageInspectorBehavior : IEndpointBehavior
    {
        #region Constructors

        public SoapClientMessageInspectorBehavior(bool traceSoapMessages, bool traceSoapMessageHeaders)
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

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new SoapClientMessageInspector(_traceSoapMessages, _traceSoapMessageHeaders));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            
        }

        #endregion //Methods
    }
}