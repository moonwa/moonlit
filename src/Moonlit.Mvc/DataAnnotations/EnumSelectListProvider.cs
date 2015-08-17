using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class EnumSelectListProvider : ISelectListProvider
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attr = fi.GetCustomAttribute<DisplayAttribute>(false);

            if (attr != null)
                return attr.GetName();
            else
                return value.ToString();
        }
        public List<SelectListItem> GetSelectList(ModelMetadata modelMetadata, object model)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var value in Enum.GetValues(modelMetadata.ModelType.ToWithoutNullableType()))
            {
                var descAttr = GetEnumDescription((Enum)value);
                items.Add(new SelectListItem()
                {
                    Text = descAttr,
                    Value = ((int)value).ToString()
                });
            }
            return items;
        }
    }
}