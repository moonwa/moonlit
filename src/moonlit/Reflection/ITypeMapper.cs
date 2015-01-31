using System;

namespace Moonlit.Reflection
{
    public interface ITypeMapper
    {
        object Map(object value, Type type);
    }
}