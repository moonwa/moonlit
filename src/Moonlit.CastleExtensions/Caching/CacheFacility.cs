using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using System.Diagnostics;
using Castle.MicroKernel;
using Castle.DynamicProxy;
using Castle.MicroKernel.Proxy;
using System.Collections;
using Castle.MicroKernel.Registration;
using Moonlit.CastleExtensions.Caching;

namespace Moonlit.CastleExtensions.Caching
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
