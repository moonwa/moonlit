using System.Collections.Generic;

namespace Moonlit.Mvc
{
    public interface IPrivilegeLoader
    {
        Privileges Load();
    }
}