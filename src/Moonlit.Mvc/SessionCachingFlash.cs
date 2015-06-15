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
        private readonly ICacheManager _cacheManager;

        public SessionCachingFlash(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager.GetPrefixCacheManager("CacheFlash::");
        }

        public void Set(object target)
        {
            _cacheManager.Set(HttpContext.Current.Session.SessionID, target, TimeSpan.FromSeconds(60));
        }

        public void Remove()
        {
            _cacheManager.Remove(HttpContext.Current.Session.SessionID);
        }

        public object Get(Type type)
        {
            var obj = _cacheManager.Get(HttpContext.Current.Session.SessionID, type);
            _cacheManager.Remove(HttpContext.Current.Session.SessionID);
            return obj;
        }

        public async Task SetAsync(object target)
        {
            await _cacheManager.SetAsync(HttpContext.Current.Session.SessionID, target, TimeSpan.FromSeconds(60)).ConfigureAwait(false);
        }

        public Task<Object> GetAsync(Type type)
        {
            return _cacheManager.GetAsync(HttpContext.Current.Session.SessionID, type);
        }

        public Task RemoveAsync()
        {
            return _cacheManager.RemoveAsync(HttpContext.Current.Session.SessionID);
        }
    }
}
