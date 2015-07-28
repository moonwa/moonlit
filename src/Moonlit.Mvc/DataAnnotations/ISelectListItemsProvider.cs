using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public interface ISelectListItemsProvider
    {
        List<SelectListItem> GetSelectList(ModelMetadata modelMetadata, object model);
    }
}