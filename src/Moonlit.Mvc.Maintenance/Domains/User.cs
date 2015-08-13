using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class User : IIdentity, IUser, IKeyObject
    {
        #region Implementation of IKeyObject

        string IKeyObject.Key
        {
            get { return this.UserId.ToString(); }
        }

        #endregion
        public int UserId { get; set; }

        [StringLength(32)]
        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserUserName")]
        [Field(FieldWidth.W6)]
        [TextBox]
        public string UserName { get; set; }
        [StringLength(32)]
        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserLoginName")]
        [Field(FieldWidth.W6)]
        [TextBox]
        public string LoginName { get; set; }

        [StringLength(128)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserPassword")]
        [Field(FieldWidth.W6)]
        [PasswordBox]
        public string Password { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserGender")]
        [LiteralCell]
        [Field(FieldWidth.W6)]
        [SelectList(typeof(EnumSelectListItemsProvider))]
        public Gender? Gender { get; set; }

        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserDateOfBirth")]
        [Field(FieldWidth.W6)]
        [DatePicker]
        public DateTime? DateOfBirth { get; set; }

        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsEnabled")]
        [Field(FieldWidth.W6)]
        [CheckBox]
        public bool IsEnabled { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserCulture")]
        [Field(FieldWidth.W6)]
        [SelectList(typeof(CultureSelectListItemsProvider))]
        [Required(ErrorMessageResourceType = typeof(MaintCultureTextResources), ErrorMessageResourceName = "ValidationRequired")]
        public int CultureId { get; set; }



        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsSuper")]
        [Field(FieldWidth.W6)]
        [Literal]
        public bool IsSuper { get; set; }

        [DbContextExport(Ignore = true)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserRoles")]
        [MultiSelectList(typeof(RolesSelectListItemsProvider))]
        [Field(FieldWidth.W6)]
        public virtual ICollection<Role> Roles { get; set; }

        [LiteralCell]
        [Literal]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsBuildIn")]
        public bool IsBuildIn { get; set; }


        [StringLength(64)]
        public string Avatar { get; set; }
        string IIdentity.Name
        {
            get { return LoginName; }
        }

        string IIdentity.AuthenticationType
        {
            get { return "Maint"; }
        }

        bool IIdentity.IsAuthenticated
        {
            get { return true; }
        }

        public string HashPassword(string rawString)
        {
            var salted = Encoding.UTF8.GetBytes(string.Concat(rawString, LoginName));

            SHA256 hasher = new SHA256Managed();
            var hashed = hasher.ComputeHash(salted);

            return Convert.ToBase64String(hashed);
        }
    }
}