using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Mvc
{
    public interface IMoonlitModel
    {
        void SetObject(string key, object target);
        object GetObject(string key);
    }
}
