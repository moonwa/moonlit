using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Domains
{
    [DebuggerDisplay("#{CultureTextId} {Name}:{Text}")]
    public class CultureText
    {
        public int CultureTextId { get; set; }
        [StringLength(128)]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(MaintCultureTextResources), ErrorMessageResourceName = "ValidationRequired")]
        public int CultureId { get; set; }
        [StringLength(4000)]
        public string Text { get; set; }
        public bool IsEdited { get; set; }
        public virtual Culture Culture { get; set; }
    }
}