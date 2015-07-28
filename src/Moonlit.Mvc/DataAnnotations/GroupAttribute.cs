using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class GroupAttribute : Attribute, IMetadataAware
    {
        private readonly string _name;
        internal const string MetadataAdditionalKey = "ecard_group";
        public GroupAttribute(string name)
        {
            _name = name;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = this._name;
        }
    }
}