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
        [DllImport("MWGateway.dll")]//�ʻ���½
        private static extern int MongateConnect(string ip, int port, string account, string pwd);
        [DllImport("MWGateway.dll")]//���͵�����Ϣ
        private static extern int MongateSendSms(int clientsock, string mobi, string note);
        [DllImport("MWGateway.dll")]//��������
        private static extern int MongateTestConn(int clientsock);
        [DllImport("MWGateway.dll")]//��ѯ���
        private static extern int MongateQueryBalance(int clientsocket);
        [DllImport("MWGateway.dll")]//��ѯʹ��	
        private static extern int MongateQueryUsed(int clientsocket);
        [DllImport("MWGateway.dll")]//�ʻ���ֵ
        private static extern int MongateRecharge(int clientsocket, string cardno, string cardpwd);
        [DllImport("MWGateway.dll")]//�����޸�
        private static extern int MongateChangePwd(int clientsocket, string account, string oldpwd,
                                                   string newpwd);
        [DllImport("MWGateway.dll")]//�Ͽ�����
        private static extern int MongateDisconnect(int clientsock);
        [DllImport("MWGateway.dll")]//������ֵ��Ϣ
        private static extern int MongateVasGetSms(int clientsock, ref byte recvbuf);
        [DllImport("MWGateway.dll")]//������ֵ˫����Ϣ
        private static extern int MongateVasSendSms(int clientsock, string feecode, string spno,
                                                    string opercode, string linkid, string mobis, string msg, int icount);
        [DllImport("MWGateway.dll")]//���տͷ���Ϣ
        private static extern int MongateCsGetSms(int clientsock, ref byte recvbuf);
        [DllImport("MWGateway.dll")]//���Ϳͷ�˫����Ϣ
        private static extern int MongateCsSendSms(int clientsock, string mobi, string note, int icount,
                                                   StringBuilder msgno);
        [DllImport("MWGateway.dll")]//��ȡ״̬����
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