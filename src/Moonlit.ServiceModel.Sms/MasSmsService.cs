using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Moonlit.ServiceModel.Sms
{
    public class MasSmsService : SmsServiceBase
    {
        protected override void OnSend(string number, string message)
        {
            var config = base.Config;
            BasicHttpBinding binding = new BasicHttpBinding();
            var endPoint = new EndpointAddress(config.Host);

            RemoteSendSMSClient client = new RemoteSendSMSClient(binding, endPoint);
            var g = client.sendSMS1(config.UserName, new[] { number }, message, 0, 0, config.Password);
        }

        protected override void BeginSend()
        {
        }

        protected override void EndSend()
        {
        }
    }
}
