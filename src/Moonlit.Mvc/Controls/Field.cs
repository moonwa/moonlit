using System.Web.Mvc;

namespace Moonlit.Mvc.Controls
{
    public class Field : Control
    {
        public string Label { get; set; }
        public string FieldName { get; set; }
        public Control Control { get; set; }
        public int Width { get; set; }
        public string Description { get; set; }

        public void OnReadyRender(ControllerContext context)
        {
            if (this.Control != null)
            {
                Control.Name = FieldName;
            }
        }
    }
}