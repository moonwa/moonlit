using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class TextBox : Editor
    {
        public int? MaxLength { get; set; }
        public string PlaceHolder { get; set; }
    }
    public class DropdownList : Editor
    {
        public string PlaceHolder { get; set; }
        public IList<SelectListItem> Items { get; set; }
    }

    public class SelectListItem
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public bool IsSelected { get; set; }
    }
}