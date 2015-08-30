using System;
using System.Collections;
using System.Collections.Generic;
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
            Fields = new List<Field>();
            Buttons = new List<IClickable>();
        }
        public override void OnReadyRender(ControllerContext context)
        {
            foreach (var criterion in this.Fields)
            {
                criterion.OnReadyRender(context);
            }
        }

        public List<Field> Fields { get; set; }
        public List<IClickable> Buttons { get; set; }
    }
}