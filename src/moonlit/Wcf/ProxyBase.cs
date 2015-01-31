using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Wcf
{
    public class ProxyBase<TClient>
        where TClient : new()
    {
        public virtual TClient CreateClient()
        {
            return new TClient();
        }
    }
}
