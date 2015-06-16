using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class MultipleSelectList : Control
    {
        public bool Enabled { get; set; }

        public MultipleSelectList()
        {
            Enabled = true;
        }
        public IList<SelectListItem> Items { get; set; }
    }
}