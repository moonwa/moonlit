using System.Collections.Generic;
using System.Linq;
using Moonlit.Caching;

namespace Moonlit
{
    public class CacheKeyManager
    {
        private readonly ICacheManager _cacheManager;

        public CacheKeyManager(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        private static object _locker = new object();
        List<string> _registeredCacheKeys = new List<string>();
        public void RegisterCacheKey(string cacheKey)
        {

            if (Clone().Any(x => string.Equals(x, cacheKey)))
            {
                return;
            }
            lock (_locker)
            {
                if (Clone().Any(x => string.Equals(x, cacheKey)))
                {
                    _registeredCacheKeys.Add(cacheKey);
                }
            }
        }

        private List<string> Clone()
        {
            lock (_locker)
            {
                return _registeredCacheKeys.ToList();
            }
        }


        public void ClearCache()
        {
            foreach (var cacheKey in Clone())
            {
                _cacheManager.Remove(cacheKey);
            }
        }
    }
}