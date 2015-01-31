using System;
using System.Collections;
using System.Reflection;

namespace Moonlit
{
    public class AppDomainResolvor : IDisposable
    {
        private readonly AppDomain _appDomain;
        Hashtable hash = new Hashtable();
        public void RegisterAssembly(Assembly assembly)
        {
            hash[assembly.GetName().ToString()] = assembly;
        }
        public AppDomainResolvor()
            : this(AppDomain.CurrentDomain)
        {

        }

        private AppDomainResolvor(AppDomain appDomain)
        {
            _appDomain = appDomain;
            _appDomain.AssemblyResolve += new ResolveEventHandler(_appDomain_AssemblyResolve);
        }

        Assembly _appDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return (Assembly)hash[args.Name];
        }


        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}