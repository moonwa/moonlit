using System.ComponentModel.DataAnnotations;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class SignInModel
    {
        [Display(Prompt = "login name")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Prompt = "password")]
        public string Password { get; set; }

        [Display(Name = "remember me")]
        public bool IsRememberMe { get; set; }

        public Template CreateTemplate()
        {
            return new SimpleBoxTemplate
            {
                Title = CultureTextResources.SignInTitle,
                Description = CultureTextResources.SignInDescription,
                Fields = new[]
                {
                    new Field
                    {
                        FieldName = "UserName",
                        Control = new TextBox()
                        {
                            Value = UserName,
                            PlaceHolder = CultureTextResources.SignInUserName,
                            Icon = "user"
                        }
                    },
                    new Field()
                    {
                        FieldName = "Password",
                        Control = new PasswordBox()
                        {
                            Icon = "lock",
                            PlaceHolder = CultureTextResources.SignInPassword
                        }
                    }
                },
                Buttons = new IClickable[]
                {
                    new Button
                    {
                        ActionName = "",
                        Text = CultureTextResources.SignIn 
                    }
                },
                Additional = new List
                {
                    Style = ListStyle.Unstyled,
                }
            };
        }
    }
}