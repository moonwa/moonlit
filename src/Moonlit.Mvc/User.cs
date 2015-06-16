using System.Security.Principal;

namespace Moonlit.Mvc
{
    public interface IUser : IIdentity
    { 
        string Avatar { get; }
    }
}