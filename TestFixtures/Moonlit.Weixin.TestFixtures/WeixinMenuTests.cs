using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.Weixin.Tests
{
    [TestClass]
    public class WeixinMenuTests : TestBase
    {
        [TestMethod]
        public void AddMenuItemTest()
        {
            var menu = WeixinMenu.Create();
            menu.AddMenuItem(new MenuButton("test1", "text"));
            menu.AddMenuItem(new MenuButton("test1", "text"));
            menu.AddMenuItem(new MenuButton("test1", "text"));
            try
            {
                menu.AddMenuItem(new MenuButton("test1", "text"));
                Assert.Fail("the max items count in root menu is 3");
            }
            catch (Exception)
            {
            }
            Assert.AreEqual(3, menu.Items.Count());
        }
    }
}