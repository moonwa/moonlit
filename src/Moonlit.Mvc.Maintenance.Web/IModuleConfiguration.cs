using Autofac;

namespace Moonlit.Mvc.Maintenance
{
    public interface IModuleConfiguration
    {
        void Configure(IContainer container);
    }
}