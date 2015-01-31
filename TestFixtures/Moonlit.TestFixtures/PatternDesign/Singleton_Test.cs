using Moonlit.PatternDesign;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.PatternDesign
{


    /// <summary>
    ///This is a test class for Singleton_Test and is intended
    ///to contain all Singleton_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class Singleton_Test
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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


        /// <summary>
        ///A test for Instance
        ///</summary>
        public void Instance_TestHelper<T>()
            where T : new()
        {
        }

        [TestMethod()]
        public void Instance_Test()
        {
            MySingle single1 = Singleton<MySingle>.Instance;
            MySingle single2 = Singleton<MySingle>.Instance;
            Assert.AreEqual(single1, single2);
            Assert.IsTrue(object.ReferenceEquals(single1, single2));
        }
        class MySingle
        {

        }
    }
}
