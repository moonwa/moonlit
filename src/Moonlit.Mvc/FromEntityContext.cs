using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class FromEntityContext
    {
        public bool IsPostback { get; set; }
        public ControllerContext ControllerContext { get; set; }
    }
}