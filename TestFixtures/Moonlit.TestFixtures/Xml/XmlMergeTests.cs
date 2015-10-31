using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Xml;

namespace Moonlit.TestFixtures.Xml
{


    /// <summary>
    ///This is a test class for WordEnumeratorTest and is intended
    ///to contain all WordEnumeratorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XmlMergeTests
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
        public void MergeTest()
        {
            var xml1 = ReadXmlFromStream("Moonlit.TestFixtures.Xml.Xmls.merge1.xml");
            var xml2 = ReadXmlFromStream("Moonlit.TestFixtures.Xml.Xmls.merge2.xml");

            var ele = XmlMerge.Merge(XElementFinder, xml1, xml2);

            var book = ele.Elements().FirstOrDefault(x => (string)x.Attribute("name") == "c#");
            AssertAttributes(book, true, "1990");
            AssertChild(book, "name", "c#");
            AssertChild(book, "publish", "1984");

            book = ele.Elements().FirstOrDefault(x => (string)x.Attribute("name") == "c++");
            AssertAttributes(book, true, "2009");
            AssertChild(book, "name", "C++");
            AssertChild(book, "publish", "1990");

            book = ele.Elements().FirstOrDefault(x => (string)x.Attribute("name") == "c");
            AssertAttributes(book, false, "2000");
            AssertChild(book, "name", "C Language");
            AssertChild(book, "publish", "1980");
        }

        private void AssertChild(XElement book, string name, string value)
        {
            var e = book.Elements("bookProperty").FirstOrDefault(x => (string)x.Attribute("property") == name);
            Assert.AreEqual(value, (string)e.Attribute("value"));
        }

        private void AssertAttributes(XElement book, bool isActivied, string expiredDate)
        {
            Assert.AreEqual(isActivied, (bool)book.Attribute("isActived"));
            Assert.AreEqual(expiredDate, (string)book.Attribute("expiredDate"));
        }


        //       <book name="c#" isActivied="true">
        //  <book property="name" value="c#" />
        //  <book property="publish" value="1985" />
        //</book>
        class Book
        {
            public bool? IsActivied { get; set; }
            public bool? Name { get; set; }
        }
        XElement XElementFinder(XElement parent, XElement ele)
        {
            switch (ele.Name.LocalName)
            {
                case "book":
                    return
                        parent.Elements().FirstOrDefault(
                            x => (string)x.Attribute("name") == (string)ele.Attribute("name"));
                case "bookProperty":
                    return
                        parent.Elements().FirstOrDefault(
                            x => (string)x.Attribute("property") == (string)ele.Attribute("property"));
                default:
                    return null;
            }
        }
        XElement ReadXmlFromStream(string name)
        {
            using (var stream = typeof(XmlMergeTests).Assembly.GetManifestResourceStream(name))
            {
                return XElement.Load(stream);
            }
        }
    }
}
