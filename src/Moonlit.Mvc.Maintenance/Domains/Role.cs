using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Moonlit.Collections;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class Role 
    {
        public int RoleId { get; set; }
        [StringLength(32)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RoleName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Name { get; set; }

        [StringLength(8000)]
        public string Privileges { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RolePrivileges")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        [NotMapped]
        public string[] PrivilegeArray
        {
            get
            {
                if (string.IsNullOrEmpty(Privileges))
                {
                    return new string[0];
                }
                return Privileges.ToLowerInvariant().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            set
            {
                if (EnumerableHelper.IsNullOrEmpty(value))
                {
                    Privileges = "";
                }
                else
                {
                    Privileges = "," + string.Join(",", value) + ",";
                }
            }
        }


        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RoleIsEnabled")]
        public bool IsEnabled { get; set; }
        
        public bool IsBuildIn { get; set; }
      
    }

}