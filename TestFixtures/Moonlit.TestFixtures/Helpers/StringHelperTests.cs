using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Helpers
{
    [TestClass()]
    public class StringHelperTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod()]
        public void ToSentence_Test()
        {
            Assert.AreEqual("Hello World", StringHelper.ToSentence("HelloWorld"));
            Assert.AreEqual("Removed RAF", StringHelper.ToSentence("RemovedRAF"));
            Assert.AreEqual("RAF Parent", StringHelper.ToSentence("RAFParent"));
            Assert.AreEqual("What Is RAF Parent", StringHelper.ToSentence("WhatIsRAFParent"));
            Assert.AreEqual("Hello_World", StringHelper.ToSentence("HelloWorld", "_"));
        }
        [TestMethod()]
        public void ToAbsoluteLocalPath_Test()
        {
            Assert.AreEqual(@"d:\a\b\test", StringHelper.ToAbsoluteLocalPath(@"..\test", @"d:\a\b\c"));
            Assert.AreEqual(@"c:\test", StringHelper.ToAbsoluteLocalPath(@"c:\test", @"d:\a\b\c"));
            Assert.IsNull(StringHelper.ToAbsoluteLocalPath(@"test", @"a\b\c"));
        }
        [TestMethod()]
        public void StartsWithIgnoreCase_Test()
        {
            Assert.IsTrue(@"Abc".StartsWithIgnoreCase("a"));
            Assert.IsTrue(@"Abc".StartsWithIgnoreCase(null, true));
            Assert.IsFalse(((string)null).StartsWithIgnoreCase(null, true));
            Assert.IsFalse(((string)null).StartsWithIgnoreCase(null, false));
        }

    }
}