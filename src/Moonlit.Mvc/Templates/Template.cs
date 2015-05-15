using System.Web.Mvc;

namespace Moonlit.Mvc.Templates
{ 

    public abstract class Template  
    {
        protected Template()
        {
            this.Site = new Site();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public abstract string ViewName { get; }
        public Site Site { get; set; }

        public virtual void OnReadyRender(ControllerContext context)
        {

        }
    }
}