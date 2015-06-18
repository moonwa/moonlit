using System;

namespace Moonlit.Mvc.Maintenance
{
    public class DbContextExportAttribute : Attribute
    {
        public bool Ignore { get; set; }
    }
}
