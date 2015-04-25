using System.Collections.Generic;
using System.Web;

namespace Moonlit.Mvc
{
    public abstract class Control
    {
        public string Name { get; set; }
        public abstract IHtmlString Render( );
    }
}