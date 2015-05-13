using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
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

    public class Script
    {
        public string Content { get; set; }
        public string Src { get; set; }
        public string Id { get; set; }
        public IeVersionCriteria Criteria { get; set; }
        public string ToString(UrlHelper url, Theme theme)
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
    public class StyleLink
    {
        public string Id { get; set; }
        public string Href { get; set; }
        public StyleLinkMedia Media { get; set; }
        public IeVersionCriteria Criteria { get; set; }

        public StyleLink()
        {
            Media = StyleLinkMedia.Normal;
        }
    }

    public enum IeVersionCriteriaOperator
    {
        Lt, Lte, Gt, Gte, Eq
    }
    public class IeVersionCriteria : IHtmlElementCriteria
    {
        public IeVersionCriteria(int version, IeVersionCriteriaOperator @operator)
        {
            Version = version;
            Operator = @operator;
        }

        public IeVersionCriteriaOperator Operator { get; set; }
        public int Version { get; set; }

        public string BeginTag
        {
            get { return string.Format("<!--[if {0} IE {1}]><!-->", GetOperatorText(), Version); }
        }

        private string GetOperatorText()
        {
            switch (Operator)
            {
                case IeVersionCriteriaOperator.Lt:
                    return "lt";
                case IeVersionCriteriaOperator.Lte:
                    return "lte";
                case IeVersionCriteriaOperator.Gt:
                    return "gt";
                case IeVersionCriteriaOperator.Gte:
                    return "gte";
                case IeVersionCriteriaOperator.Eq:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string EndTag { get { return "<![endif]-->"; } }
    }
    public interface IHtmlElementCriteria
    {
        string BeginTag { get; }
        string EndTag { get; }
    }
    public enum StyleLinkMedia
    {
        Normal,
        Print
    }
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

        public Theme Theme { get; internal set; }

        private readonly Dictionary<string, Script> _scripts = new Dictionary<string, Script>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, StyleLink> _styleLinks = new Dictionary<string, StyleLink>(StringComparer.OrdinalIgnoreCase);
        public void RegisterScript(string name, Script script)
        {
            _scripts[name] = script;
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
                buffer.Append(url.Link(Theme, link.Value));
            }
            return MvcHtmlString.Create(buffer.ToString());
        }
        public IHtmlString RenderScripts(UrlHelper url)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (KeyValuePair<string, Script> name2Script in _scripts)
            {
                var script = name2Script.Value;
                buffer.AppendLine(script.ToString(url, Theme));
            }
            return MvcHtmlString.Create(buffer.ToString());
        }

        public IFlash Flash
        {
            get { return DependencyResolver.Resolve<IFlash>(); }
        }

        internal IDependencyResolver DependencyResolver { get; set; }
    }


}