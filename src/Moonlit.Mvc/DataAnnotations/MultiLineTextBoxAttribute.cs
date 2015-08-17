using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MultiLineTextBoxAttribute : ControlAttribute
    {
        public string PlaceHolder { get; set; }
        public int? MaxLength { get; set; }
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new MultiLineTextBox
            {
                Name = metadata.PropertyName,
                Value = model == null ? "" : model.ToString(),
                Enabled = !metadata.IsReadOnly,
                PlaceHolder = PlaceHolder,
                MaxLength = MaxLength
            };
        }
 
    }
}