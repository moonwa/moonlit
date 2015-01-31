using System;

namespace Moonlit.I18n
{
    public interface IResourceSource
    {
        CulturedResource GetResource(string culture, StringComparer resKeyComparer);
    } 
}