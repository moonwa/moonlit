using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class MoonlitContext
    {
        public static MoonlitContext Current
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;
                var context = httpContext.Items["moonlit_context"] as MoonlitContext;
                if (context == null)
                {
                    context = new MoonlitContext();
                    httpContext.Items["moonlit_context"] = context;
                }
                return context;
            }
        }
        private readonly Dictionary<string, string> _name2Scripts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _name2Styles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public void RegisterScript(string name, string script)
        {
            _name2Scripts[name] = "<script>" + script + "</script>";
        }
        public void RegisterStyle(string name, string style)
        {
            _name2Styles[name] = style;
        }

        public IHtmlString RenderScripts()
        {
            StringBuilder buffer = new StringBuilder();
            
            foreach (var name2Script in _name2Scripts)
            {
                buffer.Append(name2Script.Value);
            }
            return MvcHtmlString.Create(buffer.ToString());
        }
    }
}