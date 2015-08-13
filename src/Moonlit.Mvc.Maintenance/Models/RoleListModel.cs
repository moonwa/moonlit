using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class RoleListModel : IPagedRequest
    {
        public RoleListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "UserName";
        }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "Keyword")]
        [Field(FieldWidth.W6)]
        [TextBox]
        public string Keyword { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RoleIsEnabled")]
        [Field(FieldWidth.W6)]
        [CheckBox]
        public bool? IsEnabled { get; set; } 
    }
}