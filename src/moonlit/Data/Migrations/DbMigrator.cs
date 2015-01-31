using System;
using System.Data.Common;
using System.Linq;
using System.Text;
using Moonlit.Data.Migrations.Migrators;

namespace Moonlit.Data.Migrations
{
    internal abstract class DbMigrator
    {
        internal MigrationStatement CreateStatement()
        {
            return DoCore();
        }

        protected abstract MigrationStatement DoCore();

        protected static object Quete(string table)
        {
            var arr = table.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 1)
                return "[" + arr[0].Trim() + "]";
            return "[" + string.Join("].[", arr.Select(x => x.Trim())) + "]";
        }

        protected object Name(params string[] table)
        {
            return string.Join("_", table.Select(x => x.Replace(".", "_").ToUpper()));
        }

        internal string ToString(ColumnModel x)
        {
            StringBuilder builder = new StringBuilder();
            switch (x.StoreType)
            {
                case PrimitiveTypeName.TypeInt:
                    builder.Append("int");
                    break;
                case PrimitiveTypeName.TypeBoolean:
                    builder.Append("bit");
                    break;
                case PrimitiveTypeName.TypeDateTime:
                    builder.Append("datetime");
                    break;
                case PrimitiveTypeName.TypeDecimal:
                    builder.Append("decimal");
                    break;
            }

            if (x.FixedLength.HasValue)
            {
                builder.Append("nchar(" + x.FixedLength + ")");
            }

            if (x.IsMaxLength == true)
            {
                builder.Append("nvarchar(max)");
            }
            else if (x.MaxLength.HasValue)
            {
                builder.Append("nvarchar(" + x.MaxLength + ")");
            }

            if (x.Precision.HasValue)
            {
                builder.Append("(18, " + x.Precision + ")");
            }
            if (x.Identity)
            {
                builder.Append(" Identity(1, 1)");
            }
            if (x.Nullable == true)
                builder.Append(" null");
            else
                builder.Append(" not null");
            return builder.ToString();
        }
    }
}