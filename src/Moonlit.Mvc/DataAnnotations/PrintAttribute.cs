using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PrintAttribute : Attribute, IMetadataAware
    {
        private readonly string _template;

        public PrintAttribute(string template)
        {
            _template = template;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.TemplateHint = "print/" + _template;
            metadata.AdditionalValues[HiddenLabelAttribute.MetadataAdditionalKey] = true;
        }

    }
}