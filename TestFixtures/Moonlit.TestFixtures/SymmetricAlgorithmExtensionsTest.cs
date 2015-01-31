using Moonlit.Security;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

    #region TestFrameworkUsing
#if !NUNIT

#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestContext = Moonlit.Diagnostics.TestContextHelper;
#endif
#endregion

namespace Moonlit.TestFixtures
{


    /// <summary>
    ///这是 SymmetricAlgorithmExtensionsTest 的测试类，旨在
    ///包含所有 SymmetricAlgorithmExtensionsTest 单元测试
    ///</summary>
    [TestClass()]
    public class SymmetricAlgorithmExtensionsTest
    {


        //private TestContext testContextInstance;

        ///// <summary>
        /////获取或设置测试上下文，上下文提供
        /////有关当前测试运行及其功能的信息。
        /////</summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}

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
        ///Encryptor 的测试
        ///</summary>
        [TestMethod()]
        public void EncryptorTest()
        {
            Test("hello world");
            Test("Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试Encryptor 的测试");
        }

        private static void Test(string hello)
        {
            System.Security.Cryptography.Rijndael rij = System.Security.Cryptography.Rijndael.Create();
            var data = Encoding.UTF8.GetBytes(hello);
            var cryptoData = rij.Encryptor(data, 0, data.Length);
            var decryptoData = rij.Dencryptor(cryptoData, 0, cryptoData.Length);
            string decryptoString = Encoding.UTF8.GetString(decryptoData);
            Assert.AreEqual(hello, decryptoString);
        }
    }
}
