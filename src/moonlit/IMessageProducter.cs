using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit
{
    public interface IMessageProducter<TMessage>
    {
        Task PostAsync(TMessage message);
    }
}
