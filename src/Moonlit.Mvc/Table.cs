using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Table
    {
        public IColumn[] Columns { get; set; }
        public IHtmlString Render(IEnumerable items)
        {
            TagBuilder tableBuilder = new TagBuilder("table");
            tableBuilder.AddCssClass("table table-striped table-hover");
            tableBuilder.InnerHtml += CreateHeaderTagBuilder().ToString(TagRenderMode.Normal);
            tableBuilder.InnerHtml += CreateBodyTagBuilder(items).ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(tableBuilder.ToString(TagRenderMode.Normal));
        }

        private TagBuilder CreateBodyTagBuilder(IEnumerable items)
        {

            TagBuilder bodyBuilder = new TagBuilder("tbody");

            foreach (var item in items)
            {
                var trBuilder = new TagBuilder("tr");
                foreach (var column in Columns)
                {
                    trBuilder.InnerHtml += column.RenderCell(item);
                }
                bodyBuilder.InnerHtml += trBuilder.ToString(TagRenderMode.Normal);
            }
            return bodyBuilder;
        }

        private TagBuilder CreateHeaderTagBuilder()
        {
            TagBuilder headerBuilder = new TagBuilder("thead");
            foreach (var column in Columns)
            {
                headerBuilder.InnerHtml += column.RenderHeader();
            }
            return headerBuilder;
        }
    }
}