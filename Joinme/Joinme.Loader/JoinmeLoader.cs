using Autofac;
using Autofac.Builder;
using Joinme.Loader.Databases;
using Joinme.Models;

namespace Joinme.Loader
{
    internal static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> OnStrategy<TLimit, TActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration, JoinmeLoader.IJoinmeLoaderStrategy strategy,
            params object[] lifetimeScopeTags)
        {
            return strategy.Register(registration, lifetimeScopeTags);
        }
    }
    public class JoinmeLoader
    {
        public IContainer Container { get; private set; } 
        public ContainerBuilder ContainerBuilder { get; } = new ContainerBuilder();

        public static IJoinmeLoaderStrategy Web { get; } = new WebLoaderStrategy();

        public void Load(IJoinmeLoaderStrategy strategy)
        {
            RegisterRepository(ContainerBuilder, strategy);
            FinishBuild(ContainerBuilder);
        }

        private void FinishBuild(ContainerBuilder containerBuilder)
        {
            Container = containerBuilder.Build();
        }

        public void RegisterRepository(ContainerBuilder containerBuilder, IJoinmeLoaderStrategy strategy)
        {
            containerBuilder.RegisterType<EFRepository<Tenant, CoreDatabase>>()
                .As<IRepository<Tenant>>().OnStrategy(strategy);
        }

        public interface IJoinmeLoaderStrategy
        {
            IRegistrationBuilder<TLimit, TActivatorData, TStyle> Register<TLimit, TActivatorData, TStyle>(
                IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration, params object[] lifetimeScopeTags);
        }

        private class WebLoaderStrategy : IJoinmeLoaderStrategy
        {
            public IRegistrationBuilder<TLimit, TActivatorData, TStyle> Register<TLimit, TActivatorData, TStyle>(
                IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration,
                params object[] lifetimeScopeTags)
            {
                return registration.InstancePerRequest();
            }
        }
    }

}