using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    public class DatePickerAttribute : ControlAttribute
    {
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new DatePicker 
            {
                Name = metadata.PropertyName,
                Value = model == null ? (DateTime?) null : Convert.ToDateTime(model)
            };
        }
    }
}