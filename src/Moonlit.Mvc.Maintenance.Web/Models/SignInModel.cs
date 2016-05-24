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
                Title = MaintCultureTextResources.SignIn,
                Description = MaintCultureTextResources.SignInDescription,
                Fields = new[]
                {
                    new Field
                    {
                        FieldName = "UserName",
                        Label = MaintCultureTextResources.SignInUserName,
                        Control = new TextBox()
                        {
                            Value = UserName,
                            PlaceHolder = MaintCultureTextResources.SignInUserName,
                            Icon = "user"
                        }
                    },
                    new Field()
                    {
                        FieldName = "Password",
                        Label = MaintCultureTextResources.SignInPassword,
                        Control = new PasswordBox()
                        {
                            Icon = "lock",
                            PlaceHolder = MaintCultureTextResources.SignInPassword
                        }
                    }
                },
                Buttons = new IClickable[]
                {
                    new Button
                    {
                        ActionName = "",
                        Text = MaintCultureTextResources.SignIn 
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