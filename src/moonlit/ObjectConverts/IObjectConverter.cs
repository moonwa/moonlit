using System;

namespace Moonlit.ObjectConverts
{
    public interface IObjectConverter
    {
        bool TryConvert(ConvertArgs args);
    }
}