using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc.Themes.Dojo
{
    public class DojoHelper
    {
        private readonly HttpContextBase _context;
        private List<string> _requires;

        internal DojoHelper(HttpContextBase context)
        {
            _context = context;
            _requires = new List<string>();
        }
        public DojoHelper AddRequire(string require)
        {
            _requires.Add(require);
            return this;
        }

        public IHtmlString Requires(params string[] requires)
        {
            var list = requires.Concat(_requires);
            var html = string.Join(",", list.Distinct(StringComparer.OrdinalIgnoreCase).Select(x=> "'" + x + "'"));
            return MvcHtmlString.Create(html);
        }
    }
}