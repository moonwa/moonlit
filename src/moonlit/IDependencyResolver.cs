using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit
{
    public interface IDependencyResolver
    {
        object Resolve(Type serviceType);
        object Resolve(Type serviceType, string key);
        IEnumerable<object> ResolveAll(Type serviceType);
        void Release(object service);
    }
}
