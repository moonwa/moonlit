using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.Data.Fixtures
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class XmlDataSourceFixture
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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void XmlDataSourceTest()
        {
            //XmlDatabase database = new XmlDatabase(new ResourceXmlSource(this.GetType().Assembly, "Moonlit.Data.Fixtures.Resources", ".config"));
            //var persons = database.Query<Person>().OrderBy(x => x.Age).ToList();
            //Assert.AreEqual("zz", persons[0].Name);
            //Assert.AreEqual("string name", persons[0].Note.Trim());
            //Assert.IsTrue(string.IsNullOrWhiteSpace(persons[0].Remark));
            //Assert.IsTrue(string.IsNullOrWhiteSpace(persons[0].FullName));
        }
    }


}