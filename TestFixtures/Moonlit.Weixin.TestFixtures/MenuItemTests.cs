using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Weixin.Tests
{
    [TestClass()]
    public class MenuItemTests
    {
        [TestMethod()]
        public void AddMenuItemTest()
        {
            var menuItem = new MenuItem("text");
            menuItem.AddMenuItem(new MenuButton("test1", "text"));
            menuItem.AddMenuItem(new MenuButton("test1", "text"));
            menuItem.AddMenuItem(new MenuButton("test1", "text"));
            menuItem.AddMenuItem(new MenuButton("test1", "text"));
            menuItem.AddMenuItem(new MenuButton("test1", "text"));
            try
            {
                menuItem.AddMenuItem(new MenuButton("test1", "text"));
                Assert.Fail("the max items count in root menu is 5");
            }
            catch (Exception)
            {
            }
            Assert.AreEqual(5, menuItem.Items.Count());
        }
    }
}