using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class List : Control
    {
        public List()
        {
            this.Items = new List<Control>();
        }
        public IList<Control> Items { get; set; }
        public ListStyle Style { get; set; }
    }
}