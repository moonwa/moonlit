using System;
using Moonlit.Reflection;

namespace Moonlit
{
    public interface ITypeResolvor
    {
        Type ResolveType(string typeName, bool ignoreCase);
        void AddTypeAlias(string name, Type type);
    }
}