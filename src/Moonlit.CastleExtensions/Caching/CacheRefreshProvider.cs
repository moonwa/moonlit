using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Moonlit.CastleExtensions.Caching
{
    public class CacheRefreshProvider : CacheProvider
    {
        public CacheRefreshProvider(string cacheKey)
            : base(cacheKey)
        {
        }
        #region ICacheArgumentProvider 成员

        public override void Process(IInvocation invocation)
        {
            invocation.Proceed();
            RemoveCache(GetCacheKey(invocation));
        }

        #endregion
    }
}
