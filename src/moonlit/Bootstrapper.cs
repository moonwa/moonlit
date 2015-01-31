using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moonlit.Modularity;

namespace Moonlit
{
    public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;

    public abstract class Bootstrapper
    {
        private readonly List<IModule> _modules = new List<IModule>();
        protected abstract IDependencyResolver Container { get; }

        public void Run()
        {
        }

        public TModule Install<TModule>() where TModule : IModule
        {
            return (TModule)Install(typeof(TModule));
        }

        private IModule Install(Type moduleType)
        {
            if (!typeof(IModule).IsAssignableFrom(moduleType))
            {
                throw new Exception("moduleType " + moduleType.FullName + " have to implement the " +
                                    typeof(IModule).FullName);
            }

            var module = GetModule(moduleType);
            if (module == null)
            {
                module = (IModule)Container.Resolve(moduleType);
                if (module == null)
                {
                    throw new Exception("fail to resolve module type " + moduleType.FullName);
                }
                return InstallModule(module);
            }
            return module;
        }

        private IModule InstallModule(IModule module)
        {
            if (module == null) throw new ArgumentNullException("module");
            if (_modules.Any(x => x == module))
            {
                return module;
            }
            foreach (IModule dependencyModule in module.Dependencies)
            {
                InstallModule(dependencyModule);
            }
            module.Bootstrapper = this;
            module.Init();
            _modules.Add(module);
            return module;
        }

        public TModule GetModule<TModule>() where TModule : IModule
        {
            return (TModule)GetModule(typeof(TModule));
        }
        public IModule GetModule(Type moduleType)
        {
            return _modules.FirstOrDefault(x => x.GetType() == moduleType);
        }

        public IEnumerable<IModule> GetModules()
        {
            return _modules.AsReadOnly();
        }
    }
}