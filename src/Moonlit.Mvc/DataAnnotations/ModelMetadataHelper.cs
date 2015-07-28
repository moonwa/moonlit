using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using MultiSelectList = System.Web.Mvc.MultiSelectList;

namespace Moonlit.Mvc
{
    public static class ModelMetadataHelper
    {
        public static string GetSort(this ModelMetadata metadata)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(SortAttribute.MetadataAdditionalKey, out obj))
            {
                return obj.ToString();
            }
            return null;
        }
        public static bool IsRenderEnabled(this ModelMetadata metadata, ViewContext vc)
        {
            object obj;
            if (metadata.AdditionalValues.TryGetValue(RenderAttribute.MetadataAdditionalKey, out obj))
            {
                RenderAttribute norender = (RenderAttribute)obj;

                return norender.IsRender(vc);
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
        //        public static int GetMaxStringLength(this ModelMetadata metadata)
        //        {
        //            object obj;
        //            if (metadata.AdditionalValues.TryGetValue(FixStringLengthAttribute.MetadataAdditionalKey, out obj))
        //            {
        //                return Convert.ToInt32(obj);
        //            }
        //            if (metadata.AdditionalValues.TryGetValue(EcardStringLengthAttribute.MetadataAdditionalKey, out obj))
        //            {
        //                return Convert.ToInt32(obj);
        //            }
        //            return 50;
        //        }
   
        

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
            if (metadata.AdditionalValues.TryGetValue(TextBoxAttribute.MetadataAdditionalKey, out obj))
            {
                return (IControllBuilder)obj;
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