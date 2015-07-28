using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public interface IControllBuilder
    {
        Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext);
    }
}