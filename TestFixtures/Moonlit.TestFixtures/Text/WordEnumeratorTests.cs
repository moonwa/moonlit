using System.Linq;
using Moonlit.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Text
{ 
    /// <summary>
    ///This is a test class for WordEnumeratorTest and is intended
    ///to contain all WordEnumeratorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WordEnumeratorTests
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
        ///A test for GetEnumerator
        ///</summary>
        [TestMethod()]
        public void GetEnumeratorTest()
        {
            var items = new WordEnumerator("hello world, it's a good idea!", new[] { ' ', ',', '!' }).ToList();
            CollectionAssert.AreEqual(new[] { "hello", "world", "it's", "a", "good", "idea" }, items);
            items = new WordEnumerator("<hello world, it's a good idea>", new[] { ' ', ',', '<', '>' }).ToList();
            CollectionAssert.AreEqual(new[] { "hello", "world", "it's", "a", "good", "idea" }, items);
            items = new WordEnumerator("<hello world, it's a good idea", new[] { ' ', ',', '<', '>' }).ToList();
            CollectionAssert.AreEqual(new[] { "hello", "world", "it's", "a", "good", "idea" }, items);

            items = new WordEnumerator("<hello world, it's a good idea", new []{' ', ','}, new[] { '<', '>' }).ToList();
            CollectionAssert.AreEqual(new[] { "<", "hello", "world", "it's", "a", "good", "idea" }, items);
        }
    }
}
