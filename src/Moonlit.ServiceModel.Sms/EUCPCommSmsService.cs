using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;

namespace Moonlit.ServiceModel.Sms
{
    public class EUCPCommSmsService : SmsServiceBase
    {
        public delegate void deleSQF(string mobile, string senderaddi, string recvaddi, string ct, string sd, ref int flag);
        [DllImport("EUCPComm.dll", EntryPoint = "SendSMS")]  //即时发送
        public static extern int SendSMS(string sn, string mn, string ct, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendSMSEx")]  //即时发送(扩展)
        public static extern int SendSMSEx(string sn, string mn, string ct, string addi, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendScheSMS")]  // 定时发送
        public static extern int SendScheSMS(string sn, string mn, string ct, string ti, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendScheSMSEx")]  // 定时发送(扩展)
        public static extern int SendScheSMSEx(string sn, string mn, string ct, string ti, string addi, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "ReceiveSMS")]  // 接收短信
        public static extern int ReceiveSMS(string sn, deleSQF mySmsContent);

        [DllImport("EUCPComm.dll", EntryPoint = "ReceiveSMSEx")]  // 接收短信
        public static extern int ReceiveSMSEx(string sn, deleSQF mySmsContent);

        //[DllImport("EUCPComm.dll",EntryPoint="ReceiveStatusReport")]  // 接收短信报告
        //public static extern int ReceiveStatusReport(string sn,delegSMSReport mySmsReport);  

        //[DllImport("EUCPComm.dll",EntryPoint="ReceiveStatusReportEx")]  // 接收短信报告(带批量ID)
        //public static extern int ReceiveStatusReportEx(string sn,delegSMSReportEx mySmsReportEx);  

        [DllImport("EUCPComm.dll", EntryPoint = "Register")]   // 注册 
        public static extern int Register(string sn, string pwd, string EntName, string LinkMan, string Phone, string Mobile, string Email, string Fax, string sAddress, string Postcode);

        [DllImport("EUCPComm.dll", EntryPoint = "GetBalance", CallingConvention = CallingConvention.Winapi)] // 余额 
        public static extern int GetBalance(string m, System.Text.StringBuilder balance);


        [DllImport("EUCPComm.dll", EntryPoint = "ChargeUp")]  // 存值
        public static extern int ChargeUp(string sn, string acco, string pass);

        [DllImport("EUCPComm.dll", EntryPoint = "GetPrice")]  // 价格
        public static extern int GetPrice(string m, System.Text.StringBuilder balance);

        [DllImport("EUCPComm.dll", EntryPoint = "RegistryTransfer")]  //申请转接
        public static extern int RegistryTransfer(string sn, string mn);

        [DllImport("EUCPComm.dll", EntryPoint = "CancelTransfer")]  // 注销转接
        public static extern int CancelTransfer(string sn);

        [DllImport("EUCPComm.dll", EntryPoint = "UnRegister")]  // 注销
        public static extern int UnRegister(string sn);

        [DllImport("EUCPComm.dll", EntryPoint = "SetProxy")]  // 设置代理服务器 
        public static extern int SetProxy(string IP, string Port, string UserName, string PWD);

        [DllImport("EUCPComm.dll", EntryPoint = "RegistryPwdUpd")]  // 修改序列号密码
        public static extern int RegistryPwdUpd(string sn, string oldPWD, string newPWD);
        protected override void OnSend(string number, string message)
        {
            var config = base.Config;
            var sendResult = SendSMS(config.UserName, number, message, "1");
            if (sendResult != 1)
            {
                throw new Exception(string.Format("send message failed: {0}", sendResult));
            } 
        }

        protected override void BeginSend()
        {
             
        }

        protected override void EndSend()
        {
        }
    }
}