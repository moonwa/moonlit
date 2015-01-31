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

        #region ���Ӳ�������
        // 
        //��д����ʱ������ʹ����������:
        //
        //ʹ�� ClassInitialize ���������еĵ�һ������ǰ�����д���
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ʹ�� ClassCleanup �����������е����в��Ժ������д���
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //ʹ�� TestInitialize ������ÿ������ǰ�����д���
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //ʹ�� TestCleanup ��������ÿ�����Ժ����д���
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void ResolveType_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver();
            Assert.IsNull(resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures, version=1.0.0.0, culture=neutral", false));
            Assert.IsNull(resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures, version=1.0.0.0", false));
            Assert.IsNull(resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures", false));

            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures, version=1.0.0.0, culture=neutral", true));
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures, version=1.0.0.0", true));
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("moonlit.testfixtures.democlass, moonlit.testfixtures", true));
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("Moonlit.TestFixtures.DemoClass, Moonlit.TestFixtures, Version=1.0.0.0, Culture=neutral", false));
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("Moonlit.TestFixtures.DemoClass, Moonlit.TestFixtures, Version=1.0.0.0", false));
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("Moonlit.TestFixtures.DemoClass, Moonlit.TestFixtures", false));


        }
        [TestMethod()]
        public void DumplicateAdd_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver();

            resolver.AddTypeAlias("int", typeof(Int64));
            resolver.AddTypeAlias("int", typeof(Int32));
        }
        [TestMethod()]
        public void AddAssemblyAlias_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver();

            resolver.AddAssemblyAlias("test", "Moonlit.TestFixtures, Version=1.0.0.0, Culture=neutral");
            Assert.AreEqual(typeof(DemoClass), resolver.ResolveType("Moonlit.TestFixtures.DemoClass, test", true));
        }
        [TestMethod()]
        public void AddTypeAlias_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver();

            resolver.AddAssemblyAlias("test", "Moonlit.TestFixtures, Version=1.0.0.0, Culture=neutral");
            resolver.AddTypeAlias("int", typeof(Int32));
            Assert.AreEqual(typeof(Int32), resolver.ResolveType("int", true));
            Assert.AreEqual(typeof(Int32), resolver.ResolveType("Int", true));
            Assert.IsNull(resolver.ResolveType("Int", false));
        }
        [TestMethod()]
        public void DomainTypeResolver_Test()
        {
            DomainTypeResolver resolver = new DomainTypeResolver();
            Assert.AreEqual(typeof(object), resolver.ResolveType("object", true));
            Assert.AreEqual(typeof(Int32), resolver.ResolveType("int", true));
            Assert.AreEqual(typeof(object[]), resolver.ResolveType("object[]", true));
        }

    }
    class DemoClass
    {

    }
}