using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc.Controls
{
    public class MultiSelectList : Control
    {
        public bool Enabled { get; set; }

        public MultiSelectList(IEnumerable<SelectListItem> items, IEnumerable selectedValues)
        {
            Enabled = true;
            Items = new System.Web.Mvc.MultiSelectList(items, "Value", "Text", selectedValues);
        }
        public System.Web.Mvc.MultiSelectList Items { get; set; }
    }
}