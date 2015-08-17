using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class ControlAttribute : Attribute, IMetadataAware, IControllBuilder
    {
        internal const string MetadataAdditionalKey = "moonlit_controllerbuilder";
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = this;
        }

        public abstract Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext);
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MappingAttribute : Attribute, IMetadataAware
    {
        public MappingAttribute()
        {

        }

        public string To { get; set; }
        internal const string MetadataAdditionalKey = "moonlit_Mapping";
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (string.IsNullOrEmpty(To))
            {
                To = metadata.PropertyName;
            }
            metadata.AdditionalValues[MetadataAdditionalKey] = this;
        }
    }
}