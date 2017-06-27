namespace Figlut.Server.Toolkit.Web.Service.SOAP
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Description;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class SoapServerMessageInspectorBehavior : IEndpointBehavior
    {
        #region Constructors

        public SoapServerMessageInspectorBehavior(bool traceSoapMessages, bool traceSoapMessageHeaders)
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
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new SoapServerMessageInspector(_traceSoapMessages, _traceSoapMessageHeaders));
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion //Methods
    }
}
