using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class Panel : Control
    {
        public Control Control { get; set; }
        public string Title { get; set; }
        public bool IsCollapsable { get; set; }
        public bool IsClosable { get; set; }
    }
}