using System.Security.Principal;

namespace Moonlit.Mvc
{
    public interface IUserPrincipal : IPrincipal
    {
        string[] Privileges { get; set; }
    }
}