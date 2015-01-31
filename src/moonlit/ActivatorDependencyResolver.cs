using System;
using System.Collections.Generic;

namespace Moonlit
{
    public class ActivatorDependencyResolver : IDependencyResolver
    {
        public object Resolve(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }

        public object Resolve(Type serviceType, string key)
        {
            return Activator.CreateInstance(serviceType);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            yield return Activator.CreateInstance(serviceType);
        }

        public void Release(object service)
        {
        }
    }
}