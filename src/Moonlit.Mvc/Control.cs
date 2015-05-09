using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public abstract class Control
    {
        public string Name { get; set; }
        public abstract IHtmlString Render(HtmlHelper htmlHelper);
         
    }
}