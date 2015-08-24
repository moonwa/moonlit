using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using MultiSelectList = System.Web.Mvc.MultiSelectList;

namespace Moonlit.Mvc
{
    public static class ModelMetadataHelper
    {
        public static MappingAttribute GetMapping(this ModelMetadata metadata)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(MappingAttribute.MetadataAdditionalKey, out obj))
            {
                return ((MappingAttribute)obj);
            }
            return null;
        }
        public static string GetSort(this ModelMetadata metadata)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(SortAttribute.MetadataAdditionalKey, out obj))
            {
                return obj.ToString();
            }
            return null;
        }
        public static string MakeUrl(this ModelMetadata metadata, ControllerContext controllerContext)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(LinkProviderAttribute.MetadataAdditionalKey, out obj))
            {
                return ((LinkProviderAttribute)obj).MakeUrl(metadata, controllerContext);
            }
            return null;
        }
        public static bool IsRenderEnabled(this ModelMetadata metadata, ViewContext vc)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(RenderAttribute.MetadataAdditionalKey, out obj))
            {
                RenderAttribute renderAttr = (RenderAttribute)obj;

                return renderAttr.IsRender(vc);
            }
            return true;
        }
        public static bool IsLabelEnabled(this ModelMetadata metadata)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(HiddenLabelAttribute.MetadataAdditionalKey, out obj))
            {
                return false;
            }
            return true;
        }


        public static string GetGroup(this ModelMetadata metadata)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(GroupAttribute.MetadataAdditionalKey, out obj))
            {
                return obj.ToString();
            }
            return null;
        }
        public static IControllBuilder GetControlBuilder(this ModelMetadata metadata)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(ControlAttribute.MetadataAdditionalKey, out obj))
            {
                return (IControllBuilder)obj;
            }
            return null;
        }
        public static ICellTemplateBuilder GetCellTemplateBuilder(this ModelMetadata metadata)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(CellTemplateBuilderAttribute.MetadataAdditionalKey, out obj))
            {
                return (ICellTemplateBuilder)obj;
            }
            return null;
        }
        public static Field GetField(this ModelMetadata metadata)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(FieldAttribute.MetadataAdditionalKey, out obj))
            {
                return (Field)obj;
            }
            return null;
        }
    }
}