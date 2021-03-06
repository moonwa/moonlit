using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TextBoxAttribute : ControlAttribute
    {
        public string Icon { get; set; }
        public string PlaceHolder { get; set; }
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new TextBox
            {
                Enabled = !metadata.IsReadOnly,
                Name = metadata.PropertyName,
                Icon = Icon,
                Value = model == null ? "" : model.ToString(),
                PlaceHolder = this.PlaceHolder,
            };
        }
    }
}