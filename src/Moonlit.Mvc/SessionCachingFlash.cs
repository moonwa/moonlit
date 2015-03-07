using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Moonlit.Caching;

namespace Moonlit.Mvc
{
    public class SessionCachingFlash : IFlash
    {
        private const string CacheKeyPrefix = "CacheFlash::";
        private readonly ICacheManager _cacheManager;

        public SessionCachingFlash(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void Set(object target)
        {
            var key = CacheKeyPrefix + HttpContext.Current.Session.SessionID;
            _cacheManager.Set(key, target, TimeSpan.FromSeconds(60));
        }

        public void Remove()
        {
            var key = CacheKeyPrefix + HttpContext.Current.Session.SessionID;
            _cacheManager.Remove(key);
        }

        public object Get(Type type)
        {
            var key = CacheKeyPrefix + HttpContext.Current.Session.SessionID;
            return _cacheManager.Get(key, type);
        }

        public async Task SetAsync(object target)
        {
            var key = CacheKeyPrefix + HttpContext.Current.Session.SessionID;
            await _cacheManager.SetAsync(key, target, TimeSpan.FromSeconds(60)).ConfigureAwait(false);
        }

        public Task<Object> GetAsync(Type type)
        {
            var key = CacheKeyPrefix + HttpContext.Current.Session.SessionID;
            return _cacheManager.GetAsync(key, type);
        }

        public Task RemoveAsync()
        {
            var key = CacheKeyPrefix + HttpContext.Current.Session.SessionID;
            return _cacheManager.RemoveAsync(key);
        }
    }
}
