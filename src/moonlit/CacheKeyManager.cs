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

        List<string> _registeredCacheKeys = new List<string>();
        public void RegisterCacheKey(string cacheKey)
        {
            if (_registeredCacheKeys.Any(x => string.Equals(x, cacheKey)))
            {
                return;
            }
            _registeredCacheKeys.Add(cacheKey);
        }

        public void ClearCache()
        {
            foreach (var cacheKey in _registeredCacheKeys)
            {
                _cacheManager.Remove(cacheKey);
            }
        }
    }
}