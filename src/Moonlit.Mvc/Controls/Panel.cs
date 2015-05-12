using System.Collections.Generic;

namespace Moonlit.Mvc.Controls
{
    public class TabTable : Control
    {
        public string Title { get; set; }
        public Control Control { get; set; }
    }
    public class Panel : Control
    {
        public Control Control { get; set; }
        public string Title { get; set; }
        public bool IsCollapsable { get; set; }
        public bool IsClosable { get; set; }
    }

    public class Form : Control 
    {
        public Form()
        {
            this.Type = FormType.Horizontal;
        }

        public FormType Type { get; set; }
        public Control Control { get; set; }
    }

    public enum FormType
    {
        Horizontal
    }
}