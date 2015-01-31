using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Reflection;
 
namespace Moonlit.TestFixtures
{


    /// <summary>
    ///这是 AssemblyDescriptor_Test 的测试类，旨在
    ///包含所有 AssemblyDescriptor_Test 单元测试
    ///</summary>
    [TestClass()]
    public class AssemblyDescriptor_Test
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
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


        /// <summary>
        ///GetPath 的测试
        ///</summary>
        [TestMethod()]
        public void GetPath_Test()
        {
            Assembly assembly = typeof(AssemblyDescriptor_Test).Assembly;
            string assemblyPath = assembly.Location;

            Assert.IsTrue(File.Exists(assemblyPath));

            var assembly1 = AssemblyExtension.SafeLoadFile(assemblyPath);

            Assert.AreNotSame(assembly, assembly1);
            var type = assembly1.GetType("Moonlit.TestFixtures.AssemblyDescriptor_Test");
            Assert.IsNotNull(type);
        }
    }
}
