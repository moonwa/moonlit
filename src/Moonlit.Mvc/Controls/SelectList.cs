using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc.Controls
{
    public class SelectList : Control
    {
        public bool Enabled { get; set; }

        public SelectList(IEnumerable<SelectListItem> selectListItems, string selectedValue)
        {
            Items = new System.Web.Mvc.SelectList(selectListItems, "Value", "Text", selectedValue);
            Enabled = true;
        }

        public System.Web.Mvc.SelectList Items { get; set; }
        public string PlaceHolder { get; set; }
    }
}