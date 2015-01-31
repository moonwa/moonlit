using System;
using System.Threading.Tasks;

namespace Moonlit.Caching
{
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