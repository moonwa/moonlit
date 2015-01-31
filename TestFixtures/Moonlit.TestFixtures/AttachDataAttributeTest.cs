using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures
{
    [TestClass()]
    public class AttachDataAttributeTest
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


        /// <summary>
        ///Value 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Moonlit.dll")]
        public void Test()
        {
            AgeRange range = AgeRange.From19To29;
            Assert.AreEqual("19至29岁", range.GetAttachedData<string>(AgeRangeAttachData.Text)); 
        }

        public enum AgeRange
        {
            [AttachData(AgeRangeAttachData.Text, "18岁及以下")]
            LessThan18,

            [AttachData(AgeRangeAttachData.Text, "19至29岁")]
            From19To29,

            [AttachData(AgeRangeAttachData.Text, "30岁及以上")]
            Above29
        }

        public enum AgeRangeAttachData
        {
            Text
        }

    }
}
