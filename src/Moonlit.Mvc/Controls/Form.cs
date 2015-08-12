namespace Moonlit.Mvc.Controls
{
    public class Form : Control 
    {
        public Form()
        {
            this.Type = FormType.Horizontal;
        }

        public FormType Type { get; set; }
        public Control Control { get; set; }
    }
}