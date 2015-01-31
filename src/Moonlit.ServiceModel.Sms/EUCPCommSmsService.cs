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
        [DllImport("EUCPComm.dll", EntryPoint = "SendSMS")]  //��ʱ����
        public static extern int SendSMS(string sn, string mn, string ct, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendSMSEx")]  //��ʱ����(��չ)
        public static extern int SendSMSEx(string sn, string mn, string ct, string addi, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendScheSMS")]  // ��ʱ����
        public static extern int SendScheSMS(string sn, string mn, string ct, string ti, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "SendScheSMSEx")]  // ��ʱ����(��չ)
        public static extern int SendScheSMSEx(string sn, string mn, string ct, string ti, string addi, string priority);

        [DllImport("EUCPComm.dll", EntryPoint = "ReceiveSMS")]  // ���ն���
        public static extern int ReceiveSMS(string sn, deleSQF mySmsContent);

        [DllImport("EUCPComm.dll", EntryPoint = "ReceiveSMSEx")]  // ���ն���
        public static extern int ReceiveSMSEx(string sn, deleSQF mySmsContent);

        //[DllImport("EUCPComm.dll",EntryPoint="ReceiveStatusReport")]  // ���ն��ű���
        //public static extern int ReceiveStatusReport(string sn,delegSMSReport mySmsReport);  

        //[DllImport("EUCPComm.dll",EntryPoint="ReceiveStatusReportEx")]  // ���ն��ű���(������ID)
        //public static extern int ReceiveStatusReportEx(string sn,delegSMSReportEx mySmsReportEx);  

        [DllImport("EUCPComm.dll", EntryPoint = "Register")]   // ע�� 
        public static extern int Register(string sn, string pwd, string EntName, string LinkMan, string Phone, string Mobile, string Email, string Fax, string sAddress, string Postcode);

        [DllImport("EUCPComm.dll", EntryPoint = "GetBalance", CallingConvention = CallingConvention.Winapi)] // ��� 
        public static extern int GetBalance(string m, System.Text.StringBuilder balance);


        [DllImport("EUCPComm.dll", EntryPoint = "ChargeUp")]  // ��ֵ
        public static extern int ChargeUp(string sn, string acco, string pass);

        [DllImport("EUCPComm.dll", EntryPoint = "GetPrice")]  // �۸�
        public static extern int GetPrice(string m, System.Text.StringBuilder balance);

        [DllImport("EUCPComm.dll", EntryPoint = "RegistryTransfer")]  //����ת��
        public static extern int RegistryTransfer(string sn, string mn);

        [DllImport("EUCPComm.dll", EntryPoint = "CancelTransfer")]  // ע��ת��
        public static extern int CancelTransfer(string sn);

        [DllImport("EUCPComm.dll", EntryPoint = "UnRegister")]  // ע��
        public static extern int UnRegister(string sn);

        [DllImport("EUCPComm.dll", EntryPoint = "SetProxy")]  // ���ô�������� 
        public static extern int SetProxy(string IP, string Port, string UserName, string PWD);

        [DllImport("EUCPComm.dll", EntryPoint = "RegistryPwdUpd")]  // �޸����к�����
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