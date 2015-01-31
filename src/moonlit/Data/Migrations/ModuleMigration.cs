using System;

namespace Moonlit.Data.Migrations
{
    internal class ModuleMigration
    {
        public string Name { get; set; }
        public Version Version { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}