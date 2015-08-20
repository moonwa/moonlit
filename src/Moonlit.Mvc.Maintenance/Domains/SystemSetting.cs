using System.ComponentModel.DataAnnotations;

namespace Moonlit.Mvc.Maintenance.Domains
{
    /// <summary>
    /// This object represents the properties and methods of a Site.
    /// </summary>
    public class SystemSetting
    {
        public SystemSetting()
        { 
        }
         
        public int SystemSettingId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Category { get; set; }
        [StringLength(4000)]
        public string Value { get; set; }
    }
}