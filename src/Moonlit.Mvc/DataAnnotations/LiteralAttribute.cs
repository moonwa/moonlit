using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LiteralAttribute : ControlAttribute
    {
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new Literal
            {
                Name = metadata.PropertyName,
                Text = model == null ? "" : model.ToString(),
            };
        }
    }
}