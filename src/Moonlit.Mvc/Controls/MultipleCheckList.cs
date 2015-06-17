using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class MultiSelectList : Control
    {
        public bool Enabled { get; set; }

        public MultiSelectList()
        {
            Enabled = true;
        }
        public IList<SelectListItem> Items { get; set; }
    }
}