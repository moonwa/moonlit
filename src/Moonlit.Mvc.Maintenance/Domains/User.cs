using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class User : IIdentity, IUser 
    {
        public int UserId { get; set; }

        [StringLength(32)]
        
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserUserName")]
        
        
        public string UserName { get; set; }
        [StringLength(32)]
        
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserLoginName")]
        
        
        public string LoginName { get; set; }

        [StringLength(128)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserPassword")]
        
        public string Password { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserGender")]
        public Gender? Gender { get; set; }

        
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserDateOfBirth")]
        
        public DateTime? DateOfBirth { get; set; }

        
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsEnabled")]
        
        public bool IsEnabled { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserCulture")]
        
        [Required(ErrorMessageResourceType = typeof(MaintCultureTextResources), ErrorMessageResourceName = "ValidationRequired")]
        public int CultureId { get; set; }



        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsSuper")]
        
        public bool IsSuper { get; set; }

        [DbContextExport(Ignore = true)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserRoles")]
        
        public virtual ICollection<Role> Roles { get; set; }

        
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