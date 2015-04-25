using System.Web.Mvc;

namespace Moonlit.Mvc.Templates
{
    public interface ITemplate
    {
        string ViewName { get; }
        void OnReadyRender(ControllerContext context);
    }
}