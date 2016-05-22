using System;
using System.Linq;
using System.Collections.Generic;

namespace Moonlit
{
    public static class DependencyResolverHelper
    {
        public static T Resolve<T>(this IDependencyResolver container, bool throwError= true)
        {
            if (container == null) throw new ArgumentNullException("container");
            try
            {
                return (T) container.Resolve(typeof (T));
            }
            catch 
            {
                if (throwError)
                {
                    throw;
                }
                return default(T);
            }
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