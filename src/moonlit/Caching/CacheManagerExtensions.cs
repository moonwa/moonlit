using System;
using System.Threading.Tasks;

namespace Moonlit.Caching
{
    public static class CacheManagerExtensions
    {
        public static void Set<T>(this ICacheManager cacheManager, string key, T value, TimeSpan? expiredTime)
        {
            cacheManager.Set(key, value, expiredTime);
        }
        public static Task SetAsync<T>(this ICacheManager cacheManager, string key, T value, TimeSpan? expiredTime)
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