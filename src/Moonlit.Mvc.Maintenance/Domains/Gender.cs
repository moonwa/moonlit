using System.ComponentModel.DataAnnotations;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public enum Gender
    {
        [Display(Name = "GenderMale", ResourceType = typeof(MaintCultureTextResources))]
        Male = 1,
        [Display(Name = "GenderFemale", ResourceType = typeof(MaintCultureTextResources))]
        Female = 2
    }
}