using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordBoxAttribute : ControlAttribute
    {
        public string Icon { get; set; }
        public string PlaceHolder { get; set; }

        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new PasswordBox
            {
                Enabled = !metadata.IsReadOnly,
                Icon = Icon,
                PlaceHolder = PlaceHolder,
                Name = metadata.PropertyName,
            };
        }
    }
}