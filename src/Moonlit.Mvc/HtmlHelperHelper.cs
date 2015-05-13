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
        public static bool HasError(this HtmlHelper helper, string key)
        {
            ModelState modelState;
            if (helper.ViewData.ModelState.TryGetValue(key, out modelState))
            {

                if (modelState.Errors != null && modelState.Errors.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static string GetError(this HtmlHelper helper, string key)
        {
            ModelState modelState;
            if (helper.ViewData.ModelState.TryGetValue(key, out modelState))
            {

                if (modelState.Errors != null && modelState.Errors.Count > 0)
                {
                    return modelState.Errors[0].ErrorMessage;
                }
            }
            return string.Empty;
        }
    }
}
