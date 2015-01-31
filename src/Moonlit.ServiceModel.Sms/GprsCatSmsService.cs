using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;

namespace Moonlit.ServiceModel.Sms
{
    public class GprsCatSmsService : SmsServiceBase
    {
        [DllImport("sms.dll", EntryPoint = "Sms_Connection")]
        public static extern uint Sms_Connection(string CopyRight, uint Com_Port, uint Com_BaudRate, out string Mobile_Type, out string CopyRightToCOM);

        [DllImport("sms.dll", EntryPoint = "Sms_Disconnection")]
        public static extern uint Sms_Disconnection();

        [DllImport("sms.dll", EntryPoint = "Sms_Send")]
        public static extern uint Sms_Send(string Sms_TelNum, string Sms_Text);

        [DllImport("sms.dll", EntryPoint = "Sms_Receive")]
        public static extern uint Sms_Receive(string Sms_Type, out string Sms_Text);

        [DllImport("sms.dll", EntryPoint = "Sms_Delete")]
        public static extern uint Sms_Delete(string Sms_Index);

        [DllImport("sms.dll", EntryPoint = "Sms_AutoFlag")]
        public static extern uint Sms_AutoFlag();

        [DllImport("sms.dll", EntryPoint = "Sms_NewFlag")]
        public static extern uint Sms_NewFlag();
        private string CopyRightToCOM = "";
        private string TypeStr = "";
        String CopyRightStr = "//深圳市国爵电子有限公司,网址www.gprscat.com //";
        protected override void BeginSend()
        {
            _messageCount++;
            var config = base.Config;
            var port = _messageCount % (config.QueueCount - config.Port) + config.Port;
            if (Sms_Connection(CopyRightStr, (uint)port, 9600, out TypeStr, out CopyRightToCOM) != 1)
            {
                throw new Exception(string.Format("open com {0} failed", port));
            }
        }
        protected override void EndSend()
        {
            Sms_Disconnection();
        }
        protected override void OnSend(string number, string message)
        {
            Sms_Send(number, message);
        }

        private static int _messageCount = 0;
    }
}