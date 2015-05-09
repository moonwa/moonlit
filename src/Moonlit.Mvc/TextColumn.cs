using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;

namespace Moonlit.Mvc
{
    public class RowBoundItem
    {
        public object Value { get; set; }
        public object Target { get; set; }
    }
    public class TextColumn : IColumn
    {
        public string Header { get; set; }
        public string Sort { get; set; }
        public string Field { get; set; }
        public Func<RowBoundItem, string> Formatter { get; set; }

        public TextColumn()
        {
            this.Formatter = (item) => item.Value == null ? "" : item.Value.ToString();
        }
        public string RenderHeader()
        {
            TagBuilder columnBuilder = new TagBuilder("th");
            TagBuilder aBuilder = new TagBuilder("a");
            var sort = this.Sort;
            if (string.Equals(HttpContext.Current.Request.Params["sort"], sort, StringComparison.OrdinalIgnoreCase))
            {
                sort = sort + " desc";
            }
            aBuilder.Attributes["onclick"] = "submit_with_action(this, '',  'Sort', '" + sort + "')";
            aBuilder.InnerHtml = Header;
            columnBuilder.InnerHtml = aBuilder.ToString(TagRenderMode.Normal);
            var header = columnBuilder.ToString(TagRenderMode.Normal);
            return header;
        }

        public string RenderCell(object target)
        {
            var accessor = EntityAccessor.GetAccessor(target.GetType());
            var value = accessor.GetProperty(target, Field);
            var formatedValue = Formatter(new RowBoundItem() { Value = value, Target = target });

            TagBuilder tdBuilder = new TagBuilder("td");
            tdBuilder.InnerHtml = formatedValue;
            return tdBuilder.ToString(TagRenderMode.Normal);
        }
    }
    public class CheckBoxColumn : IColumn
    {
        public string Field { get; set; }
        public string RenderCell(object item)
        {
            var property = EntityAccessor.GetAccessor(item.GetType()).GetProperty(item, Field);
            if (property == null)
            {
                return string.Empty;
            }
            return "<td><input type='checkbox' name='selectedIds' value='" + property.ToString() + "'></td>";
        }

        public string RenderHeader()
        {
            return "<th></th>";
        }
    }
    public interface IColumn
    {
        string RenderCell(object item);
        string RenderHeader();
    }
}