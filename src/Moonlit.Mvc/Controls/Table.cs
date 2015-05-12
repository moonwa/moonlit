using System;
using System.Collections;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Moonlit.Mvc.Controls
{
    public class Table : Control
    {
        public TableColumn[] Columns { get; set; }
        public IEnumerable DataSource { get; set; }
    }

    public class RowBoundItem
    {
        public object Target { get; set; }
    }

    public enum ColumnDirection
    {
        Left, Center, Right
    }
    public class TableColumn
    {
        public string Header { get; set; }
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

    public class Literal : Control
    {
        public string Text { get; set; }
    }

    //    public class CheckBoxColumn 
    //    {
    //        public string Field { get; set; }
    //        public string RenderCell(object item)
    //        {
    //            var property = EntityAccessor.GetAccessor(item.GetType()).GetProperty(item, Field);
    //            if (property == null)
    //            {
    //                return string.Empty;
    //            }
    //            return "<td><input type='checkbox' name='selectedIds' value='" + property.ToString() + "'></td>";
    //        }
    //
    //
    //    } 
}