using System.ComponentModel.DataAnnotations;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public enum SystemJobStatus
    {
        [Display(Name = "SystemJobStatusInit", ResourceType = typeof(MaintCultureTextResources))] 

        Init = 1,
        [Display(Name = "SystemJobStatusSuccess", ResourceType = typeof(MaintCultureTextResources))]
        Success = 2,
        [Display(Name = "SystemJobStatusError", ResourceType = typeof(MaintCultureTextResources))]
        Error = 3,
        [Display(Name = "SystemJobStatusAbort", ResourceType = typeof(MaintCultureTextResources))]
        Abort = 4,
    }
}