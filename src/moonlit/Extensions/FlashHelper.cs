using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit
{
    public static class FlashHelper
    {
        public static T Get<T>(this IFlash flash)
        {
            return (T)flash.Get(typeof(T));
        }
        public static async Task<T> GetAsync<T>(this IFlash flash)
        {
            return (T)await flash.GetAsync(typeof(T)).ConfigureAwait(false);
        }
    }
}
