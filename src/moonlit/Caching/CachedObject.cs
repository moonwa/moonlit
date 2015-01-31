using System;

namespace Moonlit.Caching
{
    public sealed class CachedObject<TCachedItems>
        where TCachedItems : class
    {
        private readonly ICacheManager _cacheManager;
        private readonly Func<TCachedItems> _cachedItems;
        private readonly string _cacheKey;
        private readonly Locker _locker = new Locker();
        public CachedObject(ICacheManager cacheManager, string cacheKey, Func<TCachedItems> cachedItems)
        {
            if (cacheManager == null) throw new ArgumentNullException("cacheManager");
            if (cacheKey == null) throw new ArgumentNullException("cacheKey");
            if (cachedItems == null) throw new ArgumentNullException("cachedItems");

            _cacheManager = cacheManager;
            _cacheKey = cacheKey;
            _cachedItems = cachedItems;
        }

        public void ClearCache()
        {
            _cacheManager.Remove(_cacheKey);
        }
        public void ClearCache(string cacheKey)
        {
            _cacheManager.Remove(cacheKey);
        }
        public TCachedItems GetData()
        {
            return GetDataCore(_cacheKey);
        }

        private TCachedItems GetDataCore(string cacheKey)
        {
            TCachedItems cachedItems = (TCachedItems)_cacheManager.Get(cacheKey, typeof(TCachedItems));
            if (cachedItems == null)
            {
                lock (_locker.GetLocker(this.GetType().FullName))
                {
                    cachedItems = (TCachedItems)_cacheManager.Get(cacheKey, typeof(TCachedItems));
                    if (cachedItems == null)
                    {
                        cachedItems = _cachedItems();
                        _cacheManager.Set(cacheKey, cachedItems, null);
                    }
                }
            }
            return cachedItems;
        }
    }
}