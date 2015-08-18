using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Controls
{
    public class Link : Control, IClickable
    {
        public Link()
        {

        }
        public Link(string text, string url)
            : this(text, url, LinkStyle.Normal)
        {
        }

        public Link(string text, string url, LinkStyle style)
        {
            Text = text;
            Url = url;
            Style = style;
        }

        public string Url { get; set; }
        public string Text { get; set; }
        public LinkStyle Style { get; set; }
    }
}