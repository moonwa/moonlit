namespace Moonlit.Mvc.Controls
{
    public class MultiLineTextBox : Control
    {
        public int? MaxLength { get; set; }
        public string PlaceHolder { get; set; }
        public string Value { get; set; } 
        public bool Enabled { get; set; }

        public MultiLineTextBox()
        {
            Enabled = true;
        }
    }
}