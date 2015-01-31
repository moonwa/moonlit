using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Moonlit.Wcf.DependencyResolverExtensions
{
    public class DependencyResolverInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;
        private readonly IDependencyResolver _unityContainer;

        public DependencyResolverInstanceProvider(Type type, IDependencyResolver unityContainer)
        {
            _serviceType = type;
            _unityContainer = unityContainer;
        }
        #region IInstanceProvider Members
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return _unityContainer.Resolve(_serviceType);
        }
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
        #endregion
    }
}