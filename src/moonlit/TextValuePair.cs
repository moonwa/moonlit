namespace Moonlit
{
    public class TextValuePair : IValue
    {
        public TextValuePair(string text, string value)
        {
            Text = text;
            Value = value;
        }
        public override string ToString()
        {
            return Text;
        }
        public TextValuePair()
        {

        }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}