using System;

namespace Moonlit.Data.Migrations
{
    public class ColumnModel
    {
        public bool? Nullable { get; set; }

        public string Name { get; set; }

        public string StoreType { get; set; }

        public bool Identity { get; set; }

        public byte? Precision { get; set; }

        public int? MaxLength { get; set; }

        public bool? FixedLength { get; set; }

        public bool? IsMaxLength { get; set; }
    }
}