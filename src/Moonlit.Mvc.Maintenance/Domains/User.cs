using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class User : IIdentity, IUser
    {
        public int UserId { get; set; }
        [StringLength(32)]
        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserLoginName")]
        public string LoginName { get; set; }

        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsEnabled")]
        public bool IsEnabled { get; set; }
        [StringLength(128)]
        public string Password { get; set; }

        public int CultureId { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserGender")]
        [LiteralCell]
        public Gender? Gender { get; set; }

        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserDateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
        public string HashPassword(string rawString)
        {
            byte[] salted = Encoding.UTF8.GetBytes(string.Concat(rawString, this.LoginName));

            SHA256 hasher = new SHA256Managed();
            byte[] hashed = hasher.ComputeHash(salted);

            return Convert.ToBase64String(hashed);
        }
        [StringLength(32)]
        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserUserName")]
        public string UserName { get; set; }

        public bool IsSuper { get; set; }
        [DbContextExport(Ignore = true)]
        public virtual ICollection<Role> Roles { get; set; }

        string IIdentity.Name
        {
            get { return this.LoginName; }
        }

        string IIdentity.AuthenticationType
        {
            get { return "Maint"; }
        }
        bool IIdentity.IsAuthenticated
        {
            get { return true; }
        }

        [StringLength(64)]
        public string Avatar { get; set; }

        [LiteralCell]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsBuildIn")]
        public bool IsBuildIn { get; set; }
    }


}