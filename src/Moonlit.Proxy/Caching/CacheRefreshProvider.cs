using Castle.DynamicProxy;

namespace Moonlit.Proxy.Caching
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
