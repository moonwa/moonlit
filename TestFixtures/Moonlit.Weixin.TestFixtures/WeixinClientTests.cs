using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Weixin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Moonlit.Xml;

namespace Moonlit.Weixin.Tests
{
    [TestClass()]
    public class WeixinDataTests : TestBase
    {
        [TestMethod()]
        public async Task SignJsapi()
        {
            WeixinData data = new WeixinData();
            data["jsapi_ticket"] = "sM4AOVdWfPE4DxkXGEs8VMCPGGVi4C3VM0P37wVUCFvkVAy_90u5h9nbSlYy3-Sl-HhTdfl2fzFy1AOcHKP7qg";
            data["noncestr"] = "Wm3WZYTPz0wzccnW";
            data["timestamp"] = "1414587457";
            data["url"] = "http://mp.weixin.qq.com?params=value";
            Assert.AreEqual("0f9de62fce790f9a083d5c99e95740ceb90c27ed", data.SignJsapi());
        }
    }

    [TestClass()]
    public class WeixinSerializerTests : TestBase
    {
        [TestMethod()]
        public async Task SerializeContentTextTest()
        {
            var xele = new TextMessage()
            {
                ToUserName = "zz",
                FromUserName = "tom",
                CreateTime = DateTime.Parse("2015-1-1"),
                Content = "hello",
            }.ToXml();
    

            Assert.AreEqual("xml", xele.Name);
            Assert.AreEqual("zz", xele.Element("ToUserName").Value);
            Assert.AreEqual("tom", xele.Element("FromUserName").Value);
            Assert.AreEqual("1420070400", xele.Element("CreateTime").Value);
            Assert.AreEqual("text", xele.Element("MsgType").Value);
            Assert.AreEqual("hello", xele.Element("Content").Value);
        }
        [TestMethod()]
        public async Task DeserializeTextMessageTest()
        { 
            var content = MPClient.Parse(@"<xml><ToUserName><![CDATA[gh_ee234cfa54cd]]></ToUserName>
<FromUserName><![CDATA[oNm45xK4rQ82C2Z5mHmar5qrq2ew]]></FromUserName>
<CreateTime>1447513799</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[tt]]></Content>
<MsgId>6217024427614158504</MsgId>
</xml>") as TextMessage;
             
            Assert.AreEqual("gh_ee234cfa54cd", content.ToUserName);
            Assert.AreEqual("oNm45xK4rQ82C2Z5mHmar5qrq2ew", content.FromUserName);
            Assert.AreEqual(DateTime.Parse("11/14/2015 23:09:59"), content.CreateTime.ToLocalTime());
            Assert.AreEqual("tt", content.Content);
        }
        [TestMethod()]
        public async Task DeserializePicturePhotoEventMessageTest()
        { 
            var message = MPClient.Parse(@"  <xml><ToUserName><![CDATA[gh_e136c6e50636]]></ToUserName>
<FromUserName><![CDATA[oMgHVjngRipVsoxg6TuX3vz6glDg]]></FromUserName>
<CreateTime>1408090651</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[pic_sysphoto]]></Event>
<EventKey><![CDATA[6]]></EventKey>
<SendPicsInfo><Count>1</Count>
<PicList><item><PicMd5Sum><![CDATA[1b5f7c23b5bf75682a53e7b6d163e185]]></PicMd5Sum>
</item>
</PicList>
</SendPicsInfo>
</xml>") as PhotoEventMessage;
             
            Assert.AreEqual("gh_e136c6e50636", message.ToUserName);
            Assert.AreEqual("oMgHVjngRipVsoxg6TuX3vz6glDg", message.FromUserName);
            Assert.AreEqual("6", message.EventKey);
        }
        [TestMethod()]
        public async Task DeserializeImageMessageTest()
        { 
            var message = MPClient.Parse(@"  <xml>
                <ToUserName><![CDATA[gh_ee234cfa54cd]]></ToUserName>
                <FromUserName><![CDATA[oNm45xK4rQ82C2Z5mHmar5qrq2ew]]></FromUserName>
                <CreateTime>1447517266</CreateTime>
                <MsgType><![CDATA[image]]></MsgType>
                <PicUrl><![CDATA[http://mmbiz.qpic.cn/mmbiz/ohGfkhF6WDymzYJFK3Qib5fwYPtBUJ4dyO5CjWtkOGIUhZHrejzdmc70ueCiaLvUbWBicdOQV8sJhibfkVrUmHu2ZQ/0]]></PicUrl>
                <MsgId>6217039318265774317</MsgId>
                <MediaId><![CDATA[hrJzzphu6KiUKfFx32hFOCOZfCORV77opcq4c_Igog-KNyaKo_-KyZ8UJ0rpmuDe]]></MediaId>
</xml>") as ImageMessage;
             
            Assert.AreEqual("gh_ee234cfa54cd", message.ToUserName);
            Assert.AreEqual("oNm45xK4rQ82C2Z5mHmar5qrq2ew", message.FromUserName);
            Assert.AreEqual("http://mmbiz.qpic.cn/mmbiz/ohGfkhF6WDymzYJFK3Qib5fwYPtBUJ4dyO5CjWtkOGIUhZHrejzdmc70ueCiaLvUbWBicdOQV8sJhibfkVrUmHu2ZQ/0", message.PicUrl);
            Assert.AreEqual("hrJzzphu6KiUKfFx32hFOCOZfCORV77opcq4c_Igog-KNyaKo_-KyZ8UJ0rpmuDe", message.MediaId);
        }
    }

  
    [TestClass()]
    public class WeixinClientTests : TestBase
    {
        [TestMethod()]
        public async Task RequiredAccessTokenAsyncTest()
        {
            var token = await Client.RequiredAccessTokenAsync();

            Assert.IsFalse(string.IsNullOrWhiteSpace(token.Token));
            Assert.IsTrue(token.ExpireTime > DateTime.Now);
            Thread.Sleep(3000);
            Assert.IsTrue(token.ExpireTime > DateTime.Now);
        }
        [TestMethod()]
        public async Task CreateMenuAsyncTest()
        {
            var menu = WeixinMenu.Create();
            var menuItem = new MenuItem("会员事务");
            menuItem.AddMenuItem(new ViewButton(Client.MakeOAuth("http://weixin.ecard.chihank.com//test", WeiXinOAuthType.Base, "123"), "修改基础资料"));
            menuItem.AddMenuItem(new ViewButton("http://weixin.3ecard.net", "修改交易密码"));
            menuItem.AddMenuItem(new ViewButton("http://weixin.3ecard.net", "加盟商户"));
            menu.AddMenuItem(menuItem);

            menuItem = new MenuItem("会员交易");
            menuItem.AddMenuItem(new ViewButton("http://weixin.3ecard.net", "实时交易"));
            menuItem.AddMenuItem(new ViewButton("http://weixin.3ecard.net", "交易记录"));
            menu.AddMenuItem(menuItem);
            await Client.UpdateMenuAsync(menu);
        }
        [TestMethod()]
        public async Task UserMakeOAuthUrlTest()
        {
            var url = Client.MakeOAuth("http://www.baidu.com", WeiXinOAuthType.Base, "12345").ToLower();
            Assert.AreEqual($"https://open.weixin.qq.com/connect/oauth2/authorize?appid={Client.AppId}&redirect_uri=http%3A%2F%2Fwww.baidu.com&response_type=code&scope=snsapi_base&state=12345#wechat_redirect".ToLower(), url);
        }
    }
}