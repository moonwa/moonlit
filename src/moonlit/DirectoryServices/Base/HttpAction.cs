using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.DirectoryServices
{
    [System.Flags]
    public enum HttpAction
    {
        Get     = 0x00000001,
        Post    = 0x00000002, 
        Head    = 0x00000004,
        Trace   = 0x00000008,
        Debug   = 0x00000010
    }
}
