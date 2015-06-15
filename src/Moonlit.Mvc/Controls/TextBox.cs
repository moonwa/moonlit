namespace Moonlit.Mvc.Controls
{
    public class TextBox : Control
    {
        public int? MaxLength { get; set; }
        public string PlaceHolder { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public bool Enabled { get; set; }

        public TextBox()
        {
            Enabled = true;
        }
    }
}