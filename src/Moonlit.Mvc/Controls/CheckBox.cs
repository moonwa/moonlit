namespace Moonlit.Mvc.Controls
{
    public class CheckBox : Control
    {
        public CheckBox()
        {
            this.Enabled = true;
        }
        public bool Checked { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Enabled { get; set; }
    }
}