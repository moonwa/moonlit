using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.Weixin.Tests
{
    [TestClass]
    public class XmlObjectTest
    {
        [TestMethod]
        public void DoSignTest()
        {
            var data = "appid=wxd930ea5d5a258f4f&body=test&device_info=1000&mch_id=10000100&nonce_str=ibuaiVcKdpRxkhJA";
            Assert.AreEqual("9A0A8659F005D6984697E2CA0A9CF3B7", PaymentObject.DoSign(data));
        }
    }
}