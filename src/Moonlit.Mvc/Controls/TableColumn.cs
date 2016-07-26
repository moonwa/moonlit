using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Moonlit.Mvc.Html;

namespace Moonlit.Mvc.Controls
{
    public class TableColumn
    {
        public string Header { get; set; }
        public Func<Control> HeaderTemplate { get; set; }

        public IHtmlString RenderHeader(HtmlHelper html)
        {
            if (!string.IsNullOrEmpty(Header))
            {
                return MvcHtmlString.Create(Header);
            }
            return html.Render(HeaderTemplate());
        }
        public string Sort { get; set; }
        public ColumnDirection Direction { get; set; }
        public Func<RowBoundItem, Control> CellTemplate { get; set; }
        public bool IsSortable { get { return !string.IsNullOrWhiteSpace(Sort); } }

        public string SortExpress
        {
            get
            {
                var sort = this.Sort;
                if (string.Equals(HttpContext.Current.Request.Params["sort"], sort, StringComparison.OrdinalIgnoreCase))
                {
                    sort = sort + " desc";
                }
                return sort;
            }
        }

        public SortDirection? SortDirection
        {
            get
            {
                var sort = this.Sort;
                if (string.Equals(HttpContext.Current.Request.Params["sort"], sort, StringComparison.OrdinalIgnoreCase))
                {
                    return System.Web.Helpers.SortDirection.Ascending;
                }
                if (string.Equals(HttpContext.Current.Request.Params["sort"], sort + " desc", StringComparison.OrdinalIgnoreCase))
                {
                    return System.Web.Helpers.SortDirection.Descending;
                }
                return null;
            }
        }

        public TableColumn()
        {

        }
    }
}