using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    public static class ControlHelper
    {
        public static ControlCollection Combine<T>(this IEnumerable<T> items) where T : Control
        {
            return new ControlCollection
            {
                Controls = items.Cast<Control>().ToList()
            };
        }
    }
    public static class HtmlHelperHelper
    {
        public static object GetModelStateValue(this HtmlHelper helper, string key, Type destinationType)
        {
            ModelState modelState;
            if (helper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }
    }
}
