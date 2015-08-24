using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MappingAttribute : Attribute, IMetadataAware
    {
        public MappingAttribute()
        {
            OnlyNotPostback = true;
        }

        public bool OnlyNotPostback { get; set; }
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