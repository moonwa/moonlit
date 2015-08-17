using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HiddenAttribute : ControlAttribute 
    {
        #region Overrides of ControlAttribute

        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new Hidden
            {
                Name = metadata.PropertyName,
                Value = model == null ? "" : model.ToString()
            };
        }
         

        #endregion
    }
}