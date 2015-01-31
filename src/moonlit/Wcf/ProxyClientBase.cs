using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Moonlit.Wcf
{
    public class ProxyClientBase<TChannel> : ClientBase<TChannel> where TChannel : class
    {
        public TChannel Proxy { get { return this.Channel; } }
        public ProxyClientBase()
        {
        }

        public ProxyClientBase(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public ProxyClientBase(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public ProxyClientBase(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public ProxyClientBase(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public ProxyClientBase(ServiceEndpoint endpoint)
            : base(endpoint)
        {
        }

        public ProxyClientBase(InstanceContext callbackInstance)
            : base(callbackInstance)
        {
        }

        public ProxyClientBase(InstanceContext callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
        }

        public ProxyClientBase(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public ProxyClientBase(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public ProxyClientBase(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {
        }

        public ProxyClientBase(InstanceContext callbackInstance, ServiceEndpoint endpoint)
            : base(callbackInstance, endpoint)
        {
        }
    }
}