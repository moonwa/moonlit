using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Script
    {
        public string Content { get; set; }
        public string Src { get; set; }
        public string Id { get; set; }
        public IeVersionCriteria Criteria { get; set; }
        public string ToString(UrlHelper url)
        {
            TagBuilder tagBuilder = new TagBuilder("script");

            if (!string.IsNullOrWhiteSpace(Id))
            {
                tagBuilder.Attributes["id"] = Id;
            }

            if (!string.IsNullOrWhiteSpace(Src))
            {
                tagBuilder.Attributes["src"] = Src;
            }

            if (!string.IsNullOrWhiteSpace(Content))
            {
                tagBuilder.InnerHtml = Content;
            }

            if (Criteria != null)
            {
                return Criteria.BeginTag + tagBuilder.ToString(TagRenderMode.Normal) + Criteria.EndTag;
            }
            return tagBuilder.ToString(TagRenderMode.Normal);
        }
    }
}