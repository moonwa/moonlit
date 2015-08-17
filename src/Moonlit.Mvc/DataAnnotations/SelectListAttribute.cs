using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectListAttribute : ControlAttribute
    {
        private readonly Type _providerType;

        public SelectListAttribute(Type providerType)
        {
            _providerType = providerType;
        }

        public static string ObjectToString(object selectedValue)
        {
            if (selectedValue == null)
            {
                return "";
            }
            if (selectedValue.GetType().IsEnum)
            {
                return ((int)selectedValue).ToString();
            }
            return selectedValue.ToString();
        }

        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            var selectListItemsProvider = (DependencyResolver.Current.GetService(this._providerType) ?? Activator.CreateInstance(_providerType)) as ISelectListProvider;
            var selectListItems = selectListItemsProvider.GetSelectList(metadata, controllerContext.Controller.ViewData.Model);
            selectListItems.Insert(0, new SelectListItem() { Text = "", Value = "" });
            var selectedValue = ObjectToString(model);
            return new Moonlit.Mvc.Controls.SelectList(selectListItems, selectedValue);
        }
    }
}