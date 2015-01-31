using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Moonlit.Wcf.DependencyResolverExtensions
{
    /// <summary>  
    /// </summary>
    public class DependencyResolverBehavior : IServiceBehavior
    {
        private readonly IDependencyResolver  _dependencyResolver;
        public DependencyResolverBehavior()
        {
            _dependencyResolver = DependencyResolverContainerFactory.Container;
        }
        public DependencyResolverBehavior(IDependencyResolver  dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = cdb as ChannelDispatcher;
                if (cd != null)
                {
                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {
                        if (ed.ContractName != "IMetadataExchange")
                        {
                            var provider = new DependencyResolverInstanceProvider(serviceDescription.ServiceType, _dependencyResolver);
                            ed.DispatchRuntime.InstanceProvider = provider;
                        }
                    }
                }
            }
        }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }
    }
}