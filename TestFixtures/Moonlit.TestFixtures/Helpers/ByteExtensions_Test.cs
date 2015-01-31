using Microsoft.VisualStudio.TestTools.UnitTesting;

    #region TestFrameworkUsing
#if !NUNIT

#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestContext = Moonlit.Diagnostics.TestContextHelper;
#endif
#endregion
namespace Moonlit.TestFixtures.Helpers
{

    [TestClass()]
    public class StringExtensions_Test
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
        [TestMethod()]
        public void FromCame_Test()
        {
            Assert.AreEqual("ACD Ticket", StringHelper.FromCamel("ACDTicket"));
            Assert.AreEqual("Ticket", StringHelper.FromCamel("Ticket"));
            Assert.AreEqual("NBA Current APC Ticket CCTV", StringHelper.FromCamel("NBACurrentAPCTicketCCTV"));
            Assert.AreEqual("ETicket", StringHelper.FromCamel("ETicket", 3));
            Assert.AreEqual("ETTicket", StringHelper.FromCamel("ETTicket", 3));
            Assert.AreEqual("ETC Ticket", StringHelper.FromCamel("ETCTicket", 3));
            Assert.AreEqual("ABCDEF Ticket", StringHelper.FromCamel("ABCDEFTicket", 3));
            Assert.AreEqual("ABC DEF Ticket", StringHelper.FromCamel("ABCDEFTicket", 3, "ABC"));
        }
    }

    [TestClass()]
    public class ByteExtensions_Test
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


        [TestMethod()]
        public void ToBcd_Test()
        {
            Assert.AreEqual(0x12, ((byte)12).ToBcd());
            Assert.AreEqual(0x2, ((byte)2).ToBcd());
            Assert.AreEqual(0x10, ((byte)10).ToBcd());
        }
        /// <summary>
        ///GetPath 的测试
        ///</summary>
        [TestMethod()]
        public void FromBcd_Test()
        {
            Assert.AreEqual(12, ((byte)0x12).FromBcd());
            Assert.AreEqual(2, ((byte)2).FromBcd());
            Assert.AreEqual(10, ((byte)10).FromBcd());
        }
    }
}
