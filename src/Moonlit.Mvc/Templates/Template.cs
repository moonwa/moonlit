using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc.Templates
{

    public abstract class Template
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public abstract string ViewName { get; }

        public ISite Site { get; set; }

        public virtual void OnReadyRender(ControllerContext context)
        {

        }

    }

    public interface ISite
    {
        string Title { get; }
        string CopyRight { get; }
    }
}