using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class SortAttribute : Attribute, IMetadataAware
    {
        internal const string MetadataAdditionalKey = "ecard_sort";
        public string Sort { get; private set; }
        public SortAttribute(string sort)
        {
            Sort = sort;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = Sort;
        }
    }
}