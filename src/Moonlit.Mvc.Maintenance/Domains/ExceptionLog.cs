using System;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class ExceptionLog
    {
        public int ExceptionLogId { get; set; }
        public string Exception { get; set; }
        public string RouteData { get; set; }
        public DateTime CreationTime { get; set; }
    }
}