using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Xml;

namespace Moonlit.TestFixtures
{


    /// <summary>
    ///This is a test class for XmlComparerTest and is intended
    ///to contain all XmlComparerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XmlComparerTests
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
        ///A test for XmlComparer Constructor
        ///</summary>
        [TestMethod()]
        public void SameXmlTest()
        {
            XmlComparer target = new XmlComparer();
            //target.AddIdentify
            XmlCompareResults result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(result.IsValid);
        }
        /// <summary>
        ///A test for XmlComparer Constructor
        ///</summary>
        [TestMethod()]
        public void DiffAttrXmlTest()
        {
            XmlComparer target = new XmlComparer();
            //target.AddIdentify
            XmlCompareResults result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a2"" >
                                    <X />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);

            result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a1"" >
                                    <X attr=""xx"" />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);

            result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a1"" >
                                    <X/>
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" >
                                    <X  attr=""xx"" />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);
        }
        /// <summary>
        ///A test for XmlComparer Constructor
        ///</summary>
        [TestMethod()]
        public void DiffOrderXmlTest()
        {
            XmlComparer target = new XmlComparer();
            //target.AddIdentify
            XmlCompareResults result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <B attra=""a1"" />
                                <C attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(result.IsValid);
        }
        /// <summary>
        ///A test for XmlComparer Constructor
        ///</summary>
        [TestMethod()]
        public void DiffOrderAndAttrXmlTest()
        {
            XmlComparer target = new XmlComparer();
            //target.AddIdentify
            XmlCompareResults result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <B attra=""a1"" />
                                <C attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <C attra=""a2"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);
        }
        /// <summary>
        ///A test for XmlComparer Constructor
        ///</summary>
        [TestMethod()]
        public void DiffElementXmlTest()
        {
            XmlComparer target = new XmlComparer();
            //target.AddIdentify
            XmlCompareResults result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <B attra=""a1"" />
                                <D attra=""a1"" />
                                <C attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);
            result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <B attra=""a1"" />
                                <C attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <C attra=""a1"" />
                                <D attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);
            result = target.Compare(@"<Persons attr1=""2"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <B attra=""a1"" />
                                <C attra=""a1"" />
                             </Persons>", @"<Persons attr1=""2"">
                                <A attra=""a1"" />
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);
        }


        /// <summary>
        ///A test for XmlComparer Constructor
        ///</summary>
        [TestMethod()]
        public void NamespaceXmlTest()
        {
            XmlComparer target = new XmlComparer();
            //target.AddIdentify
            XmlCompareResults result = target.Compare(@"<Persons attr1=""2""  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>",
                                        @"<Persons attr1=""2""  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");
            Assert.IsTrue(result.IsValid);

            result = target.Compare(@"<Persons attr1=""2""  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <D attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>",
                                       @"<Persons attr1=""2""  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <C attra=""a1"" />
                                <B attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);

            result = target.Compare(@"<Persons attr1=""2""  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"" xmlns:store=""china"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <store:D attra=""a1"" />
                             </Persons>",
                                       @"<Persons attr1=""2""  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"" xmlns:store=""china"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <store:D attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(result.IsValid);

            result = target.Compare(@"<Persons attr1=""2""  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"" xmlns:store=""china"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <store:D attra=""a1"" />
                             </Persons>",
                                       @"<Persons attr1=""2""  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"" xmlns:store=""china"">
                                <A attra=""a1"" >
                                    <X />
                                </A>
                                <D attra=""a1"" />
                             </Persons>");

            Assert.IsTrue(!result.IsValid);
        }
        /// <summary>
        ///A test for XmlComparer Constructor
        ///</summary>
        [TestMethod()]
        public void IdentityXmlTest()
        {
            XmlComparer target = new XmlComparer();
            target.AddIdentify(new AttributeIdentify("A", "name"));
            XmlCompareResults result = target.Compare(@"<Persons>
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a2"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>",
                                      @"<Persons >
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a2"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>");
            Assert.IsTrue(result.IsValid);
            result = target.Compare(@"<Persons>
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a1"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>",
                                     @"<Persons >
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a2"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>");
            Assert.IsTrue(!result.IsValid);
            target = new XmlComparer();
            target.AddIdentify(new AttributeIdentify("A", "name", "urn:schemas-microsoft-com:windows:storage:mapping:CS"));
            result = target.Compare(@"<Persons xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"" xmlns:store=""china"">
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a2"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>",
                                     @"<Persons  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"" xmlns:store=""china"">
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a2"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>");
            Assert.IsTrue(result.IsValid);
            result = target.Compare(@"<Persons xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"" xmlns:store=""china"">
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a1"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>",
                                     @"<Persons  xmlns=""urn:schemas-microsoft-com:windows:storage:mapping:CS"" xmlns:store=""china"">
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a2"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>");
            Assert.IsTrue(!result.IsValid);
        }
        /// <summary>
        ///A test for XmlComparer Constructor
        ///</summary>
        [TestMethod()]
        public void XmlCompareResultsTest()
        {
            XmlComparer target = new XmlComparer();
            target.AddIdentify(new AttributeIdentify("A", "name"));
            XmlCompareResults result = target.Compare(@"<Persons>
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a1"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>",
                                      @"<Persons >
                                <A name=""1"" value=""a1"" />
                                <A name=""2"" value=""a2"" />
                                <A name=""3"" value=""a3"" />
                             </Persons>");
            Assert.IsTrue(!result.IsValid);
            Assert.AreEqual(1, result.Errors.Count());
        }
    }
}
