using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    public class CheckBoxAttribute : ControlAttribute
    {
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new CheckBox 
            {
                Checked = model != null && Convert.ToBoolean(model),
                Enabled = !metadata.IsReadOnly,
                Name = metadata.PropertyName,
                Text = metadata.DisplayName,
                Value = "true",
            };
        }
    }
}