using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Controls
{
    public class Button : Control, IClickable
    {
        public Button()
        {

        }

        public Button(string text)
            : this(text, "")
        {

        }
        public Button(string text, string actionName)
        {
            Text = text;
            ActionName = actionName;
            Name = actionName;
        }

        public string ActionName
        {
            get { return this.Name; }
            set { Name = value; }
        }

        public string Text { get; set; }

    }
}