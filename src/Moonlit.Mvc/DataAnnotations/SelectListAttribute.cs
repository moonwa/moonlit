using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public bool IncludeAll { get; set; }


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
            var selectListItemsProvider = (DependencyResolver.Current.GetService(this._providerType) ?? Activator.CreateInstance(_providerType)) as ISelectListItemsProvider;
            var selectListItems = selectListItemsProvider.GetSelectList(metadata, controllerContext.Controller.ViewData.Model);
            if (IncludeAll)
            {
                selectListItems.Insert(0, new SelectListItem() { Text = "", Value = "" });
            }
            var selectedValue = ObjectToString(model);
            return new Moonlit.Mvc.Controls.SelectList(selectListItems, selectedValue);
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class MultiSelectListAttribute : ControlAttribute
    {
        private readonly Type _providerType;

        public MultiSelectListAttribute(Type providerType)
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
            var selectListItemsProvider = (DependencyResolver.Current.GetService(this._providerType) ?? Activator.CreateInstance(_providerType)) as ISelectListItemsProvider;
            var selectListItems = selectListItemsProvider.GetSelectList(metadata, controllerContext.Controller.ViewData.Model);

            var selectedValue = ((IEnumerable)model ?? new string[0]).Cast<object>().Select(x => ObjectToString(x)).ToArray();
            return new Controls.MultiSelectList(selectListItems, selectedValue);
        }
    }

}