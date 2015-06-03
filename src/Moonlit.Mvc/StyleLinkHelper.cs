using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public static class StyleLinkHelper
    {
        public static string Link(this UrlHelper url, Theme theme, StyleLink link)
        {
            TagBuilder tagBuilder = new TagBuilder("link");
            tagBuilder.Attributes["rel"] = "stylesheet";
            tagBuilder.Attributes["type"] = "text/css";
            if (link.Media != StyleLinkMedia.Normal)
            {
                tagBuilder.Attributes["media"] = link.Media.ToString().ToLowerInvariant();
            }
            if (!string.IsNullOrWhiteSpace(link.Id))
            {
                tagBuilder.Attributes["id"] = link.Id;
            }

            tagBuilder.Attributes["href"] = link.Href;
            if (link.Criteria != null)
            {
                return link.Criteria.BeginTag + tagBuilder.ToString(TagRenderMode.Normal) + link.Criteria.EndTag;
            }
            return tagBuilder.ToString(TagRenderMode.Normal);
        }
    }
}