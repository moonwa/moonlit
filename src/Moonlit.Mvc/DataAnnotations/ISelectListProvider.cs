using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public interface ISelectListProvider
    {
        List<SelectListItem> GetSelectList(ModelMetadata modelMetadata, object model);
    }
}