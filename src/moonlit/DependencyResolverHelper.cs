using System;
using System.Linq;
using System.Collections.Generic;

namespace Moonlit
{
    public static class DependencyResolverHelper
    {
        public static T Resolve<T>(this IDependencyResolver container)
        {
            if (container == null) throw new ArgumentNullException("container");
            return (T)container.Resolve(typeof(T));
        }
        public static T Resolve<T>(this IDependencyResolver container, string key)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (key == null) throw new ArgumentNullException("key");
            return (T)container.Resolve(typeof(T), key);
        }

        public static IEnumerable<T> ResolveAll<T>(this IDependencyResolver container)
        {
            if (container == null) throw new ArgumentNullException("container");
            return container.ResolveAll(typeof(T)).Cast<T>();
        }
    }
}