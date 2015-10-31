#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Configuration.ConsoleParameter;

#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestContext = Moonlit.Diagnostics.TestContextHelper;
#endif
namespace Moonlit.TestFixtures.Configuration
{
    
    
    /// <summary>
    ///This is a test class for ValueArgument_Test and is intended
    ///to contain all ValueArgument_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class ValueArgumentTests
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
        ///A test for ValueArgument Constructor
        ///</summary>
        [TestMethod()]
        public void DefaultValue_Test()
        {
            ValueParameter av = new ValueParameter("av", "tt", true);
            ValueParameter bv = new ValueParameter("bv", "bv", "ttbv");

            Parser arg = new Parser();

            arg.AddArguments(av, bv)
               ;
            arg.Parse(new string[] { 
                "mwwtools" ,"io" ,"synch" ,@"/a", @"tt" 
            });

            Assert.IsTrue(av.Defined);
            Assert.AreEqual("tt", av.Value);
            Assert.IsTrue(bv.Defined);
            Assert.AreEqual("ttbv", bv.Value);
        }
    }
}
