using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Extensions
{
    [TestClass]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void TestTrim()
        {
            Assert.AreNotEqual("0.01", 0.01000000m.ToString());
            Assert.AreEqual("0.01", 0.01000000m.Trim().ToString());
            Assert.AreEqual("0", 0.0000000m.Trim().ToString());
            Assert.AreEqual("0", 0m.Trim().ToString());
            Assert.AreEqual("10", 10m.Trim().ToString());
        }
    }
}
