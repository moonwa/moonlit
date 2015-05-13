using System.Web.Mvc;

namespace Moonlit.Mvc.Templates
{
    public interface ITemplate
    {
        string Title { get; }
        string Description { get; }
        string ViewName { get; }
        void OnReadyRender(ControllerContext context);
    }

    public abstract class Template : ITemplate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public abstract string ViewName { get; }
        public virtual void OnReadyRender(ControllerContext context)
        {

        }
    }
}