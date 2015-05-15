using System.ComponentModel.DataAnnotations;

namespace Moonlit.Mvc.Sample.Models
{
    public class SignInModel  
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}