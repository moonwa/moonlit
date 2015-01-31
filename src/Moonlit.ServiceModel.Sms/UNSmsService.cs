using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Moonlit.ServiceModel.Sms
{
    /// <summary>
    /// UNCom ����ƽ̨
    /// </summary>
    public class UNSmsService : SmsServiceBase
    {
        [DllImport("winsms")]
        public static extern string Sendsms(string uid, string pwd, string mobno, string content, string mycode, string time);

        ////����� ����
        //function Remoney(uid,pwd:pchar):pchar;stdcall;  

        ////���ն���  ����
        //function Getsms(uid,pwd:pchar):pchar;stdcall;

        ////�޸����� ����
        //function Editpwd(uid,pwd,newpwd:pchar):pchar;stdcall; 


        static UNSmsService()
        {
            _errors.Add(0, "���ͳɹ�");
            _errors.Add(-1, "��ǰ�˺�����");
            _errors.Add(-2, "��ǰ�û�ID����");
            _errors.Add(-3, "��ǰ�������");
            _errors.Add(-4, "����������������ݵ����ʹ���");
            _errors.Add(-5, "�ֻ������ʽ����");
            _errors.Add(-6, "�������ݱ��벻��");
            _errors.Add(-7, "�������ݺ��������ַ�");
            _errors.Add(-8, "�޽�������");
            _errors.Add(-9, "ϵͳά����..");
            _errors.Add(-10, "�ֻ�������������");
            _errors.Add(-11, "�������ݳ���");
            _errors.Add(-12, "��������");
        }

        protected override void BeginSend()
        {
        }
        protected override void EndSend()
        {
        }

        static Dictionary<int, string> _errors = new Dictionary<int, string>();
        protected override void OnSend(string number, string message)
        {
            var config = base.Config;
            var s = Sendsms(config.UserName, config.Password, number, message, "", "&now&");
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Trim();

                var pos = s.IndexOf("/");
                if (pos > 0)
                {
                    s = s.Substring(0, pos);
                    int v = 0;
                    if (int.TryParse(s, out v))
                    {
                        if (v == 0) return;
                        if (_errors.ContainsKey(v))
                            throw new Exception(_errors[v]);
                    }
                }
            }
            throw new Exception(s ?? "null");
        }
    }
}