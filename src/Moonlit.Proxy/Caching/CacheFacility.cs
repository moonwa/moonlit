using Castle.MicroKernel.Registration;

namespace Moonlit.Proxy.Caching
{
    public class CacheFacility : Castle.MicroKernel.Facilities.AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(Component.For<CacheInterceptor>());
            Kernel.ComponentModelBuilder.AddContributor(new CacheComponentInspector());
        }
    }
}
