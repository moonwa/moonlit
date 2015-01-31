using System.Collections.Generic;

namespace Moonlit.I18n
{
    public delegate IEnumerable<string> ResourcePathResolver(string name, string cultureName);
}