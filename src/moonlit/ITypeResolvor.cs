using System;
using System.Collections.Generic;
using Moonlit.Reflection;

namespace Moonlit
{
    public interface ITypeResolvor
    {
        Type ResolveType(string typeName);
        IEnumerable<Type> ResolveTypes(System.Predicate<Type> predicate);
        void AddTypeAlias(string name, Type type);
    }
}