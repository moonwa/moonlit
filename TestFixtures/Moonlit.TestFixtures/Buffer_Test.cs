using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

    #region TestFrameworkUsing
#if !NUNIT

#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif
#endregion
namespace Moonlit.TestFixtures
{


    /// <summary>
    ///This is a test class for Buffer_Test and is intended
    ///to contain all Buffer_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class Buffer_Test
    {

#if !NUNIT
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
#endif
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
        ///A test for BlockCompare
        ///</summary>
        public void BlockCompare_TestHelper<T>()
            where T : IComparable<T>
        {
            Assert.AreEqual(0, Buffer<T>.BlockCompare(null, null));
        }

        [TestMethod()]
        public void BlockCompare_Test()
        {
            int[] left = new int[] { 1, 2, 3, 4, 5 };
            int[] right = new int[] { 1, 2, 3, 4, 5 };
            Assert.IsTrue(Buffer<int>.BlockCompare(left, right) == 0);
            Assert.IsTrue(Buffer<int>.BlockCompare(right, left) == 0);
            left = new int[] { 1, 3, 4, 5 };
            right = new int[] { 1, 2, 3, 4, 5 };
            Assert.IsTrue(Buffer<int>.BlockCompare(left, right) == 1);
            Assert.IsTrue(Buffer<int>.BlockCompare(left, 0, right, 0, 4) == 1);
            left = new int[] { 1, 2, 3, 4 };
            right = new int[] { 1, 2, 3, 4, 5 };
            Assert.IsTrue(Buffer<int>.BlockCompare(left, right) == -1);
            Assert.IsTrue(Buffer<int>.BlockCompare(left, 0, right, 0, 4) == 0);
        }

    }
}
