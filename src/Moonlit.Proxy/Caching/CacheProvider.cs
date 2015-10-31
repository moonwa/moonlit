using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Moonlit.Caching;

namespace Moonlit.Proxy.Caching
{
    public abstract class CacheProvider
    {
        static CacheProvider()
        {
            CacheManager = new MemoryCacheManager();
        }
        public string CacheKey { get; private set; }

        public static ICacheManager CacheManager
        {
            get { return _cacheManager; }
            set { _cacheManager = value; }
        }

        protected CacheProvider(string cacheKey)
        {
            CacheKey = cacheKey;
        }

        private static ICacheManager _cacheManager;
        protected void RemoveCache(string key)
        {
            CacheManager.Remove(key);
        }
        protected object GetCache(string key)
        {
            Dictionary<string, object> di = (Dictionary<string, object>)(CacheManager.Get(key, typeof(Dictionary<string, object>)) ?? new Dictionary<string, object>());
            object cachedItem;
            if (di.TryGetValue(key, out cachedItem))
            {
                return cachedItem;
            }
            return null;
        }
        protected void AddCache(string key, object obj)
        {
            Dictionary<string, object> di = (Dictionary<string, object>)(CacheManager.Get(key, typeof(Dictionary<string, object>)) ?? new Dictionary<string, object>());
            di[key] = obj;
            CacheManager.Set(key, di, null);
        }
        protected string GetCacheKey(IInvocation invocation)
        {
            List<String> args = new List<string>();
            args.Add(CacheKey);
            foreach (var arg in invocation.Arguments)
            {
                if (arg == null)
                    args.Add("");
                else
                    args.Add(arg.ToString());
            }
            return string.Join(",", args.ToArray());
        }

        public abstract void Process(IInvocation invocation);
    }
}
