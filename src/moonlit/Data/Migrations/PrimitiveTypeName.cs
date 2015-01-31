using System;

namespace Moonlit.Data.Migrations
{
    internal class PrimitiveTypeName
    {
        public const string TypeString = "string";
        public const string TypeBoolean = "bool";
        public const string TypeDecimal = "decimal";
        public const string TypeDateTime = "datetime";
        public const string TypeInt = "int";
        public const string TypeObject = "object";

        public static string GetName(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = type.GetGenericArguments()[0];
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return TypeBoolean;
                case TypeCode.Char:
                case TypeCode.String:
                    return TypeString;
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return TypeDecimal;
                case TypeCode.DateTime:
                    return TypeDateTime;
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return TypeInt;
                default:
                    return TypeObject;
            }
        }
    }
}