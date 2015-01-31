using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using log4net;

namespace Moonlit.ServiceModel.Sms
{
    public class MwGatewaySmsService : SmsServiceBase
    {
        [DllImport("MWGateway.dll")]//帐户登陆
        private static extern int MongateConnect(string ip, int port, string account, string pwd);
        [DllImport("MWGateway.dll")]//发送单向信息
        private static extern int MongateSendSms(int clientsock, string mobi, string note);
        [DllImport("MWGateway.dll")]//测试连接
        private static extern int MongateTestConn(int clientsock);
        [DllImport("MWGateway.dll")]//查询余额
        private static extern int MongateQueryBalance(int clientsocket);
        [DllImport("MWGateway.dll")]//查询使用	
        private static extern int MongateQueryUsed(int clientsocket);
        [DllImport("MWGateway.dll")]//帐户充值
        private static extern int MongateRecharge(int clientsocket, string cardno, string cardpwd);
        [DllImport("MWGateway.dll")]//密码修改
        private static extern int MongateChangePwd(int clientsocket, string account, string oldpwd,
                                                   string newpwd);
        [DllImport("MWGateway.dll")]//断开连接
        private static extern int MongateDisconnect(int clientsock);
        [DllImport("MWGateway.dll")]//接收增值信息
        private static extern int MongateVasGetSms(int clientsock, ref byte recvbuf);
        [DllImport("MWGateway.dll")]//发送增值双向信息
        private static extern int MongateVasSendSms(int clientsock, string feecode, string spno,
                                                    string opercode, string linkid, string mobis, string msg, int icount);
        [DllImport("MWGateway.dll")]//接收客服信息
        private static extern int MongateCsGetSms(int clientsock, ref byte recvbuf);
        [DllImport("MWGateway.dll")]//发送客服双向信息
        private static extern int MongateCsSendSms(int clientsock, string mobi, string note, int icount,
                                                   StringBuilder msgno);
        [DllImport("MWGateway.dll")]//读取状态报告
        private static extern int MongateCsGetStatusReport(int clientsock, ref byte recvbuf);

        private int clientsock;
        protected override void BeginSend()
        {
            var config = Config;
            clientsock = MongateConnect(config.Host, config.Port, config.UserName, config.Password);
            if (clientsock <= 0)
            {
                throw new Exception(string.Format("connect to server {0} failed", clientsock));
            }
        }
        protected override void EndSend()
        {
            MongateDisconnect(clientsock);
        }
        protected override void OnSend(string number, string message)
        {
            var sendResult = MongateSendSms(clientsock, number, message);
            if (sendResult != 1)
            {
                throw new Exception(string.Format("send message {0} failed", sendResult));
            }
        } 
    }
}