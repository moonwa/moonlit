using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc.Maintenance
{
    public class MyA : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.IsInRole(this.Roles);
        }
    }
    public class EnumAttribute : Attribute, IMetadataAware
    {
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            var modelType = metadata.ModelType;
            bool includeEmpty = false;
            if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                modelType = modelType.GetGenericArguments()[0];
                includeEmpty = true;
            }

            List<SelectListItem> items = new List<SelectListItem>();
            if (includeEmpty)
            {
                items.Add(new SelectListItem() { Text = "", Value = "" });
            }
            var fields = modelType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var selected = metadata.Model == null ? false : ((int)metadata.Model == (int)field.GetValue(null));
                var value = ((int)field.GetValue(null)).ToString();
                var text = field.Name;
                items.Add(new SelectListItem()
                {
                    Text = text,
                    Value = value,
                    Selected = selected
                });
            }
            metadata.AdditionalValues["SelectListItems"] = items;
            metadata.TemplateHint = "SelectList";
        }
    }
}