using System;

namespace Moonlit.Caching
{
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CacheQueryAttribute : CacheAttribute
    {
        public string CacheKey { get; set; }

        public CacheQueryAttribute(string cacheKey)
        {
            CacheKey = cacheKey;
        } 
    }
}
