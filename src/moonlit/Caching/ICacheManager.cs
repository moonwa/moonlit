using System;
using System.Threading.Tasks;

namespace Moonlit.Caching
{
    public static class CacheManagerHelper
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
    }
    public interface ICacheManager
    {
        /// <summary>
        ///     set a item into cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object Get(string key, Type type);
        /// <summary>
        ///     set a item into cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exist(string key);

        /// <summary>
        ///     get a item from cache by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expired"></param>
        void Set(string key, object value, TimeSpan? expired);

        /// <summary>
        ///     remove a item from cache by key
        /// </summary>
        /// <param name="key">the key of item</param>
        /// <returns></returns>
        void Remove(string key);

        /// <summary>
        ///     set a item into cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiredTime"></param>
        /// <returns></returns>
        Task SetAsync(string key, object value, TimeSpan? expiredTime);

        /// <summary>
        ///     get a item from cache by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<object> GetAsync(string key, Type type);

        /// <summary>
        ///     remove a item from cache by key
        /// </summary>
        /// <param name="key">the key of item</param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}