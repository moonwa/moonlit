using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures
{
    [TestClass()]
    public class DictionaryHelperTests
    {
        [TestMethod]
        public void ExtendTest()
        {
            IDictionary<string, object> di1 = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Value1", 100},
                    {"Value2", 200},
                    {"Array1",
                        new object[] { 1,"2" } 
                    },
                    {
                        "Dict1", new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                        {
                            {"user1", "user33"}
                        }
                    }
                };
            IDictionary<string, object> di2 = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Value1", 1},
                    {"Value3", 3},
                    {"Array1",
                        new object[] { "1" } 
                    },
                    {
                        "Dict1", new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                        {
                            {"user1", "user1"},
                            {"user2", "user2"}
                        }
                    }
                };
            IDictionary<string, object> di3 = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)    {
                    {"Value2", 2},
                    {
                        "Dict2", new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                        {
                            {"user1", "user1"}
                        }
                    }
                };

            var di = DictionaryHelper.Extend( di1, di2, di3);
            Assert.AreEqual(1, di["Value1"]);
            Assert.AreEqual(2, di["Value2"]);
            Assert.AreEqual(3, di["Value3"]);
        }
    }
}