using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlit.Caching
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly object _itemsLocker = new object();
        /// <summary>
        /// the defautl expired time
        /// </summary>
        public TimeSpan DefaultExpiredTime { get; set; }
        /// <summary>
        /// constructor of <see cref="MemoryCacheManager"/>
        /// </summary>
        public MemoryCacheManager()
        {
            DefaultExpiredTime = TimeSpan.FromMinutes(5);

        }

        /// <summary>
        /// set a item into cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiredTime"></param>
        /// <returns></returns>
        public Task SetAsync(string key, object value, TimeSpan? expiredTime)
        {
            Set(key, value, expiredTime);
            return Task.FromResult(false);
        }


        public bool Exist(string key)
        {
            return Get(key, typeof(object)) != null;
        }
        private System.Runtime.Caching.MemoryCache _cacheStore = new System.Runtime.Caching.MemoryCache("aa");

        public void Set(string key, object value, TimeSpan? expiredTime)
        {
            expiredTime = expiredTime ?? DefaultExpiredTime;
            lock (_itemsLocker)
            {
                _cacheStore.Set(key, value, DateTimeOffset.Parse(expiredTime.ToString()));
            }
        }

        /// <summary>
        /// get a item from cache by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<object> GetAsync(string key, Type type)
        {
            return Task.FromResult((object)Get(key, type));
        }
        public object Get(string key, Type type)
        {
            lock (_itemsLocker)
            {
                return _cacheStore.Get(key);
            }
        }

        /// <summary>
        /// remove a item from cache by key
        /// </summary>
        /// <param name="key">the key of item</param>
        /// <returns></returns>
        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.FromResult(false);
        }

        public void Remove(string key)
        {
            lock (_itemsLocker)
            {
                _cacheStore.Remove(key);
            }
        }
    }
}