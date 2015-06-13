using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Moonlit.Mvc.Sample.Controllers;

namespace Moonlit.Mvc.Sample.Models
{
    public class User : IUser
    {
        [Required]
        public string UserName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        string IIdentity.Name
        {
            get { return UserName; }
        }

        string IIdentity.AuthenticationType
        {
            get { return "custom"; }
        }

        bool IIdentity.IsAuthenticated
        {
            get { return true; }
        }

        string IUser.Avatar
        {
            get { return "#"; }
        }
    }
}