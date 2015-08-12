using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class HiddenLabelAttribute : Attribute, IMetadataAware
    {

        internal const string MetadataAdditionalKey = "ecard_HiddenLabel";
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = true;
        }
    }
}