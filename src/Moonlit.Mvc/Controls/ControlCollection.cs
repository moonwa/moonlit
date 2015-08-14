using System.Collections.Generic;
using System.Linq;

namespace Moonlit.Mvc.Controls
{
    public class ControlCollection : Control
    {
        public ControlCollection()
        {
            Controls = new List<Control>();
        }

        public ControlCollection(params Control[] controls)
        {
            Controls = controls.ToList();
        }
        public IList<Control> Controls { get; set; }
    }
}