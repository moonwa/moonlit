using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Services.Spelling
{


    /// <summary>
    ///This is a test class for SpellingExtensionsTest and is intended
    ///to contain all SpellingExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpellingExtensionsTests
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
        ///A test for ToTypeNamePlural
        ///</summary>
        [TestMethod()]
        public void ToTypeNamePluralTest()
        {
            ISpelling spelling = new MySpelling();
            Assert.AreEqual("MySpellings", spelling.ToPlural(typeof(MySpelling)));
            Assert.AreEqual("MySpellings", spelling.ToPlural(typeof(IEnumerable<MySpelling>)));
            Assert.AreEqual("MySpellings", spelling.ToPlural(typeof(IQueryable<MySpelling>)));
            Assert.AreEqual("MySpellings", spelling.ToPlural(typeof(IList<MySpelling>)));
            Assert.AreEqual("MySpellings", spelling.ToPlural(typeof(List<MySpelling>)));
        }

        private class MySpelling : ISpelling
        {
            public string ToPlural(string word)
            {
                return word + "s";
            }
        }
    }
}
