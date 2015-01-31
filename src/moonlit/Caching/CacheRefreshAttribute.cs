using System;

namespace Moonlit.Caching
{
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CacheRefreshAttribute : CacheAttribute
    {
        public string CacheKey { get; set; }

        public CacheRefreshAttribute(string cacheKey)
        {
            CacheKey = cacheKey;
        } 
    }
}
