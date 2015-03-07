using System;
using System.Threading.Tasks;
using Moonlit.Caching;

namespace Moonlit
{
    public interface IFlash
    {
        /// <summary>
        ///     set a item into cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Get( Type type);

        /// <summary>
        ///     get a item from cache by key
        /// </summary>
        /// <param name="value"></param>
        void Set(  object value);

        /// <summary>
        ///     remove a item from cache by key
        /// </summary>
        /// <param name="key">the key of item</param>
        /// <returns></returns>
        void Remove( );

        /// <summary>
        ///     set a item into cache
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync( object value);

        /// <summary>
        ///     get a item from cache by key
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<object> GetAsync(Type type);

        /// <summary>
        ///     remove a item from cache by key
        /// </summary>
        /// <returns></returns>
        Task RemoveAsync( );
    }

}