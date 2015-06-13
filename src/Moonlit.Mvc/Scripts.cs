using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Scripts
    {
        private readonly Dictionary<string, Script> _scripts = new Dictionary<string, Script>(StringComparer.OrdinalIgnoreCase);
        public static Scripts Current
        {
            get
            {
                return HttpContext.Current.GetObject<Scripts>(true);
            }
        }

        public void RegisterScript(string name, Script script)
        {
            _scripts[name] = script;
        }
        public IHtmlString RenderScripts(UrlHelper url)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (KeyValuePair<string, Script> name2Script in _scripts)
            {
                var script = name2Script.Value;
                buffer.AppendLine(script.ToString(url));
            }
            return MvcHtmlString.Create(buffer.ToString());
        }
    }
}
