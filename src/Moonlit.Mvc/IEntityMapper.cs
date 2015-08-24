using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public interface IEntityMapper<T>
    {
        void ToEntity(T entity, ControllerContext controllerContext);
    }
}