using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc.Templates
{
    public static class TemplateHelper
    {
        public static Field[] MakeFields(object model, ControllerContext controllerContext)
        {
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            var props = modelMetadata.Properties.Where(x => ModelMetadataHelper.GetField(x) != null && ModelMetadataHelper.GetControlBuilder(x) != null).OrderBy(x => x.GetSort()).ToList();
            return props.Select(x =>
            {
                var field = x.GetField();
                field.Control = x.GetControlBuilder().CreateControl(x, x.Model, controllerContext);
                field.Name = x.PropertyName;
                return field;
            }).ToArray();
        }
    }
}