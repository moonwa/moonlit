using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class ButtonGroup : Control, IClickable
    {
        public string Text { get; set; }
        public List<Button> Buttons { get; set; }

        public ButtonGroup(string text)
        {
            Text = text;
            Buttons = new List<Button>();
        }
    }
}