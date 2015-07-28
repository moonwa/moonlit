using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute, IMetadataAware 
    {
        public FieldAttribute(FieldWidth width)
        {
            Width = width;
        }

        internal const string MetadataAdditionalKey = "moonlit_field";
        public FieldWidth Width { get; set; }
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = new Field()
            {
                FieldName = metadata.PropertyName,
                Label = metadata.DisplayName ?? metadata.PropertyName,
                Width = (int)Width
            };
        }
    }
}