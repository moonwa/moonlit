using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class DropdownList : Control
    {
        public bool Enabled { get; set; }

        public DropdownList()
        {
            Enabled = true;
        }
        public string PlaceHolder { get; set; }
        public IList<SelectListItem> Items { get; set; }
    }
}