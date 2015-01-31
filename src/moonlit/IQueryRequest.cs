using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit
{
    public interface IQueryRequest
    {
        string OrderBy { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
    }
}
