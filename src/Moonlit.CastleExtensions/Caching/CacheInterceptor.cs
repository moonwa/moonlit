using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Core;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using Moonlit.Caching;

namespace Moonlit.CastleExtensions.Caching
{
    public class CacheInterceptor : IInterceptor, IOnBehalfAware
    {
        public void Intercept(IInvocation invocation)
        {
            CacheAttribute cache = invocation.MethodInvocationTarget.GetCustomAttribute<CacheAttribute>(true);
            if (cache == null)
            {
                invocation.Proceed();
                return;
            }
            var provider = GetProvider(cache);
            if (provider == null)
                invocation.Proceed();
            else
                provider.Process(invocation);
        }

        private CacheProvider GetProvider(CacheAttribute cacheAttr)
        {
            CacheQueryAttribute cacheQueryAttr = cacheAttr as CacheQueryAttribute;
            if (cacheQueryAttr != null)
            {
                return new CacheQueryProvider(cacheQueryAttr.CacheKey);
            }


            CacheRefreshAttribute cacheRefreshAttr = cacheAttr as CacheRefreshAttribute;
            if (cacheRefreshAttr != null)
            {
                return new CacheRefreshProvider(cacheRefreshAttr.CacheKey);
            }
            return null;
        }


        public void SetInterceptedComponentModel(Castle.Core.ComponentModel target)
        {

        }
    }

}
