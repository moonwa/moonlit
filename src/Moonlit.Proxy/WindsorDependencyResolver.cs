using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;

namespace Moonlit.Proxy
{
    public class WindsorDependencyResolver : IDependencyResolver
    {
        private readonly Func<IWindsorContainer> _container;

        public WindsorDependencyResolver(Func<IWindsorContainer> container)
        {
            _container = container;
        }

        public object Resolve(Type serviceType)
        {
            return GetContainer().Resolve(serviceType);
        }

        public object Resolve(Type serviceType, string key)
        {
            return GetContainer().Resolve(key, serviceType);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {

            return GetContainer().ResolveAll(serviceType).Cast<object>();
        }

        public void Release(object service)
        {
            GetContainer().Release(service);
        }

        public IWindsorContainer GetContainer()
        {
            return _container();
        }
    }
}
