namespace Moonlit.Mvc
{
    public interface IUserLoader
    {
        IUserPrincipal GetUserPrincipal(string name);
    }
}