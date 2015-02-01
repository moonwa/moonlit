using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures
{
    [TestClass()]
    public class TypeResolver_Test
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试属性
        // 
        //编写测试时，还可使用以下属性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void ResolveType_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver(false);
            DomainTypeResolver ignoreCaseResolver = new DomainTypeResolver(true);
            Assert.IsNull(resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures, version=1.0.0.0, culture=neutral"));
            Assert.IsNull(resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures, version=1.0.0.0"));
            Assert.IsNull(resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures"));

            Assert.AreEqual(typeof(DemoClass), ignoreCaseResolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures, version=1.0.0.0, culture=neutral"));
            Assert.AreEqual(typeof(DemoClass), ignoreCaseResolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures, version=1.0.0.0"));
            Assert.AreEqual(typeof(DemoClass), ignoreCaseResolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures"));
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("Moonlit.TestFixtures.DemoClass, Moonlit.TestFixtures, Version=1.0.0.0, Culture=neutral"));
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("Moonlit.TestFixtures.DemoClass, Moonlit.TestFixtures, Version=1.0.0.0"));
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("Moonlit.TestFixtures.DemoClass, Moonlit.TestFixtures"));


        }
        [TestMethod()]
        public void DumplicateAdd_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver(true);

            resolver.AddTypeAlias("int", typeof(Int64));
            resolver.AddTypeAlias("int", typeof(Int32));
        }
        [TestMethod()]
        public void AddAssemblyAlias_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver(true);

            resolver.AddAssemblyAlias("test", "Moonlit.TestFixtures, Version=1.0.0.0, Culture=neutral");
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("Moonlit.TestFixtures.DemoClass, test"));
        }
        [TestMethod()]
        public void AddTypeAlias_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver(false);
            DomainTypeResolver ignoreCaseResolver = new DomainTypeResolver(true);

            resolver.AddAssemblyAlias("test", "Moonlit.TestFixtures, Version=1.0.0.0, Culture=neutral");
            resolver.AddTypeAlias("int", typeof(Int32));
            Assert.AreEqual(typeof(Int32), ignoreCaseResolver.ResolveType("int"));
            Assert.AreEqual(typeof(Int32), ignoreCaseResolver.ResolveType("Int"));
            Assert.IsNull(resolver.ResolveType("Int"));
        }
        [TestMethod()]
        public void DomainTypeResolver_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver(true);
            Assert.AreEqual(typeof(object), resolver.ResolveType("object"));
            Assert.AreEqual(typeof(Int32), resolver.ResolveType("int"));
            Assert.AreEqual(typeof(object[]), resolver.ResolveType("object[]"));
        }

    }
    class DemoClass
    {

    }
}