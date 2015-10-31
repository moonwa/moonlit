using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Mef;

namespace Moonlit.TestFixtures.Mef
{
    [TestClass()]
    public class MefResolvorTests
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        [TestMethod]
        public void Resolve_Test()
        {
            var container = new CompositionContainer(new AssemblyCatalog(typeof(A).Assembly));
            Moonlit.Mef.MefDependencyResolver resolver = new MefDependencyResolver( container);
            var a = resolver.Resolve(typeof(IA));
            Assert.IsNotNull(a);
            Assert.AreEqual(typeof(A), a.GetType());
        }
        [TestMethod]
        public void ResolveWithKey_Test()
        {
            var container = new CompositionContainer(new AssemblyCatalog(typeof(A).Assembly));
            Moonlit.Mef.MefDependencyResolver resolver = new MefDependencyResolver( container);
            var a = resolver.Resolve(typeof(IA), "B");
            Assert.IsNotNull(a);
            Assert.AreEqual(typeof(B), a.GetType());
        }
        [TestMethod]
        public void ResolveAll_Test()
        {
            var container = new CompositionContainer(new AssemblyCatalog(typeof(A).Assembly));
            Moonlit.Mef.MefDependencyResolver resolver = new MefDependencyResolver(container);
            var a = resolver.ResolveAll(typeof(IC));
            Assert.IsNotNull(a);
            Assert.AreEqual(2, a.Count());
        }
        [Export(typeof(IA))]
        public class A : IA
        {

        }
        [Export("B", typeof(IA))]
        public class B : IA
        {

        }
        [Export(typeof(IC))]
        public class C1 : IC
        {

        }
        [Export(typeof(IC))]
        public class C2 : IC
        {

        }

        public interface IC
        {
        }

        public interface IA
        {
        }
    }
}