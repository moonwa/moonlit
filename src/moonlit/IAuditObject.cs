using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit
{

    public interface IAuditObject
    {
        int? CreationUserId { get; set; }
        DateTime? CreationTime { get; set; }
        int? UpdateUserId { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
