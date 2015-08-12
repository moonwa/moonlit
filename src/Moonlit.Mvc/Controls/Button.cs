using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Controls
{
    public class Button : Control, IClickable
    {
        [System.Obsolete]
        public Button()
        {
            
        }
        public Button(string text, string actionName)
        {
            Text = text;
            ActionName = actionName;
        }

        public string ActionName { get; set; }
        public string Text { get; set; }

    }
}