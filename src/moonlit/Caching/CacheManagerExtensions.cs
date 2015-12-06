using System;
using System.Threading.Tasks;

namespace Moonlit.Caching
{
    public static class CacheManagerExtensions
    {
        class PrefixKeyCacheManager : ICacheManager
        {
            private readonly string _prefix;
            private readonly ICacheManager _cacheManager;

            public PrefixKeyCacheManager(string prefix, ICacheManager cacheManager)
            {
                _prefix = prefix;
                _cacheManager = cacheManager;
            }

            public object Get(string key, Type type)
            {
                return _cacheManager.Get(MakeKey(key), type);
            }

            public bool Exist(string key)
            {
                return _cacheManager.Exist(MakeKey(key));
            }

            private string MakeKey(string key)
            {
                return _prefix + key;
            }

            public void Set(string key, object value, TimeSpan? expired)
            {
                _cacheManager.Set(MakeKey(key), value, expired);
            }

            public void Remove(string key)
            {
                _cacheManager.Remove(MakeKey(key));
            }

            public Task SetAsync(string key, object value, TimeSpan? expiredTime)
            {
                return _cacheManager.SetAsync(MakeKey(key), value, expiredTime);
            }

            public Task<object> GetAsync(string key, Type type)
            {
                return _cacheManager.GetAsync(MakeKey(key), type);
            }

            public Task RemoveAsync(string key)
            {
                return _cacheManager.RemoveAsync(MakeKey(key));
            }
        }

        public static ICacheManager GetPrefixCacheManager(this ICacheManager cacheManager, string prefix)
        {
            return new PrefixKeyCacheManager(prefix, cacheManager);
        }

        public static void Set<T>(this ICacheManager cacheManager, string key, T value, TimeSpan? expiredTime = null)
        {
            cacheManager.Set(key, value, expiredTime);
        }
        public static Task SetAsync<T>(this ICacheManager cacheManager, string key, T value, TimeSpan? expiredTime = null)
        {
            return cacheManager.SetAsync(key, value, expiredTime);
        }
        public static T Get<T>(this ICacheManager cacheManager, string key)
        {
            return (T)cacheManager.Get(key, typeof(T));
        }
        public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key)
        {
            var item = await cacheManager.GetAsync(key, typeof(T));
            return (T)item;
        }
    }
}