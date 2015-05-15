using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class TextBox : Control
    {
        public int? MaxLength { get; set; }
        public string PlaceHolder { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
    }
    public class List : Control
    {
        public List()
        {
            this.Items = new List<Control>();
        }
        public IList<Control> Items { get; set; }
        public ListStyle Style { get; set; }
    }

    public enum ListStyle
    {
        Unstyled
    }
}