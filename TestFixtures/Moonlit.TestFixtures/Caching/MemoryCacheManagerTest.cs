using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Caching;

namespace Moonlit.TestFixtures.Caching
{
    [TestClass]
    public class MemoryCacheManagerTest
    {
        [TestMethod]
        public void Test()
        {
            MemoryCacheManager cache = new MemoryCacheManager();
            cache.Set("name", "123456");
            Assert.AreEqual("123456", cache.Get<string>("name"));
        }
    }
}
