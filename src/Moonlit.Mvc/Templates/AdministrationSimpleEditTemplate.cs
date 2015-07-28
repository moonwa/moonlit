using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc.Templates
{
    public class AdministrationSimpleEditTemplate : Template
    {  
        public object Model { get; set; } 
        public string FormTitle { get; set; }
        public AdministrationSimpleEditTemplate(object model)
            : this()
        {
            Model = model;
        }
        public AdministrationSimpleEditTemplate()
        {
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

        public Field[] Fields { get; set; }
        public IClickable[] Buttons { get; set; }
    }
}