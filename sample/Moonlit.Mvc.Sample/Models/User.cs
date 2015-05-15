using System;
using System.ComponentModel.DataAnnotations;
using Moonlit.Mvc.Sample.Controllers;

namespace Moonlit.Mvc.Sample.Models
{
    public class User
    {
        [Required]
        public string UserName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}