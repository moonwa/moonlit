using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class TextBox : TagControl
    {
        public int? MaxLength { get; set; }
        public string PlaceHolder { get; set; }
        protected override TagBuilder CreateTagBuilder()
        {
            TagBuilder tagBuilder = new TagBuilder("input");
            if (MaxLength != null)
            {
                tagBuilder.Attributes["maxlength"] = this.MaxLength.ToString();
            }
            if (!string.IsNullOrWhiteSpace(PlaceHolder))
            {
                tagBuilder.Attributes["placeholder"] = this.PlaceHolder.Trim();
            }
            return tagBuilder;
        }
    }

    public class Table
    {
        public Column[] Columns { get; set; }
    }

    public class Column
    {
        public ColumnHeader Header { get; set; }
        public ColumnCell Cell { get; set; }
    }

    public class ColumnCell
    {
    }

    public abstract class ColumnHeader
    {
        public string Sort { get; set; }
    }
    public class TextColumnHeader : ColumnHeader
    {
        public string Text { get; set; }
    }
}