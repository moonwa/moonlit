using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.Weixin.Tests
{
    [TestClass]
    public class UnifiedOrderTest
    {
        [TestMethod]
        public void SerialializeTest()
        {
            //var order = UnifiedOrder.CreateJsapi();
            //order.FeeType = FeeType.CNY;
            //var xml = order.ToXml();
            //Console.WriteLine(1);
            var d = PaymentObject.DoSign(
                    "appid=wxd8b64943ac261c4c&attach=RC201606161652410000000022&body=会员卡0000000000000001充值0.01元&detail=会员卡0000000000000001充值0.01&device_info=WEB&fee_type=CNY&mch_id=1327313401&nonce_str=00027497064e495eb91c92d8afa517e3&notify_url=http://weixintest.ecard.chihank.com/000000002/Payment/Payment/Notify&openid=oS7mJwK6jQZoBs2ZAPN3NSFmvxLg&out_trade_no=RC201606161652410000000022&spbill_create_ip=59.40.231.46&time_expire=20160617165305&time_start=20160616165241&total_fee=1&trade_type=JSAPI&key=1234881IKyudkeoi484fjklj98rt3489jt4i");
            Assert.AreEqual("5EE82CFAB5397E9D9543BEABE95CA0FD", d);
        }
    }
}
