using System;
using System.Collections;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc.Templates
{
    public class SimpleBoxTemplate : Template
    { 

        private readonly ControllerContext _controllerContext;
        public override string ViewName { get { return "templates/SimpleBox"; } }

        public SimpleBoxTemplate(ControllerContext controllerContext)
        {
            _controllerContext = controllerContext;
            Fields = new Field[0];
            Buttons = new IClickable[0];
        }
        public override void OnReadyRender(ControllerContext context)
        {
            foreach (var criterion in this.Fields)
            {
                criterion.OnReadyRender(context);
            }
        }

        public Field[] Fields { get;   set; }
        public IClickable[] Buttons { get; set; }
        public List Additional { get; set; }
    }

}