using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Configuration;

namespace Moonlit.TestFixtures.Configuration
{
    /// <summary>
    ///这是 ParserTest 的测试类，旨在
    ///包含所有 ParserTest 单元测试
    ///</summary>
    [TestClass()]
    public class HostsTests
    {
        [TestMethod()]
        public void OpenTest()
        {
            var hosts = Hosts.Open();
            hosts.SetHost( "mycomputer", "127.0.0.2");
            hosts.Save();

            hosts = Hosts.Open();
            Assert.AreEqual("127.0.0.2", hosts.GetIP("mycomputer"));
        }
    }
}