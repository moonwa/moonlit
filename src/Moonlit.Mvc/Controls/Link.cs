using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Controls
{
    public class Link : Control, IClickable
    {
        public string Url { get; set; }
        public string Text { get; set; }
        public LInkStyle Style { get; set; }
    }

    public enum LInkStyle
    {
        Normal,
        Button
    }
}