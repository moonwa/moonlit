using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Styles
    {
        private readonly Dictionary<string, StyleLink> _styleLinks = new Dictionary<string, StyleLink>(StringComparer.OrdinalIgnoreCase);
        public static Styles Current
        {
            get { return HttpContext.Current.GetObject<Styles>(true); }
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
