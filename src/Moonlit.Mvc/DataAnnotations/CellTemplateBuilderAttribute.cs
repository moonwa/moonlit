using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class CellTemplateBuilderAttribute : Attribute, IMetadataAware, ICellTemplateBuilder
    {
        internal const string MetadataAdditionalKey = "moonlit_celltemplatebuilder";
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = this;
        }

        public abstract Func<RowBoundItem, Control> CreateCellTemplate(ModelMetadata propertyMetadata, ControllerContext controllerContext);
    }
}