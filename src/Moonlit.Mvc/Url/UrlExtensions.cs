using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Moonlit.Mvc.Url
{
    public static class UrlExtensions
    {
        public static string Asset(this System.Web.Mvc.UrlHelper urlHelper, string url)
        {
            return "http://assets.chihank.com/" + url;
        }
    }
}
