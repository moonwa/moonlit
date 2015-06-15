namespace Moonlit.Mvc.Controls
{
    public class PasswordBox : Control
    {
        public string PlaceHolder { get; set; }
        public string Icon { get; set; }
        
        public bool Enabled{ get; set; }

        public PasswordBox()
        {
            Enabled = true;
        }
    }
}