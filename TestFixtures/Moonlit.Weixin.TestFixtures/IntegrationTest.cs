using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.Weixin.Tests
{
    [TestClass()]
    public class IntegrationTest
    {
        [TestMethod()]
        public async Task MenuTest()
        {
            MPClient client = MakeMPClient();

            var menuItem = new MenuItem("text");
            menuItem.AddMenuItem(new MenuButton("test1", "text"));
            menuItem.AddMenuItem(new MenuButton("test1", "text")); 
            menuItem.AddMenuItem(new MediaButton("JdW_0g3R89AYK57-MXDpN83i_RngLTz_spquCwFY45E", "media1"));


            var menu = WeixinMenu.Create();
            menu.AddMenuItem(menuItem);
            await client.UpdateMenuAsync(menu);

        }
        [TestMethod()]
        public async Task GetMaterialNewsListAsyncTest()
        {
            MPClient client = MakeMPClient();

            var list = await client.GetMaterialNewsListAsync(0, 20);

        }
        [TestMethod()]
        public async Task GetMaterialImageListAsyncTest()
        {
            MPClient client = MakeMPClient();

            var list = await client.GetMaterialImageListAsync(0, 20);

        }

        private static MPClient MakeMPClient()
        {
            return new MPClient("wxd8b64943ac261c4c", "1bd49f44b5f70605e182a618c640d9b3", "AcdrdW33Wpbd2FwMC4S93yJJk3nHM0g0");
        }
    }
}
