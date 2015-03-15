using System.Web.Mvc;

namespace Moonlit.Mvc.Html
{
    public static class CheckedExtensions
    {
        public static string Checked(this HtmlHelper html, bool ischecked)
        {
            if (ischecked)
            {
                return "checked=\"checked\"";
            }
            return string.Empty;
        }
    }
}