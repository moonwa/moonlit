using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlit.Caching
{
    public class MemoryCacheManager : ICacheManager
    {
        private Timer _timer;
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
            _timer = new Timer(OnTimer, null, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));
        }

        private void OnTimer(object state)
        {
            lock (_itemsLocker)
            {
                List<string> removeKeys = new List<string>();
                foreach (var cacheItem in _cacheItems)
                {
                    if (cacheItem.Value.ExpiredTime < DateTime.Now)
                    {
                        removeKeys.Add(cacheItem.Key);
                    }
                }
                foreach (var removeKey in removeKeys)
                {
                    _cacheItems.Remove(removeKey);
                }
            }
        }

        class CacheItem
        {
            public object Data { get; set; }
            public DateTime ExpiredTime { get; set; }
        }

        readonly IDictionary<string, CacheItem> _cacheItems = new Dictionary<string, CacheItem>(StringComparer.OrdinalIgnoreCase);

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

        public void Set(string key, object value, TimeSpan? expiredTime)
        {
            expiredTime = expiredTime ?? DefaultExpiredTime;
            lock (_itemsLocker)
            {
                _cacheItems[key] = new CacheItem
                {
                    Data = value,
                    ExpiredTime = DateTime.Now.Add(expiredTime.Value)
                };
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
                CacheItem item;
                if (_cacheItems.TryGetValue(key, out item))
                {
                    if (item.ExpiredTime < DateTime.Now)
                    {
                        _cacheItems.Remove(key);
                        return null;
                    }
                    return item.Data;
                }
                return null;
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
                _cacheItems.Remove(key);
            }
        }
    }
}