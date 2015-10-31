using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Linq.Expressions;

namespace Moonlit.TestFixtures
{
    [TestClass()]
    public class DynamicDictionaryMapperTests
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
        ///A test for BuildToDictionary
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Moonlit.dll")]
        public void BuildToDictionaryTest()
        {
            DynamicDictionaryMapper mapper = new DynamicDictionaryMapper(typeof(A));
            var dict = mapper.ToDictionary(new A { Name = "tom", Age = 33 });
            Assert.AreEqual(dict["NAME"], "tom");
            Assert.AreEqual(dict["age"], 33);
        }
        #region Nested type: A

        public class A
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public string this[string index]
            {
                get { return index; }
                set
                {
                    /* set the specified index to value here */
                }
            }
        }

        #endregion
    }
}
