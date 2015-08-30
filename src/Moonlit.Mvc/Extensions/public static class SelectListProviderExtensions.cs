using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public static class SelectListProviderExtensions
    {
        public static List<SelectListItem> GetSelectList<T>(this ISelectListProvider provider, T model, string propertyName)
        {
            var property = ModelMetadataProviders.Current.GetMetadataForProperty(() => model, typeof(T), propertyName);
            return provider.GetSelectList(property, model);
        }
    }
}
