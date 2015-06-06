using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc.Styles
{
    public class Styles
    {
        private readonly Dictionary<string, StyleLink> _styleLinks = new Dictionary<string, StyleLink>(StringComparer.OrdinalIgnoreCase);
        public static Styles Current
        {
            get
            {
                var loader = DependencyResolver.Current.GetService<StylesLoader>();
                if (loader == null)
                {
                    return null;
                }
                return loader.Styles;
            }
        }
        public void RegisterStyleLink(string name, StyleLink style)
        {
            _styleLinks[name] = style;
        }

        public IHtmlString RenderCss(UrlHelper url)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (var link in _styleLinks)
            {
                buffer.Append(url.Link(link.Value));
            }
            return MvcHtmlString.Create(buffer.ToString());
        } 
    }
}
