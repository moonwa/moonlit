using Castle.DynamicProxy;

namespace Moonlit.Proxy.Caching
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
