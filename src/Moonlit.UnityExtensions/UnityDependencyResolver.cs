using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Moonlit.UnityExtensions
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        private Func<IUnityContainer> _container;

        public UnityDependencyResolver(Func<IUnityContainer> container)
        {
            _container = container;
        }

        public object Resolve(Type serviceType)
        {
            return _container().Resolve(serviceType);
        }

        public object Resolve(Type serviceType, string key)
        {
            return _container().Resolve(serviceType, key);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            return _container().ResolveAll(serviceType);
        }

        public void Release(object service)
        {
        }
    }
}
