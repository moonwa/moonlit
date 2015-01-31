using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Mef;
using Moonlit.Modularity;

namespace Moonlit.TestFixtures.Modularity
{
    [TestClass]
    public class AssemblyModuleCatalogFixture
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void MyBootstrapper_Test()
        {
            MyBootstrapper bootstrapper = new MyBootstrapper();
            bootstrapper.Install<ModuleA>();
            bootstrapper.Install<ModuleB>();

            var modules = bootstrapper.GetModules().ToList().Select(x => x.GetType()).ToArray();
            CollectionAssert.AreEqual(new[] { typeof(ModuleA), typeof(ModuleB) }, modules);

            bootstrapper = new MyBootstrapper();
            bootstrapper.Install<ModuleB>();
            bootstrapper.Install<ModuleA>();

            modules = bootstrapper.GetModules().ToList().Select(x => x.GetType()).ToArray();
            CollectionAssert.AreEqual(new[] { typeof(ModuleA), typeof(ModuleB) }, modules);
        }
        [TestMethod]
        public void AssemblyModuleCatalog_Test()
        {
            MyBootstrapper bootstrapper = new MyBootstrapper();
            var moduleA = bootstrapper.Install<ModuleA>();
            var moduleB = bootstrapper.Install<ModuleB>();

            var args = new EventArgs<string>();
            moduleA.TriggerClick(args);
            Assert.AreEqual("100", args.Value);
        }

        class MyBootstrapper : Bootstrapper
        { 
            MefDependencyResolver _dependencyResolver;
            public MyBootstrapper()
            {
                var container = new CompositionContainer(new AssemblyCatalog(typeof(MyBootstrapper).Assembly));
                _dependencyResolver = new MefDependencyResolver(container);
            }
            protected override IDependencyResolver Container
            {
                get { return _dependencyResolver; }
            }
        }

        [Export(typeof(ModuleA))]
        [PartCreationPolicy(CreationPolicy.Shared)]
        public class ModuleA : IModule
        {
            public void Init()
            {

            }
            public event AsyncEventHandler<EventArgs<string>> Click = async delegate { };

            public void TriggerClick(EventArgs<string> args)
            {
                Click(this, args).Wait();
            }
            public IEnumerable<IModule> Dependencies { get { return Enumerable.Empty<IModule>(); } }
            public Bootstrapper Bootstrapper { set; private get; }
        }

        [PartCreationPolicy(CreationPolicy.Shared)]
        [Export(typeof(ModuleB))]
        public class ModuleB : IModule
        {

            [Import(typeof(ModuleA))]
            public ModuleA ModuleA { get; set; }

            public IEnumerable<IModule> Dependencies { get { yield return ModuleA; } }
            public Bootstrapper Bootstrapper { set; private get; }

            public void Init()
            {
                ModuleA.Click += this.ModuleA_Click;
            }

            private Task ModuleA_Click(object sender, EventArgs<string> e)
            {
                e.Value = "100";
                return Task.FromResult<object>(null);
            }
        }
    }
}