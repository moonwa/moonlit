using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Controls
{
    public class Button : Control, IClickable
    {
        public string ActionName { get; set; }
        public string Text { get; set; }

    }
}