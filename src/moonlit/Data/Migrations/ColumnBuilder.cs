using System;
using Moonlit.Data.Migrations.Migrators;

namespace Moonlit.Data.Migrations
{
    public class ColumnBuilder
    {
        public ColumnModel Boolean(bool? nullable = null, string name = null)
        {
            return new ColumnModel
                {
                    Nullable = nullable,
                    Name = name,
                    StoreType = PrimitiveTypeName.TypeBoolean,
                };
        }
        public ColumnModel DateTime(bool? nullable = null, string name = null)
        {
            return new ColumnModel
                {
                    Nullable = nullable,
                    Name = name,
                    StoreType = PrimitiveTypeName.TypeDateTime,
                };
        }
        public ColumnModel Decimal(bool? nullable = null, byte? precision = null, string name = null, bool identity = false)
        {
            return new ColumnModel
                {
                    Nullable = nullable,
                    Name = name,
                    Precision = precision,
                    Identity = identity,
                    StoreType = PrimitiveTypeName.TypeDecimal,
                };
        }
        public ColumnModel Int(bool? nullable = null, bool identity = false, string name = null)
        {
            return new ColumnModel
                {
                    Nullable = nullable,
                    Name = name,
                    Identity = identity,
                    StoreType = PrimitiveTypeName.TypeInt,
                };
        }
        public ColumnModel String(bool? nullable = null, int? maxLength = null, bool? fixedLength = null, bool? isMaxLength = null, string name = null)
        {
            return new ColumnModel
                {
                    Nullable = nullable,
                    Name = name,
                    MaxLength = maxLength ?? 50,
                    FixedLength = fixedLength,
                    IsMaxLength = isMaxLength,
                    StoreType = PrimitiveTypeName.TypeString,
                };
        }

    }
}