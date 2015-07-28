using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public abstract class ControlAttribute : Attribute, IMetadataAware, IControllBuilder
    {
        internal const string MetadataAdditionalKey = "moonlit_controllerbuilder";
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = this;
        }

        public abstract Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext);
    }
}