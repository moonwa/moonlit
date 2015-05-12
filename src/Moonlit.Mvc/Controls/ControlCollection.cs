using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class ControlCollection : Control
    {
        public IList<Control> Controls { get; set; }
    }
}