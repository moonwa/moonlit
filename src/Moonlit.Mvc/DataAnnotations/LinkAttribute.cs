using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LinkAttribute : ControlAttribute
    {
        public LinkAttribute()
        {
            Style = LinkStyle.Normal;
        }
        public LinkStyle Style { get; set; }
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            if (model == null)
            {
                return null;
            }
            var containerTypeMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, metadata.ContainerType);
            var url = containerTypeMetadata.MakeUrl(controllerContext);

            return new Link(model.ToString(), url, Style);
        }
    }
}