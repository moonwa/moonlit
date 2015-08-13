namespace Moonlit.Mvc.Controls
{
    public class Hidden : Control , IHidden
    {
        public string Value { get; set; }

        public Hidden()
        {
        }
    }

    public interface IHidden
    {
    }
}