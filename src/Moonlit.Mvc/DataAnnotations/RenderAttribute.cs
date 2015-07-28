using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class RenderAttribute : Attribute, IMetadataAware
    {
        private readonly Type _renderType;

        internal const string MetadataAdditionalKey = "ecard_render";
        public RenderAttribute(Type renderType)
        {
            _renderType = renderType;
        }

        public object Parameter { get; set; }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = this;
        }

        public bool IsRender(ViewContext vc)
        {
            IRenderJudge renderJudge = (IRenderJudge)Activator.CreateInstance(_renderType);
            return renderJudge.IsRender(vc, Parameter);
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class HiddenLabelAttribute : Attribute, IMetadataAware
    {

        internal const string MetadataAdditionalKey = "ecard_HiddenLabel";
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = true;
        }
    }
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