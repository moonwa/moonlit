using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class Role : IKeyObject
    {
        public int RoleId { get; set; }
        [StringLength(32)]
        [Field(FieldWidth.W6)]
        [TextBox]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RoleName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        [LiteralCell]
        public string Name { get; set; }
        [StringLength(8000)]
        public string Privileges { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RolePrivileges")]
        [MultiSelectList(typeof(PrivilegeSelectListItemsProvider))]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        [Field(FieldWidth.W6)]
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
                if (value == null)
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
        [Field(FieldWidth.W6)]
        [CheckBox]
        [LiteralCell]
        public bool IsEnabled { get; set; }
        [LiteralCell]
        public bool IsBuildIn { get; set; }
        #region Implementation of IKeyObject

        string IKeyObject.Key
        {
            get { return this.RoleId.ToString(); }
        }

        #endregion
    }

}