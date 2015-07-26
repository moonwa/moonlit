using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class ExceptionLog
    {
        public int ExceptionLogId { get; set; }
        [StringLength(8000)]
        public string Exception { get; set; }
        [StringLength(1280)]
        public string RouteData { get; set; }
        public DateTime CreationTime { get; set; }
    }
}