using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    public abstract class Control
    {
        public string Name { get; set; }
        public string Id
        {
            get { return string.IsNullOrEmpty(Name) ? "" : TagBuilder.CreateSanitizedId(Name); }
        }

        protected virtual IDictionary<string, string> CreateAttributes()
        {
            return new Dictionary<string, string>();
        }

        public IHtmlString RenderAttributes()
        {
            StringBuilder builder = new StringBuilder();
            var attributes = this.CreateAttributes();
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    if (attribute.Value != null)
                    {
                        builder.AppendFormat(" {0}=\"{1}\"", attribute.Key, attribute.Value);
                    }
                }
            }
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}