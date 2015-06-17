using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class SelectList : Control
    {
        public bool Enabled { get; set; }

        public SelectList()
        {
            Enabled = true;
        }
        public string PlaceHolder { get; set; }
        public IList<SelectListItem> Items { get; set; }
    }
}