using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class SystemJob : IAuditObject
    {
        public SystemJob()
        {
            StartTime = DateTime.Now;
            Status = SystemJobStatus.Init;
        }


        public void SetHandler(ISystemJobHandler handler)
        {

            HandlerData = JsonConvert.SerializeObject(handler);
            HandlerType = handler.GetType().AssemblyQualifiedName;
        }
        public int SystemJobId { get; set; }
        public DateTime StartTime { get; set; }
        [StringLength(300)]
        public string HandlerType { get; set; }
        [StringLength(4000)]
        public string HandlerData { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Title { get; set; }
        public int Category { get; set; }

        public DateTime? ExecuteTime { get; set; }
        public SystemJobStatus Status { get; set; }
        [StringLength(4000)]
        public string Result { get; set; }
        #region Implementation of IAuditObject

        public int? CreationUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateTime { get; set; }

        #endregion
    }
}
