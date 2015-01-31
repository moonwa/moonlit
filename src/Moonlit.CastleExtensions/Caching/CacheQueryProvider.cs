using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Moonlit.CastleExtensions.Caching
{
    public class CacheQueryProvider : CacheProvider
    {
        public CacheQueryProvider(string cacheKey)
            : base(cacheKey)
        {
        }
        #region ICacheArgumentProvider 成员


        public override void Process(IInvocation invocation)
        {
            string key = GetCacheKey(invocation);
            object data = GetCache(key);
            if (data != null)
            {
                invocation.ReturnValue = data;
                return;
            }
            invocation.Proceed();
            AddCache(key, invocation.ReturnValue);
        }

        #endregion
    }
}
