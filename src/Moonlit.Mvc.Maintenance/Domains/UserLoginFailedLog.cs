using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class UserLoginFailedLog
    {
        public long UserLoginFailedLogId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [StringLength(32)]
        public string IpAddress { get; set; }
        public DateTime CreationTime { get; set; }
    }
}