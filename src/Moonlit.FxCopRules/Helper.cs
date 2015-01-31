using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.FxCop.Sdk;

namespace Moonlit.FxCopRules
{
    public class Helper
    {
        public static bool IsDataContractNesscery(TypeNode typeNode)
        {
            return !Helper.IsPrimitive(typeNode)
                    && typeNode.FullName != typeof(void).FullName
                    && !Helper.IsNullablePrimitive(typeNode)
                   && !(typeNode.Template != null && typeNode.Template.FullName == typeof(List<>).FullName)
                   ;
        }

        public static bool IsNullable(TypeNode typeNode)
        {
            return typeNode.Template != null && typeNode.Template.FullName == typeof(Nullable<>).FullName;
        }
        public static bool IsNullablePrimitive(TypeNode typeNode)
        {
            return IsNullable(typeNode) && IsPrimitive(typeNode.TemplateArguments[0]);
        }
        public static bool IsPrimitive(TypeNode typeNode)
        {
            return typeNode.IsPrimitive || typeNode.FullName == typeof(decimal).FullName ||
                   typeNode.FullName == typeof(void).FullName;
        }
    }
}